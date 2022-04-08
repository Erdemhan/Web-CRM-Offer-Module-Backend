using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crmweb.Models.OfferModels;

namespace crmweb.Models.UnifiedModels
{
    public class DashboardInfo
    {
        public int OfferCount { get; set; }
        public int AcceptedOfferCount { get; set; }
        public int RejectedOfferCount { get; set; }
        public int AwaitingOfferCount { get; set; }
        public List<OfferDashboardInfo> OffersList { get; set; }
    }
}
