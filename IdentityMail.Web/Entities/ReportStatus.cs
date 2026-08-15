using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.Entities
{
    public enum ReportStatus
    {
        [Display(Name = "İnceleme Bekliyor")]
        Pending = 0,

        [Display(Name = "İncelendi")]
        Reviewed = 1,

        [Display(Name = "Reddedildi")]
        Rejected = 2,

        [Display(Name = "İşlem Yapıldı")]
        ActionTaken = 3
    }
}