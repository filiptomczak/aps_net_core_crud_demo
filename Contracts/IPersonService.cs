using Contracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPersonService
    {
        public PersonResponse AddPerson(PersonRequest? person);
        public PersonResponse GetPersonByPersonId(Guid? id);
        public IEnumerable<PersonResponse> GetAll();
        public IEnumerable<PersonResponse> GetFiltered(string searchBy, string? searchString);
    }
}
