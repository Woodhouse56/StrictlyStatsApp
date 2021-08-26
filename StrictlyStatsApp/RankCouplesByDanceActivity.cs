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
            string dance = Intent.GetStringExtra("Dance");
            ActivityType activityType = (ActivityType)Intent.GetIntExtra("ActivityType", -1);

            List<string> rankedCouples;
            rankedCouples = uow.GetCouplesRankedForDance(danceId);

            if (rankedCouples.Count == 0)
            {
                string message;
                if (activityType == ActivityType.Rankings)
                    message = "There are no scores for the " + dance;
                else
                    message = "No scores have been added to the database";

                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetMessage(message);
                dlgAlert.SetTitle("No Scores");
                dlgAlert.SetButton("OK", (s, e) => { Finish(); });
                dlgAlert.Show();
                return;
            }

            ListView rankedCouplesListView = FindViewById<ListView>(Resource.Id.rankedCouplesListView);
            Button okButton = FindViewById<Button>(Resource.Id.okButton);
            TextView headingTextView = FindViewById<TextView>(Resource.Id.headingTextView);

            headingTextView.Text = $"Rankings for the, {dance}";

            rankedCouplesListView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, rankedCouples);
            okButton.Click += OkButton_Click;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}