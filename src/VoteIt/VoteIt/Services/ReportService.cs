using System;
using VoteIt.Enums;
using VoteIt.Repositories;

namespace VoteIt.Services
{
    public class ReportService : IScopedProcessingService
    {
        private static DateTime _lastNotifyTime;

        private readonly NotifyService _notifyService;
        private readonly FeedService _feedService;

        public ReportService(NotifyService notifyService,
            FeedService feedService)
        {
            this._notifyService = notifyService;
            this._feedService = feedService;
        }

        public void DoWork()
        {
            var now = DateTime.Now;

            if (IsOnTime(now) == false)
            {
                return;
            }

            //// 年報
            if (now.Month == 1 &&
                now.Day == 1)
            {
            }

            //// 季報
            if (now.Month % 3 == 0 &&
                now.Day == 1)
            {
            }

            /// 月報
            if (now.Day == 1)
            {
            }

            //// 週報
            if (now.DayOfWeek == DayOfWeek.Monday)
            {
                this._feedService.HotFeed(ReportEnum.Weekly);
            }
        }

        /// <summary>
        /// 是否為通知時間
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        private bool IsOnTime(DateTime now)
        {
            TimeSpan timeSpan;
            if (_lastNotifyTime != null)
            {
                timeSpan = now.Subtract(_lastNotifyTime);

                //// 間隔一天內不通知
                if (timeSpan.TotalDays < 1)
                {
                    return false;
                }
            }

            var notifyTime = new DateTime(now.Year, now.Month, now.Day, 11, 55, 0);
            timeSpan = now.Subtract(notifyTime);

            //// 發送通知時間 11:55 ~ 12:05
            if (timeSpan.TotalMinutes > 10)
            {
                return false;
            }

            _lastNotifyTime = now;
            return true;
        }
    }
}