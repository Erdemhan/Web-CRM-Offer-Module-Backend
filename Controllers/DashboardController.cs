using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Common.Auxiliary;
using crmweb.Models.UnifiedModels;
using crmweb.Services;

namespace crmweb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService DashboardService;

        public DashboardController( DashboardService DashboardService)
        {
            this.DashboardService = DashboardService;
        }

        [HttpGet]
        [Route("startup")]
        public async Task<Result<DashboardInfo>> DashboardTask()
        {
            return await  DashboardService.DashboardStartup();
        }
    }
}
