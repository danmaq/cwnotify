module Notify

open System
open System.Drawing
open System.Reflection
open System.Runtime.InteropServices
open System.Windows.Forms

/// <summary>
/// Retrieves a handle to the foreground window (the window with which the user is currently working).
/// </summary>
/// <returns>The return value is a handle to the foreground window.</returns>
[<DllImport(@"user32")>]
extern IntPtr GetForegroundWindow();

/// <summary>Returns the Windows desktop window.</summary>
/// <returns>Identifies the Windows desktop window.</returns>
[<DllImport(@"user32")>]
extern IntPtr GetDesktopWindow();

/// <summary>
/// Brings the thread that created the specified window into the foreground and activates the window.
/// </summary>
/// <param name="hWnd">
/// A handle to the window that should be activated and brought to the foreground. 
/// </param>
/// <returns>If the window was brought to the foreground, the return value is nonzero.</returns>
[<DllImport(@"user32")>]
extern [<MarshalAs(UnmanagedType.Bool)>] bool SetForegroundWindow(IntPtr hWnd);

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

/// <summary>
/// Notification icon will not disappear even if pressing the Alt + F4 key.
/// </summary>
/// <param name="notify">Instance of notification icon.</param>
/// <param name="cms">Instance of context menu.</param>
let cancelAltF4 (notify: NotifyIcon) (cms:ContextMenuStrip) =
    let setForeground = fun () -> SetForegroundWindow(GetDesktopWindow()) |> ignore
    cms.PreviewKeyDown.Add <|
        fun args -> if args.Alt then setForeground()
    cms.Closing.Add <|
        fun args ->
            let fi =
                typeof<NotifyIcon>.GetField(
                    "window", BindingFlags.NonPublic ||| BindingFlags.Instance)
            if GetForegroundWindow() = (fi.GetValue(notify) :?> NativeWindow).Handle
            then setForeground()

let start appName =
    let notify = new NotifyIcon()
    //notify.BalloonTipText <- "FUGA"
    notify.Text <- appName;
    notify.ContextMenuStrip <-
        let cms = new ContextMenuStrip()
        cancelAltF4 notify cms
        ("E&xit" |> cms.Items.Add).Click.Add <| fun args -> Application.Exit()
        cms
    notify.Icon <- SystemIcons.Application
    viewIcon notify Environment.TickCount
    Application.ApplicationExit.Add <| fun args -> notify.Dispose()
    Application.Run()
