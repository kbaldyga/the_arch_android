
namespace thearch_android_test

open System
open System.Collections.Generic
open System.Linq
open System.Text

open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget

open thearch_api_wrapper

[<Activity (Label = "CircuitProblemsActivity")>]
type CircuitProblemsActivity() =
  inherit ListActivity()

  let mutable items: (string*string) list = []
  override x.OnCreate(bundle) =
    base.OnCreate (bundle)
    items <- api.sectorData 
        |> List.map(fun s ->  
            (Map.find("sector_name") <| snd s, Map.find("sector_info_short") <| snd s))

    x.ListAdapter <- new SectorsAdapter(x, items.ToList())

  override x.OnListItemClick(listView, view, position, id) =
    let t = items.[position]
    Toast.MakeText(x, fst t, ToastLength.Short).Show()
