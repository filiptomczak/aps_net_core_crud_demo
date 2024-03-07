using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO
{
    public class PersonRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid? CountryId { get; set; }
        public Person ToPerson()
        {
            return new Person()
            {
                Name = this.Name,
                Email = this.Email,
                CountryId = this.CountryId,
            };
        }
    }
}
