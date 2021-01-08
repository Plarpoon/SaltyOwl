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
            String spreadsheetId = "16W56LWqt6wDaYAU5xNdTWCdaY_gkuQyl4CE1lPpUui4";
            String range = "Class Data!G163:I";
            _ =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            /*
             *      THIS IS EXAMPLE DATA AND NEEDS TO BE DELETED
             *
            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            if (values != null && values.Count > 0)
            {
                Console.WriteLine("Name, Major");
                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    Console.WriteLine("{0}, {1}", row[0], row[4]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            Console.Read();
            */
        }
    }
}