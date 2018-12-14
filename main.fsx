open animals
open System
open System.IO

/// <summary> The CLT AnimalExperiment conducts evils tests on animals,
/// running a matrix lifelike simulation on unknowning and unsuspecting
/// animals!!</summary>
/// <param name="T"> T is an int, representing how many ticks to run</param>
/// <param name="n"> n is the number represention the board witdh</param>
/// <param name="e"> e represents the amount of moose to start off with</param>
/// <param name="fegl"> fegl is the time to breed a new moose</param>
/// <param name="u"> u represents the amount of wolves to start off with</param>
/// <param name="fulv"> fulv is the time to breed a new wolf</param>
/// <param name="s"> s represents hunger for the wolves before starvation</param>
/// <param name="v"> v is a bool, verbose true or false. Default is false</param>
/// <param name="out"> out is a string for the output, default is test.txt</param>
/// <remarks> Would have been smart if the first argument or
/// something was a name of a text file to write to </remarks>
/// <returns> A text file called test.txt with results </returns>
[<EntryPoint>]
let main args =
    let mutable str = "Wolf,  Moose\n"
    match args with
    // With 9 arguments
    | [|T; n; e; felg; u; fulv; s; v;out|] ->
        let isle = animals.environment(int32(n), int32(e), int32(felg), int32(u), int32(fulv), int32(s), Convert.ToBoolean(v))
        for i in 0..int32(T) do
            str <- str + sprintf "%3s, %3s\n" (string(isle.board.wolves.Length)) (string(isle.board.moose.Length))
            if (Convert.ToBoolean(v)) then
                printfn "In the year %d" i
                printfn "%A" isle
            isle.tick ()
        File.WriteAllText(string(out), str)
    
    // With 8 arguments
    | [|T; n; e; felg; u; fulv; s; v|] ->
        let isle = animals.environment(int32(n), int32(e), int32(felg), int32(u), int32(fulv), int32(s), Convert.ToBoolean(v))
        for i in 0..int32(T) do
            str <- str + sprintf "%3s, %3s\n" (string(isle.board.wolves.Length)) (string(isle.board.moose.Length))
            if (Convert.ToBoolean(v)) then
                printfn "In the year %d" i
                printfn "%A" isle
            isle.tick ()
        File.WriteAllText("test.txt", str)
    
    // With 7 arguments
    | [|T; n; e; felg; u; fulv; s|] ->
        let isle = animals.environment(int32(n), int32(e), int32(felg), int32(u), int32(fulv), int32(s), false)
        for i in 0..int32(T) do
            str <- str + sprintf "%3s, %3s\n" (string(isle.board.wolves.Length)) (string(isle.board.moose.Length))
            isle.tick ()
        File.WriteAllText("test.txt", str)
    
    // Anything else
    | _ -> printfn """
    The CLT AnimalExperiment conducts evils tests on animals, running 
    a matrix lifelike simulation on unknowning and unsuspecting animals!!

    mono animalExperiment.exe [T] [] [] [] [] [] [] []

    [T]     T is an int, representing how many ticks to run
    [n]     n is the number represention the board witdh
    [e]     e represents the amount of moose to start off with
    [fegl]  fegl is the time to breed a new moose
    [u]     u represents the amount of wolves to start off with
    [fulv]  fulv is the time to breed a new wolf
    [s]     s represents hunger for the wolves before starvation
    [v]     v is a bool, verbose true or false. Default is false
    [out]   out is a string for the output, default is test.txt
    
    
    Eg: mono main.exe 50 10 10 10 10 10 false filename.txt"""
    /// Exit code
    0