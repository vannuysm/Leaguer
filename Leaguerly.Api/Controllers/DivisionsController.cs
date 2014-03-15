﻿using System.Net;
using Leaguerly.Api.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Leaguerly.Api.Controllers
{
    public class DivisionsController : ApiController
    {
        private readonly LeaguerlyDbContext _db;

        public DivisionsController(LeaguerlyDbContext db) {
            _db = db;
        }

        public async Task<IHttpActionResult> Get() {
            var divisions = await _db.Divisions.ToListAsync();

            return Ok(divisions);
        }

        public async Task<IHttpActionResult> GetByLeagueId(int leagueId) {
            var divisions = await _db.Divisions
                .Where(division => division.LeagueId == leagueId)
                .ToListAsync();

            return Ok(divisions);
        }

        public async Task<IHttpActionResult> Get(int id) {
            var division = await _db.Divisions.FindAsync(id);

            return Ok(division);
        }

        public async Task<IHttpActionResult> Post([FromBody] Division division) {
            _db.Divisions.Add(division);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = division.Id }, division);
        }

        public async Task<IHttpActionResult> Put(int id, [FromBody] Division division) {
            division.Id = id;

            _db.Entry(division).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Delete(int id) {
            var division = new Division { Id = id };

            _db.Divisions.Remove(division);
            await _db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}