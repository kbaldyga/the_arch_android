
namespace thearch_android_test

open System.Collections.Generic
open Android.App
open Android.Content
open Android.OS
open Android.Widget

type SectorsAdapter(context: Activity, items: IList<(int * string * string)>) =
    inherit ArrayAdapter<(int*string*string)>(context, Android.Resource.Id.Text1, items)

    override x.GetView(position, convertView, parent) = 
        let view = context.LayoutInflater.Inflate(Resource_Layout.CircuitProblemsRow, null)
        let id,name,info = x.GetItem(position) in
        view.FindViewById<TextView>(Resource_Id.Text1).Text <- name
        view.FindViewById<TextView>(Resource_Id.Text2).Text <- info
        view