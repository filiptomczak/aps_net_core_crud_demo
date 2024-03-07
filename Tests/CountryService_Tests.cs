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
    public class CountryService_Tests
    {
        private readonly ICountryService _countryService;
        public CountryService_Tests()
        {
            _countryService = new CountryService();
        }
        #region AddCountry
        [Fact]
        public void AddCountry_ReturnNull()
        {
            //arrange
            CountryRequest? request = null;
            
            //assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //act
                _countryService.AddCountry(request);
            });
        }
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            CountryRequest request = new CountryRequest() { CountryName = null };
            Assert.Throws<ArgumentException>(() =>
            {
                _countryService.AddCountry(request);
            });
        }
        [Fact]
        public void AddCountry_CountryNameAlreadyExists()
        {
            CountryRequest request1 = new CountryRequest() { CountryName = "UK" };
            CountryRequest request2 = new CountryRequest() { CountryName = "UK" };
            _countryService.AddCountry(request1);
            Assert.Throws<ArgumentException>(() =>
            {
                _countryService.AddCountry(request2);
            });
        }
        [Fact]
        public void AddCountry_CreatesProperId()
        {
            CountryRequest request = new CountryRequest() { CountryName = "UK" };
            CountryResponse response = _countryService.AddCountry(request);
            Assert.True(response.CountryId != Guid.Empty);

        }
        #endregion
        #region GetAllCountries
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            var resultList = _countryService.GetAllCountries();

            Assert.Empty(resultList);
        }
        [Fact]
        public void GetAllCountries_GetProperCount()
        {
            var countryRequestList = new List<CountryRequest>() {
                new CountryRequest(){CountryName="USA"},
                new CountryRequest(){CountryName="POLAND"},
                new CountryRequest(){CountryName="UK"},
            };
            var countryResponseList = new List<CountryResponse>();
            foreach(var countryRequest in countryRequestList)
            {
                countryResponseList.Add(_countryService.AddCountry(countryRequest));
            }

            var resultList = _countryService.GetAllCountries();

            foreach(var expectedCountry in countryResponseList)
            {
                Assert.Contains(expectedCountry, resultList);
            }
        }
        #endregion
        #region GetCountryById
        [Fact]
        public void GetCountryById_ReturnNull()
        {
            Guid? CountryId = null;
            var result = _countryService.GetCountryById(CountryId);
            Assert.Null(result);
        }
        [Fact]
        public void GetCountryById_ReturnProperCountry()
        {
            var requestCountry = new CountryRequest() { CountryName = "USA" };
            var responseCountry = _countryService.AddCountry(requestCountry);

            var resultCountry = _countryService.GetCountryById(responseCountry.CountryId);

            Assert.Equal(resultCountry, responseCountry);            
        }
        #endregion
    }
}
