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

        private Dance danceObject;

        private Button submitChangesButton;
        private Button cancelButton;
        private Button deleteDanceButton;
        private EditText danceName;
        private EditText description;
        private Spinner degreeOfDifficultySpinner;
        string dance;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ModifyDances);


            int danceId = Intent.GetIntExtra("DanceId", -1);
            danceId -= 1;
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

            if (dance != "newDance")
            {

                danceObject = uow.getDanceEnitity(danceId);

                danceName.SetText(danceObject.DanceName, TextView.BufferType.Editable);
                description.SetText(danceObject.Description, TextView.BufferType.Editable);
                danceObject.DegreeOfDifficulty -= 1;
                degreeOfDifficultySpinner.SetSelection(danceObject.DegreeOfDifficulty);
            } else
            {
                danceObject = new Dance { };
            }

            submitChangesButton.Click += SubmitChangesButton_Click; ;
            cancelButton.Click += CancelButton_Click;
            deleteDanceButton.Click += DeleteDanceButton_Click;
        }

        private void DeleteDanceButton_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
            alertDiag.SetTitle("Confirm deletion");
            alertDiag.SetMessage("This action is irrevocable");
            alertDiag.SetPositiveButton("Delete", (senderAlert, args) => {
                uow.Dances.Delete(danceObject);
                Toast.MakeText(this, "Deleted", ToastLength.Short).Show();
                Finish();
            });
            alertDiag.SetNegativeButton("Cancel", (senderAlert, args) => {
                alertDiag.Dispose();
            });
            Dialog diag = alertDiag.Create();
            diag.Show();
        }

        private void SubmitChangesButton_Click(object sender, EventArgs e)
        {
            danceObject.DanceName = danceName.Text;
            danceObject.Description = description.Text;

            if (dance != "newDance")
            {
                uow.Dances.Update(danceObject);
            }
            else
            {
                uow.Dances.Insert(danceObject);
            }
            Finish();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            long selectedDegreeOfDifficulty = spinner.GetItemIdAtPosition(e.Position);
            danceObject.DegreeOfDifficulty = unchecked((int)selectedDegreeOfDifficulty);
            danceObject.DegreeOfDifficulty += 1;
        }

    }
}