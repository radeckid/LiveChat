using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LiveChatRegisterLogin.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class PersonsController : Controller
    {

        [HttpGet]
        public ActionResult<IEnumerable<Person>> Get()
        {
            return new Person[] {
            new Person(1,"Damian", "Radecki"),
            new Person(2, "Damian", "Kurkiewicz")
            };
        }

    }
}