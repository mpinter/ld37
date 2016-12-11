namespace flyyoufools {
    // Level "format":
    // p player
    // e empty
    // w wall
    // r rook - dorovnavac
    // x rook, ktory ide najprv po xovej osi
    // c charging enemy - dumb juan
    // f following enemy - smart juan - "From the shadows I come." - Stalker
    // Ked je to kapitalkami, je to possessed
    public class TestLevel {
        static public List<string[]> maps = new List<string>();

        maps.Add({
            "e e e e R e e e",
            "w r w w w e c e",
            "e e e e r e e e",
            "e e e c c e e e",
            "e e e e C r e e",
            "e F w e e f e e",
            "p e f e e f e e",
            "c e f e e e e F"
            });

        maps.Add({
            "e e R e F e e e",
            "e c e e e e r e",
            "e e r e e w e e",
            "e r w f e e f e",
            "e e c e e w e e",
            "e e e e e e w e",
            "p e C w e f e e",
            "e e e w F e e e"
            });
    }
}