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
    [Activity(Label = "Select Week Number")]
    public class SelectDanceActivity : Activity
    {
        Spinner dancesSpinner;

        string selectedDance;
        int selectedDanceId;
        IStrictlyStatsUOW uow = Global.UOW;
        ActivityType activityType;
        List<Dance> dances;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectDance);

            dancesSpinner = FindViewById<Spinner>(Resource.Id.dancesSpinner);
            dancesSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            Button okButton = FindViewById<Button>(Resource.Id.okButton);
            Button cancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            activityType = (ActivityType)Intent.GetIntExtra("ActivityType", -1);

            //Create array of strings containing dance names
            dances = uow.Dances.GetAll();
            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, dances);
            dancesSpinner.Adapter = adapter;

            dancesSpinner.SetSelection(0);

            okButton.Click += OkButton_Click;
            cancelButton.Click += CancelButton_Click;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Intent rankCouplesByDanceActivityIntent = new Intent(this, typeof(RankCouplesByDanceActivity));
            rankCouplesByDanceActivityIntent.PutExtra("Dance", selectedDance);
            rankCouplesByDanceActivityIntent.PutExtra("DanceId", selectedDanceId);
            StartActivity(rankCouplesByDanceActivityIntent);
            Finish();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            selectedDance = string.Format("{0}", spinner.GetItemIdAtPosition(e.Position));
            selectedDanceId = int.Parse(selectedDance);
            selectedDanceId += 1;
        }

    }

}