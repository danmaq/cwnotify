﻿module MyStatus

open System.IO
open System.Runtime.Serialization
open System.Runtime.Serialization.Json
open System.Text

/// <summary>Response of "my/status"</summary>
[<DataContract>]
type MyStatus() =

    /// <summary></summary>
    [<DataMember>]
    member val unread_room_num = 0 with get, set

    /// <summary></summary>
    [<DataMember>]
    member val mention_room_num = 0 with get, set

    /// <summary></summary>
    [<DataMember>]
    member val mytask_room_num = 0 with get, set

    /// <summary>Unread messages.</summary>
    [<DataMember>]
    member val unread_num = 0 with get, set

    /// <summary></summary>
    [<DataMember>]
    member val mention_num = 0 with get, set

    /// <summary>Numbers of total tasks.</summary>
    [<DataMember>]
    member val mytask_num = 0 with get, set

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    override this.ToString() =
        sprintf
            "MyStatus[unread_room_num:%d, mention_room_num:%d, mytask_room_num:%d, unread_num:%d, mention_num:%d, mytask_num:%d"
            this.unread_room_num this.mention_room_num this.mytask_room_num this.unread_num this.mention_num this.mytask_num

/// <summary>Parsing JSON Strings.</summary>
/// <param name="json">JSON Strings.</param>
/// <returns>If parsed JSON correctly, will return instance of <c>MyStatus</c>.</returns>
let ParseJSON (json:string) =
    use stream = new MemoryStream(json |> Encoding.UTF8.GetBytes)
    let serializer = DataContractJsonSerializer(typeof<MyStatus>)
    match stream |> serializer.ReadObject with
        | :? MyStatus as result -> Some result
        | _ -> None
