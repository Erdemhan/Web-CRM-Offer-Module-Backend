using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using crmweb.Data.Base;

namespace crmweb.Data.Entities
{
    public class OfferHeader : Entity
    {
        public int CompanyId { get; set; }
        public int CompanyContactId { get; set; }
        public int OfferNo { get; set; }
        public int RevisionNo { get; set; }
        public int CreateById { get; set; }

        public byte State { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? ReleaseDate { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? ValidationDate{ get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime CreateTime { get; set; }
        public string CommercialConditions { get; set; }
        public string Payment { get; set; }
        public string Times { get; set; }
        public string AdditionalDescriptions { get; set; }

        public virtual User Createby { get; set; }

        // Navigation Properties

        public virtual Company OfferCompany { get; set; }
        public virtual CompanyContact OfferCompanyContact { get; set; }

        public virtual ICollection<OfferDetail> OfferDetail { get; set; }


    }
}
