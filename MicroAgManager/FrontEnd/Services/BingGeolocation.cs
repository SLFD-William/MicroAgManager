    using System.Text.Json.Serialization;

    namespace FrontEnd.Services
    {
    //I need a class to deserialize a BingLocationResponse JSON
    public class BingLocationResponse
    {
        [JsonPropertyName("authenticationResultCode")]
        public string AuthenticationResultCode { get; set; }

        [JsonPropertyName("brandLogoUri")]
        public string BrandLogoUri { get; set; }

        [JsonPropertyName("copyright")]
        public string Copyright { get; set; }

        [JsonPropertyName("resourceSets")]
        public ResourceSet[] ResourceSets { get; set; }

        [JsonPropertyName("statusCode")]
        public long StatusCode { get; set; }

        [JsonPropertyName("statusDescription")]
        public string StatusDescription { get; set; }

        [JsonPropertyName("traceId")]
        public string TraceId { get; set; }
    }
    //I need a class to deserialize a ResourceSet JSON
    public class ResourceSet
    {
        [JsonPropertyName("estimatedTotal")]
        public long EstimatedTotal { get; set; }

        [JsonPropertyName("resources")]
        public Resource[] Resources { get; set; }
    }
    //I need a class to deserialize a Resource JSON
    public class Resource
    {
        [JsonPropertyName("__type")]
        public string Type { get; set; }

        [JsonPropertyName("bbox")]
        public double[] Bbox { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("point")]
        public Point Point { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonPropertyName("confidence")]
        public string Confidence { get; set; }

        [JsonPropertyName("entityType")]
        public string EntityType { get; set; }

        [JsonPropertyName("geocodePoints")]
        public GeocodePoint[] GeocodePoints { get; set; }

        [JsonPropertyName("matchCodes")]
        public string[] MatchCodes { get; set; }
    }
    //I need a class to deserialize a Point JSON
    public class Point
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; set; }
    }
    //I need a class to deserialize a Address JSON
    public class Address
    {
        [JsonPropertyName("addressLine")]
        public string AddressLine { get; set; }

        [JsonPropertyName("adminDistrict")]
        public string AdminDistrict { get; set; }

        [JsonPropertyName("adminDistrict2")]
        public string AdminDistrict2 { get; set; }

        [JsonPropertyName("countryRegion")]
        public string CountryRegion { get; set; }

        [JsonPropertyName("formattedAddress")]
        public string FormattedAddress { get; set; }

        [JsonPropertyName("locality")]
        public string Locality { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }
    }
    //I need a class to deserialize a GeocodePoint JSON
    public class GeocodePoint
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; set; }

        [JsonPropertyName("calculationMethod")]
        public string CalculationMethod { get; set; }

        [JsonPropertyName("usageTypes")]
        public string[] UsageTypes { get; set; }
    }
    }

