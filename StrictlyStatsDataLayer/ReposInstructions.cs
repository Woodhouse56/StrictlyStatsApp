using SQLite;
using StrictlyStatsDataLayer.Models;

namespace StrictlyStatsDataLayer
{
    class ReposInstructions : Repository<Instruction>, IReposInstructions
    {
        public ReposInstructions(SQLiteConnection con) : base(con)
        {

        }
    }
}