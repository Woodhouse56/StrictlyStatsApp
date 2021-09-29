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

/*
References:
Code elaborates upon foundations within the original source code provided by Behague, Peter.
Behague, P. (2021) [online] StrictlyStatsStarter.zip, Available from:
https://canvas.qa.com/courses/1741/files/948721 [Accessed 02/09/21] [1]
*/

namespace StrictlyStats
{
    enum ActivityType
    {
        EnterScores,
        Rankings,
        //<-*****Behague, P (2021) [1] - START
        RankingsByDance,
        //<-*****Behague, P (2021) [1] - STOP
        VoteOff,
        AppAdministration
    }
}