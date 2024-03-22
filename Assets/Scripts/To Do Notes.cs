
/*
    - Find tilemap we can use while building everything else
        - Needs to have at least a platforms and a death/spikes tilemap
        - If we can't find anything, lets just use the Tilevania ones
    - Find character art we can use while building everything else
    - Refine player controller
        - Might even want to consider totally rebuilding to be honest, not from scratch but starting over using parts from the old controller
        - I personally dont feel like it can be fully tested without animations on the player or at least the players model and size
    - Camera exploration
    - Make level 1 prototype and get testing feedback
    - Sort everything out with the input system
        - Players spawning in from a main menu, not just when they press a button
        - Where the ball spawns from scene to scene, not just a prefab
        - Where the player spawns from scene to scene, not just in the prefab
    - Start menu, pause menu
    - Saving game and loading
    - Final character art
        - Doesn't actually have to be final, but at this point we need to start going in this direction
    - Final tilemap art
        - Again, not actually final
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    - can tweak the collision velocity reset for horizontal stuff
    - set up a player select screen
    - set up a way to reset the ball (can just be a cheat code thing)
    - set up a way to reset the scene/respawn the player if they bug out
    - can start actually looking at level design with the jump reset mechanic!!
    - can start concepting ideas for art and what not
    - set up script for Player1 to be a certain color and Player2 to be another
    - set up player rotation on turn - DONE but not implemented
    - set up light animations
    - add time to wait until next jump reset - DONE
    - death and respawn mechanics - DONE
    - camera follow on both players - DONE
    - code cleanup on player controller
    - project cleanup on assets, making more folders for different types of scripts and stuff


    For now lets keep the mechanics pretty simple, and focus on the environment doing the work
    Ris mentioned something that makes the ball move slower


    The "Need to Have":
    - Gameplay: Two players working together to get both players to the objective. Using mechanical skill and coordination between two players to reach objective
        - Need to have times where it's easy to get one player over but hard to get both
    - Story: The two are best friends, and their bond is not the turmoil of the story - that stays strong always

    Story:
    - Two best friends who are always solid - "goofy" individuals who lose track of time
    - Left behind because they were doing something dumb - or maybe they have to go on an important mission as volunteer because they weren't paying attention
    - They are less fortunate, so they are scrappy - (poor, do they have parents?)


    - When having the players be two different images, we can make a script that when they enter they either have sprite 1 or 2?


    Known Fixes that must be made:
    - Sometimes (not sure why yet), the player will automatically pass the ball back instantly
    - Player can catch ledges and jump
    - Might be a weird double jump interaction when you throw the ball
    - Camera sucks right now, doesnt really work when on the move making precise jumps


    Prototype To Do:
    - Basically everything related to art
    - Animations
    - Change from box to capsule collider
    

*/