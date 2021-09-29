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

/*
References:
Code elaborates upon foundations within the original source code provided by Behague, Peter.
Behague, P. (2021) [online] StrictlyStatsStarter.zip, Available from:
https://canvas.qa.com/courses/1741/files/948721 [Accessed 02/09/21] [1]
Sunday, B, F. (2019) [online] Hiding the navigation soft keys Android Xamarin Forms, Available from:
https://viblo.asia/p/hiding-the-navigation-soft-keys-android-xamarin-forms-ByEZkO0YZQ0 [Accessed 11/09/21] [4]

*/

namespace StrictlyStats
{
    [Activity(Label = "Select Week Number")]
    public class SelectWeekNumberActivity : Activity
    {
        private Spinner weekNumberSpinner;
        private int selectedWeekNumber;
        private IStrictlyStatsUOW uow = Global.UOW;
        private ActivityType activityType;

        //<-*****Behague, P (2021) [1]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //<-*****Sunday, B, F. (2019) [4] - START
            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LowProfile;
            uiOptions |= (int)SystemUiFlags.Fullscreen;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
            //<-*****Sunday, B, F. (2019) [4] - END

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

        //<-*****Behague, P (2021) [1]
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

        //<-*****Behague, P (2021) [1]
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        //<-*****Behague, P (2021) [1]
        private void ContinueButton_Click(object sender, EventArgs e)
        {
            Intent enterScoreIntent = new Intent(this, typeof(EnterScoresActivity));
            enterScoreIntent.PutExtra("WeekNumber", selectedWeekNumber);
            StartActivity(enterScoreIntent);
            Finish();
        }
    }
}