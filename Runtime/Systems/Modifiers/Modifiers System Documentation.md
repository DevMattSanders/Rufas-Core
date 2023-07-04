## Overview
- A generic modifier system that can be used to add and remove effects to game entities (for example: a character burning or an object disabling gravity).  

## Modifiers (Scriptable Object)
- Create a individual scriptable object for each type of modifier that inherits from this class.
- Contains a few virtual functions that can be overridden.
	- Can Run (returns a bool)
	- Enable and Disable
	- Tick Modifier (called every frame via the modifier target) 

## Modifier Target (Monobehaviour)
- Add this component to the game objects you want to be 'modifiable'.  This component contains a list of it's current modifiers and will call the functions on each individual modifier scriptable object.