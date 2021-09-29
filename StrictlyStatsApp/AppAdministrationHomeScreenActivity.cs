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
    [Activity(Label = "App Administration Home Screen")]

    class AppAdministrationHomeScreenActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        ActivityType activityType;

        //<-*****Behague, P (2021) [1]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AdminHomeScreen);

            Button modifyCouplesButton = FindViewById<Button>(Resource.Id.modifyCouplesButton);
            Button modifyDancesButton = FindViewById<Button>(Resource.Id.modifyDancesButton);
            Button modifyScoresButton = FindViewById<Button>(Resource.Id.modifyScoresButton);
            Button returnToMainMenuButton = FindViewById<Button>(Resource.Id.returnToMainMenuButton);

            modifyCouplesButton.Click += ModifyCouplesButton_Click;
            modifyDancesButton.Click += modifyDancesButton_Click;
            modifyScoresButton.Click += ModifyScoresButton_Click;
            returnToMainMenuButton.Click += returnToMainMenuButton_Click;
        }

        private void ModifyScoresButton_Click(object sender, EventArgs e)
        {
            /** Ready for Further Development */
        }

        //<-*****Behague, P (2021) [1]
        private void modifyDancesButton_Click(object sender, EventArgs e)
        {
            Intent modifyDancesIntent = new Intent(this, typeof(SelectDanceActivity));
            modifyDancesIntent.PutExtra("ActivityType", (int)ActivityType.AppAdministration);
            StartActivity(modifyDancesIntent);
            Finish();
        }

        private void ModifyCouplesButton_Click(object sender, EventArgs e)
        {
            /** Ready for Further Development */
        }

        //<-*****Behague, P (2021) [1]
        private void returnToMainMenuButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}