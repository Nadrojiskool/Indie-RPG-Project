using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public class Unit : Object
    {
        // Notes:
        // - Need to establish unit hierarchies so that dumber units can spawn in larger numbers
        // > This means dumber units won't path as far, and are limited to a small number of attacks
        // > Lowest on the hierachy are creatures which have no pathing and can only cast single tile attacks at most
        // >> The next tier may have small tile range attacks and limited pathing
        // >>> The next tier may have advanced pathing and dependent hierachies (including unit-independent animation and logic threads)

        /// <summary> Future Unit Action Array Index
        /// 0: Idle
        /// 1: Pathing
        /// 2: Follow Player
        /// 3: Gather Resources
        /// 4: Attack Target
        /// 
        /// 9: Sleep
        /// </summary>


        /// <summary> Unit Stats Array Index
        /// 0: Level (LVL) = 1 + Stats 10-30
        /// 1: Health (HP) = 10 * (2 + Vitality(11) + Physique(12))
        /// 2: Attack (ATK) = 1 + Combat(14) * (Agility(13) + Physique(12))
        /// 3: Defense (DEF) = 1 + Combat(14) * (Agility(13) + Physique(12) + Vitality(11))
        /// 4: Speed (SPD) = 1 / (101 - (Agility(13) * Vitality(11)))
        /// 5: 
        /// 9: 
        /// 10: Experience
        /// 11: Vitality
        /// 12: Physique
        /// 13: Agility
        /// 14: Combat
        /// 15: Magic
        /// 16: Knowledge
        /// 17: Leadership
        /// 18: Sociability
        /// 19: Expertise
        /// /// /// /// /// /// /// ///
        /// Expected Max Level of Professions is 100.
        /// Every 10 levels should unlock a new Technique, Skill or Ability.
        /// 256 * Level = XP to Next Level
        /// If base action experience is expected to be 10 then level 1 action count to next level is 26
        /// If base action time is expected to be 10 seconds then time to level 2 is 260 seconds (4min 20sec)
        /// Time from 10-11 is ~43min /// Time from 20-21 is ~1hr 26min /// 50-51 is 3hr 35min /// 99-100 ~6hr ///
        /// Total Time from 1-100 would be approx 15 days played. 
        /// If there were at least 10 skills, that would be 150 days for a single max character.
        /// The goal for auto AI would be double this, or nearly a year of keeping a unit active.
        /// The goal for the player would be to half this before offering gameplay-centric buffs and bonuses.
        /// >> Example gameplay-centric buff/bonus: Party members receive +1 bonus XP of certain action's Profession type.
        /// /// /// /// /// /// /// ///
        /// As long as you can acquire a Recipe, you can learn it and craft it.
        /// I kind of like Black Desert Online's model of variable material inputs affecting output yield
        /// In theory you could craft anything to a varying degree of success, but there are only two ways to guarantee success
        /// You can do some sort of activity with another unit to have them teach you the recipe or just gain enough experience by crafting the item
        /// /// /// /// /// /// /// ///
        /// 20: Gathering
        /// ///  Level Unlocks  /// ///
        /// >> 01: Scavenge (Grab Wood, Stones and Berries)
        /// >> 10: 
        /// /// /// /// /// /// /// ///
        /// 21: Wood Cutting 
        /// ///  Level Unlocks  /// ///
        /// /// /// /// /// /// /// ///
        /// 22: Wood Working
        /// ///  Level Unlocks  /// ///
        /// /// /// /// /// /// /// ///
        /// 23: Mining
        /// 
        /// Professions are Based on Productivity = Technology * (Knowledge + Skill + Expertise)
        /// This hopefully reflects reality to the extent that technology begets exponentially increases in productivity
        /// Certain machines can gather resources at a base rate, but are dependent on additional technology, skillsets, Operator performance and Machine Condition
        /// The goal is to intermingle a realistic balance which reflects natural time spent doing something (Experience/Knowledge)
        /// With the actual ability required to commit an action (Level/Expertise/Unit.Skills)
        /// This is to get around the traditional mining leveling structure, which is being shaped below
        /// There are a gamut of ore and minerals which occur naturally and will produce the desired materials if skillfully refined
        /// The progression is expected to reflect a natural technology progression, but more flexible (less linear) systems are being considered
        /// You can see and mine any minable object if unit has sufficient technology (tools), but the object will appear as an, "Unknown Mineral"
        /// As such, you may not be able to mine the object at peak efficiency and can only trade the mineral in person (peer-to-peer)
        /// With many resources in the game, it may be difficult to identify specific sprites without the knowledge
        /// A unit also wouldn't be able to refine a resource that they didn't have knowledge about, but they can get the knowledge from other professions or units
        /// This suggest that a worker could get the skill required to run a mining machine, but never needs the mining skill
        /// Somebody with little mining skill could also still mine in bulk, but would need a third-party to identify minerals, sell on the market or use them
        /// Across the board the mining skill would increase activity productivity and reduce deterioration of active objects of an excavation-type Task
        /// In this situation, earned experience would be split between Machine Operating and Mining skills
        /// If a machine breaks down during operation, it will trigger a flag for the operator where they will either repair the machine or call a technician
        /// Example: A Pickaxe would have a certain productivity based on an object variable (i.e. hardness)
        /// >> The harder the material, the faster it is going to dull and break your pickaxe head
        /// >> You are also going to have a difficult time extracting minerals if your pick isn't hard enough
        /// >> But a hard pick isn't going to be phased by softer materials like Lead or Tin and will need very few repairs (hope you brought a good Blacksmith)
        /// >> Mining camps are going to require a good Blacksmithing station and Blacksmith
        /// 
        /// https://education.jlab.org/glossary/abund_uni.html
        /// ///  Level Unlocks  /// ///
        /// >> 01: Stone
        /// >> 02: Flint
        /// >> 03: Clay
        /// >> 04: Sandstone
        /// >> 05: Limestone
        /// >> 08: Salt
        /// >> 10: Copper
        /// >> >> Native Copper
        /// >> >> Chalcopyrite (CuFeS2)
        /// >> >> Chalcocite (Cu2S)
        /// >> >> Covellite (CuS)
        /// >> >> Bornite (2Cu2S·CuS·FeS)
        /// >> >> Tetrahedrite (Cu3SbS3 + x(Fe,Zn)6Sb2S9)
        /// >> >> Digenite (Cu9S5)
        /// >> >> Malachite (CuCO3•Cu(OH)2)
        /// >> >> Azurite (2CuCO3·Cu(OH)2)
        /// >> >> Cuprite (Cu2O)
        /// >> >> Chrysocolla (Cu,Al)2H2Si2O5(OH)4·n(H2O)
        /// >> >> Tennantite (Cu12As4S13)
        /// >> >> Dioptase (CuSiO2(OH)2)
        /// >> 15: Tin
        /// >> >> Cassiterite(SnO2)
        /// >> 18: Lead 
        /// >> >> Galena(PbS) >> Often associated with the minerals sphalerite, calcite and fluorite
        /// >> 19: Antimony
        /// >> >> Stibnite(Sb2S3)
        /// >> >> Valentinite(Sb2O3)
        /// >> 20: Silver
        /// >> 22: Nickel (1751)
        /// >> >> Niccolite(NiAs)
        /// >> >> Pentlandite(NiS2FeS)
        /// >> 23: Bismuth (1753)
        /// >> 25: Iron 
        /// >> >> Taconite(100% Delicious)
        /// >> >> Magnetite(Fe3O4, 72.4% Fe), 
        /// >> >> Hematite(Fe2O3, 69.9% Fe), 
        /// >> >> Goethite(FeO(OH), 62.9% Fe), 
        /// >> >> Limonite(FeO(OH)·n(H2O), 55% Fe)
        /// >> >> Siderite(FeCO3, 48.2% Fe)]
        /// >> 26: Calcite (CaCO3)
        /// >> 27: Arsenic [Free]
        /// >> >> Arsenopyrite(FeAsS)
        /// >> >> Realgar(AsS)
        /// >> >> Orpiment(As2S3)
        /// >> 28: Zinc (1400s:1746)
        /// >> >> Calamine(ZnCO3)
        /// >> 30: Gold [Free]
        /// >> 32: Manganese (1774)
        /// >> >> Pyrolusite(MnO2)
        /// >> 33: Molybdenum (1778:1781)
        /// >> >> Molybdenite(MoS2)
        /// >> >> Wulfenite(PbMoO4)
        /// >> >> Powellite(CaMoO4)
        /// >> 34: Quartz
        /// >> 35: Carbon (Coal)
        /// >> 36: Tellurium (1782)
        /// >> >> Sylvanite(AgAuTe4)
        /// >> >> Calaverite(AuTe2)
        /// >> >> Krennerite(AuTe2)
        /// >> 37: Phosphorus
        /// >> >> Phosphate Rock(Ca3(PO4)2)
        /// >> 38: Tungsten (1783)
        /// >> 40: Aluminum (1787:1825)
        /// >> >> Bauxite
        /// >> 42: Chromium (1797)
        /// >> >> Crocoite(PbCrO4)
        /// >> >> Chromite(FeCr2O4)
        /// >> 43: Palladium [Bi-Product] (1803)
        /// >> 45: Platinum (1735)
        /// >> >> Sperrylite(PtAs2)
        /// >> 47: Selenium (1817)
        /// >> >> Eucairite(CuAgSe)
        /// >> >> Crooksite(CuThSe)
        /// >> >> Clausthalite (PbSe)
        /// >> 48: Lithium (1817)
        /// >> 50: Mythril
        /// >> 52: Cobolt (1739)
        /// >> >> Cobaltite (CoAsS)
        /// >> >> Erythrite (Co3(AsO4)2)
        /// >> 53: Runite
        /// >> 55: Titanium (1791:1910)
        /// >> 58: Pyrite (FeS2)
        /// >> 60: Obsidian
        /// >> 62: Aelium
        /// >> Aelinite
        /// >> 65: Vanadium (1801:1867)
        /// >> 67: Niobium (1734:1864)
        /// >> >> Columbite((Fe, Mn, Mg)(Nb, Ta)2O6)
        /// >> >> Pyrochlore((Ca, Na)2Nb2O6(O, OH, F))
        /// >> 68: Tantalum
        /// >> >> Columbite((Fe, Mn)Nb2O6)
        /// >> >> Tantalite((Fe, Mn)(Ta, Nb)2O6)
        /// >> >> Euxenite ((Y, Ca, Er, La, Ce, U, Th)(Nb, Ta, Ti)2O6)
        /// >> 70: Orichalcum
        /// >> >> Orichalite
        /// >> 75: Diamond
        /// >> 78: Dilithium
        /// >> 80: Sauronic (or Essence of Sauron)
        /// >> Sauronite
        /// >> 82: Indium (1863)
        /// >> 83: Germanium (1886)
        /// >> >> Argyrodite(Ag8GeS6)
        /// >> 85: Uranium (1789)
        /// >> 87: Thorium (1828)
        /// >> >> Thorite(ThSiO4)
        /// >> >> Thorianite (ThO2)
        /// >> >> Monazite((Ce, La, Th, Nd, Y)PO4)
        /// >> 88: Plutonium
        /// >> 90: Adamantium
        /// >> Adamantite
        /// >> 92: Trillium
        /// >> 93: Lapiz Lazuli
        /// >> >> Lazurite ((Na,Ca)8[(S,Cl,SO4,OH)2|(Al6Si6O24)])
        /// >> 95: Imperial Gold
        /// >> 97: Scarlithium
        /// >> Scarlite
        /// >> Scarletite
        /// >> 98: Moonstone
        /// >> >> Lunite ((Na,K)AlSi3O8)
        /// >> 99: 
        /// >> 1000 (Expertise 10): Starlite
        /// >> >> Plumbignium
        /// /// /// /// /// /// /// ///
        /// >> Tin: Small quantities of tin are recovered from complex sulfides such as Stannite, Cylindrite, Franckeite, Canfieldite, and Teallite. 
        /// >> >> >> Minerals with tin are almost always associated with granite rock, usually at a level of 1% tin oxide content.
        /// /// /// /// /// /// /// ///
        /// 24: Smithing
        /// ///  Level Unlocks  /// ///
        /// 
        /// /// /// /// /// /// /// ///
        /// 25: Engineering
        /// ///  Level Unlocks  /// ///
        /// 
        /// /// /// /// /// /// /// ///
        /// 26: Construction
        /// ///  Level Unlocks  /// ///
        /// 
        /// /// /// /// /// /// /// ///
        /// 27: Hunting
        /// ///  Level Unlocks  /// ///
        /// 
        /// /// /// /// /// /// /// ///
        /// 28: Skinning
        /// ///  Level Unlocks  /// ///
        /// 
        /// /// /// /// /// /// /// ///
        /// 29: Butchering
        /// ///  Level Unlocks  /// ///
        /// 
        /// /// /// /// /// /// /// ///
        /// 30: Cooking
        /// ///  Level Unlocks  /// ///
        /// 
        /// /// /// /// /// /// /// ///
        /// 100: Worker Capacity
        /// 101: Gold
        /// 102: Workers
        /// 
        /// 
        /// </summary>

        public int[] Stats = new int[200];
        public int ID { get; set; }
        public byte ActionID = 0;
        public Stopwatch ActionTime = new Stopwatch();
        public int ActionDuration { get; set; }
        public float Rotation { get; set; }
        public int LastMove { get; set; }
        public int Depth { get; set; }
        public int AutoX { get; set; }
        public int AutoY { get; set; }
        //public sbyte[] Path;
        public int[] DestinationOffset = new int[2] { 0, 0 };
        public int[] OriginOffset = new int[2] { 0, 0 };
        public sbyte LeftOrRight = 0; // Cached Rotation (LastMove) // Negative (-) is FavorLeft // Positive (+) is FavorRight //
        
        // Bloat for the following Test Pathing variable is concerning
        public List<int[]> Pathed { get; set; }

        // Job.TaskList (Action IDs) 
        public Register.Job Job { get; set; }

        //public static List<Unit> Active = new List<Unit>();

        public Unit(int x, int y, int id, int[] array) : base($"{id}", x, y, 0, 0)
        {
            this.X = x;
            this.Y = y;
            this.ID = id;
            this.Stats = array;
        }

        public void Attack(Unit unit)
        {
            unit.Stats[1] -= Check.Min(this.Stats[2] - unit.Stats[3], 1);
        }

        public void CheckSlash(Unit unit)
        {
            int x = X + Game1.MovementXY[LastMove, 0] + Game1.MovementXY[Check.LoopInt(LastMove - 1, 1, 4), 0];
            int y = Y + Game1.MovementXY[LastMove, 1] + Game1.MovementXY[Check.LoopInt(LastMove - 1, 1, 4), 1];
            for (int i = 0; i < 3; i++)
            {
                if (unit.X == (x + (i * Game1.MovementXY[Check.LoopInt(LastMove + 1, 1, 4), 0]))
                    && unit.Y == (y + (i * Game1.MovementXY[Check.LoopInt(LastMove + 1, 1, 4), 1])))
                {
                    Attack(unit);
                }
            }
        }
        
        public void CheckAOE(Unit unit, int x, int y, int width, int height)
        {
            for (int a = 0; a < height; a++)
            {
                for (int b = 0; b < width; b++)
                {
                    if (unit.X == (x + b)
                        && unit.Y == (y + a))
                    {
                        Attack(unit);
                    }
                }
            }
        }
    }
}
