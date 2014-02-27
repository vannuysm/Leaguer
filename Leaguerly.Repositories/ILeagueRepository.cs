using Leaguerly.Repositories.DataModels;
using System.Collections.Generic;

namespace Leaguerly.Repositories
{
    public interface ILeagueRepository : IRepository<League, int>
    {
        IEnumerable<Division> GetDivisions(int id);
    }
}