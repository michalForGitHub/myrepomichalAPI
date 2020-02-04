using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherApp.Models
{
    public class WeatherResponse
    {

        public string Name { get; set; }
        public int Timezone { get; set; }
        public IEnumerable<WeatherDescription> Weather { get; set; }

        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public Sys Sys { get; set; }
    }

    public class WeatherDescription
    {
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class Main
    {
        public string Temp { get; set; }
        public string Pressure { get; set; }
        public string Humidity { get; set; }

    }
    public class Wind
    {

        public string Speed { get; set; }
        public string Deg { get; set; }
    }

    public class Sys
    {

        public string Country { get; set; }

    }
    public class More
    {
        public Wind Wind { get; set; }
        public Sys Sys { get; set; }
      

    }
}