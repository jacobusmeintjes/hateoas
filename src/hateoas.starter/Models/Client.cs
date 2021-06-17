using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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

    public class ClientListFactory
    {
        private readonly LinkGenerator _linkGenerator;

        public ClientListFactory(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }
        public Client AddLinksToClient(HttpContext httpContext, Client client)
        {
            var getLink = _linkGenerator.GetUriByAction(httpContext, "GetClientById", values: new {client.Id});
            var get = new Link(getLink, "self", "GET");
            client.Links.Add(get);

            var deleteLink = _linkGenerator.GetUriByAction(httpContext, "DeleteClient", values: new {client.Id});
            var delete = new Link(deleteLink, "delete_client", "DELETE");
            client.Links.Add(delete);

            var updateLink = _linkGenerator.GetUriByAction(httpContext, "UpdateClient", values: new {client.Id});
            var update = new Link(updateLink, "update_client", "PUT");
            client.Links.Add(update);
            
            var updatePartialLink = _linkGenerator.GetUriByAction(httpContext, "UpdatePartialClient", values: new {client.Id});
            var updatePartial = new Link(updatePartialLink, "partial_update_client", "PATCH");
            client.Links.Add(updatePartial);

            return client;
        }
    }
}