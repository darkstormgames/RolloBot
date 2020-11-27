using System.Collections.Generic;

namespace RolloBot.Client.Helper.GameObjects
{
    public enum TrackType
    {
        Nitro = 0,
        Retro,
        DLC
    }

    public enum Console
    {
        Switch = 0,
        SNES,
        N64,
        GBA,
        GCN,
        DS,
        Wii,
        _3DS
    }

    public class MK8DTrack
    {
        public string NameDEU { get; private set; }
        public string NameENG { get; private set; }
        public string NameFRA { get; private set; }
        public string NameSPA { get; private set; }
        public string NameITA { get; private set; }
        public string NameJPN { get; private set; }

        public TrackType TrackType { get; private set; }
        public Console Console { get; private set; }

        public MK8DTrack(string nameDEU, string nameENG, string nameFRA, string nameSPA, string nameITA, string nameJPN, TrackType type, Console console)
        {
            this.NameDEU = nameDEU;
            this.NameENG = nameENG;
            this.NameFRA = nameFRA;
            this.NameSPA = nameSPA;
            this.NameITA = nameITA;
            this.NameJPN = nameJPN;

            this.TrackType = type;
            this.Console = console;
        }

        public string GetName(string language)
        {
            switch (language)
            {
                case "deu":
                    return NameDEU;
                case "fra":
                case "spa":
                case "ita":
                case "jpn":
                case "eng":
                default:
                    return NameENG;
            }
        }

        public string GetAbbreviation()
        {
            switch (this.NameDEU)
            {
                case "Mario Kart-Stadion": return "MKS";
                case "Wasserpark": return "WP";
                case "Zuckersüßer Canyon": return "SSC";
                case "Steinblock-Ruinen": return "TR";
                case "Marios Piste": return this.TrackType == TrackType.Nitro ? "MC" : "rMC";
                case "Toads Hafenstadt": return "TH";
                case "Gruselwusel-Villa": return "TM";
                case "Shy Guys Wasserfälle": return "SGF";
                case "Sonnenflughafen": return "SA";
                case "Delfinlagune": return "DS";
                case "Discodrom": return "ED";
                case "Wario-Abfahrt": return "MW";
                case "Wolkenstraße": return "CC";
                case "Knochentrockene Dünen": return "BDD";
                case "Bowsers Festung": return "BC";
                case "Regenbogen-Boulevard": return this.TrackType == TrackType.Nitro ? "RR" : (this.TrackType == TrackType.Retro ? "rRRD" : "dRR");
                case "Kuhmuh-Weide": return "rMMM";
                case "Cheep-Cheep-Strand": return "rCCB";
                case "Toads Autobahn": return "rTT";
                case "Staubtrockene Wüste": return "rDDD";
                case "Donut-Ebene 3": return "rDP3";
                case "Königliche Rennpiste": return "rRRY";
                case "DK Dschungel": return "rDKJ";
                case "Wario-Arena": return "rWS";
                case "Sorbet-Land": return "rSL";
                case "Instrumentalpiste": return "rMP";
                case "Yoshi-Tal": return "rYV";
                case "Ticktack-Trauma": return "rTTC";
                case "Röhrenraserei": return "rPPS";
                case "Vulkangrollen": return "rGV";
                case "Yoshis Piste": return "dYC";
                case "Excitebike-Stadion": return "dEA";
                case "Große Drachenmauer": return "dDD";
                case "Mute City": return "dMC";
                case "Baby-Park": return "dBP";
                case "Käseland": return "dCL";
                case "Wilder Wipfelweg": return "dWW";
                case "Animal Crossing-Dorf": return "dAC";
                case "Warios Goldmine": return "dWGM";
                case "Polarkreis-Parcours": return "dIIO";
                case "Hyrule-Piste": return "dHC";
                case "Koopa-Großstadtfieber": return "dNBC";
                case "Party-Straße": return "dRIR";
                case "Marios Metro": return "dSBS";
                case "Big Blue": return "dBB";
                default: return string.Empty;
            }
        }







        public static List<MK8DTrack> GetAllTracks()
        {
            List<MK8DTrack> result = new List<MK8DTrack>
            {
                // Pilz-Cup
                new MK8DTrack("Mario Kart-Stadion", "Mario Kart Stadium",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Wasserpark", "Water Park",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Zuckersüßer Canyon", "Sweet Sweet Canyon",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Steinblock-Ruinen", "Thwomp Ruins",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),

                // Blumen-Cup
                new MK8DTrack("Marios Piste", "Mario Circuit",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Toads Hafenstadt", "Toad Harbor",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Gruselwusel-Villa", "Twisted Mansion",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Shy Guys Wasserfälle", "Shy Guy Falls",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),

                // Stern-Cup
                new MK8DTrack("Sonnenflughafen", "Sunshine Airport",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Delfinlagune", "Dolphin Shoals",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Discodrom", "Electrodrome",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Wario-Abfahrt", "Mount Wario",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),

                // Spezial-Cup
                new MK8DTrack("Wolkenstraße", "Cloudtop Cruise",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Knochentrockene Dünen", "Bone-Dry Dunes",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Bowsers Festung", "Bowser's Castle",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),
                new MK8DTrack("Regenbogen-Boulevard", "Rainbow Road",
                                    "", "",
                                    "", "",
                TrackType.Nitro, Console.Switch),

                // Panzer-Cup
                new MK8DTrack("Kuhmuh-Weide", "Moo Moo Meadows",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.Wii),
                new MK8DTrack("Marios Piste", "Mario Circuit",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.GBA),
                new MK8DTrack("Cheep-Cheep-Strand", "Cheep Cheep Beach",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.DS),
                new MK8DTrack("Toads Autobahn", "Toad's Turnpike",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.N64),

                // Bananen-Cup
                new MK8DTrack("Staubtrockene Wüste", "Dry Dry Desert",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.GCN),
                new MK8DTrack("Donut-Ebene 3", "Donut Plains 3",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.SNES),
                new MK8DTrack("Königliche Rennpiste", "Royal Raceway",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.N64),
                new MK8DTrack("DK Dschungel", "DK Jungle",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console._3DS),

                // Blatt-Cup
                new MK8DTrack("Wario-Arena", "Wario Stadium",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.DS),
                new MK8DTrack("Sorbet-Land", "Sherbet Land",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.GCN),
                new MK8DTrack("Instrumentalpiste", "Music Park",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console._3DS),
                new MK8DTrack("Yoshi-Tal", "Yoshi Valley",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.N64),

                // Blitz-Cup
                new MK8DTrack("Ticktack-Trauma", "Tick-Tock Clock",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.DS),
                new MK8DTrack("Röhrenraserei", "Piranha Plant Slide",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console._3DS),
                new MK8DTrack("Vulkangrollen", "Grumble Volcano",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.Wii),
                new MK8DTrack("Regenbogen-Boulevard", "Rainbow Road",
                                    "", "",
                                    "", "",
                TrackType.Retro, Console.N64),

                // Ei-Cup
                new MK8DTrack("Yoshis Piste", "Yoshi Circuit",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.GCN),
                new MK8DTrack("Excitebike-Stadion", "Excitebike Arena",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.Switch),
                new MK8DTrack("Große Drachenmauer", "Dragon Driftway",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.Switch),
                new MK8DTrack("Mute City", "Mute City",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.Switch),

                // Crossing-Cup
                new MK8DTrack("Baby-Park", "Baby Park",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.GCN),
                new MK8DTrack("Käseland", "Cheese Land",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.GBA),
                new MK8DTrack("Wilder Wipfelweg", "Wild Woods",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.Switch),
                new MK8DTrack("Animal Crossing-Dorf", "Animal Crossing",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.Switch),

                // Triforce-Cup
                new MK8DTrack("Warios Goldmine", "Wario's Goldmine",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.Wii),
                new MK8DTrack("Regenbogen-Boulevard", "Rainbow Road",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.SNES),
                new MK8DTrack("Polarkreis-Parcours", "Ice Ice Outpost",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.Switch),
                new MK8DTrack("Hyrule-Piste", "Hyrule Circuit",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.Switch),

                // Glocken-Cup
                new MK8DTrack("Koopa-Großstadtfieber", "Neo Bowser City",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console._3DS),
                new MK8DTrack("Party-Straße", "Ribbon Road",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.GBA),
                new MK8DTrack("Marios Metro", "Super Bell Subway",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.Switch),
                new MK8DTrack("Big Blue", "Big Blue",
                                    "", "",
                                    "", "",
                TrackType.DLC, Console.Switch)
            };

            return result;
        }
    }
}
