using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crmweb.Models.OfferModels
{
    public class OfferItemTransfer
    {
        
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int RevisionNo { get; set; }
        public string OfferNo { get; set; }
        public string Header { get; set; }
        public string CompanyName { get; set; }
        public string CompanyContactName { get; set; }
        public DateTime CreateTime { get; set; }
        public byte State { get; set; }

    }
}

