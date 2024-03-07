using Contracts.DTO;
using System;
using System.Collections.Generic;

namespace Contracts
{
    public interface ICountryService
    {
        public CountryResponse AddCountry(CountryRequest? countryRequest);
        public IEnumerable<CountryResponse> GetAllCountries();
        public CountryResponse? GetCountryById(Guid? guid);
    }
}
