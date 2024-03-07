﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTO
{
    public class CountryResponse
    {
        public Guid? CountryId { get; set; }
        public string? CountryName { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(CountryResponse))
                return false;
            var countryToCompare = obj as CountryResponse;
            return this.CountryId == countryToCompare.CountryId &&
                this.CountryName == countryToCompare.CountryName;
        }
    }
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName,
            };
        } 

    }
}
