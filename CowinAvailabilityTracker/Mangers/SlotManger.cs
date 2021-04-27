using CowinAvailabilityTracker.Constants;
using CowinAvailabilityTracker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static CowinAvailabilityTracker.Constants.Enums;

namespace CowinAvailabilityTracker.Mangers
{

    public class SlotManger
    {
        public static IDictionary<string, string> centers = new Dictionary<string, string>();

        public async Task FindSlot()
        {
            switch (AppContext.VaccineSearchMode)
            {
                case VaccineSearchMode.DEFAULT:
                    await SearchSlot(null);
                    break;

                case VaccineSearchMode.COVAXIN:
                    Console.WriteLine(" Searching for COVAXIN");
                    await SearchSlot("COVAXIN");
                    break;

                case VaccineSearchMode.COVISHIELD:
                    Console.WriteLine(" Searching for COVISHIELD");
                    await SearchSlot("COVISHIELD");
                    break;

                case VaccineSearchMode.ALL:
                    Console.WriteLine(" Searching for COVAXIN");
                    await SearchSlot("COVAXIN");
                    Console.WriteLine(" Searching for COVISHIELD");
                    await SearchSlot("COVISHIELD");
                    break;
            }
        }

        public async Task SearchSlot(string vaccine)
        {
            switch (AppContext.runMode)
            {
                case RunMode.RunAllForOnce:
                    await RunAllDistricts(vaccine);
                    break;

                case RunMode.District:
                    await GetSpecifiedWeeks(AppContext.districtId, AppContext.district, vaccine);
                    break;
            }
        }

        public async Task RunAllDistricts(string vaccine)
        {
            foreach (var district in AppContext.districtsList.Districts)
            {
                await GetSpecifiedWeeks(district.district_id, district.district_name, vaccine);
            }
        }

        public async Task GetSpecifiedWeeks(int districtId, string districtName, string vaccine)
        {
            int dateInterval = 7;
            int i = 0;
            DateTime date = DateTime.Now.AddDays(1);

            while (AppContext.WeeksToSearch > i)
            {
                var dateToProcess = date.ToString("dd-MM-yyyy");
                await ProcessDistrict(districtId, districtName, vaccine, dateToProcess);
                date = date.AddDays(dateInterval);
                i++;
            }
        }

        public async Task ProcessDistrict(int districtId, string districtName, string vaccine, string date = null)
        {

            date ??= DateTime.Now.AddDays(1).ToString("dd-MM-yyyy");
            var enddate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddDays(7).ToString("dd-MM-yyyy");

            var vaccinekey = vaccine != null ? $"&vaccine={vaccine}" : null;
            var vaccineMessage = vaccine != null ? $"for {vaccine}" : null;

            var apiURI = $"{URIConstant.CalendarByDistrict}?district_id={districtId}&date={date}{vaccinekey}";
            try
            {
                var str = await GetSlotByCenter(apiURI);

                if (str.Centers.Count == 0)
                {
                    Console.WriteLine($"No Centers were found in {districtName} form {date} to {enddate}{vaccineMessage}");
                }
                else
                {
                    Console.WriteLine($"Centers were found in {districtName} form {date} to {enddate}{vaccineMessage}");

                    foreach (var center in str.Centers)
                    {
                        var slots = string.Join(Environment.NewLine, center.sessions.Select(x => $" {x.available_capacity} slots available on {x.date}{vaccineMessage}"));
                        var slotMessage = $"{center.fee_type} vaccination at {center.name} located in {center.district_name} district with pincode {center.pincode}"
                            + Environment.NewLine + slots + Environment.NewLine;

                        Console.WriteLine(slotMessage);
                        await CheckAndPost(date, center.center_id, slotMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Occured in { districtName} {vaccineMessage} with following exception {ex.Message} " +
                    $"{Environment.NewLine} for URL {apiURI}");
                return;
            }
        }

        public async Task<CenterList> GetSlotByCenter(string uri)
        {
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(uri);
            return JsonConvert.DeserializeObject<CenterList>(content);
        }

        public async Task CheckAndPost(string date, int centerName, string slotMessage)
        {
            var dictKey = string.Concat(date, '#', centerName);
            if (centers.ContainsKey(dictKey))
            {
                string storedMessage = centers[dictKey];
                if (storedMessage == slotMessage)
                {
                    return;
                }
                else
                {
                    centers[dictKey] = slotMessage;
                    await ChatManager.PostToChatAsync(slotMessage);
                }
            }
            else
            {
                centers.Add(dictKey, slotMessage);
                await ChatManager.PostToChatAsync(slotMessage, true);
            }
        }
    }
}
