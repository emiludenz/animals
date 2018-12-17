module animals
type symbol = char
type position = int * int
type neighbour = position * symbol
val mSymbol : symbol
val wSymbol : symbol
val eSymbol : symbol
val rnd : System.Random
type animal =
  class
  /// <summary>Create a new animal represented with symbol symb and which reproduces every repLen ticks.</summary>
  /// <param name="symb">A symbol (character) used to represent the animal type when the board is printed.</param>
  /// <param name="repLen">The time in ticks until the animal attempts to produce an offspring.</param>
    new : symb:symbol * repLen:int -> animal
    override ToString : unit -> string
    member position : position option
    member reproduction : int
    member symbol : symbol
    member resetReproduction : unit -> unit
    member position : position option with set
    member updateReproduction : unit -> unit
  end
type moose =
  class
    inherit animal
    /// <summary>Create a moose with symbol 'm'.</summary>
    /// <param name="repLen">The number of ticks until a moose attempts to produce an offspring.</param>
    new : repLen:int -> moose
    member tick : unit -> moose option
  end
type wolf =
  class
    inherit animal
    /// <summary>Create a wolf with symbol 'w'.</summary>
    /// <param name="repLen">The number of ticks until a wolf attempts to produce an offspring.</param>
    /// <param name="hungLen">The number of ticks since it last ate until a wolf dies.</param>
    new : repLen:int * hungLen:int -> wolf
    member hunger : int
    member resetHunger : unit -> unit
    member tick : unit -> wolf option
    member updateHunger : unit -> unit
  end
type board =
  {width: int;
   mutable moose: moose list;
   mutable wolves: wolf list;}
type environment =
  class
  /// <summary>Create a new environment.</summary>
  /// <param name="boardWidth">The width of the board.</param>
  /// <param name="NMooses">The initial number of moose.</param>
  /// <param name="NWolves">The initial number of wolves.</param>
  /// <param name="mooseRepLen">The number of ticks until a moose attempts to produce an offspring.</param>
  /// <param name="wolvesRepLen">The number of ticks until a wolf attempts to produce an offspring.</param>
  /// <param name="wolvesHungLen">The number of ticks since it last ate until a wolf dies.</param>
  /// <param name="verbose">If the verbose flag is true, then messages are printed on screen at key events.</param>
    new : boardWidth:int * NMooses:int * mooseRepLen:int * NWolves:int *
          wolvesRepLen:int * wolvesHungLen:int * verbose:bool -> environment
    override ToString : unit -> string
    member board : board
    member count : int
    member size : int
    member tick : unit -> unit
  end

