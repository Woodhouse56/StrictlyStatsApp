using StrictlyStatsDataLayer.Models;

namespace StrictlyStatsDataLayer
{
    public interface IReposDances : IRepository<Dance>
    {
        public Dance GetDancebyId(int DanceId);

        public void DeleteScoresForDanceId(int DanceId);

    }
}