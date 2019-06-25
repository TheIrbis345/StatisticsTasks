using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace StatisticsTasks
{
    public class UseCredentials
    {
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private SheetsService service;

        public SheetsService GetCredential()
        {
            UserCredential credential;
            using (var stream = new FileStream("C:\\Users\\user\\Desktop\\StatisticsTasks\\StatisticsTasks\\credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credPath = "C:\\Users\\user\\Desktop\\StatisticsTasks\\StatisticsTasks\\new_token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                //service
                service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "StatisticsTasks",
                });
            }
            return service;
        }
    }
}