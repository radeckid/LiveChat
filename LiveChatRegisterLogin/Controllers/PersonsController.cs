using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveChatRegisterLogin.Data;
using Microsoft.AspNetCore.Mvc;

namespace LiveChatRegisterLogin.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class PersonsController : Controller
    {
        private readonly DataContext _context = DataContext.GetInstance();

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var persons = _context.Persons.ToList();
            return Ok(persons);
        }

        [HttpPost]
        public IActionResult AddPerson([FromBody] Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();

            return Ok(person);
        }

    }
}