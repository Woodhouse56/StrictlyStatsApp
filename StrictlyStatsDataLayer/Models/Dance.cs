using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace StrictlyStatsDataLayer.Models
{
    [Table("Dances")]
    public class Dance
    {
        public Dance()
        {
        }

        public Dance(int danceID, string danceName, string description, int degreeOfDifficulty)
        {
            DanceID = danceID;
            DanceName = danceName;
            Description = description;
            DegreeOfDifficulty = degreeOfDifficulty;
        }

        [PrimaryKey, AutoIncrement]
        public int DanceID
        {
            get;
            set;
        }

        public string DanceName
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int DegreeOfDifficulty
        {
            get;
            set;
        }

        public override string ToString()
        {
            return DanceName;
        }

        [OneToMany(CascadeOperations = CascadeOperation.None)]
        public List<Score> Scores { get; set; }
    }
}