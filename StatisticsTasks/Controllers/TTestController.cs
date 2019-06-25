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
    public class TTestController : Controller
    {
        // GET: TTest
        UseCredentials cred = new UseCredentials();
        //service for google sheets api
        private SheetsService service;
        //sheet ID
        string sheetID;
        //number of values
        int N;
        //asks the tails of the distribution tails
        int tails;
        //asks the type of the test
        int type;
        //url link
        string url;
        //code of task
        string task;
        FormulaEntities db = new FormulaEntities();
        // GET: ExpectedValue
        public ActionResult ST_Result()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetN(string Num,string tls,string tp)
        {
            N = Int32.Parse(Num);
            tails = Int32.Parse(tls);
            type = Int32.Parse(tp);
            CreateSheet(N,tails,type);
            PutFormula();
            LogIn();
            return View("ST_Result");
        }
        //the main procedure
        public void CreateSheet(int N,int tails,int types)
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
            //a range of values - only columns for this task
            ValueRange valueRange = new ValueRange
            {
                MajorDimension = "ROWS"
            };

            //formula as query  
            task = "ST";
            var q = "SELECT * FROM FormTable WHERE CodeTask ={0}";
            var query = db.Database.SqlQuery<FormTable>(q, task).FirstOrDefault();
            var tmp = query.Formula.Replace("N", N.ToString()).Replace("tails", tails.ToString()).Replace("type", type.ToString());
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