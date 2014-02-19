module TimerLoop

open System
open System.Net
open System.Windows.Forms

/// <summary></summary>
[<Literal>]
let INTERVAL = 30

/// <summary></summary>
/// <returns>JSON Strings.</returns>
let downloadAsync() =
    async {
        use client = new WebClient()
        client.Headers.Add(@"X-ChatWorkToken", Resources.apikey)
        let uri = new Uri @"https://api.chatwork.com/v1/my/status"
        let! result = uri |> client.AsyncDownloadString
        return result
    } |> Async.RunSynchronously

/// <summary></summary>
/// <returns>Unreaded message count.</returns>
let getUnreadCount() =
    Maybe.maybe
        {
            let! status = downloadAsync() |> MyStatus.ParseJSON
            return status.unread_num
        }

/// <summary></summary>
/// <param name="notify"></param>
/// <returns>Timer object.</returns>
let createTimer notify =
    let timer = new Timer()
    timer.Interval <- INTERVAL * 1000
    timer.Tick.Add <| fun argv ->
        match getUnreadCount() with
            | Some n -> notify n
            | _ -> ()   // Failed.
    timer.Start()
    timer
