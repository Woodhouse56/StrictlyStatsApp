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
    [Activity(Label = "App Administration Home Screen")]

    class AppAdministrationHomeScreenActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        ActivityType activityType;
        const string originPage = "AppAdministrationHomeScreenActivity";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AdminHomeScreen);
            Button modifyCouplesButton = FindViewById<Button>(Resource.Id.modifyCouplesButton);
            Button modifyDancesButton = FindViewById<Button>(Resource.Id.modifyDancesButton);
            Button modifyScoresButton = FindViewById<Button>(Resource.Id.modifyScoresButton);
            Button returnToMainMenuButton = FindViewById<Button>(Resource.Id.returnToMainMenuButton);

            modifyDancesButton.Click += modifyDancesButton_Click;
            returnToMainMenuButton.Click += returnToMainMenuButton_Click;
        }

        private void returnToMainMenuButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void modifyDancesButton_Click(object sender, EventArgs e)
        {
            Intent modifyDancesIntent = new Intent(this, typeof(SelectDanceActivity));
            modifyDancesIntent.PutExtra("OriginPage", originPage);
            StartActivity(modifyDancesIntent);
            Finish();
        }
    }
}