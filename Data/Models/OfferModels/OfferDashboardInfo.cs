using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crmweb.Models.OfferModels
{
    public class OfferDashboardInfo
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
    }
}
