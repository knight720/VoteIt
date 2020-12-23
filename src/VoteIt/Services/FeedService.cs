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
			var title = "上週";
            var span = this.GetSpan(DateTime.Now, report);

            var feedList = this._feedRepository.GetFeedListWithFeedLike(span.startDateTime, span.endDateTime)
                .OrderByDescending(i => i.FeedLike)
                .Take(3);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{title} TOP *{feedList.Count()}*");

            foreach (var i in feedList)
            {
                stringBuilder.AppendLine($"{i.FeedTitle}, {i.FeedCreatedUser} {i.FeedCreatedDateTime.ToString("yyyy/MM/dd HH:mm")}");
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

            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MinValue;

            if (report == ReportEnum.Weekly)
            {
                //// 星期一
                var week = (int)now.DayOfWeek;
                end = now.AddDays(-week);
                start = end.AddDays(-7);
                end = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59);
            }
            else if (report == ReportEnum.Monthly)
            {
                //// 每月 1號
                start = now.AddMonths(-1);
                start = new DateTime(start.Year, start.Month, 1);
                var endDay = DateTime.DaysInMonth(start.Year, start.Month);
                end = new DateTime(start.Year, start.Month, endDay, 23, 59, 59);
            }
            else if (report == ReportEnum.Quarterly)
            {
                //// 1, 4, 7, 10月 1號
                var mod = now.Month % 3;
                mod = (mod == 0) ? 3 : mod;
                end = now.AddMonths(-mod);
                start = end.AddMonths(-2);
                start = new DateTime(start.Year, start.Month, 1);
                var endDay = DateTime.DaysInMonth(end.Year, end.Month);
                end = new DateTime(end.Year, end.Month, endDay, 23, 59, 59);
            }
            else if (report == ReportEnum.Yearly)
            {
                //// 1月 1號
                start = now.AddYears(-1);
                start = new DateTime(start.Year, 1, 1);
                end = new DateTime(start.Year, 12, 31, 23, 59, 59);
            }

            this._logger.LogInformation("Time: {0} ~ {1}", report, start, end);
            return (startDateTime: start, endDateTime: end);
        }
    }
}