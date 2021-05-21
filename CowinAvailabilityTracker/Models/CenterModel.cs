using Newtonsoft.Json;
using System.Collections.Generic;

namespace CowinAvailabilityTracker.Models
{
    public class CenterList
    {
        public List<CenterModel> Centers { get; set; }
    }

    public class CenterModel
    {
        public int Center_id { get; set; }
        public string Name { get; set; }
        public string State_name { get; set; }
        public string District_name { get; set; }
        public string Block_name { get; set; }
        public int Pincode { get; set; }
        public string Fee_type { get; set; }
        public List<Session> Sessions { get; set; }
    }

    public class Session
    {
        public string Session_id { get; set; }
        public string Date { get; set; }
        public float Available_capacity { get; set; }
        public int Min_age_limit { get; set; }
        public string Vaccine { get; set; }
        public List<string> Slots { get; set; }
        public float Available_capacity_dose1 { get; set; }
        public float Available_capacity_dose2 { get; set; }
    }
}
