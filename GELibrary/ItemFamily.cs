using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Gnomoria specific
using Game;
using GameLibrary;

namespace GELibrary
{
    public class ItemFamily
    {
        #region Private static attributes

        /// <summary>
        /// List of families, subfamilies and items organized as a multi-dimension dictionary.
        /// Automatic initialized by the first call to one of the static public methods.
        /// </summary>
        private static Dictionary<string,Dictionary<string,Dictionary<string,ItemID>>> families = null;
        
        /// <summary>
        /// The string used to represent all subfamilies in a family, or all item types in a subfamily. Could be localized in the future.
        /// </summary>
        private static string ALL_DISPLAY_NAME = "<All>";

        /// <summary>
        /// The ItemID value associated with the ALL_DISPLAY_NAME item name. Must be different from the IDs of the real items.
        /// </summary>
        private static ItemID ALL_ITEM_ID = ItemID.Count;

        #endregion

        #region Public methods

        /// <summary>
        /// Return a list of the higher level family names.
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetFamilies()
        {
            if (families == null)
            {
                Initialize();
            }
            List<string> list = families.Keys.ToList();
            list.Sort();
            return list;
        }

        public static IList<string> GetSubFamilies(string familyName)
        {
            if (families == null)
            {
                Initialize();
            }
            List<string> list = families[familyName].Keys.ToList();
            list.Sort();
            return list;
        }

        public static IList<string> GetItemNames(string familyName, string subFamilyName)
        {
            if (families == null)
            {
                Initialize();
            }
            List<string> list = families[familyName][subFamilyName].Keys.ToList();
            list.Sort();
            return list;
        }

        public static IList<ItemID> GetItemIDs (string familyName, string subFamilyName, string itemTypeName)
        {
            if (families == null)
            {
                Initialize();
            }
            if (itemTypeName == ALL_DISPLAY_NAME)
            {
                // Get the list of all item IDs for the subfamily, and remove ALL_ITEM_ID from it (placeholder, not real ID)
                List<ItemID> list = new List<ItemID>();
                list.AddRange(families[familyName][subFamilyName].Values.ToList());
                list.Remove(ALL_ITEM_ID);
                return list;
                
            }
            else
            {
                List<ItemID> list = new List<ItemID>();
                list.Add(families[familyName][subFamilyName][itemTypeName]);
                return list;
            }
        }

        #endregion
        
        #region Private methods
        /// <summary>
        /// Add a new item in the right family and subfamily, creating all the necessary structures on the way.
        /// The item is also added to an "All" subfamily.
        /// </summary>
        /// <param name="familyKey"></param>
        /// <param name="subFamilyKey"></param>
        /// <param name="itemID"></param>
        private static void Add(string familyKey, string subFamilyKey, ItemID itemID)
        {
            Dictionary<string,Dictionary<string,ItemID>> familyValue = null;
            Dictionary<string, ItemID> subFamilyValue = null;
            Dictionary<string, ItemID> allSubFamilyValue = null;
            string displayName = Item.GroupName(itemID);

            // Create family (and associated sub-dictionary) if needed. Get its reference.
            if (families.ContainsKey(familyKey) == false)
            {
                families.Add (familyKey, new Dictionary<string,Dictionary<string,ItemID>>());
            }
            familyValue = families[familyKey];

            // If the family dictionary, create the sub-family if needed. Get its reference.
            if (familyValue.ContainsKey(subFamilyKey) == false)
            {
                familyValue.Add (subFamilyKey, new Dictionary<string,ItemID> ());
                // Add a "All" entry to the new subfamily. Associated ItemID is not relevant.
                familyValue[subFamilyKey].Add(ALL_DISPLAY_NAME, ItemID.Count);
            }
            subFamilyValue = familyValue[subFamilyKey];

            // Add the item to the subfamily dictionary. Duplicates are not allowed.
            if (subFamilyValue.ContainsKey(displayName) == false)
            {
                subFamilyValue.Add(displayName, itemID);
            }
            else
            {
                throw new InvalidOperationException("Adding twice " + familyKey + "/" + subFamilyKey + "/" + displayName);
            }

            // Finally, create the "All" subfamily if needed. Add the item to it.
            // This subfamily contains the union of all items in the regular subfamilies.
            // If the family dictionary, create the sub-family if needed. Get its reference.
            if (familyValue.ContainsKey(ALL_DISPLAY_NAME) == false)
            {
                familyValue.Add(ALL_DISPLAY_NAME, new Dictionary<string, ItemID>());
                // Add a "All" entry to the new subfamily. Associated ItemID is not relevant.
                familyValue[ALL_DISPLAY_NAME].Add(ALL_DISPLAY_NAME, ALL_ITEM_ID);
            }
            allSubFamilyValue = familyValue[ALL_DISPLAY_NAME];
            allSubFamilyValue.Add(displayName, itemID);
        }

        private static void Initialize()
        {
            families = new Dictionary<string, Dictionary<string, Dictionary<string, ItemID>>>();

            Add("Goods", "Soil", ItemID.RawSoil);

            Add("Goods", "Stone", ItemID.RawStone);
            Add("Goods", "Stone", ItemID.Block);
            Add("Goods", "Stone", ItemID.Chisel);
            Add("Goods", "Stone", ItemID.StoneKnifeBlade);
            Add("Goods", "Stone", ItemID.Knife);
            Add("Goods", "Stone", ItemID.Sawblade);
            Add("Goods", "Stone", ItemID.Hearth);
            Add("Goods", "Stone", ItemID.Mold);
            Add("Goods", "Stone", ItemID.Furnace);

            Add("Goods", "Wood", ItemID.RawWood);
            Add("Goods", "Wood", ItemID.Plank);
            Add("Goods", "Wood", ItemID.Workbench);
            Add("Goods", "Wood", ItemID.Stick);
            Add("Goods", "Wood", ItemID.Loom);
            Add("Goods", "Wood", ItemID.BedFrame);
            Add("Goods", "Wood", ItemID.FancyBedFrame);
            Add("Goods", "Wood", ItemID.TrainingDummy);

            Add("Goods", "Coal", ItemID.RawCoal);

            Add("Goods", "Metal", ItemID.RawOre);
            Add("Goods", "Metal", ItemID.Bar);
            Add("Goods", "Metal", ItemID.Anvil);
            Add("Goods", "Metal", ItemID.CuttingWheel);
            Add("Goods", "Metal", ItemID.File);
            Add("Goods", "Metal", ItemID.BallPeenHammer);
            Add("Goods", "Metal", ItemID.Wrench);

            Add("Goods", "Gem", ItemID.RawGem);
            Add("Goods", "Gem", ItemID.Gem);

            Add("Goods", "Cloth", ItemID.RawCloth);
            Add("Goods", "Cloth", ItemID.Bolt);
            Add("Goods", "Cloth", ItemID.String);
            Add("Goods", "Cloth", ItemID.Bandage);
            Add("Goods", "Cloth", ItemID.Mattress);

            Add("Goods", "Hide", ItemID.RawHide);
            Add("Goods", "Hide", ItemID.Needle);

            Add("Goods", "Crafts", ItemID.Statuette);
            Add("Goods", "Crafts", ItemID.PetRock);
            Add("Goods", "Crafts", ItemID.PuzzleBox);
            Add("Goods", "Crafts", ItemID.CommemorativeCoin);

            Add("Goods", "Jewelry", ItemID.Ring);
            Add("Goods", "Jewelry", ItemID.Necklace);
            Add("Goods", "Jewelry", ItemID.GemmedRing);
            Add("Goods", "Jewelry", ItemID.GemmedNecklace);

            Add("Goods", "Food", ItemID.Fruit);
            Add("Goods", "Food", ItemID.Egg);
            Add("Goods", "Food", ItemID.Meat);
            Add("Goods", "Food", ItemID.Sausage);
            Add("Goods", "Food", ItemID.Bread);
            Add("Goods", "Food", ItemID.Sandwich);

            Add("Goods", "Drink", ItemID.Milk);
            Add("Goods", "Drink", ItemID.Wine);
            Add("Goods", "Drink", ItemID.Beer);

            Add("Goods", "Plant", ItemID.Seed);
            Add("Goods", "Plant", ItemID.Clipping);
            Add("Goods", "Plant", ItemID.Wheat);
            Add("Goods", "Plant", ItemID.Straw);
            Add("Goods", "Plant", ItemID.StrawPile);

            Add("Goods", "Body Parts", ItemID.Corpse);
            Add("Goods", "Body Parts", ItemID.BodyPart);
            Add("Goods", "Body Parts", ItemID.Bone);
            Add("Goods", "Body Parts", ItemID.Skull);

            Add("Furniture", "Furniture", ItemID.Table);
            Add("Furniture", "Furniture", ItemID.Chair);
            Add("Furniture", "Furniture", ItemID.Bed);
            Add("Furniture", "Furniture", ItemID.FancyBed);
            Add("Furniture", "Furniture", ItemID.Statue);
            Add("Furniture", "Furniture", ItemID.WoodDoor);
            Add("Furniture", "Furniture", ItemID.StoneDoor);
            Add("Furniture", "Furniture", ItemID.Torch);
            Add("Furniture", "Furniture", ItemID.Pillar);
            Add("Furniture", "Furniture", ItemID.Dresser);
            Add("Furniture", "Furniture", ItemID.Cabinet);

            Add("Furniture", "Storage", ItemID.ResourcePile);
            Add("Furniture", "Storage", ItemID.Crate);
            Add("Furniture", "Storage", ItemID.Barrel);
            Add("Furniture", "Storage", ItemID.Bag);

            Add("Mechanism", "Part", ItemID.Rod);
            Add("Mechanism", "Part", ItemID.Gear);
            Add("Mechanism", "Part", ItemID.Spring);
            Add("Mechanism", "Part", ItemID.Spike);
            Add("Mechanism", "Part", ItemID.MechanismBase);
            Add("Mechanism", "Part", ItemID.TrapBase);
            Add("Mechanism", "Part", ItemID.WindmillBlade);
            Add("Mechanism", "Part", ItemID.Cylinder);
            Add("Mechanism", "Part", ItemID.Screw); // Not present in game list


            Add("Mechanism", "Link", ItemID.Axle);
            Add("Mechanism", "Link", ItemID.Gearbox);
            Add("Mechanism", "Switch", ItemID.Lever);
            Add("Mechanism", "Switch", ItemID.PressurePlate);

            Add("Mechanism", "Device", ItemID.Floodgate); // Is it Mechanical Wall?
            Add("Mechanism", "Device", ItemID.Hatch);

            Add("Mechanism", "Power Source", ItemID.Handcrank);

            Add("Mechanism", "Trap", ItemID.SpikeTrap);
            Add("Mechanism", "Trap", ItemID.BladeTrap);

            Add("Tool", "Tool", ItemID.PickaxeHead);
            Add("Tool", "Tool", ItemID.Pickaxe);
            Add("Tool", "Tool", ItemID.FellingAxeHead);
            Add("Tool", "Tool", ItemID.FellingAxe);

            Add("Weapons", "Melee", ItemID.Sword);
            Add("Weapons", "Melee", ItemID.HandAxe);
            Add("Weapons", "Melee", ItemID.Hammer);
            Add("Weapons", "Melee", ItemID.Claymore);
            Add("Weapons", "Melee", ItemID.BattleAxe);
            Add("Weapons", "Melee", ItemID.Warhammer);
            Add("Weapons", "Melee", ItemID.Shield);

            Add("Weapons", "Melee parts", ItemID.Hilt);
            Add("Weapons", "Melee parts", ItemID.Haft);
            Add("Weapons", "Melee parts", ItemID.SwordBlade);
            Add("Weapons", "Melee parts", ItemID.HandAxeHead);
            Add("Weapons", "Melee parts", ItemID.HammerHead);
            Add("Weapons", "Melee parts", ItemID.ClaymoreBlade);
            Add("Weapons", "Melee parts", ItemID.BattleAxeHead);
            Add("Weapons", "Melee parts", ItemID.WarhammerHead);
            Add("Weapons", "Melee parts", ItemID.ShieldBoss);
            Add("Weapons", "Melee parts", ItemID.ShieldBacking);

            Add("Weapons", "Ranged", ItemID.Crossbow);
            Add("Weapons", "Ranged", ItemID.CrossbowBolt);
            Add("Weapons", "Ranged", ItemID.Quiver);
            Add("Weapons", "Ranged", ItemID.Pistol);
            Add("Weapons", "Ranged", ItemID.Blunderbuss);
            Add("Weapons", "Ranged", ItemID.MusketRound);
            Add("Weapons", "Ranged", ItemID.AmmoPouch);
            Add("Weapons", "Ranged", ItemID.MusketRoundPile);
            Add("Weapons", "Ranged", ItemID.CrossbowBoltPile);

            Add("Weapons", "Ranged parts", ItemID.PistolStock);
            Add("Weapons", "Ranged parts", ItemID.PistolBarrel);
            Add("Weapons", "Ranged parts", ItemID.BlunderbussStock);
            Add("Weapons", "Ranged parts", ItemID.BlunderbussBarrel);
            Add("Weapons", "Ranged parts", ItemID.CrossbowStock);
            Add("Weapons", "Ranged parts", ItemID.CrossbowBow);

            Add("Armor", "Metal", ItemID.Helmet);
            Add("Armor", "Metal", ItemID.Breastplate);
            Add("Armor", "Metal", ItemID.Pauldron);
            Add("Armor", "Metal", ItemID.Greave);
            Add("Armor", "Metal", ItemID.Gauntlet);
            Add("Armor", "Metal", ItemID.Boot);

            Add("Armor", "Leather", ItemID.LeatherHelm);
            Add("Armor", "Leather", ItemID.LeatherCuirass);
            Add("Armor", "Leather", ItemID.LeatherBracer);
            Add("Armor", "Leather", ItemID.LeatherGreave);
            Add("Armor", "Leather", ItemID.LeatherGlove);
            Add("Armor", "Leather", ItemID.LeatherBoot);

            Add("Armor", "Bone", ItemID.SkullHelmet);
            Add("Armor", "Bone", ItemID.BoneShirt);

            Add("Armor", "Parts", ItemID.ArmorPlate);
            Add("Armor", "Parts", ItemID.LeatherArmorPanel);
            Add("Armor", "Parts", ItemID.Padding);
            Add("Armor", "Parts", ItemID.LeatherStrap);

        }
        #endregion
    }
}
