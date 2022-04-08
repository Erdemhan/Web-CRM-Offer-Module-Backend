using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using crmweb.Common.Auxiliary;
using crmweb.Data;
using crmweb.Models.CompanyModels;
using crmweb.Models.OfferModels;
using crmweb.Models.UnifiedModels;

namespace crmweb.Services
{
    public class DashboardService
    {
        //Member Variables/////////////////////////////////////////////////////
        private MainDb context;
        private readonly IMapper _mapper;

        //Constructor//////////////////////////////////////////////////////////
        public DashboardService(MainDb Context, IMapper mapper)
        {
            context = Context;
            _mapper = mapper;
        }

        public async Task<Result<DashboardInfo>> DashboardStartup()
        {
            DashboardInfo info = new DashboardInfo();

            // query yazılacak
            var query = await context.OfferHeaders
                .Include(o => o.OfferCompany).ToListAsync();

            info.OfferCount = query.Count();
            info.AcceptedOfferCount = query.Where(o => o.State == 3).Count();

            info.AwaitingOfferCount = query.Where(o => o.State == 2).Count();

            info.RejectedOfferCount = query.Where(o => o.State == 4).Count();

            info.OffersList = _mapper.Map<List<OfferDashboardInfo>>(query.OrderByDescending(o => o.CreateTime).Take(10));



            try
            {
                return Result<DashboardInfo>.PrepareSuccess(info);
            }
            catch (Exception e)
            {

                return Result<DashboardInfo>.PrepareFailure(e.ToString());
            }
        }
    }
}
