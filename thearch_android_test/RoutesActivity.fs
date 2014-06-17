
namespace thearch_android

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

[<Activity (Label = "RoutesActivity",
            Theme = "@style/Theme.GlobalTheme")>]
type RoutesActivity() =
  inherit Activity()

  [<DefaultValue>]val mutable database:DatabaseAccess

  override x.OnCreate(bundle) =
    base.OnCreate (bundle)
    let columns = 3
    let sectorId = x.Intent.Extras.GetInt "sector_id"
    let sectorData = api.getSectorById sectorId
    x.Title <- sectorData.[api.k_sectorName]
    x.SetContentView Resource_Layout.RoutePicker
    let viewGroup = x.FindViewById(Resource_Id.tableLayout) :?> TableLayout
    let routes = api.getRoutesBySector sectorId 
                    |> List.sortBy(fun i -> snd i |> Map.find api.k_sortOrder |> int)
                    |> List.toArray 
    x.database <- new DatabaseAccess(api.c_dbName, x)
    let checkedRoutes = x.database.getCheckedRoutes()
    for i in 0..routes.Length/columns do
        let row = new TableRow(x)
        row.Id <- i
        row.LayoutParameters <- new TableRow.LayoutParams(TableRow.LayoutParams.WrapContent, 
                                                            TableRow.LayoutParams.WrapContent)
        for j in 0..columns-1 do
            if(i*columns+j<routes.Length) then
                let route = routes.[i*columns+j]
                let btn = new ToggleButton(x)
                btn.Id <- fst route
                let grade = snd route |> Map.find api.k_techGrade
                let name = snd route |> Map.find api.k_routeName
                let text = name.Replace("Problem ", "") + " " + grade
                btn.Text <- text
                btn.TextOn <- text
                btn.TextOff <- text
                btn.Gravity <- GravityFlags.Center
                if checkedRoutes.Any(fun r -> r = fst route) then
                    btn.Checked <- true
                btn.Click.Add(fun args ->
                    if btn.Checked then x.database.checkRoute <| fst route
                    else x.database.uncheckRoute <| fst route
                )
                row.AddView btn
       
        viewGroup.AddView row