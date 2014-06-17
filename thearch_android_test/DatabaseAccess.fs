
namespace thearch_android

open System
open System.Collections.Generic
open System.Linq
open System.Text
open System.IO

open Android.App
open Android.Content
open Android.OS
open Android.Runtime
open Android.Views
open Android.Widget
open Android.Database.Sqlite

open thearch_api_wrapper

type DatabaseAccess(dbName:string, context:Context) =
    let mutable db:Option<SQLiteDatabase> = None
    do
        let dbPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.ToString(), dbName)
        in
        if not <| File.Exists dbPath then
            let br = new BinaryReader(context.Assets.Open api.c_dbName)
            let bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create))
            let mutable (buffer:byte array) = Array.zeroCreate 2048
            let mutable len = br.Read(buffer, 0, buffer.Length)
            while len > 0  do
                bw.Write(buffer, 0, len)
                len <- br.Read(buffer, 0, buffer.Length)

        if File.Exists dbPath then
            db <- Some(SQLiteDatabase.OpenOrCreateDatabase(dbPath, null))
            let query = "create table if not exists routes_checked(id integer primary key)"
            db.Value.ExecSQL query

    member x.checkRoute (id:int) =
        let query = "insert into routes_checked(id) values(" + id.ToString() + ")"
        db.Value.ExecSQL query

    member x.uncheckRoute (id:int) = 
        let query = "delete from routes_checked where id = " + id.ToString()
        db.Value.ExecSQL query
       
    member x.getCheckedRoutes() =     
        let query = "select id from routes_checked"
        let cursor = db.Value.RawQuery (query, null)
        let routes = new List<int>()
        if cursor.MoveToFirst() then
            while not cursor.IsAfterLast do
                routes.Add <| cursor.GetInt 0 |> ignore
                cursor.MoveToNext() |> ignore
        routes

    interface IDisposable with
        member x.Dispose() = if db.Value <> null then db.Value.Close()
