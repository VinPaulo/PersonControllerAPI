using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Person.Models
{
    public class PersonModel
    {
        public PersonModel(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
        public Guid Id { get; init; }

        public string Name { get; private set; }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void SetInactive()
        {
            Name = "desativado";
        }
    }
}