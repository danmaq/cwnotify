module Notify

open Printf
open System
open System.Drawing
open System.Net
open System.Windows.Forms

/// <summary>
/// Notification icon will not disappear even if pressing the Alt + F4 key.
/// </summary>
/// <param name="notify">Instance of notification icon.</param>
/// <param name="cms">Instance of context menu.</param>
let cancelAltF4 notify (cms:ContextMenuStrip) =
    cms.PreviewKeyDown.Add <| CancelVanish.onPreviewKeyDown
    cms.Closing.Add <| CancelVanish.onClosing notify

/// <summary>
/// display a notification icon. If icon fails to display, and then retry automatically.
/// </summary>
/// <param name="notify">Instance of notification icon.</param>
/// <param name="prevTick">previous tick time.</param>
let rec viewIcon (notify: NotifyIcon) prevTick =
    notify.Visible <- true
    let tick = Environment.TickCount - prevTick
    if tick >= 4000 then
        notify.Visible <- false
        viewIcon notify tick

let downloadAsync = fun () ->
    ()

/// <summary></summary>
/// <param name="notify">Instance of notification icon.</param>
/// <param name="apikey">API key of ChatWork.</param>
let createTimer (notify: NotifyIcon) apikey =
    let timer = new Timer()
    timer.Interval <- 30 * 1000
    timer.Tick.Add <| fun argv ->
        let data =
            async {
                let client = new WebClient()
                client.Headers.Add(@"X-ChatWorkToken", apikey)
                let! result = client.AsyncDownloadString <| new Uri @"https://api.chatwork.com/v1/my/status"
                return result
            } |> Async.RunSynchronously
        let num =
            Maybe.maybe
                {
                    let! status = data |> MyStatus.ParseJSON
                    return status.unread_num
                }
        match num with
            | Some n when n > 0 ->
                notify.BalloonTipText <- sprintf @"%d 件の新着" n
                notify.ShowBalloonTip 10000;
                ()
            | _ -> ()
        ()
    timer.Start()
    timer

/// <summary></summary>
/// <param name="appName">Application name.</param>
/// <param name="apikey">API key of ChatWork.</param>
let start appName apikey =
    use notify = new NotifyIcon()
    notify.Text <- appName
    notify.ContextMenuStrip <-
        let cms = new ContextMenuStrip()
        cancelAltF4 notify cms
        ("E&xit" |> cms.Items.Add).Click.Add <| fun args -> Application.Exit()
        cms
    notify.Icon <- SystemIcons.Application
    viewIcon notify Environment.TickCount
    use timer = createTimer notify apikey
    Application.Run()
