# animals


To use this CLT you should have mono installed.


To compile the program, run either ./make.sh or the following command

:$ fsharpc -a animalsSmall.fs && fsharpc -r animalsSmall.dll main.fsx --out:animalExperiment.exe


To use the program the following parameters should be provided to create a new environment, where a text file, called "test.txt", is returned when the program terminates.

:$ mono main.exe [T] [n] [e] [felg] [u] [fulv] [s] [v[OPTIONAL]] [out[OPTIONAL]]

Tick [T]:
How many ticks to run

boardWidth [n]:
The width of the board.

NMooses [e]:
The initial number of moose.

mooseRepLen [felg]:
The number of ticks until a moose attempts to produce an offspring.

NWolves [u]:
The initial number of wolves.

wolvesRepLen [fulv]:
The number of ticks until a wolf attempts to produce an offspring.

wolvesHungLen [s]:
The number of ticks since it last ate until a wolf dies.

verbose [OPTIONAL]: 
If the verbose flag is true, then messages are printed on screen at key events.

output [OPTIONAL]:
Name the file instead of the default "test.txt". Remember ".txt".

Eg. :$ mono main.exe 50 20 5 5 5 5 5 false firstTest.txt
