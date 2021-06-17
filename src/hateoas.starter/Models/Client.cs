using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace hateoas.starter.Models
{
    public class Client
    {
        public string Id { get; set; }
        
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        
        public string Email { get; set; }
        

        public string Gender { get; set; }

        public List<Link> Links { get; set; } = new List<Link>();
        
        
        
    }
}