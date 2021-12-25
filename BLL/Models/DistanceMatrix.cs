using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class DistanceMatrix
    {
        public string authenticationResultCode { get; set; }
        public string brandLogoUri { get; set; }
        public string copyright { get; set; }
        public Resourceset[] resourceSets { get; set; }
        public int statusCode { get; set; }
        public string statusDescription { get; set; }
        public string traceId { get; set; }

    }

    public class Resourceset
    {
        public int estimatedTotal { get; set; }
        public Resource[] resources { get; set; }
    }

    public class Resource
    {
        public string __type { get; set; }
        public Location[] destinations { get; set; }
        public Location[] origins { get; set; }
        public Result[] results { get; set; }
        public string travelMode { get; set; }
        public string timeUnit { get; set; }
    }

    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Result
    {
        public int destinationIndex { get; set; }
        public int originIndex { get; set; }
        public int totalWalkDuration { get; set; }
        public float travelDistance { get; set; }
        public float travelDuration { get; set; }
    }

}
