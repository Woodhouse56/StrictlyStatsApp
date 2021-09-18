using SQLite;
using System;

namespace StrictlyStatsDataLayer.Models
{
    [Serializable]
    [Table("Instructions")]
    public class Instruction
    {
        public Instruction()
        {
        }
        public Instruction(string instructionHeading, string instructionDetail)
        {
            InstructionHeading = instructionHeading;
            InstructionDetail = instructionDetail;
        }

        [PrimaryKey, AutoIncrement]
        public int InstructionID
        {
            get;
            set;
        }

        public string InstructionDetail { get; set; }
        public string InstructionHeading { get; set; }
    }
}