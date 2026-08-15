using IdentityMail.Web.Entities;

namespace IdentityMail.Web.DTOs.UserMessageDtos
{
    public class InboxViewModel
    {
        public List<UserMessage> Messages { get; set; } = new();

        // ARAMA / FİLTRE
        public string? Search { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public MessageCategory? Category { get; set; }

        // all / read / unread
        public string? ReadStatus { get; set; }

        public bool ImportantOnly { get; set; }

        // newest / oldest
        public string Sort { get; set; } = "newest";


        // SAYFALAMA
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalCount { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling(
                TotalCount / (double)PageSize);
    }
}