using Contracts;
using Contracts.DTO;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly List<Country> _countries;
        public CountryService()
        {
            _countries = new List<Country>();
        }
        public CountryResponse AddCountry(CountryRequest? request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (request.CountryName == null)
            {
                throw new ArgumentException(nameof(request.CountryName));
            }
            var nameAlreadyExist = _countries.SingleOrDefault(c => c.CountryName == request.CountryName);
            if (nameAlreadyExist != null)
            {
                throw new ArgumentException("Country already exists");
            }
            var country = request.ToCountry();
            country.CountryId = Guid.NewGuid();
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public IEnumerable<CountryResponse> GetAllCountries()
        {            
            return _countries.Select(c => c.ToCountryResponse());
        }

        public CountryResponse? GetCountryById(Guid? guid)
        {
            if (guid == null)
                return null;
            return _countries.SingleOrDefault(c => c.CountryId == guid)?.ToCountryResponse();
        }
    }
}
