﻿using System;
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
        public Dance GetDancebyId(int DanceId)
        {
            return con.Table<Dance>().ElementAt(DanceId);
        }

        public void DeleteScoresForDanceId(int DanceId)
        {
            con.Table<Dance>().Delete(s => s.DanceID == DanceId);
        }

    }
}