using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class CountryRequest
    {
        public string? CountryName { get; set; }
        public Guid? CountryId { get; set; }
        public Country ToCountry()
        {
            return new Country() { CountryName = CountryName };
        }
    }
}
