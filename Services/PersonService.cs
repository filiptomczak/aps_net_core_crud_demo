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

            //ValidationHelper.ModelValidation(request);
            if(string.IsNullOrEmpty(request.Email))
            {
                throw new ArgumentException(nameof(request.Email));
            }
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

        public bool DeletePerson(Guid? id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            var personToDelete = _persons.SingleOrDefault(p => p.PersonId == id);

            if(personToDelete == null)
            {
                return false;
            }

            _persons.Remove(personToDelete);
            return true;
        }

        public IEnumerable<PersonResponse> GetAll()
        {
            return _persons.Select(p => ConvertToPersonResponse(p));
        }

        public IEnumerable<PersonResponse> GetFiltered(string searchBy, string searchString)
        {
            var filteredPersons = new List<PersonResponse>();
            var persons = GetAll().ToList();

            if (string.IsNullOrEmpty(searchBy))
                return persons;


            switch (searchBy)
            {
                case nameof(Person.Name):
                    filteredPersons = persons.Where(p =>
                    string.IsNullOrEmpty(searchString) ?
                    true :
                    p.Name.Contains(searchString)).ToList();
                    break;

                case nameof(Person.Email):
                    filteredPersons = persons.Where(p =>
                    string.IsNullOrEmpty(searchString) ?
                    true :
                    p.Email.Contains(searchString)).ToList();
                    break;

                default:
                    break;
            }
            return filteredPersons;
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

        public IEnumerable<PersonResponse> GetSorted(IEnumerable<PersonResponse> allPersons, string sortBy, SortOrder sortOrder)
        {
            var sortedPersons = new List<PersonResponse>();
            var persons = GetAll().ToList();

            if (string.IsNullOrEmpty(sortBy))
                return persons;


            switch (sortBy)
            {
                case nameof(Person.Name):
                    sortedPersons = sortOrder == SortOrder.ASC ?
                        persons.OrderBy(p => p.Name).ToList() :
                        persons.OrderByDescending(p => p.Name).ToList();
                    break;

                case nameof(Person.Email):
                    sortedPersons = sortOrder == SortOrder.ASC ?
                        persons.OrderBy(p => p.Email).ToList() :
                        persons.OrderByDescending(p => p.Email).ToList();
                    break;

                default:
                    break;
            }
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdate? personUpdate)
        {
            if(personUpdate == null)
            {
                throw new ArgumentNullException();
            }
            if(personUpdate.Name==null || personUpdate.Email == null)
            {
                throw new ArgumentException();
            }
            ValidationHelper.ModelValidation(personUpdate);

            var matchPerson = _persons.SingleOrDefault(p => p.PersonId == personUpdate.Id);
            if(matchPerson == null)
            {
                throw new ArgumentNullException();
            }

            matchPerson = personUpdate.ToPerson();
            
            return matchPerson.ToPersonResponse();
        }

        private PersonResponse ConvertToPersonResponse(Person person)
        {
            var response = person.ToPersonResponse();
            response.Country = _countryService.GetCountryById(person.CountryId)?.CountryName;
            return response;
        }
    }
}
