using Contracts;
using Contracts.DTO;
using Entities;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly List<Person> _persons;
        private readonly ICountryService _countryService;
        public PersonService()
        {
            _persons = new List<Person>();
            _countryService = new CountryService();
        }
        public PersonResponse AddPerson(PersonRequest? request)
        {
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ValidationHelper.ModelValidation(request);
            /*if(string.IsNullOrEmpty(request.Email))
            {
                throw new ArgumentException(nameof(request.Email));
            }*/
            var emailAlreadyExist = _persons.SingleOrDefault(p => p.Email == request.Email);
            if (emailAlreadyExist != null)
            {
                throw new ArgumentException("Email already exists");
            }
            var person = request.ToPerson();
            person.PersonId = Guid.NewGuid();
            _persons.Add(person);
            return ConvertToPersonResponse(person);
        }

        public PersonResponse GetPersonByPersonId(Guid? id)
        {
            if(id == null)
            {
                throw new ArgumentNullException();
            }
            if (id == Guid.Empty)
            {
                throw new ArgumentException();
            }
            var person = _persons.SingleOrDefault(p => p.PersonId == id);
            if (person == null)
            {
                throw new NullReferenceException();
            }
            return ConvertToPersonResponse(person);
        }

        private PersonResponse ConvertToPersonResponse(Person person)
        {
            var response = person.ToPersonResponse();
            response.Country = _countryService.GetCountryById(person.CountryId).CountryName;
            return response;
        }
    }
}
