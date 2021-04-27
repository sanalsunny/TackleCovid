using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CowinAvailabilityTracker.Models
{
    public class DistrictList
    {
        [JsonProperty("districts")]
        public List<DistrictModel> Districts { get; set; }

        internal int Select()
        {
            throw new NotImplementedException();
        }
    }

    public class DistrictModel
    {
        public int district_id;
        public string district_name;
    }

}
