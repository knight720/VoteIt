using System;
using System.Collections.Generic;

namespace VoteIt.Models
{
    public partial class FeedLike
    {
        public long FeedLikeId { get; set; }
        public int FeedLikeFeedId { get; set; }
        public DateTime FeedLikeCreatedDateTime { get; set; }
        public string FeedLikeCreatedUser { get; set; }
        public bool FeedLikeValidFlag { get; set; }
    }
}
