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
        public IActionResult IsUserExist([FromBody] Person person)
        {
            foreach (Person searchP in _context.Persons)
            {
                if (person != null && searchP.Email == person.Email && searchP.Password == person.Password)
                {
                    return Ok("User found");
                }
            }

            return NotFound("No user found");
        }


        [HttpPost]
        public IActionResult AddPerson([FromBody] Person person)
        {
            foreach (Person searchP in _context.Persons)
            {
                if (person != null && searchP.Email == person.Email)
                {
                    return NotFound("User already exist");
                }
            }
            
            _context.Persons.Add(person);
            _context.SaveChanges();

            return Ok(person);
        }

    }
}