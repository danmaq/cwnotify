namespace Danmaq.CWNotify

/// <summary>Maybe Monad.</summary>
module Maybe =
    type MaybeBuilder() =

        /// <summary>Called for <c>let!</c> and <c>do!</c> in computation expressions.</summary>
        member this.Bind(x, f) =
            match x with
                | Some y -> f y
                | None -> None

        /// <summary>Called for <c>return!</c> in computation expressions.</summary>
        member this.Return(x) =
            x |> Some

    /// <summary>Maybe Monad.</summary>
    let maybe = MaybeBuilder()
