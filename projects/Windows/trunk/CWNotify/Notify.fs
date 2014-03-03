module Notify

open System
open System.Media
open System.Windows.Forms

/// <summary>
/// display a notification icon. If icon fails to display, and then retry automatically.
/// </summary>
/// <param name="visible"></param>
/// <param name="prevTick">previous tick time.</param>
let rec viewIcon visible prevTick =
    true |> visible
    let tick = Environment.TickCount - prevTick
    if tick >= 4000 then
        false |> visible
        viewIcon visible tick

/// <summary></summary>
/// <param name="notify"></param>
/// <param name="num"></param>
let showBalloon (notify:NotifyIcon) num =
    if num > 0 then
        SystemSounds.Asterisk.Play()
        notify.BalloonTipText <- sprintf @"%d 件の新着" num
        notify.ShowBalloonTip 10000;

/// <summary></summary>
let showAbout() =
    MessageBox.Show(Resources.aboutMessage, Resources.productName) |> ignore

let showPreference() =
    Presentation.Starter.CreatePreference() |> ignore

/// <summary></summary>
/// <param name="notify"></param>
/// <returns>ContextMenuStrip object.</returns>
let createContextMenuStrip notify =
    let cms = new ContextMenuStrip()
    cms.PreviewKeyDown.Add <| CancelVanish.onPreviewKeyDown
    cms.Closing.Add <| CancelVanish.onClosing notify
    ("&Preference" |> cms.Items.Add).Click.Add <| fun args -> showPreference()
    ("&About" |> cms.Items.Add).Click.Add <| fun args -> showAbout()
    ("E&xit" |> cms.Items.Add).Click.Add <| fun args -> Application.Exit()
    cms

/// <summary></summary>
let start() =
    use notify = new NotifyIcon()
    notify.Text <- Resources.productName
    notify.ContextMenuStrip <- createContextMenuStrip notify
    notify.Icon <- Resources.icon
    viewIcon (fun b -> notify.Visible <- b) Environment.TickCount
    use timer = TimerLoop.createTimer (notify |> showBalloon)
    Application.Run()
