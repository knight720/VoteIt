using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VoteIt.Enums;
using VoteIt.Models;
using VoteIt.Repositories;

namespace VoteIt.Controllers
{
    public class VoteItController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly FeedRepository _feedRepository;

        public VoteItController(UserManager<IdentityUser> userManager, FeedRepository feedRepository)
        {
            this._userManager = userManager;
            this._feedRepository = feedRepository;
        }

        // GET: VoteIt
        public ActionResult Index(SortEnum sort = SortEnum.New)
        {
            List<Feed> list = this._feedRepository.GetFeedList(sort);
            return View(list);
        }

        // GET: VoteIt/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VoteIt/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: VoteIt/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind("FeedTitle")] Feed feed)
        {
            var user = await this._userManager.GetUserAsync(User);

            if (user == null)
            {
                //throw new Exception("尚未認證");
            }

            feed.FeedCreatedDateTime = DateTime.Now;
            feed.FeedCreatedUser = user.Email;
            feed.FeedLike = 0;
            feed.FeedValidFlag = true;

            // TODO: Add insert logic here
            if (ModelState.IsValid)
            {
                using (var context = new VoteItDBContext())
                {
                    context.Add(feed);
                    await context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(feed);
        }

        // GET: VoteIt/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VoteIt/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VoteIt/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VoteIt/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}