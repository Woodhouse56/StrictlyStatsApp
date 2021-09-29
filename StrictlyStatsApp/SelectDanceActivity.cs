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

/*
References:
Code elaborates upon foundations within the original source code provided by Behague, Peter.
Behague, P. (2021) [online] StrictlyStatsStarter.zip, Available from:
https://canvas.qa.com/courses/1741/files/948721 [Accessed 02/09/21] [1]
*/

namespace StrictlyStats
{
    [Activity(Label = "Select Dance")]
    public class SelectDanceActivity : Activity
    {
        Spinner dancesSpinner;

        private string selectedDance;
        private int selectedDanceId;
        private IStrictlyStatsUOW uow = Global.UOW;
        private ActivityType activityType;
        private List<Dance> dances;

        //<-*****Behague, P (2021) [1]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectDance);

            dancesSpinner = FindViewById<Spinner>(Resource.Id.dancesSpinner);
            dancesSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            Button okButton = FindViewById<Button>(Resource.Id.okButton);
            Button cancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            Button addDanceButton = FindViewById<Button>(Resource.Id.addDanceButton);
            Button modifySelectedDanceButton = FindViewById<Button>(Resource.Id.modifySelectedDanceButton);

            activityType = (ActivityType)Intent.GetIntExtra("ActivityType", -1);

            /** Checks which elements of the UI to display depending on whether this page was requested by the Admin Page, or a Ranking/Other page */
            if (activityType == ActivityType.AppAdministration)
            {
                okButton.Visibility = ViewStates.Gone;

            }
            else
            {
                addDanceButton.Visibility = ViewStates.Gone;
                modifySelectedDanceButton.Visibility = ViewStates.Gone;
            }

            //Create array of strings containing dance names
            dances = uow.Dances.GetAll();
            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, dances);
            dancesSpinner.Adapter = adapter;

            dancesSpinner.SetSelection(0);

            okButton.Click += OkButton_Click;
            cancelButton.Click += CancelButton_Click;
            modifySelectedDanceButton.Click += ModifySelectedDanceButton_Click;
            addDanceButton.Click += AddDanceButton_Click;
        }

        //<-*****Behague, P (2021) [1]
        private void AddDanceButton_Click(object sender, EventArgs e)
        {
            selectedDance = "newDance";
            modifyDancesActivity(sender, e);
        }

        //<-*****Behague, P (2021) [1]
        private void ModifySelectedDanceButton_Click(object sender, EventArgs e)
        {
            modifyDancesActivity(sender, e);
        }

        //<-*****Behague, P (2021) [1]
        private void OkButton_Click(object sender, EventArgs e)
        {
            /** Call sub-method allowing for easier future development */
            RankCouplesByDanceActivity(sender, e);
        }

        //<-*****Behague, P (2021) [1]
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        //<-*****Behague, P (2021) [1]
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            selectedDance = string.Format("{0}", spinner.GetItemIdAtPosition(e.Position));
            selectedDanceId = int.Parse(selectedDance);

            /** 
             Looks into original 'dances' list and grabs DanceID 
             using key aquired from spinner.
             */
            selectedDanceId = dances[selectedDanceId].DanceID;
        }

        //<-*****Behague, P (2021) [1]
        private void RankCouplesByDanceActivity(object sender, EventArgs e)
        {
            Intent rankCouplesByDanceActivityIntent = new Intent(this, typeof(RankCouplesActivity));

            rankCouplesByDanceActivityIntent.PutExtra("Dance", selectedDance);
            rankCouplesByDanceActivityIntent.PutExtra("DanceId", selectedDanceId);
            rankCouplesByDanceActivityIntent.PutExtra("ActivityType", (int)ActivityType.RankingsByDance);

            StartActivity(rankCouplesByDanceActivityIntent);
            Finish();
        }

        //<-*****Behague, P (2021) [1]
        private void modifyDancesActivity(object sender, EventArgs e)
        {
            Intent rankCouplesByDanceActivityIntent = new Intent(this, typeof(modifyDancesActivity));

            rankCouplesByDanceActivityIntent.PutExtra("Dance", selectedDance);
            rankCouplesByDanceActivityIntent.PutExtra("DanceId", selectedDanceId);
            rankCouplesByDanceActivityIntent.PutExtra("ActivityType", (int)ActivityType.RankingsByDance);

            StartActivity(rankCouplesByDanceActivityIntent);
            Finish();
        }
    }

}