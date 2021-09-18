using StrictlyStatsDataLayer.Models;
using System.Collections.Generic;

namespace StrictlyStatsDataLayer
{
    public interface IReposCouples : IRepository<Couple>
    {
        int GetCurrentWeekNumber();

        Couple GetCoupleVotedOffForGivenWeekNumber(int weekNumber);

        List<Couple> GetCouplesStillInCompetition();

        void VoteOff(int coupleID, int weekNumber);
        Couple GetWinner();

    }
}