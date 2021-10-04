using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistence;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public readonly DataBaseContext context;
        public WeatherForecastController(DataBaseContext _context){
            this.context = _context;
        }

        /*
        [HttpGet]
        public IEnumerable<User> Get(){
            return context.Curso.ToList();
        }
        */
    }
}
