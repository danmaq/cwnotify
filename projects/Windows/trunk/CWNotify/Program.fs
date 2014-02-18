module Program

open Printf
open System
open System.Threading
open System.Windows.Forms

[<Literal>]
let APP_NAME = @"ChatWork Notify";

[<Literal>]
let CW_API_KEY = @"CW_API_KEY";

/// <summary>Entry Point.</summary>
/// <param name="argv">Arguments.</param>
/// <returns>Exit code (0).</returns>
let mutexMain argv =
    let apikey = Environment.GetEnvironmentVariable(CW_API_KEY, EnvironmentVariableTarget.User)
    if apikey = null then
        let msg = sprintf @"ユーザー環境変数 %s にChatWork APIキーを設定してください。" CW_API_KEY
        MessageBox.Show(msg, APP_NAME) |> ignore
        2
    else
        Notify.start APP_NAME apikey
        0

/// <summary>Entry Point.</summary>
/// <param name="argv">Arguments.</param>
/// <returns>Exit code.</returns>
[<EntryPoint>]
let main argv =
    use mutex = new Mutex(false, APP_NAME)
    if mutex.WaitOne(0, false) then mutexMain argv else 1
