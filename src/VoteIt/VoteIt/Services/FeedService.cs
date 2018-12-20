using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using VoteIt.Enums;
using VoteIt.Repositories;

namespace VoteIt.Services
{
    public class FeedService
    {
        private readonly FeedRepository _feedRepository;
        private readonly NotifyService _notifyService;
        private readonly UserManager<IdentityUser> _userManager;

        public FeedService(FeedRepository feedRepository,
            NotifyService notifyService,
            UserManager<IdentityUser> userManager)
        {
            this._feedRepository = feedRepository;
            this._notifyService = notifyService;
            this._userManager = userManager;
        }

        /// <summary>
        /// 是否為最多 Like 的貼文
        /// </summary>
        /// <param name="feedId"></param>
        /// <returns></returns>
        public bool IsTop(int feedId)
        {
            var currentFeed = this._feedRepository.GetFeed(feedId);
            var maxLike = this._feedRepository.MaxLike();

            var isTop = currentFeed.FeedLike == maxLike;
            if (isTop)
            {
                string message = $"*{currentFeed.FeedCreatedUser}* 的貼文，獲得最多 Like:{Environment.NewLine}*{currentFeed.FeedTitle}*";
                this._notifyService.Send(message);
            }
            return isTop;
        }

        /// <summary>
        /// 熱門排行
        /// </summary>
        public void HotFeed(ReportEnum report)
        {
            var startDate = DateTime.Now.AddMonths(-1);
            var feedList = this._feedRepository.GetFeedListWithFeedLike()
                .Where(i => i.FeedCreatedDateTime > startDate)
                .OrderByDescending(i => i.FeedLike)
                .Take(3);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("本月 TOP *3*");

            foreach (var i in feedList)
            {
                stringBuilder.AppendLine($"{i.FeedTitle}, {i.FeedCreatedUser} {i.FeedCreatedDateTime}");
            }

            this._notifyService.Send(stringBuilder.ToString());
        }

        //private ValueTuple<DateTime, DateTime> GetSpan(DateTime now)
        //{
        //    DateTime startDateTime, endDateTime;

        //    return new ValueTuple(startDateTime, endDateTime);
        //}
    }
}