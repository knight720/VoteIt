using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using VoteIt.Enums;
using VoteIt.Repositories;

namespace VoteIt.Services
{
    public class FeedService
    {
        private readonly FeedRepository _feedRepository;
        private readonly NotifyService _notifyService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger _logger;

        public FeedService(FeedRepository feedRepository,
            NotifyService notifyService,
            UserManager<IdentityUser> userManager,
            ILogger<FeedService> logger)
        {
            this._feedRepository = feedRepository;
            this._notifyService = notifyService;
            this._userManager = userManager;
            this._logger = logger;
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
            var span = this.GetSpan(DateTime.Now, report);

            var feedList = this._feedRepository.GetFeedListWithFeedLike(span.startDateTime, span.endDateTime)
                .OrderByDescending(i => i.FeedLike)
                .Take(3);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"本周 TOP *{feedList.Count()}*");

            foreach (var i in feedList)
            {
                stringBuilder.AppendLine($"{i.FeedTitle}, {i.FeedCreatedUser} {i.FeedCreatedDateTime}");
            }

            this._notifyService.Send(stringBuilder.ToString());
        }

        /// <summary>
        /// 取得起訖時間
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        private (DateTime startDateTime, DateTime endDateTime) GetSpan(DateTime now, ReportEnum report)
        {
            this._logger.LogInformation("report:{0}", report.ToString());

            var result = (startDateTime: DateTime.MinValue, endDateTime: DateTime.MinValue);

            if (report == ReportEnum.Weekly)
            {
                var week = (int)now.DayOfWeek;
                var end = now.AddDays(-week);
                var start = end.AddDays(-7);
                end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);

                result.startDateTime = start;
                result.endDateTime = end;
            }

            this._logger.LogInformation("Time: {0} ~ {1}", report, result.startDateTime, result.endDateTime);
            return result;
        }
    }
}