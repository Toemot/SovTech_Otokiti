using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SovTechAPITest1.Models;

namespace SovTechAPITest1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        private readonly IConfiguration _configuration;

        public ApiController(ILogger<ApiController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/index")]
        public IActionResult Index()
        {
            return Ok("Api Started");
        }

        [HttpGet]
        [Route("Search/{id}")]
        public IActionResult Search(int id)
        {
            var chuckUrl = $"{_configuration["ChuckCategoriesApi"]}";
            var swapiUrl = $"{_configuration["SwapiPeopleApi"]}";

            var response = new SearchModel
            {
                ChuckModel = GetAsync(chuckUrl).GetAwaiter().GetResult(),
                SwapiModel = GetAsync(swapiUrl).GetAwaiter().GetResult(),
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("chuck/categories")]
        public IActionResult Chuck()
        {
            var url = $"{_configuration["ChuckApi"]}/categories";
            var client = GetAsync(url).GetAwaiter().GetResult();

            return Ok(client);
        }

        [HttpGet]
        [Route("swapi/people")]
        public IActionResult Swapi()
        {
            var url = $"{_configuration["SwapiApi"]}";
            var client = GetAsync(url).GetAwaiter().GetResult();

            return Ok(client);
        }

        public static async Task<string> GetAsync(string uri)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(uri);

            string content = await response.Content.ReadAsStringAsync();
            return await Task.Run(() => (content));
        }

    }
}
