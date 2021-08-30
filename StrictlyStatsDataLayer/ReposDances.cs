using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using StrictlyStatsDataLayer.Models;

namespace StrictlyStatsDataLayer
{
    class ReposDances : Repository<Dance>, IReposDances
    {
        public ReposDances(SQLiteConnection con) : base(con)
        {

        }

        public void ModifyDancebyId(int DanceId, string NewName)
        {
            Dance dance = con.Table<Dance>().ElementAt(DanceId);

            dance.DanceName = "Yeyey";

            con.Update(dance);

        }

        public void DeleteScoresForDanceId(int DanceId)
        {
            con.Table<Dance>().Delete(s => s.DanceID == DanceId);
        }

    }
}