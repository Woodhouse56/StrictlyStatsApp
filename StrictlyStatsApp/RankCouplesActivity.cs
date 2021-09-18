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
    public class RankCouplesActivity : Activity
    {
        private IStrictlyStatsUOW uow = Global.UOW;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RankedCouples);


            ListView rankedCouplesListView = FindViewById<ListView>(Resource.Id.rankedCouplesListView);
            Button okButton = FindViewById<Button>(Resource.Id.okButton);
            TextView headingTextView = FindViewById<TextView>(Resource.Id.headingTextView);

            int weekNumber = Intent.GetIntExtra("WeekNumber", -1);
            ActivityType activityType = (ActivityType)Intent.GetIntExtra("ActivityType", -1);
            int danceId = Intent.GetIntExtra("DanceId", -1);

            List<string> rankedCouples = new List<string>();

            if (weekNumber != -1)
            {
                rankedCouples = uow.GetCouplesRanked(weekNumber);

                if (rankedCouples.Count == 0)
                {
                    string message;
                    if (activityType == ActivityType.Rankings)
                        message = "There are no scores for week " + weekNumber;
                    else
                        message = "No scores have been added to the database";

                    NoScoresAlertDialogueConstructor(message);

                    return;
                }

                headingTextView.Text = $"Rankings for week number: {weekNumber}";
            } else if (danceId != -1)
            {
                Dance dance = uow.getDanceEnitity(danceId);

                rankedCouples = uow.GetCouplesRanked(danceId);

                if (rankedCouples.Count == 0)
                {
                    string message;
                    if (activityType == ActivityType.Rankings)
                        message = "There are no scores for the " + dance.DanceName;
                    else
                        message = "No scores have been added to the database, for the " + dance.DanceName;

                    NoScoresAlertDialogueConstructor(message);

                    return;
                }

                headingTextView.Text = $"Rankings for the, {dance.DanceName}";
            }

            rankedCouplesListView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, rankedCouples);
            okButton.Click += OkButton_Click;
        }

        private void NoScoresAlertDialogueConstructor(string message)
        {
            var dlgAlert = (new AlertDialog.Builder(this)).Create();
            dlgAlert.SetMessage(message);
            dlgAlert.SetTitle("No Scores");
            dlgAlert.SetButton("OK", (s, e) => { Finish(); });
            dlgAlert.Show();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}