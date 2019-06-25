using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using StatisticsTasks.Models;

namespace StatisticsTasks.Controllers
{
    public class TestController : Controller
    {
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "StatisticsTasks";
        // GET: Test
        public ActionResult Get()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("C:\\Users\\user\\Desktop\\StatisticsTasks\\StatisticsTasks\\credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore("C:\\Users\\user\\Desktop\\StatisticsTasks\\StatisticsTasks\\" + credPath, true)).Result;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
            String range = "Class Data!A2:E";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                int i = 0;
                string[] res1 = new string[values.Count];
                string[] res2 = new string[values.Count];
                while (i < values.Count)
                {
                    foreach (var row in values)
                    {
                        // Print columns A and E, which correspond to indices 0 and 4.
                        res1[i] = row[0].ToString();
                        res2[i] = row[4].ToString();
                        i++;
                    }
                }
                //Console.WriteLine("Name, Major");
                ViewBag.choice1 = res1;
                ViewBag.choice2 = res2;
            }
            else
            {
                //MessageBox.Show("No data found.");
            }
            return View();
        }
    }
}