namespace thearch_api_wrapper

open System.Net
open System.IO
open System.Xml

module api =

    let crag_url = "http://thesendtopos.co.uk/appsupport/24928f42-59dd-4e3e-bf10-695f238c2c7a/crag_update.ashx"
    let route_url = "http://thesendtopos.co.uk/appsupport/24928f42-59dd-4e3e-bf10-695f238c2c7a/route_update.ashx"
    let sector_url = "http://thesendtopos.co.uk/appsupport/24928f42-59dd-4e3e-bf10-695f238c2c7a/sector_update.ashx"

    let getXml (url:string) = 
        let request = HttpWebRequest.Create(url) :?> HttpWebRequest
        let stream = request.GetResponse().GetResponseStream()
        let reader = new StreamReader(stream)
        in reader.ReadToEnd()
    
    let parseXml inputXml = 
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

    let cragData = parseXml <| getXml crag_url
    let routeData = parseXml <| getXml route_url
    let sectorData = parseXml <| getXml sector_url
    //first |> snd |> fun m -> m.["area_name"];;

    //let test = routeData |> List.map(fun i -> snd i |> Map.toList )