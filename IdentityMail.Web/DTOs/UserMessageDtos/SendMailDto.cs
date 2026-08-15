using IdentityMail.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs.UserMessageDtos
{
    public class SendMailDto
    {
        public int? DraftId { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? ReceiverMail { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public MessageCategory Category { get; set; } = MessageCategory.General;
    }
}