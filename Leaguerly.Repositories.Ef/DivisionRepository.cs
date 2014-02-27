using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Leaguerly.Repositories.DataModels;

namespace Leaguerly.Repositories.Ef
{
    public class DivisionRepository : IDivisionRepository, IDisposable
    {
        private LeaguerlyDbContext _db = new LeaguerlyDbContext();
        
        public IQueryable<Division> Query {
            get { return _db.Divisions.AsQueryable(); }
        }

        public IEnumerable<Division> All() {
            return Query.ToList();
        }

        public Division Get(int id) {
            return _db.Divisions.Find(id);
        }

        public int Create(Division newEntity) {
            _db.Divisions.Add(newEntity);
            _db.SaveChanges();
            return newEntity.Id;
        }

        public void CreateMany(IEnumerable<Division> divisions) {
            _db.Divisions.AddRange(divisions);
            _db.SaveChanges();
        }

        public void Update(Division updatedEntity) {
            _db.Entry(updatedEntity).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id) {
            var division = new Division { Id = id };
            _db.Divisions.Remove(division);
            _db.SaveChanges();
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}