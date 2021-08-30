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
    [Activity(Label = "Modify Dances Table")]
    class modifyDancesActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        ActivityType activityType;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ModifyDances);

           uow.modifyDanceElement(8, "ye");

        }

    }
}