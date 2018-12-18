using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        public async Task<bool> IsTop(int feedId)
        {
            var currentFeed = this._feedRepository.GetFeed(feedId);
            var topFeed = this._feedRepository.GetFeedListWithFeedLikeOrderByLike().FirstOrDefault();

            var isTop = currentFeed.FeedLike == topFeed.FeedLike;
            if (isTop)
            {
                var user = await this._userManager.FindByEmailAsync(currentFeed.FeedCreatedUser);

                string message = $"*{user.UserName}* 的貼文，獲得最多 Like:{Environment.NewLine}*{currentFeed.FeedTitle}*";
                this._notifyService.Send(message);
            }
            return isTop;
        }
    }
}
