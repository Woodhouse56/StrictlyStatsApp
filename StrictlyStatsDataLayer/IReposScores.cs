using StrictlyStatsDataLayer.Models;
using System.Collections.Generic;

namespace StrictlyStatsDataLayer
{
    public interface IReposScores : IRepository<Score>
    {
        List<Score> GetScoresRankedForWeek(int weekNumber);

        List<Score> GetScoresRankedForDance(int danceId);

        void DeleteScoresForWeekNumber(int DanceID);
    }
}