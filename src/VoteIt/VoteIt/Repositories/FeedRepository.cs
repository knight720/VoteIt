using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoteIt.Models;

namespace VoteIt.Repositories
{
    public class FeedRepository
    {
        private VoteItDBContext _context;

        public FeedRepository(VoteItDBContext context)
        {
            this._context = context;
        }

        public void UpdateLike(int feedId)
        {
            var feed = this._context.Feed.Where(i => i.FeedId == feedId).FirstOrDefault();
            feed.FeedLike++;
            this._context.SaveChanges();
        }

        public void CreateFeedLike(FeedLike feedLike)
        {
            this._context.FeedLike.Add(feedLike);
            this._context.SaveChanges();
        }
    }
}
