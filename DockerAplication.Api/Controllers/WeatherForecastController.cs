using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace DockerAplication.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private IConfiguration _config;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("[action]")]
        public IActionResult RetornarUsuarios()
        {
            var lista = new List<object>();
            string connection = _config.GetConnectionString("connection");
            using (var conn = new MySqlConnection(connection))
            {                
                string sql = "select nome,ambiente from usuario";
                using (var command = new MySqlCommand(sql, conn))
                {
                    command.CommandTimeout = 0;
                    conn.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var usuario = new
                            {
                                Nome = reader["nome"].ToString(),
                                Ambiente = reader["ambiente"].ToString()
                            };
                            lista.Add(usuario);
                        }
                    }
                    return Ok(lista);
                }
            }
        }
    }
}