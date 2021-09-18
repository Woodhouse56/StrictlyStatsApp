using StrictlyStatsDataLayer.Models;
using System.Collections.Generic;

namespace StrictlyStatsDataLayer
{
    //Using Unit Of Work Pattern (UOW)
    //UOW acts like a facade that will aggregate all the repositories initiation, calls and disposal in one place
    //it also separates (decouples) the application (Consol, Controllers, ASP.NET Web Page) from the connections and repositories.


    public interface IStrictlyStatsUOW
    {
        IReposCouples Couples { get; }
        IReposDances Dances { get; }
        IReposScores Scores { get; }
        IReposInstructions Instructions { get; }

        List<string> GetCouplesRanked(int weekNumber);
        public Dance getDanceEnitity(int DanceId);

    }
}