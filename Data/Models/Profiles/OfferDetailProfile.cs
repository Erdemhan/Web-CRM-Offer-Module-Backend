using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Data.Entities;
using crmweb.Models.OfferModels;

namespace crmweb.Models.Profiles
{
    public class OfferDetailProfile : Profile
    {
        public OfferDetailProfile()
        {
            CreateMap<OfferDetail, OfferDetailsInfo>().
                ReverseMap();
            CreateMap<OfferDetail, OfferDetailsRequestInfo>().
                ReverseMap();
        }
    }
}
