using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO
{
    public class PersonUpdate
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public Guid? CountryId { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var comparePerson = obj as PersonUpdate;

            return this.Name == comparePerson.Name &&
                this.Id == comparePerson.Id;
        }
        public Person ToPerson()
        {
            return new Person()
            {
                PersonId = this.Id,
                Name = this.Name,
                Email = this.Email,
                CountryId = this.CountryId
            };
        }
    }
}
