namespace IdentityMail.Web.Entities
{
    public class PasswordResetRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public AppUser User { get; set; } = null!;

        public DateTime RequestDate { get; set; }
            = DateTime.Now;

        public bool IsCompleted { get; set; }
            = false;

        public DateTime? CompletedDate { get; set; }
    }
}