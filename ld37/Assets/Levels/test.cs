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
            "r e e e e e e C",
            "e e f e e e w e",
            "e r e r e w e e",
            "e e c p e e e e",
            "e e e e e c e e",
            "e e w e r e r e",
            "e w e e e f e e",
            "f e e e e e e r"
            },
            new string[]{
            "X e e e e e e c",
            "e e f e e e w e",
            "e r e r e w e e",
            "e e c p e e e e",
            "e e e e e c e e",
            "e e w e r e r e",
            "e w e e e f e e",
            "f e e e e e e R"
            },
            new string[]{
            "X e e e e e e C",
            "e e f e e e w e",
            "e r e r e w e e",
            "e e c p e e e e",
            "e e e e e c e e",
            "e e w e r e r e",
            "e w e e e f e e",
            "F e e e e e e R"
            },
        // maps 0
            new string[]{
            "e e e e r e e e",
            "w r w w w e c e",
            "e e e e r e e e",
            "e e e c c e e e",
            "e e e e c r e e",
            "e F w e e f e e",
            "p e f e e f e e",
            "c e f e e e e f"
            },
            new string[]{
            "e e e e r e e e",
            "w r w w w e c e",
            "e e e e r e e e",
            "e e e c c e e e",
            "e e e e c r e e",
            "e F w e e f e e",
            "p e f e e f e e",
            "c e f e e e e f"
            },
            new string[]{
            "e e e e R e e e",
            "w r w w w e c e",
            "e e e e r e e e",
            "e e e c c e e e",
            "e e e e c r e e",
            "e F w e e f e e",
            "p e f e e f e e",
            "c e f e e e e F"
            },
            new string[]{
            "e e e e R e e e",
            "w r w w w e c e",
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
            "e e e w F e e e"
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
            "e e e w F e e e"
            },
         
        };
    }
}