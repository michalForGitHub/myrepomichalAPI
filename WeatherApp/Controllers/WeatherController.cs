using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;
using System.Configuration;
using Newtonsoft.Json;
using WeatherApp.Models;
using System.Web.Http.Cors;
//using System.Web.Http.Cors;


namespace WeatherApp.Controllers
{
    [RoutePrefix("Api/Weather")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class WeatherController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> GetCitiesWeather(List<WeatherRequest> weatherReq)
        {
            string APIKey = ConfigurationManager.AppSettings["APIKey"];
            string WeatherAPIPath = ConfigurationManager.AppSettings["OpenWeatherAPI"];
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(WeatherAPIPath);
                    List<object> WeatherDataResponse = new List<object>();
                    foreach (WeatherRequest req in weatherReq)
                    {
                        var response = await client.GetAsync($"/data/2.5/weather?" + buildRestQuery(req, APIKey));
                        response.EnsureSuccessStatusCode();
                        var stringResult = await response.Content.ReadAsStringAsync();
                        var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(stringResult);
                        WeatherDataResponse.Add(new
                        {
                            Temp = weatherData.Main.Temp,
                            Pressure = weatherData.Main.Pressure,
                            Humidity = weatherData.Main.Humidity,
                            Summary = string.Join(",", weatherData.Weather.Select(x => x.Main)),
                            SummaryDescription = string.Join(",", weatherData.Weather.Select(x => x.Description)),
                            Icon= weatherData.Weather.Select(x=>x.Icon),
                            City = weatherData.Name,
                            Timezone = weatherData.Timezone > 0 ? "+" + (weatherData.Timezone / 3600).ToString() : (weatherData.Timezone / 3600).ToString(),
                            MoreDetails = weatherData.Wind.Speed + "," + weatherData.Sys.Country

                        });
                    }
                    return Ok(WeatherDataResponse.AsEnumerable());
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather for specific location: {httpRequestException.Message}");
                }
            }
            return null;
        }

        public string buildRestQuery(WeatherRequest req, string APIKey)
        {
            string url = string.Empty;
            switch (req.Type)
            {
                case "city":
                    url = "q=" + req.Value + "&appid=" + APIKey;
                    break;

                case "geographicCoordinates":
                    url = "lat=" + req.Value.Split(',')[0] + "&lon=" + req.Value.Split(',')[1] + "&appid=" + APIKey;
                    break;

                default:

                    break;
            }
            return url;
        }
    }
}
