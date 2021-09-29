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
Microsoft (no date) [online] EditText Class, Available from:
https://docs.microsoft.com/en-us/dotnet/api/android.widget.edittext?view=xamarin-android-sdk-9 [Accessed 14/09/21] [2]
*/

namespace StrictlyStats
{
    [Activity(Label = "Modify Dances Table")]
    class modifyDancesActivity : Activity
    {
        IStrictlyStatsUOW uow = Global.UOW;
        ActivityType activityType;

        private Dance danceObject;

        private Button submitChangesButton;
        private Button cancelButton;
        private Button deleteDanceButton;
        private EditText danceName;
        private EditText description;
        private Spinner degreeOfDifficultySpinner;
        private string dance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ModifyDances);


            int danceId = Intent.GetIntExtra("DanceId", -1);
            dance = Intent.GetStringExtra("Dance");

            submitChangesButton = FindViewById<Button>(Resource.Id.submitChangesButton);
            cancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            deleteDanceButton = FindViewById<Button>(Resource.Id.deleteDanceButton);

            danceName = FindViewById<EditText>(Resource.Id.submittedDanceNameEditText);
            description = FindViewById<EditText>(Resource.Id.submittedDescriptionEditText);

            degreeOfDifficultySpinner = FindViewById<Spinner>(Resource.Id.degreeOfDifficultySpinner);
            var spinnerContents = new List<int> {1,2,3,4,5,6,7,8,9,10};
            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, spinnerContents);
            degreeOfDifficultySpinner.Adapter = adapter;
            degreeOfDifficultySpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            if (dance == "newDance")
            {
                danceObject = new Dance { };
            }
            else
            {
                danceObject = uow.getDanceEnitity(danceId);

                //<-*****Microsoft (no date) [2] - START
                danceName.SetText(danceObject.DanceName, TextView.BufferType.Editable);
                description.SetText(danceObject.Description, TextView.BufferType.Editable);
                //<-*****Microsoft (no date) [2] - START

                /** Offsetting due to inherent List entity Offset */
                danceObject.DegreeOfDifficulty -= 1;
                degreeOfDifficultySpinner.SetSelection(danceObject.DegreeOfDifficulty);
            }

            submitChangesButton.Click += SubmitChangesButton_Click; ;
            cancelButton.Click += CancelButton_Click;
            deleteDanceButton.Click += DeleteDanceButton_Click;
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            long selectedDegreeOfDifficulty = spinner.GetItemIdAtPosition(e.Position);
            danceObject.DegreeOfDifficulty = unchecked((int)selectedDegreeOfDifficulty);
            danceObject.DegreeOfDifficulty += 1;
        }


        private void SubmitChangesButton_Click(object sender, EventArgs e)
        {
            SubmitDanceEntity(sender, e);
        }

        private void DeleteDanceButton_Click(object sender, EventArgs e)
        {
            AlertDialogConstructor(sender, e, "Confirm deletion", "This action is irrevocable", "Delete", "Cancel");
        }


        private void CancelButton_Click(object sender, EventArgs e)
        {
            GoToAdminHomeScreen(sender, e);
        }


        private void SubmitDanceEntity(object sender, EventArgs e)
        {
            danceObject.DanceName = danceName.Text;
            danceObject.Description = description.Text;
            /** danceObject.DegreeOfDifficulty set in spinner_ItemSelected */

            if (DataVeracityChecker(sender, e))
            {
                if (dance == "newDance")
                {
                    uow.Dances.Insert(danceObject);
                    GoToAdminHomeScreen(sender, e);
                }
                else
                {
                    uow.Dances.Update(danceObject);
                    GoToAdminHomeScreen(sender, e);
                }
            }
        }

        private Boolean DataVeracityChecker(object sender, EventArgs e)
        {
            if (danceName.Text == "")
            {
                AlertDialogConstructor(sender, e, "Ensure Name field is complete", "The Name field is not permitted to be left blank.");
                return false;
            }
            else if (danceName.Text.Length > 40 || description.Text.Length > 1000)
            {
                AlertDialogConstructor(sender, e, "Too many characters entered", "The Name field character limit is 40, whilst the Description character limit is 1000");
                return false;
            }
            return true;
        }

        //<-*****Behague, P (2021) [1]
        private void AlertDialogConstructor(object sender, EventArgs e, string title, string message, string positiveButton = "Return", string negativeButton = "Quit to Administration Menu")
        {
            AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
            alertDiag.SetTitle(title);
            alertDiag.SetMessage(message);

            if (positiveButton == "Return" && negativeButton == "Quit to Administration Menu")
            {
                alertDiag.SetPositiveButton(positiveButton, (senderAlert, args) => {
                alertDiag.Dispose();
            });
            alertDiag.SetNegativeButton(negativeButton, (senderAlert, args) => {
                GoToAdminHomeScreen(sender, e);
                Finish();
            });
            } else if (positiveButton == "Delete")
            {
                alertDiag.SetPositiveButton(positiveButton, (senderAlert, args) => {
                    uow.Dances.Delete(danceObject);
                    Toast.MakeText(this, "Deleted", ToastLength.Long).Show();

                    GoToAdminHomeScreen(sender, e);
                    Finish();
                });
                alertDiag.SetNegativeButton(negativeButton, (senderAlert, args) => {
                    alertDiag.Dispose();
                });
            }
            Dialog diag = alertDiag.Create();
            diag.Show();
        }

        //<-*****Behague, P (2021) [1]
        private void GoToAdminHomeScreen(object sender, EventArgs e)
        {
            Intent adminIntent = new Intent(this, typeof(AppAdministrationHomeScreenActivity));
            adminIntent.PutExtra("ActivityType", (int)ActivityType.AppAdministration);
            StartActivity(adminIntent);
            Finish();
        }

    }
}