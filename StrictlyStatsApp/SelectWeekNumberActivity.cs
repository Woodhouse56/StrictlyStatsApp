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

namespace StrictlyStats
{
    [Activity(Label = "Select Week Number")]
    public class SelectWeekNumberActivity : Activity
    {
        private Spinner weekNumberSpinner;
        private int selectedWeekNumber;
        private IStrictlyStatsUOW uow = Global.UOW;
        private ActivityType activityType;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;

            SetContentView(Resource.Layout.SelectWeekNumber);

            weekNumberSpinner = FindViewById<Spinner>(Resource.Id.weekNumberSpinner);
            Button okButton = FindViewById<Button>(Resource.Id.okButton);
            Button cancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            activityType = (ActivityType)Intent.GetIntExtra("ActivityType", -1);

            //Calc number of weeks competition will run for
            int numberOfWeeksCompetitionWillRunFor = uow.Couples.GetAll().Count - Global.NUMBEROFCONTESTANTSINFINAL + 1;
            //Add code here that creates weekNos array with same number of entries as couples still in competition
            int[] weekNos = new int[numberOfWeeksCompetitionWillRunFor];
            for (int i = 0; i < numberOfWeeksCompetitionWillRunFor; i++) {
                weekNos[i] = i + 1;
            }

            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, weekNos);
            weekNumberSpinner.Adapter = adapter;
            int currentWeekNumber = uow.Couples.GetCurrentWeekNumber() - 1;

            if (currentWeekNumber >= numberOfWeeksCompetitionWillRunFor)
                //competition has ended
                currentWeekNumber = numberOfWeeksCompetitionWillRunFor - 1;
            weekNumberSpinner.SetSelection(currentWeekNumber);

            if(activityType == ActivityType.EnterScores)
            {
               
            }

            okButton.Click += OkButton_Click;
            cancelButton.Click += CancelButton_Click;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            int position = weekNumberSpinner.SelectedItemPosition;
            selectedWeekNumber = Convert.ToInt32(weekNumberSpinner.GetItemAtPosition(position));
            if (activityType == ActivityType.EnterScores && uow.Couples.GetCurrentWeekNumber() > selectedWeekNumber)
            {
                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetMessage("OK to overwrite existing scores for " + selectedWeekNumber + "?");
                dlgAlert.SetTitle("Selected Week already has Scores");
                dlgAlert.SetButton("OK", ContinueButton_Click);
                dlgAlert.SetButton2("Cancel", CancelButton_Click);
                dlgAlert.Show();
                return;
            }
            else if (activityType == ActivityType.Rankings)
            {
                List<string> rankedCouples;
                rankedCouples = uow.GetCouplesRanked(selectedWeekNumber);

                if (rankedCouples.Count == 0)
                {
                    string message;
                    if (activityType == ActivityType.Rankings)
                        message = "There are no scores for week " + selectedWeekNumber;
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

                headingTextView.Text = $"Rankings for week number: {selectedWeekNumber}";

                rankedCouplesListView.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, rankedCouples);
                return;
            }

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            Intent enterScoreIntent = new Intent(this, typeof(EnterScoresActivity));
            enterScoreIntent.PutExtra("WeekNumber", selectedWeekNumber);
            StartActivity(enterScoreIntent);
            Finish();
        }
    }
}