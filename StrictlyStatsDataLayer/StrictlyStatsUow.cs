using SQLite;
using StrictlyStatsDataLayer.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StrictlyStatsDataLayer
{
    public class StrictlyStatsUow : IStrictlyStatsUOW
    {
        private SQLiteConnection Connection { get; set; }

        public StrictlyStatsUow() : this(Global.DBNAME)
        {

        }

        public StrictlyStatsUow(string dbName)
        {
            CreateConnection(dbName);
        }

        private void CreateConnection(string dbName)
        {
            var docFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string dbPath = Path.Combine(docFolder, dbName);
            Connection = new SQLiteConnection(dbPath);
        }

        public List<string> GetCouplesRanked(int id)
        {
            List<Score> scores;

            /** Checks the first function used to determine whether to grab data for Dances or Weeks  */
            if ((new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name == "OnCreate")
            {
                scores = Scores.GetScoresRankedForDance(id);
            }         
            else
            {
                scores = Scores.GetScoresRankedForWeek(id);
            }
                List<string> couplesAndScores = GetCouplesAndScores(scores, (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name);

                return couplesAndScores;
        }

        private List<string> GetCouplesAndScores(List<Score> scores, string MethodCalledBy)
        {
            int i = 0;

            /** Checks the first function used to determine whether to format data for Dances or Weeks  */
            if ((new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name == "GetCouplesRankedForDance")
            {
                return scores.Select((s) =>
                    $"{++i:00}" + ") " +
                    s.Couple.CelebrityFirstName + " " +
                    s.Couple.CelebrityLastName + " & " +
                    s.Couple.ProfessionalFirstName + " " +
                    s.Couple.ProfessionalLastName + ", Score: " +
                    s.Grade + ", Week No: " +
                    s.WeekNumber
                ).ToList();
            }
            else
            {

                foreach (Score score in scores)
                {
                    score.Dance = Dances.GetById(score.DanceID);
                }

                return scores.Select((s) =>
                   $"{++i:00}" + ") " +
                   s.Couple.CelebrityFirstName + " " +
                   s.Couple.CelebrityLastName + " & " +
                   s.Couple.ProfessionalFirstName + " " +
                   s.Couple.ProfessionalLastName + ", Dance: " +
                   s.Dance.DanceName + ", Score: " +
                   s.Grade + ", Week No: " +
                   s.WeekNumber
               ).ToList();
            }
        }

        public Dance getDanceEnitity(int DanceId)
        {
            List<Dance> dances = Dances.GetAll();

            Dance dance = new Dance();

            foreach (Dance currentDance in dances)
            {
                if (currentDance.DanceID == DanceId)
                {
                    dance = currentDance;
                    break;
                }
            }
            return dance;
        }



        //repositories
        #region Repositries
        private IReposCouples _couples;
        private IReposDances _dances;
        private IReposScores _scores;
        private IReposInstructions _instructions;

        public IReposCouples Couples
        {
            get
            {
                if (_couples == null)
                {
                    _couples = new ReposCouples(Connection);
                }
                return _couples;
            }
        }

        public IReposDances Dances
        {
            get
            {
                if (_dances == null)
                {
                    _dances = new ReposDances(Connection);
                }
                return _dances;
            }
        }

        public IReposScores Scores
        {
            get
            {
                if (_scores == null)
                {
                    _scores = new ReposScores(Connection);
                }
                return _scores;
            }
        }

        public IReposInstructions Instructions
        {
            get
            {
                if (_instructions == null)
                {
                    _instructions = new ReposInstructions(Connection);
                }
                return _instructions;
            }
        }
    }
    #endregion
}