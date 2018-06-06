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
                ViewBag.CurrentTemp = JsonData["currentobservation"]["Weather"];
                ViewBag.CurrentText = JsonData["currentobservation"]["Temp"];
                ViewBag.CurrentDate = JsonData["currentobservation"]["Date"];
                ViewBag.Times = JsonData["time"]["startPeriodName"];
                ViewBag.Temps = JsonData["data"]["temperature"];
                ViewBag.Icons = JsonData["data"]["iconLink"];
                ViewBag.Descs = JsonData["data"]["weather"];
                ViewBag.Pop = JsonData["data"]["pop"];
                ViewBag.Texts = JsonData["data"]["text"];
                ViewBag.Visibility = JsonData["currentobservation"]["Visibility"];
                ViewBag.Latitude = JsonData["currentobservation"]["latitude"];
                ViewBag.Timezone = JsonData["currentobservation"]["timezone"];
                ViewBag.Winds = JsonData["currentobservation"]["Winds"];

                //So that null POPs don't show up empty
                for (int i = 0; i < ViewBag.Times.Count; i++)
                {
                    if (ViewBag.Pop[i] == null)
                    {
                        ViewBag.Pop[i] = "0";
                    }
                }
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