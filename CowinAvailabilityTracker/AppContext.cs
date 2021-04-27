using CowinAvailabilityTracker.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using static CowinAvailabilityTracker.Constants.Enums;

namespace CowinAvailabilityTracker
{
    public static class AppContext
    {
        public static RunMode runMode;
        public static int districtId;
        public static string district;
        public static int interval;
        public static int WeeksToSearch;
        public static DistrictList districtsList;
        public static VaccineSearchMode VaccineSearchMode;
        public static string chatWebhookUrl;

        static AppContext()
        {

            IConfiguration appConfiguration = new ConfigurationBuilder()
                                              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                              .AddJsonFile("AppSettings.json")
                                              .Build();

            runMode = (RunMode)Enum.Parse(typeof(RunMode), appConfiguration.GetSection("AppSettings")["RunMode"]);
            district = appConfiguration.GetSection("AppSettings")["District"];
            chatWebhookUrl = appConfiguration.GetSection("AppSettings")["ChatWebhookUrl"];
            interval = Convert.ToInt32(appConfiguration.GetSection("AppSettings")["AlertInterval"]);
            districtsList = JsonConvert.DeserializeObject<DistrictList>(File.ReadAllText("Districts.json"));
            VaccineSearchMode = (VaccineSearchMode)Enum.Parse(typeof(VaccineSearchMode), appConfiguration.GetSection("AppSettings")["VaccineSearchMode"].ToUpper());
            WeeksToSearch = Convert.ToInt32(appConfiguration.GetSection("AppSettings")["WeeksToSearch"]);

            var dis = districtsList.Districts.Where(x => String.Equals(x.district_name, district, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (runMode == RunMode.District)
            {
                if (dis == null)
                {
                    throw (new Exception("District Not Found in Kerala"));
                }
                else
                {
                    districtId = dis.district_id;
                }
            }
            else
            {
                WeeksToSearch = 1;
            }
        }
    }
}
