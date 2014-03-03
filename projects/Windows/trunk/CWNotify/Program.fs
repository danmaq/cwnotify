module Program

open System
open System.Threading
open System.Windows.Forms

/// <summary>Entry Point.</summary>
/// <param name="argv">Arguments.</param>
/// <returns>Exit code.</returns>
[<EntryPoint>]
[<STAThread>]
let main argv =
    use mutex = new Mutex(false, Resources.productName)
    if mutex.WaitOne(0, false) then
        match Resources.apikey with
            | null ->
                let msg = sprintf @"ユーザー環境変数 %s にChatWork APIキーを設定してください。" Resources.API_KEY_NAME
                MessageBox.Show(msg, Resources.productName) |> ignore
                2
            | _ ->
                Notify.start()
                0
    else 1
