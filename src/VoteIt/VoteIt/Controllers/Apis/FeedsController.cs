using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoteIt.Models;
using VoteIt.Repositories;

namespace VoteIt.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedsController : ControllerBase
    {
        private readonly VoteItDBContext _context;
        private readonly FeedRepository _feedRepositry;

        public FeedsController(VoteItDBContext context, FeedRepository feedRepository)
        {
            this._context = context;
            this._feedRepositry = feedRepository;
        }

        // GET: api/Feeds
        [HttpGet]
        public IEnumerable<Feed> GetFeed()
        {
            return _context.Feed;
        }

        // GET: api/Feeds/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeed([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feed = await _context.Feed.FindAsync(id);

            if (feed == null)
            {
                return NotFound();
            }

            return Ok(feed);
        }

        // PUT: api/Feeds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeed([FromRoute] int id, [FromBody] Feed feed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feed.FeedId)
            {
                return BadRequest();
            }

            _context.Entry(feed).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Feeds
        [HttpPost]
        public async Task<IActionResult> PostFeed([FromBody] Feed feed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Feed.Add(feed);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeed", new { id = feed.FeedId }, feed);
        }

        // DELETE: api/Feeds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeed([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feed = await _context.Feed.FindAsync(id);
            if (feed == null)
            {
                return NotFound();
            }

            _context.Feed.Remove(feed);
            await _context.SaveChangesAsync();

            return Ok(feed);
        }

        private bool FeedExists(int id)
        {
            return _context.Feed.Any(e => e.FeedId == id);
        }

        [HttpPost("UpdateLike/{feedId}")]
        public async void UpdateLike(int feedId)
        {
            this._feedRepositry.UpdateLike(feedId);
        }
    }
}