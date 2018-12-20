using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VoteIt.Repositories;

namespace VoteIt.Services
{
    public class FeedService : IScopedProcessingService
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

        public void HotFeed()
        {
            var startDate = DateTime.Now;
            var feedList = this._feedRepository.GetFeedList()
                .Where(i => i.FeedCreatedDateTime > startDate)
                .Take(3);
        }

        public void DoWork()
        {
            this.HotFeed();
        }
    }
}
