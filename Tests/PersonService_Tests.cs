using Contracts;
using Contracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class PersonService_Tests
    {
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        public PersonService_Tests()
        {
            _personService = new PersonService();
            _countryService = new CountryService();
        }
        #region AddPerson
        [Fact]
        public void AddPerson_ReturnNull()
        {
            PersonRequest? personRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.AddPerson(personRequest);
            });
        }
        [Fact]
        public void AddPerson_EmailIsNull()
        {
            PersonRequest personRequest = new PersonRequest() { Email = null };
            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(personRequest);
            });
        }
        [Fact]
        public void AddPerson_EmailExists()
        {
            PersonRequest personRequest1 = new PersonRequest() { Email = "mail@mail.com"};
            PersonRequest personRequest2 = new PersonRequest() { Email = "mail@mail.com" };

            _personService.AddPerson(personRequest1);
            Assert.Throws<ArgumentException>(() => 
            {
                _personService.AddPerson(personRequest2);
            });
        }
        [Fact]
        public void AddPerson_CreatesProperId()
        {
            PersonRequest personRequest = new PersonRequest() { Name="John", Email = "mail@mail.com" };

            var result = _personService.AddPerson(personRequest);

            Assert.True(result.PersonId != Guid.Empty);            
        }
        #endregion
        #region GetPersonByPersonId
        [Fact]
        public void GetPerson_ReturnNull()
        {
            var id = Guid.Empty;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.GetPersonByPersonId(id);
            });
        }
        [Fact]
        public void GetPerson_ReturnPerson()
        {
            var countryRequest = new CountryRequest() { CountryName = "USA" };
            var countryResponse = _countryService.AddCountry(countryRequest);
            var request = new PersonRequest() { CountryId = countryResponse.CountryId, Name = "Roman", Email = "mail@mail.pl" };
            var response = _personService.AddPerson(request);
            var expectedResponse = _personService.GetPersonByPersonId(response.PersonId);
            Assert.True(response.PersonId != Guid.Empty);
            Assert.Equal(expectedResponse, response);
        }
        #endregion
    }
}
