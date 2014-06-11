
namespace thearch_android_test

open System.Collections.Generic
open Android.App
open Android.Content
open Android.OS
open Android.Widget

type SectorsAdapter(context: Activity, items: IList<string*string>) =
    inherit ArrayAdapter<string*string>(context, Android.Resource.Id.Text1, items)

    override x.GetView(position, convertView, parent) = 
        let view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null)
        let item = x.GetItem(position) in
        view.FindViewById<TextView>(Android.Resource.Id.Text1).Text <- fst item
        view.FindViewById<TextView>(Android.Resource.Id.Text2).Text <- snd item
        view