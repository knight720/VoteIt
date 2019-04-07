using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using VoteIt.Enums;
using VoteIt.Models;

namespace VoteIt.Repositories
{
    public class FeedRepository
    {
        private VoteItDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FeedRepository(VoteItDBContext context,
            UserManager<IdentityUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public void UpdateLike(int feedId)
        {
            var feed = this._context.Feed.Where(i => i.FeedId == feedId).FirstOrDefault();
            feed.FeedLike++;
            this._context.SaveChanges();
        }

        public Feed GetFeed(int feedId)
        {
            var feed = this._context.Feed.Where(i => i.FeedId == feedId).FirstOrDefault();

            var user = this._userManager.FindByEmailAsync(feed.FeedCreatedUser);
            if (user.Result != null)
            {
                feed.FeedCreatedUser = user.Result.UserName;
            }

            return feed;
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
            var list = this._context.Feed.OrderByDescending(i => i.FeedCreatedDateTime).ToList(); ;
            return list;
        }

        public List<Feed> GetFeedList(SortEnum sortEnum, int page, int feedPage)
        {
            List<Feed> list = new List<Feed>();
            if (sortEnum == SortEnum.New)
            {
                list = this.GetFeedListWithFeedLikeOrderByDate(page, feedPage);
            }
            else if (sortEnum == SortEnum.Like)
            {
                list = this.GetFeedListWithFeedLikeOrderByLike(page, feedPage);
            }

            return list;
        }

        public void CreateFeed(Feed feed)
        {
            this._context.Add(feed);
            this._context.SaveChangesAsync();
        }

        /// <summary>
        /// 由 FeedLike 統計 Like 數
        /// </summary>
        /// <returns></returns>
        public List<Feed> GetFeedListWithFeedLike()
        {
            var feedLikeCount = this._context.FeedLike.GroupBy(fl => fl.FeedLikeFeedId)
                .Select(fl => new
                {
                    FeedLike_FeedId = fl.Key,
                    FeedLike_Count = fl.Count()
                });

            var feedList = this._context.Feed.GroupJoin(
                feedLikeCount,
                f => f.FeedId,
                l => l.FeedLike_FeedId,
                (f, flc) => new Feed
                {
                    FeedId = f.FeedId,
                    FeedTitle = f.FeedTitle,
                    FeedCreatedDateTime = f.FeedCreatedDateTime,
                    FeedCreatedUser = f.FeedCreatedUser,
                    FeedLike = flc.Count() > 0 ? flc.First().FeedLike_Count : 0
                })
                .ToList();

            //// Email to UserName
            ReplaceCreateUser(feedList);

            return feedList;
        }

        /// <summary>
        /// 根據時間由 FeedLike 統計 Like 數
        /// </summary>
        /// <returns></returns>
        public List<Feed> GetFeedListWithFeedLike(DateTime start, DateTime end)
        {
            var feedLikeCount = this._context.FeedLike
                .Where(fl =>
                    fl.FeedLikeCreatedDateTime >= start &&
                    fl.FeedLikeCreatedDateTime <= end)
                .GroupBy(fl => fl.FeedLikeFeedId)
                .Select(fl => new
                {
                    FeedLike_FeedId = fl.Key,
                    FeedLike_Count = fl.Count()
                });

            var feedList = this._context.Feed.GroupJoin(
                feedLikeCount,
                f => f.FeedId,
                l => l.FeedLike_FeedId,
                (f, flc) => new Feed
                {
                    FeedId = f.FeedId,
                    FeedTitle = f.FeedTitle,
                    FeedCreatedDateTime = f.FeedCreatedDateTime,
                    FeedCreatedUser = f.FeedCreatedUser,
                    FeedLike = flc.Count() > 0 ? flc.First().FeedLike_Count : 0
                })
                .ToList();

            //// Email to UserName
            ReplaceCreateUser(feedList);

            return feedList;
        }

        /// <summary>
        /// Email to UserName
        /// </summary>
        private void ReplaceCreateUser(List<Feed> list)
        {
            foreach (var i in list)
            {
                var user = this._userManager.FindByEmailAsync(i.FeedCreatedUser);
                if (user.Result != null)
                {
                    i.FeedCreatedUser = user.Result.UserName;
                }
            }
        }

        public List<Feed> GetFeedListWithFeedLikeOrderByLike(int page, int feedPage)
        {
            var feedList = this.GetFeedListWithFeedLike()
                .OrderByDescending(i => i.FeedLike)
                .ThenByDescending(i => i.FeedCreatedDateTime)
                .Skip(feedPage * page)
                .Take(feedPage)
                .ToList();

            return feedList;
        }

        public List<Feed> GetFeedListWithFeedLikeOrderByDate(int page, int feedPage)
        {
            var feedList = this.GetFeedListWithFeedLike()
                .OrderByDescending(i => i.FeedCreatedDateTime)
                .Skip(feedPage * page)
                .Take(feedPage)
                .ToList();

            return feedList;
        }

        /// <summary>
        /// 取得 like 的 User List
        /// </summary>
        /// <param name="feedId"></param>
        /// <returns></returns>
        public List<string> GetLikeUserList(int feedId)
        {
            var result = this._context.FeedLike
                .Where(i => i.FeedLikeFeedId == feedId)
                .Select(i => i.FeedLikeCreatedUser)
                .ToList();

            return result;
        }

        /// <summary>
        /// 最大 Like 數
        /// </summary>
        /// <returns></returns>
        public int MaxLike()
        {
            var feedLikeCount = this._context.FeedLike.GroupBy(fl => fl.FeedLikeFeedId)
                .Select(fl => new
                {
                    FeedLike_FeedId = fl.Key,
                    FeedLike_Count = fl.Count()
                })
                .OrderByDescending(i => i.FeedLike_Count)
                .FirstOrDefault();

            return feedLikeCount.FeedLike_Count;
        }

        /// <summary>
        /// Feed Count
        /// </summary>
        /// <returns></returns>
        public int FeedCount()
        {
            return this._context.Feed.Count();
        }
    }
}