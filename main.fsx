open animals 
open System
[<EntryPoint>]
let main args =
    match args with 
    | [|T; n; e; felg; u; fulv; s; v|] ->
        let isle = animals.environment(int32(n), int32(e), int32(felg), int32(u), int32(fulv), int32(s), Convert.ToBoolean(v))
        for i in 0..int32(T) do
            isle.tick ()
            if (Convert.ToBoolean(v)) then
                printfn "In the year %d" i
                printfn "%A" isle // The board after i tick(s)
    | [|T; n; e; felg; u; fulv; s|] ->
        let isle = animals.environment(int32(n), int32(e), int32(felg), int32(u), int32(fulv), int32(s), false)
        for i in 0..int32(T) do
            isle.tick ()
            
    | _ -> printfn "Must have 7 or 8 arguemnts."


    0