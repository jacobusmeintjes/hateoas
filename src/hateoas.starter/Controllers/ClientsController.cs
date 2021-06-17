using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using hateoas.starter.Helpers;
using hateoas.starter.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace hateoas.starter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : Controller
    {
        private ILogger<ClientsController> _logger;
        private readonly ClientListFactory _factory;

        private static List<Client> _clients = JsonConvert.DeserializeObject<List<Client>>(Globals.Clients);

        public ClientsController(ILogger<ClientsController> logger, ClientListFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }


        [HttpGet]
        public async Task<IActionResult> GetClients(CancellationToken cancellationToken)
        {
            foreach (var client in _clients)
            {
                _factory.AddLinksToClient(HttpContext, client);
            }

            return Ok(_clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(string id, CancellationToken cancellationToken)
        {
            var client = _clients.FirstOrDefault(_ => _.Id == id);


            _factory.AddLinksToClient(HttpContext, client);

            return Ok(client);
        }

        [HttpPost("{firstname}/{lastname}/{email}/{gender}")]
        public async Task<IActionResult> AddClientFromParams(string firstname, string lastname, string email,
            string gender, CancellationToken cancellationToken)
        {
            var client = new Client
            {
                Id = Guid.NewGuid().ToString(), FirstName = firstname, LastName = lastname, Email = email,
                Gender = gender
            };
            _clients.Add(client);

            _factory.AddLinksToClient(HttpContext, client);

            return CreatedAtAction("GetClientById", new {id = client.Id}, client);
        }


        [HttpPost]
        public async Task<IActionResult> AddClientFromBody([FromBody] Client client,
            CancellationToken cancellationToken)
        {
            _clients.Add(client);

            return CreatedAtRoute(nameof(GetClientById), new {id = client.Id});
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClient(string id, CancellationToken cancellationToken)
        {
            _clients.Remove(_clients.FirstOrDefault(_ => _.Id == id));

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient(string id, string firstname, string lastname, string email,
            string gender, CancellationToken cancellationToken)
        {
            var client = _clients.FirstOrDefault(_ => _.Id == id);

            if (!string.IsNullOrEmpty(firstname))
            {
                client.FirstName = firstname;
            }

            if (!string.IsNullOrEmpty(lastname))
            {
                client.LastName = lastname;
            }

            if (!string.IsNullOrEmpty(email))
            {
                client.Email = email;
            }

            if (!string.IsNullOrEmpty(gender))
            {
                client.Gender = gender;
            }

            foreach (var cl in _clients)
            {
                _factory.AddLinksToClient(HttpContext, cl);
            }

            return Ok(_clients);
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc">
        /// [
        /// {
        ///     "value": "Friday the 13th",
        ///     "path": "/first_name",
        ///     "op": "replace"
        /// },
        /// {
        ///     "value": "Da Rule",
        ///     "path": "/last_name",
        ///     "op": "replace"
        /// }
        /// ]
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> UpdatePartialClient(string id, [FromBody] JsonPatchDocument<Client> patchDoc,
            CancellationToken cancellationToken)
        {
            var client = _clients.FirstOrDefault(_ => _.Id == id);
            patchDoc.ApplyTo(client);

            return Ok(client);
        }
    }
}