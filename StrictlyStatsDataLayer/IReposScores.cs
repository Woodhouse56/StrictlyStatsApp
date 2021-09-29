using StrictlyStatsDataLayer.Models;
using System.Collections.Generic;

/*
References:
Code elaborates upon foundations within the original source code provided by Behague, Peter.
Behague, P. (2021) [online] StrictlyStatsStarter.zip, Available from:
https://canvas.qa.com/courses/1741/files/948721 [Accessed 02/09/21] [1]
*/

namespace StrictlyStatsDataLayer
{
    public interface IReposScores : IRepository<Score>
    {
        List<Score> GetScoresRankedForWeek(int weekNumber);

        //<-*****Behague, P (2021) [1]
        List<Score> GetScoresRankedForDance(int danceId);

        void DeleteScoresForWeekNumber(int DanceID);
    }
}