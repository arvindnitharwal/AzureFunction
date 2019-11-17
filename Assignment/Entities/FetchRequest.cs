using Assignment.Entities.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
namespace Assignment.Entities {
    public class FetchRequest : CustomerRequest{
        [JsonConverter(typeof(StringEnumConverter))]
        public Filter Filter {get;set;}
        public int Count {get;set;}
        public DateTime EntryDate {get;set;}
    }
}