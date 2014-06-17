namespace thearch_android

open System

open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget

[<Activity (Label = "The Arch Climbing Wall",
            Theme = "@style/Theme.GlobalTheme")>]
type MainActivity () =
    inherit Activity ()

    override this.OnCreate (bundle) =
        base.OnCreate (bundle)
        this.SetContentView (Resource_Layout.Main)

        let button = this.FindViewById<Button>(Resource_Id.myButton)
        button.Click.Add (fun args -> 
            this.StartActivity(typedefof<CircuitProblemsActivity>)
        )
