﻿using System.Linq;
using Leaguerly.Api.Extensions;
using Leaguerly.Api.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class PlayersController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public PlayersController(LeaguerlyDbContext db) {
            _db = db;
        }

        public async Task<IHttpActionResult> Get() {
            var players = await _db.Players.ToListAsync();

            return Ok(players);
        }

        public async Task<IHttpActionResult> Get(int id) {
            var player = await _db.Players.SingleOrDefaultAsync(p => p.Id == id);

            if (player == null) {
                return NotFound();
            }

            return Ok(player);
        }

        public async Task<IHttpActionResult> Post([FromBody] Player player) {
            if (!ModelState.IsValid) {
                return StatusCode(HttpStatusCodeExtensions.UnprocessableEntity);
            }

            var playerExists = await _db.Players.AnyAsync(p => p.Contact.Email == player.Contact.Email);
            if (playerExists) {
                return Conflict();
            }

            player.Contact.Id = await _db.Contacts
                .Where(p => p.Email == player.Contact.Email)
                .Select(p => p.Id)
                .SingleOrDefaultAsync();

            _db.Players.Add(player);
            if (player.Contact.Id > 0) {
                _db.Entry(player.Contact).State = EntityState.Modified;
            }

            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = player.Id }, player);
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody] Player player) {
            player.Id = id;

            _db.Entry(player).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Delete(int id) {
            var player = new Player { Id = id };

            _db.Players.Remove(player);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}