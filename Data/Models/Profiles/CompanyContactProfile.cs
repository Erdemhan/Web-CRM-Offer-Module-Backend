using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using crmweb.Data.Entities;
using crmweb.Models.CompanyContactModels;

namespace crmweb.Models.Profiles
{
    public class CompanyContactProfile : Profile

    {
        public CompanyContactProfile()
        {
            CreateMap<CompanyContact, CompanyContactInfo>()
                .ReverseMap();
            CreateMap<CompanyContact, CompanyContactRequestInfo>()
                .ReverseMap();
        }
    }
    
}
