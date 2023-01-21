namespace Company.Function

open System
open System.IO
open System.Net
open System.Data
open System.Linq
open System.Threading.Tasks
open System.Data.SqlClient
open System.Data.Linq
open System.Data.Linq.Mapping
open System.Data.Linq.SqlClient
open System.Data.SqlTypes
open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.OpenApi.Models
open Microsoft.Extensions.Logging
open Newtonsoft.Json
open Azure
open Azure.Core
open Azure.Data.Tables
open FSharp.Data.SqlClient
open NameAndAid

        

//Functionsモジュール
module Functions =
    //応援数を返す (room -> [{room,id1,id2,isStreaming}])
    [<FunctionName("GetPoint")>]
    let GetPointRun ([<HttpTrigger(AuthorizationLevel.Function, "get", Route = "getPoint/{roomid}")>]req: HttpRequest) ([<Sql("select * from dbo.[Room] where Room = @Room", CommandType = System.Data.CommandType.Text, Parameters = "@Room={roomid}", ConnectionStringSetting = "ConnectionSetting")>]data: IEnumerable<RoomIdStreamingStatus>) (log: ILogger) =
        async {
            log.LogInformation("Get Function processed a request.")

            return OkObjectResult(data) :> IActionResult
        } |> Async.StartAsTask


    //一意のroomIdを返すFunction (null->string)
    [<FunctionName("GetRoomNum")>]
    let GetRoomNumRun([<HttpTrigger(AuthorizationLevel.Function,"get", "post", Route = "GetRoomNum")>]req: HttpRequest) (log: ILogger) =
        async {
            log.LogInformation("Get Room Num processed a request.")
            
            let NewRoomId = 
                Guid.NewGuid().ToString()
                               
            return OkObjectResult(NewRoomId) :>IActionResult
        } |> Async.StartAsTask
    
    //配信状態をtrue("1")にするFunction 
    [<FunctionName("StartStreaming")>]
    let StartStreamingRun([<HttpTrigger(AuthorizationLevel.Function, "post", Route = "EnableStreaming")>]req: HttpRequest) ([<Sql("dbo.[Room]", ConnectionStringSetting = "ConnectionSetting")>]enabledata: IAsyncCollector<RoomIdStreamingStatus>) (log: ILogger) =
        async {
            log.LogInformation("Streaming enable processed a request.")
            let streamRender = new StreamReader(req.Body)
            let requestBody = streamRender.ReadToEndAsync() |> string
            

            let roomInfo = JsonConvert.DeserializeObject<RoomIdStreamingStatus>(requestBody)
            roomInfo.isStreaming <- "1"
            let! result1 = enabledata.AddAsync(roomInfo) |> Async.AwaitTask
            let! result2 = enabledata.FlushAsync() |> Async.AwaitTask
            result1 
            result2

            return OkObjectResult("Streaming is now avalable") :>IActionResult
        } |> Async.StartAsTask
    
    //配信状態をfalse("0")にするFunction
    [<FunctionName("StopStreaming")>]
    let StopStreamingRun([<HttpTrigger(AuthorizationLevel.Function, "post", Route = "DisableStreaming")>]req: HttpRequest) ([<Sql("dbo.[Room]", ConnectionStringSetting = "ConnectionSetting")>]enabledata: IAsyncCollector<RoomIdStreamingStatus>) (log: ILogger) =
        async {
            log.LogInformation("Streaming enable processed a request.")
            let streamRender = new StreamReader(req.Body)
            let requestBody = streamRender.ReadToEndAsync() |> string

            let roomInfo = JsonConvert.DeserializeObject<RoomIdStreamingStatus>(requestBody)
            roomInfo.isStreaming <- "0"
            let! result1 = enabledata.AddAsync(roomInfo) |> Async.AwaitTask
            let! result2 = enabledata.FlushAsync() |> Async.AwaitTask
            result1 
            result2

            return OkObjectResult("Streaming is now disable") :>IActionResult
        } |> Async.StartAsTask

