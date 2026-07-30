using System.ComponentModel.DataAnnotations;

namespace IdentityMail.Web.DTOs.UserMessageDtos
{
    public class SendMailDto
    {
        [Required(ErrorMessage = "Alıcı e-posta zorunludur.")]
        [EmailAddress]
        public string ReceiverMail { get; set; }

        [Required(ErrorMessage = "Konu zorunludur.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Mesaj boş bırakılamaz.")]
        public string Body { get; set; }
    }
}