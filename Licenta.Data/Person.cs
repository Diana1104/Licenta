using Licenta.ORM;
using System;

namespace Licenta.Data
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Encrypted]
        public string CardNo { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
