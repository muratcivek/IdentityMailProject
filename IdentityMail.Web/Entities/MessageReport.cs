namespace IdentityMail.Web.Entities
{
    public class MessageReport
    {
        public int Id { get; set; }

        public int MessageId { get; set; }

        public UserMessage Message { get; set; } = null!;

        public int ReporterId { get; set; }

        public AppUser Reporter { get; set; } = null!;

        public ReportReason Reason { get; set; }

        public string? Description { get; set; }

        public ReportStatus Status { get; set; }
            = ReportStatus.Pending;

        public DateTime CreatedDate { get; set; }
            = DateTime.Now;

        public DateTime? ReviewedDate { get; set; }

        public int? ReviewedById { get; set; }

        public AppUser? ReviewedBy { get; set; }
    }
}
