using System;
using System.Collections.Generic;

namespace VoteIt.Models
{
    public partial class Feed
    {
        public int FeedId { get; set; }
        public string FeedTitle { get; set; }
        public int FeedLike { get; set; }
        public DateTime FeedCreatedDateTime { get; set; }
        public string FeedCreatedUser { get; set; }
        public bool FeedValidFlag { get; set; }
        public int FeedParentId { get; set; }
    }
}
