using Newtonsoft.Json;
using System.Collections.Generic;

namespace CowinAvailabilityTracker.Models
{
    public class CenterList
    {
        [JsonProperty("centers")]
        public List<CenterModel> Centers { get; set; }
    }

    public class CenterModel
    {
        public int center_id { get; set; }
        public string name { get; set; }
        public string state_name { get; set; }
        public string district_name { get; set; }
        public string block_name { get; set; }
        public int pincode { get; set; }
        public string fee_type { get; set; }
        public List<session> sessions { get; set; }
    }

    public class session
    {
        public string session_id { get; set; }
        public string date { get; set; }
        public float available_capacity { get; set; }
        public List<string> slots { get; set; }
    }
}
