using SQLite;
using StrictlyStatsDataLayer.Models;
using System.Collections.Generic;
using System.Linq;

/*
References:
Code elaborates upon foundations within the original source code provided by Behague, Peter.
Behague, P. (2021) [online] StrictlyStatsStarter.zip, Available from:
https://canvas.qa.com/courses/1741/files/948721 [Accessed 02/09/21] [1]
*/

namespace StrictlyStatsDataLayer
{
    public class ReposScores : Repository<Score>, IReposScores
    {
        public ReposScores(SQLiteConnection con) : base(con)
        {

        }

        public List<Score> GetScoresRankedForWeek(int weekNumber)
        {
            List<Score> scores = Get(s => s.WeekNumber == weekNumber, s => s.Grade).ToList();
            GetCouples(scores);
            return scores;
        }

        //<-*****Behague, P (2021) [1]
        public List<Score> GetScoresRankedForDance(int danceId)
        {
            List<Score> scores = Get(s => s.DanceID == danceId, s => s.Grade).ToList();
            GetCouples(scores);
            return scores;
        }

        private List<Score> GetCouples(List<Score> scores)
        {
            scores.Reverse();
            foreach (Score score in scores)
            {
                score.Couple = con.Table<Couple>().Single(c => c.CoupleID == score.CoupleID);
            }
            return scores;
        }

        public void DeleteScoresForWeekNumber(int weekNumber)
        {
            con.Table<Score>().Delete(s => s.WeekNumber == weekNumber);
        }
    }
}