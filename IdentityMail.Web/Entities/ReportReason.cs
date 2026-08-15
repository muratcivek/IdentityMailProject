using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.Entities
{
    public enum ReportReason
    {
        [Display(Name = "Spam / Gereksiz Mesaj")]
        Spam = 0,

        [Display(Name = "Taciz veya Rahatsız Edici İçerik")]
        Harassment = 1,

        [Display(Name = "Uygunsuz İçerik")]
        InappropriateContent = 2,

        [Display(Name = "Dolandırıcılık / Şüpheli İçerik")]
        Fraud = 3,

        [Display(Name = "Diğer")]
        Other = 4
    }
}