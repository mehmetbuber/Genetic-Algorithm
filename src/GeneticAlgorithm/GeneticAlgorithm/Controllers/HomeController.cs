using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GeneticAlgorithm.Helpers;
using GeneticAlgorithm.Models;

namespace GeneticAlgorithm.Controllers
{
    public class HomeController : Controller
    {
        Random random = new Random();

        public ActionResult Index(int populationCount = 20, int variableCount = 20, int valueCount = 5)
        {
            ViewBag.populationCount = populationCount;
            ViewBag.variableCount = variableCount;
            ViewBag.valueCount = valueCount;
            var probabilities = Math.Pow(valueCount, variableCount);
            var duration = probabilities / (1000f * 60f * 60f * 24f * 365f);
            ViewBag.probabilities = probabilities;
            ViewBag.duration = duration;

            if (populationCount > 0 && variableCount > 0 && valueCount > 0)
            {
                Evolution evolution = new Evolution(populationCount, variableCount, valueCount, random);
                ViewBag.evolution = evolution;
            }

            return View();
        }

        public ActionResult Evolution(string evolutionId = "")
        {
            var evolution = DatabaseHelpers.GetEvolution(evolutionId);

            if (evolution != null)
            {
                ViewBag.populationCount = evolution.Population.Count;
                ViewBag.variableCount = evolution.VariableCount;
                ViewBag.valueCount = evolution.ValueCount;
                ViewBag.evolution = evolution;
                var probabilities = Math.Pow(evolution.ValueCount, evolution.VariableCount);
                var duration = probabilities / (1000f * 60f * 60f * 24f * 365f);
                ViewBag.probabilities = probabilities;
                ViewBag.duration = duration;
            }
            else
            {
                return Redirect("Index");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Evolve(EvolvePost data)
        {
            System.Diagnostics.Debug.WriteLine(data.evolutionId);
            System.Diagnostics.Debug.WriteLine(data);

            var evolution = DatabaseHelpers.GetEvolution(data.evolutionId);

            if (evolution != null)
            {
                evolution.Evolve(random);
                return Json(evolution);
            }

            return Json(new { Success = false });
        }

        [HttpPost]
        public ActionResult EvolveToEnd(EvolvePost data)
        {
            var evolution = DatabaseHelpers.GetEvolution(data.evolutionId);

            if (evolution != null)
            {
                evolution.EvolveToEnd(random);
                return Json(evolution);
            }

            return Json(new { Success = false });
        }

        public ActionResult SerializeJson(object obj)
        {
            var jsonResult = Json(obj, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
    }
}