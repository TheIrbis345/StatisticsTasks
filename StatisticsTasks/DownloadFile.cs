using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace StatisticsTasks
{
    public class DownloadFile
    {
        static string[] Scopes = new string[] { DriveService.Scope.Drive };
        private DriveService drive;
        public DriveService GetCredentials()
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
                drive = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "StatisticsTasks",
                });
            }
            return drive;
        }
    }
}