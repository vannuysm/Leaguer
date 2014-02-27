using Leaguerly.Repositories.DataModels;
using System.Collections.Generic;

namespace Leaguerly.Repositories
{
    public interface IDivisionRepository : IRepository<Division, int>
    {
        void CreateMany(IEnumerable<Division> divisions);
    }
}