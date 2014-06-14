namespace thearch_api_wrapper

open System.Net
open System.IO
open System.Xml

module api =

    let k_sectorName = "sector_name"
    let k_sectorInfoShort = "sector_info_short"
    let k_techGrade = "tech_grade"
    let k_sortOrder = "sort_order"
    let k_routeId = "route_id"
    let k_routeName = "route_name"
    let k_sectorId = "sector_id"
    let k_cragId = "crag_id"

    let private crag_url = "http://thesendtopos.co.uk/appsupport/24928f42-59dd-4e3e-bf10-695f238c2c7a/crag_update.ashx"
    let private route_url = "http://thesendtopos.co.uk/appsupport/24928f42-59dd-4e3e-bf10-695f238c2c7a/route_update.ashx"
    let private sector_url = "http://thesendtopos.co.uk/appsupport/24928f42-59dd-4e3e-bf10-695f238c2c7a/sector_update.ashx"

    let private getXml (url:string) = 
        let request = HttpWebRequest.Create(url) :?> HttpWebRequest
        let stream = request.GetResponse().GetResponseStream()
        let reader = new StreamReader(stream)
        in reader.ReadToEnd()
    
    let private parseXml inputXml = 
        let doc = new XmlDocument() in
        doc.LoadXml inputXml;
        let root = doc.SelectSingleNode "//plist/dict" in
        root.SelectNodes("key")
            |> Seq.cast<XmlNode>
            |> Seq.map(fun keyNode ->
                let keyValue = keyNode.InnerText |> int
                let dicts = keyNode.NextSibling
                let fullDict = 
                    dicts.SelectNodes("key")
                    |> Seq.cast<XmlNode> 
                    |> Seq.map(fun dictKeyNode -> 
                        let dictKeyValue = dictKeyNode.InnerText
                        let dictValue = dictKeyNode.NextSibling
                        in (dictKeyValue, dictValue.InnerText)
                    ) |> Map.ofSeq

                in (keyValue, fullDict)
            ) 
            |> Seq.toList

    let mutable private cragDataCached : ((int * Map<string,string>) list option) = None
    let mutable private routeDataCached : ((int * Map<string,string>) list option) = None
    let mutable private sectorDataCached : ((int * Map<string,string>) list option) = None


    let cragData = 
        match cragDataCached with
        | Some x -> x
        | None ->
            cragDataCached <- Some(parseXml <| getXml crag_url)
            cragDataCached.Value
    let routeData = 
        match routeDataCached with
        | Some x -> x
        | None ->
            routeDataCached <- Some(parseXml <| getXml route_url)
            routeDataCached.Value
    let sectorData = 
        match sectorDataCached with
        | Some x -> x
        | None ->
            sectorDataCached <- Some(parseXml <| getXml sector_url)
            sectorDataCached.Value

    let getRoutesBySector id =
        routeData |> List.filter(fun i -> (snd i).[k_sectorId] |> int = id)
    let getSectorById id =
        sectorData |> List.filter(fun i -> fst i = id) |> List.head |> snd
