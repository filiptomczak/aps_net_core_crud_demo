using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string Name { get; set; }
        public Guid? CountryId{ get; set; }
        public string Country{ get; set; }
        public string Email{ get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var comparePerson = obj as PersonResponse;

            return this.Name == comparePerson.Name &&
                this.PersonId == comparePerson.PersonId;
        }

        public PersonUpdate ToPersonUpdate()
        {
            return new PersonUpdate()
            {
                Id = PersonId,
                Name = Name,
                CountryId = CountryId,
                Email = Email,
            };
        }

        public override string ToString()
        {
            return $"Person: {this.Name}, {this.PersonId}";
        }
    }
    public static class PersonExtension
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                Name = person.Name,
                CountryId = person.CountryId,
                Email = person.Email,
            };
        }
    }
}