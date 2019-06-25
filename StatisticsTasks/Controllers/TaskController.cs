using StatisticsTasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatisticsTasks.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task
        string result;
        FormulaEntities db = new FormulaEntities();
        public ActionResult ChooseTask()
        {
             List<SelectListItem> items = new List<SelectListItem>();
             items.Add(new SelectListItem { Text = "Выборочное среднее", Value = "0", Selected=true});
             items.Add(new SelectListItem { Text = "Исправленная среднеквадратическая дисперсия", Value = "1" });
             items.Add(new SelectListItem { Text = "Т-тест Стьюдента", Value = "2" });
             items.Add(new SelectListItem { Text = "Линейная регрессия", Value = "3" });
             ViewData["Result"] = items;
            return View();
        }
        [HttpPost]
        public ActionResult WhichTask(FormCollection form)
        {
            result = "";
            string view;
            view = "";
            var tmp = form["Result"];
            CheckTaskCode(Convert.ToInt32(tmp));
            if((tmp=="0"))
                view="ExpectedValue";
            if(tmp=="1")
                view = "SampleVariance";
            if (tmp == "2")
                view = "StudentsTtest";
            if (tmp == "3")
                view="LinearRegression";
            ViewBag.choice = result;
            return View(view);

        }
        public void CheckTaskCode(int value)
        {
            if (value == 0)
                result = "Expected Value";
            if (value == 1)
                result = "Sample Variance";
            if (value == 2)
                result = "Students T-test";
            if (value == 3)
                result = "Linear Regression";
        }
        // POST: solve the chosen task
       /* [HttpPost]
        public ActionResult MainProc(string N)
        {
            ViewBag.choice = N;
            return View();
        }*/
    }
}