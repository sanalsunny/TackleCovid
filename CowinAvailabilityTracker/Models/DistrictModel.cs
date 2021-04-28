using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CowinAvailabilityTracker.Models
{
    public class DistrictList
    {
        public List<DistrictModel> Districts { get; set; }
    }

    public class DistrictModel
    {
        public int District_id { get; set; }
        public string District_name { get; set; }
    }

}
