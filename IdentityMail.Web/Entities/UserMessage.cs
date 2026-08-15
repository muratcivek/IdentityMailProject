namespace IdentityMail.Web.Entities
{
    public class UserMessage
    {
        public int Id { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public DateTime SendDate { get; set; }

        public bool IsRead { get; set; }

        public bool IsImportant { get; set; }

        public MessageCategory Category { get; set; }

        public bool IsDeletedBySender { get; set; }

        public bool IsDeletedByReceiver { get; set; }

        // TASLAK
        public bool IsDraft { get; set; }

        public DateTime? DraftUpdatedDate { get; set; }

        public int SenderId { get; set; }

        public AppUser Sender { get; set; } = null!;

        public int? ReceiverId { get; set; }

        public AppUser? Receiver { get; set; }
    }
}