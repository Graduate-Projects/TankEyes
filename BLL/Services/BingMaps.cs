using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public static class BingMaps
    {
        private static string BingMapsKey = "AsDgwZ0hCEmzokF4HHK9P7QAO4v1W1_q__bVahPnKQbml_jV-AUQyYbPtfBZNZuT";
        public static async Task<TimeSpan> CalculateTimeTrevalAsync(BLL.Models.Location Origin, BLL.Models.Location Destination)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var request = new BLL.Models.Resource
                    {
                        destinations = new Models.Location[] { Destination },
                        origins = new Models.Location[] { Origin },
                        timeUnit = "second",
                        travelMode = "driving"
                    };
                    var response = await httpClient.PostAsJsonAsync($"https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?key={BingMapsKey}", request);
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadFromJsonAsync<BLL.Models.DistanceMatrix>();
                    var travelDuration = result.resourceSets.First().resources.First().results.First().travelDuration;
                    return TimeSpan.FromSeconds(travelDuration);
                }
            }
            catch (Exception ex)
            {
                return TimeSpan.FromSeconds(0);
            }
        }
    }
}
