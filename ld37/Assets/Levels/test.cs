using System.Collections;
using System.Collections.Generic;

namespace flyyoufools {
    // Level "format":
    // p player
    // e empty
    // w inmovable stlp
    // W inmovable debris
    // r rook - dorovnavac
    // x rook, ktory ide najprv po xovej osi
    // c charging enemy - dumb juan
    // f following enemy - smart juan - "From the shadows I come." - Stalker
    // Ked je to kapitalkami, je to possessed
    public class TestLevel {
        static public List<string[]> maps = new List<string[]>() {
        // onboarding
            new string[]{
            "C e e e e e e r",
            "e e e e e e e e",
            "e e e e e f e e",
            "e f e C e e e e",
            "e e e e e e e e",
            "r e W W W W e e",
            "e e W e e W e e",
            "e e W p e W e C",
            },
            new string[]{
            "F f f e e e e r",
            "e e e c e e e e",
            "e e e e e e e e",
            "W w W w W e e e",
            "e e e e e e e e",
            "e e c W w W w W",
            "e e e e e e e e",
            "p e e e e e e c",
            },
            new string[]{
            "e e e e e e e X",
            "e e e e e e e e",
            "e e c e e c e e",
            "e e e e e e e e",
            "e e e e e e e e",
            "W w W w W w W f",
            "e e c e e e w f",
            "p e W e e e W f",
            },
            new string[]{
            "e e e e e e e r",
            "e e e e e e e e",
            "e e c e e c e e",
            "e e e e e e e e",
            "W w W w W w W f",
            "e e e e e e W e",
            "e e e e e C w f",
            "p e W e e e W f",
            },
            new string[]{
            "e e e R e r e e",
            "e e e e e e e e",
            "e e c e e c e e",
            "w e w e w e e e",
            "e e W e W w e f",
            "W e w e w e e e",
            "p w W e W w W F",
            "e e e e e e e e",
            },
            new string[]{
            "e e e e e r e e",
            "e e e e e e e e",
            "e R c e e c e e",
            "e w e e e e e e",
            "W w e f e e e f",
            "F w x e e e e e",
            "e w W w W w W e",
            "p e e e e e e e",
            },
        // maps 2
            new string[]{
            "X e e e e e e c",
            "e e f e e e w e",
            "e r e r e W e e",
            "e e c p e e e e",
            "e e e e e c e e",
            "e e W e r e r e",
            "e w e e e f e e",
            "f e e e e e e R"
            },
            new string[]{
            "X e e e e e e C",
            "e e f e e e w e",
            "e r e r e W e e",
            "e e c p e e e e",
            "e e e e e c e e",
            "e e W e r e r e",
            "e w e e e f e e",
            "F e e e e e e R"
            },
        // maps 0
            new string[]{
            "e e e e r e e e",
            "w r W w W e c e",
            "e e e e r e e e",
            "e e e c c e e e",
            "e e e e c r e e",
            "e F w e e f e e",
            "p e f e e f e e",
            "c e f e e e e f"
            },
            new string[]{
            "e e e e R e e e",
            "w r W w W e c e",
            "e e e e r e e e",
            "e e e c c e e e",
            "e e e e c r e e",
            "e F w e e f e e",
            "p e f e e f e e",
            "c e f e e e e F"
            },
            new string[]{
            "e e e e R e e e",
            "w r W w W e c e",
            "e e e e r e e e",
            "e e e c c e e e",
            "e e e e C r e e",
            "e F w e e f e e",
            "p e f e e f e e",
            "c e f e e e e F"
            },
        // maps 1
            new string[]{
            "e e R e f e e e",
            "e c e e e e r e",
            "e e r e e w e e",
            "e r w f e e f e",
            "e e c e e w e e",
            "e e e e e e w e",
            "p e c w e f e e",
            "e e e W F e e e"
            },
            new string[]{
            "e e R e f e e e",
            "e c e e e e r e",
            "e e r e e w e e",
            "e R w f e e f e",
            "e e c e e w e e",
            "e e e e e e w e",
            "p e c w e f e e",
            "e e e w F e e e"
            },
            new string[]{
            "e e R e F e e e",
            "e c e e e e r e",
            "e e r e e w e e",
            "e r w f e e f e",
            "e e c e e w e e",
            "e e e e e e w e",
            "p e C w e f e e",
            "e e e W F e e e"
            },
         
        };

        static public List<string> texts = new List<string>() {
            @"-It's bloody raining again... - John sighed, as he made his way to the so-called haunted manor. He opened the doors, entered the hallway and the door shut close behind him. 
            
-It might actually be haunted after all...Time to dust off the cleansing ritual...-

Arrow Keys - Movement
Space - Meditation to refill energy
Ctrl - Show enemy movement",
            @"-These folks once again... They like to be touchy, I hate them.-

Mannequins are spirits that remember their once human nature. They seek a body to possess, when there's nobody to possess they possess human shaped objects such as dolls or mannequins.",
            @"-Nightmares... Don't close your eyes for too long, John...-

Nightmares are spirits that possess various fabrics - clothes, curtains etc.
They creep up on unsuspecting victims and try to strangle them. They teleport when no one is looking.",
            @"-... Always angry, always loud...-

Poltergeists are evil spirits that possess stronger degrees of telekinesis. They prefer possessing furniture pieces which they recklessly fling around.",
            
            @"-...I wonder where I got the idea that it was peaceful in the countryside. It's a bloody war-zone. Slowly but surely, everything's slipping into bad craziness again.",
            @"-Hmm. I don't know if I'll get the hang of this survivalist business. I mean, what about washing -- and where's the toilet?-",
            @"-The spirits aren't so different from anyone else. They see what they want to see.-",
            @"-When it comes to arrogant parasites, I've got a short fuse. So look out, suckers, here comes the revolution!-",
            @"- I mean, I'm going to be forty in a couple of years. I can't go on pissing about with magic and stuff forever. Might be time I wised up a bit...",
            @"-Disaster's snapping at my heels and it's time that I was somewhere far away. It's all up to me again, ennit? Somehow, I've got to stay ahead and get some news aces up my sleeve. But right now, all I really need's a smoke.-",
            @"-Aaaand another one!-",
            @"-Okay, I'm just about empty.",
            @"-I'm exhausted, I think I should finish this quickly.",
            @"-Just one more day, to keep the ghosts away!-",
            @"-It would be lame, if I stumbled on the 14th day...aye?-",



        };

        static public List<int> moves = new List<int>() {
            3, 4, 4, 4, 4, 
            4, 4, 4, 4, 4, 
            4, 4, 4, 4, 4 
        };
    }
}