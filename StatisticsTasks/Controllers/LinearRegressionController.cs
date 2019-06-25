using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using StatisticsTasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatisticsTasks.Controllers
{
    public class LinearRegressionController : Controller
    {
        // GET: LinearRegression
        public ActionResult LR_Result()
        {
            return View();
        }
        UseCredentials cred = new UseCredentials();
        //service for google sheets api
        private SheetsService service;
        //sheet ID
        string sheetID;
        //number of values
        int N;
        //url link
        string url;
        //code of task
        string task;
        FormulaEntities db = new FormulaEntities();
        // GET: ExpectedValue
        [HttpPost]
        public ActionResult MainProc(string Num)
        {
            N = Int32.Parse(Num);
            CreateSheet(N);
            PutFormula();
            LogIn();
            return View("LR_Result");
        }
        //the main procedure
        public void CreateSheet(int N)
        {
            //get credentials
            service = cred.GetCredential();
            //create a spreadsheet
            Spreadsheet requestBody = new Spreadsheet();
            SpreadsheetsResource.CreateRequest request = service.Spreadsheets.Create(requestBody);
            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            Spreadsheet response = request.Execute();
            // Data.Spreadsheet response = await request.ExecuteAsync();
            // TODO: Change code below to process the `response` object:
            JsonConvert.SerializeObject(response);
            //get spreadsheet id
            sheetID = response.SpreadsheetId;
            //get a hyperlink for user to use
            url = response.SpreadsheetUrl;
        }

        //write a formula of expected value in the spreadsheet cell
        public List<FormTable> PutFormula()
        {
            //the cell with formula
            String range = "A" + (N + 1);
            //a range of values for this task is default
            ValueRange valueRange = new ValueRange
            {
                MajorDimension = "ROWS"
            };

            //formula as query  
            task = "LR";
            var q = "SELECT * FROM FormTable WHERE CodeTask ={0}";
            var query = db.Database.SqlQuery<FormTable>(q, task).FirstOrDefault();
            var tmp = query.Formula.Replace("n", N.ToString());
            var obj = new List<object> { tmp };
            valueRange.Values = new List<IList<object>> { obj };
            //update the spreadsheet
            //request
            SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(valueRange, sheetID, range);
            //allowance for editing the spreadsheet
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            //execute the request
            UpdateValuesResponse res = update.Execute();
            return null;
        }
        public void LogIn()
        {
            ViewBag.choice = url;
        }
    }
}