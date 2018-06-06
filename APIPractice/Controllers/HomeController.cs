using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace APIPractice.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HttpWebRequest WR = WebRequest.CreateHttp("https://forecast.weather.gov/MapClick.php?lat=42.335690&lon=-83.049821&FcstType=json");
            WR.UserAgent = ".NET Framework Test Client";

            HttpWebResponse Response;

            try
            {
                Response = (HttpWebResponse)WR.GetResponse();
            }
            catch (WebException e)
            {
                ViewBag.Error = "Exception";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }

            if (Response.StatusCode != HttpStatusCode.OK)
            {
                ViewBag.Error = Response.StatusCode;
                ViewBag.ErrorDescription = Response.StatusDescription;
                return View();
            }

            StreamReader reader = new StreamReader(Response.GetResponseStream());
            string WeatherData = reader.ReadToEnd();

            try
            {
                JObject JsonData = JObject.Parse(WeatherData);
                ViewBag.Times = JsonData["time"]["startPeriodName"];
                ViewBag.Temps = JsonData["data"]["temperature"];
                ViewBag.Icons = JsonData["data"]["iconLink"];
                ViewBag.Descs = JsonData["data"]["weather"];
                ViewBag.Texts = JsonData["data"]["text"];
            }
            catch (Exception e)
            {
                ViewBag.Error = "JSON Issue";
                ViewBag.ErrorDescription = e.Message;
                return View();
            }

            return View();
        }


    }
}