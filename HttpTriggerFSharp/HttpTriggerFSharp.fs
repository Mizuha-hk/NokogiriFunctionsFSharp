namespace Company.Function

open System
open System.IO
open System.Collections.Generic
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open Newtonsoft.Json
open NameAndAid


        

//Functionsモジュール
module Functions =
    //roomIdからルーム情報を返す (room -> [{room,id1,id2,isStreaming}])
    [<FunctionName("GetRoomInfo")>]
    let GetRoomInfoRun ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getRoomInfo/{roomid}")>]req: HttpRequest) ([<Sql("select * from dbo.[Room] where Room = @Room", CommandType = System.Data.CommandType.Text, Parameters = "@Room={roomid}", ConnectionStringSetting = "ConnectionSetting")>]data: IEnumerable<RoomIdStreamingStatus>) (log: ILogger) =
        async {
            log.LogInformation("Get RoomInfo Function processed a request.")

            return OkObjectResult(data) :> IActionResult
        } |> Async.StartAsTask

    //playerのIdから応援Pointを返す (id -> [{"id","point"}])
    [<FunctionName("GetPoint")>]
    let GetPointRun ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getPoint/{id}")>]req: HttpRequest) ([<Sql("select * from dbo.[Point] where Id = @Id", CommandType = System.Data.CommandType.Text, Parameters = "@Id={id}", ConnectionStringSetting = "ConnectionSetting")>]idandpoint: IEnumerable<IdAndPoint>) (log: ILogger) =
        async {
            log.LogInformation("Get Point Function processed a request")


            return OkObjectResult(idandpoint) :> IActionResult
        } |> Async.StartAsTask

    //一意のroomIdを返すFunction (null->string)
    [<FunctionName("GetRoomNum")>]
    let GetRoomNumRun([<HttpTrigger(AuthorizationLevel.Anonymous,"get", Route = "GetRoomNum")>]req: HttpRequest) (log: ILogger) =
        async {
            log.LogInformation("Get Room Num processed a request.")
            
            let NewRoomId = 
                Guid.NewGuid().ToString()
                               
            return OkObjectResult(NewRoomId) :>IActionResult
        } |> Async.StartAsTask
   
