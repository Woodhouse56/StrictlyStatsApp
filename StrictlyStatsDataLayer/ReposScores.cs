using SQLite;
using StrictlyStatsDataLayer.Models;
using System.Collections.Generic;
using System.Linq;

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