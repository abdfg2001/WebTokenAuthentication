using ConsumeTokenApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsumeTokenApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login()
        {
            string baseUrl = "http://localhost:44313";
            HttpClient client = new();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            LoginData userModel = new()
            {
                Username = "MyFortune@gmail.com",
                Password = "Administrator"
            };

            string stringData = JsonConvert.SerializeObject(userModel);
            var contentData = new StringContent(stringData,
            System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("https://localhost:44363/weatherforecast/Token", contentData).Result;
            string stringJWT = response.Content.ReadAsStringAsync().Result;
            JWT jwt = JsonConvert.DeserializeObject<JWT>(stringJWT);
            HttpContext.Session.SetString("token", jwt.Token);
            ViewBag.Data = stringJWT;
            return View("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            ViewBag.Data = "User logged out successfully!";
            return View("Index");
        }

        public IActionResult SeeData()
        {
            string baseUrl = "http://localhost:44313";
            HttpClient client = new()
            {
                BaseAddress = new Uri(baseUrl)
            };
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",HttpContext.Session.GetString("token"));
            HttpResponseMessage response = client.GetAsync("https://localhost:44363/weatherforecast/Data").Result;
            string stringData = response.Content.ReadAsStringAsync().Result;
         
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ViewBag.Data = "Unauthorized!";
            }
            else
            {
                ViewBag.Data = stringData;
            }

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
