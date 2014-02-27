using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Leaguerly.Repositories.DataModels;

namespace Leaguerly.Repositories.Ef
{
    public class LeagueRepository : ILeagueRepository, IDisposable
    {
        private LeaguerlyDbContext _db = new LeaguerlyDbContext();

        public IQueryable<League> Query {
            get { return _db.Leagues.AsQueryable(); }
        }

        public IEnumerable<League> All() {
            return Query.ToList();
        }

        public League Get(int id) {
            return _db.Leagues.Find(id);
        }

        public IEnumerable<Division> GetDivisions(int id) {
            var league = Query.Include(l => l.Divisions)
                .SingleOrDefault(l => l.Id == id);

            if (league == null) {
                throw new ArgumentException("Invalid id", "id");
            }

            return league.Divisions;
        }

        public int Create(League newEntity) {
            _db.Leagues.Add(newEntity);
            _db.SaveChanges();
            return newEntity.Id;
        }

        public void Update(League updatedEntity) {
            _db.Entry(updatedEntity).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id) {
            var league = new League { Id = id };
            _db.Leagues.Remove(league);
            _db.SaveChanges();
        }

        protected void Dispose(bool disposing) {
            if (disposing) {
                if (_db != null) {
                    _db.Dispose();
                    _db = null;
                }
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}