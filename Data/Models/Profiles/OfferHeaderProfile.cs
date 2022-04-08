using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Data.Entities;
using crmweb.Models.OfferModels;

namespace crmweb.Models.Profiles
{
    public class OfferHeaderProfile : Profile
    {
        public OfferHeaderProfile()
        {
            CreateMap<OfferHeader, OfferInfo>()
                .ReverseMap();

            CreateMap<OfferHeader, OfferRequestInfo>().ReverseMap();
            CreateMap<OfferItem, OfferHeader>().ReverseMap()
                .ForMember(o => o.CompanyName, x => x.MapFrom(o => o.OfferCompany.Name))
                .ForMember(o => o.CompanyContactName, x => x.MapFrom(o => o.OfferCompanyContact.FirstName));
            CreateMap<OfferItemTransfer, OfferItem>().ReverseMap();
            CreateMap<OfferItemTransfer, OfferHeader>().ReverseMap()
                .ForMember(o => o.CompanyName, x => x.MapFrom(o => o.OfferCompany.Name))
                .ForMember(o => o.CompanyContactName, x => x.MapFrom(o => o.OfferCompanyContact.FirstName));

            CreateMap<OfferDashboardInfo, OfferHeader>().ReverseMap()
                .ForMember(o => o.CompanyName, x => x.MapFrom(h => h.OfferCompany.Name));

        }
    }
}
