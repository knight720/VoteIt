﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VoteIt.Models;

namespace VoteIt.Controllers
{
    public class VoteItController : Controller
    {
        // GET: VoteIt
        public ActionResult Index()
        {
            List<Feed> list;
            using (var context = new VoteItDBContext())
            {
                list = context.Feed.ToList();
            }

            return View(list);
        }

        // GET: VoteIt/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VoteIt/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VoteIt/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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