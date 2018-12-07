using System.Collections.Generic;
using System.Linq;
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

        public bool IsLike(long feedId, string user)
        {
            var isLike = this._context.FeedLike.Any(i => i.FeedLikeFeedId == feedId
            && i.FeedLikeCreatedUser == user
            && i.FeedLikeValidFlag == true);

            return isLike;
        }

        public List<Feed> GetFeedList()
        {
            var list = this._context.Feed.ToList(); ;
            return list;
        }

        public void CreateFeed(Feed feed)
        {
            this._context.Add(feed);
            this._context.SaveChangesAsync();
        }

        //public List<Feed> FeedList()
        //{
        //    this._context.Feed.Join(
        //        this._context.FeedLike,
        //        i => i.FeedId,
        //        j => j.FeedLikeFeedId,
        //        (i,j) => new {
        //            i.FeedId,
        //            i.FeedTitle,
        //            //i.
        //        })
        //}
    }
}