module animals

type symbol = char
type position = int * int
type neighbour = position * symbol

let mSymbol : symbol = 'm'
let wSymbol : symbol = 'w'
let eSymbol : symbol = ' '
let rnd = System.Random ()

/// An animal is a base class. It has a position and a reproduction counter.
type animal (symb : symbol, repLen : int) =
  let mutable _reproduction = rnd.Next(1,repLen)
  let mutable _pos : position option = None
  let _symbol : symbol = symb
  member this.symbol = _symbol
  member this.position
    with get () = _pos
    and set aPos = _pos <- aPos
  member this.reproduction = _reproduction
  member this.updateReproduction () =
    _reproduction <- _reproduction - 1
  member this.resetReproduction () =
    _reproduction <- repLen
  override this.ToString () =
    string this.symbol

/// A moose is an animal
type moose (repLen : int) =
  inherit animal (mSymbol, repLen)
  member this.tick () : moose option =
    this.updateReproduction()
    if (this.reproduction = 1) then
      this.resetReproduction()
      Some(moose(this.reproduction))
    else None

/// A wolf is an animal with a hunger counter
type wolf (repLen : int, hungLen : int) =
  inherit animal (wSymbol, repLen)
  let mutable _hunger = hungLen
  member this.hunger = _hunger
  member this.updateHunger () =
    _hunger <- _hunger - 1
    if _hunger <= 0 then
      this.position <- None // Starve to death
  member this.resetHunger () =
    _hunger <- hungLen
  member this.tick () : wolf option =
    this.updateHunger()
    this.updateReproduction()
    if (this.reproduction = 1) then
      this.resetReproduction()
      Some(wolf(this.reproduction, hungLen))
    else None

/// A board is a chess-like board implicitly representedy by its width and coordinates of the animals.
type board =
  {width : int;
   mutable moose : moose list;
   mutable wolves : wolf list;}

/// An environment is a chess-like board with all animals and implenting all rules.
type environment (boardWidth : int, NMooses : int, mooseRepLen : int, NWolves : int, wolvesRepLen : int, wolvesHungLen : int, verbose : bool) =
  let _board : board = {
    width = boardWidth;
    moose = List.init NMooses (fun i -> moose(mooseRepLen));
    wolves = List.init NWolves (fun i -> wolf(wolvesRepLen, wolvesHungLen));
  }

  /// Project the list representation of the board into a 2d array.
  let draw (b : board) : char [,] =
    let arr = Array2D.create<char> boardWidth boardWidth eSymbol
    for m in b.moose do
      Option.iter (fun p -> arr.[fst p, snd p] <- mSymbol) m.position
    for w in b.wolves do
      Option.iter (fun p -> arr.[fst p, snd p] <- wSymbol) w.position
    arr

  /// return the coordinates of any empty field on the board.
  let anyEmptyField (b : board) : position =
    let arr = draw b
    let mutable i = rnd.Next b.width
    let mutable j = rnd.Next b.width
    while arr.[i,j] <> eSymbol do
      i <- rnd.Next b.width
      j <- rnd.Next b.width
    (i,j)

  /// <summary> The function corner checks x to see if it is
  /// inside the bounds as defined by the size of the board</summary>
  /// <param name="x">An int to check</param>
  /// <remarks>works only with integers</remarks>
  /// <returns>Some(x) or None</returns>
  let corner x =
    if (x < 0) || (x > _board.width-1) then None
    else Some(x)

  /// <summary>mooseNext checks if a moose is near the wolf</summary>
  /// <param name="b">The board with the moose list and wolf list</param>
  /// <param name="pos">A start postion</param>
  /// <remarks>works only with the types specified</remarks>
  /// <returns>A position</returns>
  let mooseNext (b: board) (pos:position) =
    let arr = draw b
    let mutable targets = []
    for i = -1 to 1 do
      for j = -1 to 1 do
        let x = corner (j+fst(pos))
        let y = corner (i+snd(pos))
        if (x.IsSome && y.IsSome) then
          if (arr.[x.Value,y.Value] = mSymbol) then
            targets <- List.append targets [Some(x.Value,y.Value)]
    if (targets.Length > 0) then
      targets.[rnd.Next (0, targets.Length)] else None

  /// <summary> The function move gets a new position to move to, or returns the starting position</summary>
  /// <param name="b"> The board with the moose list and wolf list</param>
  /// <param name="pos"> A start postion</param>
  /// <remarks> Works only with the type specified</remarks>
  /// <returns> A position</returns>
  let move (b: board) (pos:position) =
    let arr = draw b
    let mutable targets = []
    for i = -1 to 1 do
      for j = -1 to 1 do
        let x = corner (j+fst(pos))
        let y = corner (i+snd(pos))
        if (x.IsSome && y.IsSome) then
          if (arr.[x.Value,y.Value] = eSymbol) then
            targets <- List.append targets [x.Value,y.Value]
    if (targets.Length > 0) then
      targets.[rnd.Next (0, targets.Length)] else pos

  // populate the board with animals placed at random.
  do for m in _board.moose do
       m.position <- Some (anyEmptyField _board)
  do for w in _board.wolves do
       w.position <- Some (anyEmptyField _board)

  member this.size = boardWidth*boardWidth
  member this.count = _board.moose.Length + _board.wolves.Length
  member this.board = _board
  /// <summary> The method tick() handles the two groups of animals.
  /// Each update, the animals either move, breed or feed (wolf only)</summary>
  /// <remarks>Works only with the animals wolf and moose</remarks>
  /// <returns>Nothing, it changes the object that has been initialised</returns>
  member this.tick() =
    let mutable animalCount = 0
    /// Handling the moose population
    do for m in _board.moose do
        if (animalCount < this.size) then
          animalCount <- animalCount + 1
          let tick = m.tick()
          let curPos = Some(move _board m.position.Value)
          // Making them children
          if (tick.IsSome && animalCount < this.size && not(curPos = m.position)) then
            let _m = tick.Value
            animalCount <- animalCount + 1
            _m.position <- curPos
            _board.moose <- List.append _board.moose [_m]
          // Or just move around
          else m.position <- Some(move _board m.position.Value)
        else m.position <- None
    _board.moose <- _board.moose |> List.filter (fun i -> not(i.position = None))

    /// Handling the wolves population
    do for w in _board.wolves do
        if (animalCount < this.size) then
          animalCount <- animalCount + 1
          let tick = w.tick()
          // Eating or breading, if not dead...
          if (w.position.IsSome) then
            let target = mooseNext _board w.position.Value
            let curPos = Some(move _board w.position.Value)
            // Attack!
            if (target.IsSome) then
              _board.moose <- _board.moose |> List.filter (fun i -> not(i.position = target))
              w.position <- target
              w.resetHunger()
            // Birth if it is allowed
            elif (tick.IsSome  && animalCount < this.size && not(curPos = w.position)) then
              animalCount <- animalCount + 1
              let _w = tick.Value
              _w.position <- curPos
              _board.wolves <- List.append _board.wolves [_w]
            // Or just move around
            else w.position <- Some(move _board w.position.Value)
        else w.position <- None
    _board.wolves <- _board.wolves |> List.filter (fun i -> not(i.position = None))

  override this.ToString () =
    let arr = draw _board
    let mutable ret = "  "
    ret <- sprintf "Wolves: %d | Moose: %d\n  " _board.wolves.Length _board.moose.Length
    for j = 0 to _board.width-1 do
      ret <- ret + string (j % 10) + " "
    ret <- ret + "\n"
    for i = 0 to _board.width-1 do
      ret <- ret + string (i % 10) + " "
      for j = 0 to _board.width-1 do
        ret <- ret + string arr.[i,j] + " "
      ret <- ret + "\n"
    ret