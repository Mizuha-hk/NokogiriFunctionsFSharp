namespace Company.Function

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open NameAndAid

        

//Functionsƒ‚ƒWƒ…[ƒ‹
module Functions =
    //roomId‚©‚çƒ‹[ƒ€î•ñ‚ð•Ô‚· (room -> [{room,id1,id2,isStreaming}])
    [<FunctionName("GetRoomInfo")>]
    let GetRoomInfoRun ([<HttpTrigger(AuthorizationLevel.Function, "get", Route = "getRoomInfo/{roomid}")>]req: HttpRequest) ([<Sql("select * from dbo.[Room] where Room = @Room", CommandType = System.Data.CommandType.Text, Parameters = "@Room={roomid}", ConnectionStringSetting = "ConnectionSetting")>]data: IEnumerable<RoomIdStreamingStatus>) (log: ILogger) =
        async {
            log.LogInformation("Get RoomInfo Function processed a request.")

            return OkObjectResult(data) :> IActionResult
        } |> Async.StartAsTask

    //player‚ÌId‚©‚ç‰ž‰‡Point‚ð•Ô‚· (id -> [{"id","point"}])
    [<FunctionName("GetPoint")>]
    let GetPointRun ([<HttpTrigger(AuthorizationLevel.Function, "get", Route = "getPoint/{id}")>]req: HttpRequest) ([<Sql("select * from dbo.[Point] where Id = @Id", CommandType = System.Data.CommandType.Text, Parameters = "@Id={id}", ConnectionStringSetting = "ConnectionSetting")>]idandpoint: IEnumerable<IdAndPoint>) (log: ILogger) =
        async {
            log.LogInformation("Get Point Function processed a request")


            return OkObjectResult(idandpoint) :> IActionResult
        } |> Async.StartAsTask

    //ˆêˆÓ‚ÌroomId‚ð•Ô‚·Function (null->string)
    [<FunctionName("GetRoomNum")>]
    let GetRoomNumRun([<HttpTrigger(AuthorizationLevel.Function,"get", Route = "GetRoomNum")>]req: HttpRequest) (log: ILogger) =
        async {
            log.LogInformation("Get Room Num processed a request.")
            
            let NewRoomId = 
                Guid.NewGuid().ToString()
                               
            return OkObjectResult(NewRoomId) :>IActionResult
        } |> Async.StartAsTask
    

