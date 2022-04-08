using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using crmweb.Data.Entities;
using crmweb.Models.CompanyContactModels;
using crmweb.Models.CompanyModels;

namespace crmweb.Models.Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyInfo>()
                .ReverseMap();
            CreateMap<Company, CompanyItem>()
                .ReverseMap();
            CreateMap<Company, CompanyRequestInfo>()
                .ReverseMap();

        }
    }
}
