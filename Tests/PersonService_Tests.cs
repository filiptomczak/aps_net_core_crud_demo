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
            PersonRequest personRequest1 = new PersonRequest() { Email = "mail@mail.com" };
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
            PersonRequest personRequest = new PersonRequest() { Name = "John", Email = "mail@mail.com" };

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

            foreach (var person in addedPersons)
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
            var result = _personService.GetFiltered(searchBy: (nameof(Person)), searchString: "");
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
            var result = _personService.GetFiltered((nameof(Person)), searchString);
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
        #region GetSorted
        [Fact]
        public void GetSorted_EmptySearch()
        {
            var addedPersons = AddPerson();

            foreach (var person in addedPersons)
            {
                _testOutputHelper.WriteLine(person.ToString());
            }
            var result = _personService.GetFiltered(searchBy: (nameof(Person)), searchString: "");
            foreach (var res in result)
            {
                Assert.Contains(res, addedPersons);
                _testOutputHelper.WriteLine(res.ToString());
            }
        }
        [Fact]
        public void GetSorted_SearchByPersonName()
        {
            var addedPersons = AddPerson();
            var sorted = _personService.GetSorted(addedPersons, nameof(Person.Name), SortOrder.DESC).ToList();

            _testOutputHelper.WriteLine("Actual: ");
            foreach (var sort in sorted)
            {
                _testOutputHelper.WriteLine(sort.ToString());
            }

            var expected = addedPersons.OrderByDescending(p => p.Name).ToList();

            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(sorted[i], expected[i]);
            }
        }
        #endregion
        #region UpdatePerson
        [Fact]
        public void UpdatePerson_ReturnNull()
        {
            PersonUpdate update = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.UpdatePerson(update);
            });
        }
        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            PersonUpdate update = new PersonUpdate() { Id = Guid.NewGuid() };

            Assert.Throws<ArgumentException>(() =>
            {
                _personService.UpdatePerson(update);
            });
        }
        [Fact]
        public void UpdatePerson_NameIsEmpty()
        {
            var personUpdate = GetPersonUpdate();
            personUpdate.Name = null;

            Assert.Throws<ArgumentException>(() =>
            {
                _personService.UpdatePerson(personUpdate);
            });
        }
        [Fact]
        public void UpdatePerson_PersonUpdated()
        {
            var personUpdate = GetPersonUpdate();
            personUpdate.Name = "adam";

            var updated = _personService.UpdatePerson(personUpdate);
            Assert.Equal(personUpdate, updated.ToPersonUpdate());
        }
        private PersonUpdate GetPersonUpdate()
        {
            var country = new CountryRequest() { CountryName = "USA" };
            var responseCountry = _countryService.AddCountry(country);
            var person = new PersonRequest() { Email = "mail@mail.com", Name = "roman", CountryId = responseCountry.CountryId };
            var responsePerson = _personService.AddPerson(person);

            return responsePerson.ToPersonUpdate();
        }
        #endregion
        #region DeletePerson
        [Fact]
        public void DeletePerson_NullId()
        {
            var id = Guid.Empty;
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.DeletePerson(id);
            });
        }
        [Fact]
        public void DeletePerson_ReturnTrue()
        {
            var personToDelete = AddOnePerson();

            var result = _personService.DeletePerson(personToDelete.PersonId);

            Assert.True(result);
        }

        private PersonResponse AddOnePerson()
        {
            var country = new CountryRequest() { CountryName = "USA" };
            var responseCountry = _countryService.AddCountry(country);
            var person = new PersonRequest() { Email = "mail@mail.com", Name = "roman", CountryId = responseCountry.CountryId };
            return _personService.AddPerson(person);

        }
        #endregion
    }
}
