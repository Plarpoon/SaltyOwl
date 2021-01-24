using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace WoM_Balance_Bot
{
    internal class GoogleAPI
    {
        // Time and date.
        private const string Format = "HH:mm:ss tt";

        // Define request parameters.
        private static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };

        private static readonly string ApplicationName = "WoM Google Spreadsheet Access";
        private static readonly string spreadsheetId = "16W56LWqt6wDaYAU5xNdTWCdaY_gkuQyl4CE1lPpUui4";  // ID as written in the URL bar.
        private static readonly string sheet = "Bank Roll";                                             // Name of the Sheet page in question.
        private static readonly string range = $"{sheet}!G159:Y171";
        private static SheetsService service;

        public static void Sheets()
        {
            //  Console output
            Console.WriteLine(DateTime.Now.ToString(Format) + "Google " + "     OAuth login");

            UserCredential credential;

            using (var stream =
                new FileStream("google-credentials.json", FileMode.Open, FileAccess.Read))
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
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Write here the other functions.
            ReadEntries();
        }

        private static void ReadEntries()
        {
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                /*
                foreach (var row in values)
                {
                    // Refactor with a database instead.
                }*/
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            Console.Read();
        }
    }
}