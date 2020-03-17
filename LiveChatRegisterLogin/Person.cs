using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin
{
    public class Person
    {
        public int id {get; set;}

        public string Name { get; set; }

        public string Surname { get; set; }

        public Person(int id, string name, string surname)
        {
            this.id = id;
            Name = name;
            Surname = surname;
        }
    }
}
