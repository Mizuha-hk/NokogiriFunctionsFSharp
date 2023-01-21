namespace NameAndAid

type RoomIdStreamingStatus() =
    member val room = "" with get, set
    member val id1 = "" with get, set
    member val id2 = "" with get, set
    member val isStreaming = "" with get, set
    
type IdAndPoint() =
    member val id = "" with get, set
    member val point = "" with get, set

type NameAndId() =
    member val name = "" with get, set
    member val id = "" with get, set