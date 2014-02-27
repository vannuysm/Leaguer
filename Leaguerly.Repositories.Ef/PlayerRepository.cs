using System.Data.Entity;
using Leaguerly.Repositories.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leaguerly.Repositories.Ef
{
    public class PlayerRepository : IPlayerRepository, IDisposable
    {
        private LeaguerlyDbContext _db = new LeaguerlyDbContext();

        public IQueryable<Player> Query {
            get { return _db.Players.AsQueryable(); }
        }

        public IEnumerable<Player> All() {
            return Query.ToList();
        }

        public Player Get(int id) {
            return _db.Players.Find(id);
        }

        public int Create(Player newEntity) {
            _db.Players.Add(newEntity);
            _db.SaveChanges();
            return newEntity.Id;
        }

        public void Update(Player updatedEntity) {
            _db.Entry(updatedEntity).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id) {
            var player = new Player { Id = id };
            _db.Players.Remove(player);
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
