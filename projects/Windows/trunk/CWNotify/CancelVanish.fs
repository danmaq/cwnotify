module CancelVanish

open System.Reflection
open System.Runtime.InteropServices
open System.Windows.Forms

/// <summary>
/// Retrieves a handle to the foreground window (the window with which the user is currently working).
/// </summary>
/// <returns>The return value is a handle to the foreground window.</returns>
[<DllImport(@"user32")>]
extern nativeint GetForegroundWindow()

/// <summary>Returns the Windows desktop window.</summary>
/// <returns>Identifies the Windows desktop window.</returns>
[<DllImport(@"user32")>]
extern nativeint GetDesktopWindow()

/// <summary>
/// Brings the thread that created the specified window into the foreground and activates the window.
/// </summary>
/// <param name="hWnd">
/// A handle to the window that should be activated and brought to the foreground. 
/// </param>
/// <returns>If the window was brought to the foreground, the return value is nonzero.</returns>
[<DllImport(@"user32")>]
extern [<MarshalAs(UnmanagedType.Bool)>] bool SetForegroundWindow(nativeint hWnd)

/// <summary></summary>
/// <returns>If the window was brought to the foreground, the return value is nonzero.</returns>
let setForeground = fun () -> SetForegroundWindow(GetDesktopWindow())

/// <summary></summary>
/// <param name="args">Event arguments.</param>
let onPreviewKeyDown (args:PreviewKeyDownEventArgs) =
    if args.Alt then setForeground() |> ignore

/// <summary></summary>
/// <param name="args">Event arguments.</param>
let onClosing notify args =
    let fi =
        let bf = BindingFlags.NonPublic ||| BindingFlags.Instance
        typeof<NotifyIcon>.GetField(@"window", bf)
    match fi.GetValue(notify) with
        | :? NativeWindow as window
            when window.Handle = GetForegroundWindow() ->
                setForeground() |> ignore
        | _ -> ()
