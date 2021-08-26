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
using StrictlyStatsDataLayer;
using StrictlyStatsDataLayer.Models;

namespace StrictlyStats
{
    [Activity(Label = "Home")]
    public class RankCouplesByDanceActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RankedCouples);

            int danceId = Intent.GetIntExtra("DanceId", -1);
            ActivityType activityType = (ActivityType)Intent.GetIntExtra("ActivityType", -1);

            List<string> rankedCouples;
            rankedCouples = uow.GetCouplesRankedForDance(danceId);

            int brankedCouples = 1;
            
           //okButton.Click += OkButton_Click;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}