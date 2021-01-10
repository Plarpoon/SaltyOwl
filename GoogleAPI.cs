using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

namespace WoM_Balance_Bot
{
    public class GoogleAPI
    {
        //  Time and date.
        private const string Format = "HH:mm:ss tt";

        //  Google Sheet API init.
        private static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };

        private static readonly string ApplicationName = "WoM Google Spreadsheet Access";

        public static void Sheets()
        {
            UserCredential credential;

            Console.WriteLine(DateTime.Now.ToString(Format) + "Google " + "     OAuth login");

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                if (File.Exists("token.json"))
                    Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "16W56LWqt6wDaYAU5xNdTWCdaY_gkuQyl4CE1lPpUui4";  // ID as written in the URL bar.
            String sheet = "Bank Roll";                                             // Name of the Sheet page in question.
            String range = $"{sheet}!G159:Y169";                                    // Range of rows to read and scrape.

            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    //Console.WriteLine("{0} {2}", row[0]);             need to be edited!!!
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
        }
    }
}