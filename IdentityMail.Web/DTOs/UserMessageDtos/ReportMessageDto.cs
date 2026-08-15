using IdentityMail.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs.UserMessageDtos
{
    public class ReportMessageDto
    {
        public int MessageId { get; set; }

        [Required(ErrorMessage = "Şikayet nedeni seçiniz.")]
        public ReportReason Reason { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
