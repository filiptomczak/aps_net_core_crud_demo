using Contracts;
using Contracts.DTO;
using Entities;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class PersonService_Tests
    {
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;
        public PersonService_Tests(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonService();
            _countryService = new CountryService();
            _testOutputHelper = testOutputHelper;
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

            Assert.Throws<ArgumentException>(() =>
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
        #region GetAll
        [Fact]
        public void GetAll_ReturnEmpty()
        {
            var all = _personService.GetAll();
            Assert.Empty(all);
        }
        [Fact]
        public void GetAll_ReturnCount()
        {
            var request1 = new PersonRequest { Name = "p1", Email = "mail1@mail.com" };
            var request2 = new PersonRequest { Name = "p2", Email = "mail2@mail.com" };
            _personService.AddPerson(request1);
            _personService.AddPerson(request2);
            var expectedCount = 2;
            var countResult = _personService.GetAll().Count();
            Assert.True(expectedCount == countResult);
        }
        [Fact]
        public void GetAll_ContainsObj()
        {
            var request1 = new PersonRequest { Name = "p1", Email = "mail1@mail.com" };
            var request2 = new PersonRequest { Name = "p2", Email = "mail2@mail.com" };
            
            var addedPersons = new List<PersonResponse>();
            addedPersons.Add(_personService.AddPerson(request1));
            addedPersons.Add(_personService.AddPerson(request2));

            foreach(var person in addedPersons)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }
            var result = _personService.GetAll();
            foreach (var res in result)
            {
                Assert.Contains(res, addedPersons);
                _testOutputHelper.WriteLine(res.ToString());
            }
        }
        #endregion
        #region GetFiltered
        [Fact]
        public void GetFiltered_EmptySearch()
        {
            var addedPersons = AddPerson();

            foreach (var person in addedPersons)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }
            var result = _personService.GetFiltered(searchBy:(nameof(Person)),searchString:"");
            foreach (var res in result)
            {
                Assert.Contains(res, addedPersons);
                _testOutputHelper.WriteLine(res.ToString());
            }
        }
        [Fact]
        public void GetFiltered_SearchByPersonName()
        {
            var addedPersons = AddPerson();
            var searchString = "ab";
            var result = _personService.GetFiltered((nameof(Person)),searchString);
            foreach (var res in result)
            {
                if (res.Name != null)
                {
                    if (res.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(res, addedPersons);
                        _testOutputHelper.WriteLine(res.ToString());
                    }
                }
            }
        }
        private IEnumerable<PersonResponse> AddPerson()
        {
            var request1 = new PersonRequest { Name = "pablo", Email = "mail1@mail.com" };
            var request2 = new PersonRequest { Name = "aimar", Email = "mail2@mail.com" };

            var addedPersons = new List<PersonResponse>();
            addedPersons.Add(_personService.AddPerson(request1));
            addedPersons.Add(_personService.AddPerson(request2));

            foreach (var person in addedPersons)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }
            return addedPersons;
        }
        #endregion
    }
}
