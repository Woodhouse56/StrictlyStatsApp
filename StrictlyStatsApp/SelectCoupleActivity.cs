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
Ochieng, C. (2020) [online] Xamarin Android – ListView – Images,Text and ItemClick, Available from:
https://camposha.info/xamarin-android-listview-imagestext-and-itemclick/ [Accessed 16/09/21] [3]
*/

namespace StrictlyStats
{
    [Activity(Label = "Select Couple")]
    public class SelectCoupleActivity : ListActivity
    {
        private IStrictlyStatsUOW uow = Global.UOW;
        private List<Couple> couples;
        private ActivityType activityType;
        private int position = -1;

        //<-*****Behague, P (2021) [1]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            activityType = (ActivityType)Intent.GetIntExtra("ActivityType", -1);

            couples = uow.Couples.GetCouplesStillInCompetition();

            this.ListAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, couples.ToArray<Couple>());

            this.ListView.ItemClick += ListView_ItemClick;
        }

        //<-*****Ochieng, C (2020) [3]
        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            Intent intent;
            position = e.Position;
            var dlgAlert = (new AlertDialog.Builder(this)).Create();
            dlgAlert.SetMessage("OK to vote off " + couples[e.Position] + "?");
            dlgAlert.SetTitle("Vote Off");
            dlgAlert.SetButton("OK", ContinueButton_Click);
            dlgAlert.SetButton2("Cancel", CancelButton_Click);
            dlgAlert.Show();
            return;
        }

        //<-*****Behague, P (2021) [1]
        private void CancelButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        //<-*****Behague, P (2021) [1]
        private void ContinueButton_Click(object sender, EventArgs e)
        {
            int weekNumber = uow.Couples.GetCurrentWeekNumber();
            try
            {
                uow.Couples.VoteOff(couples[position].CoupleID, weekNumber);
                //Calc number of weeks competition will run for
                int numberOfWeeksCompetitionWillRunFor = uow.Couples.GetAll().Count - Global.NUMBEROFCONTESTANTSINFINAL + 1;
                //Check for end of competiton
                if(numberOfWeeksCompetitionWillRunFor == weekNumber)
                {
                    //We have a winner
                    Couple couple = uow.Couples.GetWinner();
                    Intent intent = new Intent(this, typeof(WinnerActivity));
                    intent.PutExtra("WinningCoupleID", couple.CoupleID);
                    StartActivity(intent);
                    Finish();
                }
            }
            catch(Exception exn)
            {
                var dlgAlert = (new AlertDialog.Builder(this)).Create();
                dlgAlert.SetMessage(exn.Message);
                dlgAlert.SetTitle("Vote Off Error");
                dlgAlert.SetButton("OK", (s, ev) => { return; });
                dlgAlert.Show();
                return;
            }
            Finish();
        }
    }
}