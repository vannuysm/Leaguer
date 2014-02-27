using System.Data.Entity;
using Leaguerly.Repositories.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leaguerly.Repositories.Ef
{
    public class TeamRepository : ITeamRepository, IDisposable
    {
        private LeaguerlyDbContext _db = new LeaguerlyDbContext();

        public IQueryable<Team> Query {
            get { return _db.Teams.AsQueryable(); }
        }

        public IEnumerable<Team> All() {
            return Query.ToList();
        }

        public Team Get(int id) {
            return _db.Teams.Find(id);
        }

        public int Create(Team newEntity) {
            _db.Teams.Add(newEntity);
            _db.SaveChanges();
            return newEntity.Id;
        }

        public void Update(Team updatedEntity) {
            _db.Entry(updatedEntity).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id) {
            var team = new Team { Id = id };
            _db.Teams.Remove(team);
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
