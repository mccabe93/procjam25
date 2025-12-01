ProcJam 2025 submission created in ~1.5 days. I worked along with the artist nebelstern https://www.instagram.com/teknomegami/

Game: https://mumike.itch.io/frozen-zombies-in-space

The core of the game, and the jam, is a random level generator. I aimed to keep the level generation models flexible for porting into other game engines, and I think that goal was accomplished.

Level Generation Methodology:
1. Generate rooms and connecting hallways.
2. Populate each room via cellular automata.

Rooms and Halls Algorithm (Assets/Source/Models/RoomHallGenerator.cs)
For each room i in N rooms...
1. Generate a random width and height for room.
2. Generate a random hallway length for room.
3. Randomly choose a Side (Above, Below, Left or Right) until placement of room will not collide with any other room.
4. Fill the room in the grid then draw its hallway to the previous room (if not first room)

Cellular Automata Algorithm (Assets/Source/Models/CellularAutomataGenerator.cs)
The general CA is the classic roguelike cave generation CA. You can read more about it here: https://www.roguebasin.com/index.php?title=Cellular_Automata_Method_for_Generating_Random_Cave-Like_Levels
I use the automata as an iterative process for populating rooms. It does not determine the layout of a room in this game. Instead each iteration of the automata is used in placing tile variants and enemies. 

It's easier to understand visually so let me give an example.
```
Epoch 0 (Random Fill)
# # # # #
# . # . #
# # # . #
# . # . #
# # # # #
```

We use Epoch 1 to create tile variants. Since this is a destructive CA and each epoch can be assumed to be usually smaller than the previous epoch, we can take advantage of this to place everything interesting closeby, and we get a fresh state to work from every epoch allowing us a clean implementation.

```
Epoch 1 (Tile Variants)
# # # # #
# . . . #
# # # . #
# # . # #
# # # # #
```
We can now select random tile variants for the non-edge cells.

```
Epoch 1 (Tile Variants - Translated)
# # # # #
# . . . #
# ^ ^ . #
# ^ . ~ #
# # # # #
```
We make this translation and retain the original state. So now we can iterate again.

```
Epoch 2 (Spawn Actors)
# # # # #
# . . . #
# # . . #
# . . # #
# # # # #
```
The two remaining non-edge cells can be used to spawn enemies. Our actors will always spawn in the regions marked with tile variants. This can be desirable, but I've included a fill method that can double as a way to search for clumps of empty tiles as well and populate them. In this game we don't make use of the empty tiles.

That's the whole idea! Hope this is helpful to somebody out there.
