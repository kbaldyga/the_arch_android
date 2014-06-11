
namespace thearch_android_test
open Android.App
open Android.OS

open thearch_api_wrapper

[<Activity (Label = "SplashScreen", MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true)>]
type SplashScreen() =
  inherit Activity()

  override x.OnCreate(bundle) =
    base.OnCreate (bundle)
    api.cragData |> ignore
    api.routeData |> ignore
    api.sectorData |> ignore
    x.StartActivity(typedefof<MainActivity>)