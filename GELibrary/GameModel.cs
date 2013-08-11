using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

using Game;
using GameLibrary;


namespace GELibrary
{
    /// <summary>
    /// This class holds the reference to the GnomanEmpire instance and manages it.
    /// The GnomanEmpire class (and its static instance) represent the Gnomoria game model, and are used to manipulate save games.
    /// </summary>
    public class GameModel
    {
        #region Attributes

        // Only set after a successfull call to loadGame()
        private GnomanEmpire _gnomanEmpire;

        private string mSaveGameFolder;
        private string mSettingsFolder;

        // Arrays storing the skill names (displayed) and the enum (used in queries).
        private string[] skillNames;
        private CharacterSkillType[] skillTypes;

        // Array storing the attribute names (displayed) and the related enum values (used in queries)
        private string[] attributeNames;
        private CharacterAttributeType[] attributeTypes;

        // Main data tables

        /// <summary>
        /// Main data table for gnome attributes and skills. One row per gnome, filled with name, profession, current tasks, attributes and skill values.
        /// Computed by Load_Character_Skills(), accessed by getCharSkills();
        /// </summary>
        private DataTable mCharSkills;

        /// <summary>
        /// Average values of character skills. Key is the skill name (that is, the column name in the data table), value is the average.
        /// </summary>
        private Dictionary<string, double> mAverageCharSkills;

        /// <summary>
        /// Standard deviation values of character skills. Key is the skill name (that is, the column name in the data table), value is the stddev.
        /// </summary>
        private Dictionary<string, double> mStdDevCharSkills;

        /// <summary>
        /// Main data table for gnome statistics and skills. One row per gnome, filled with name, statistics.
        /// Computed by Load_Character_Stats(), accessed by getCharStats();
        /// </summary>
        private DataTable mCharStats;

        /// <summary>
        /// Map with some kingdom values. Keys are "KingdomName", "TotalWealth" and "GameDate".
        /// Computed by Load_Kingdom_Overview(), accessed by getKingdomOverview().
        /// </summary>
        private Dictionary<string, string> mKingdomOverview;

        #endregion // Attributes

        #region Constructor

        // Constructor
        public GameModel()
        {
        }

        #endregion // Constructor

        #region Methods

        public DataTable FindItems(IList<ItemID> itemIDs, GameLibrary.ItemQuality quality)
        {
            if (_gnomanEmpire == null)
            {
                throw new InvalidOperationException("You must LoadGame() prior to FindItems()");
            }

            Dictionary<int, List<Item>> searchResult = null;
            ItemsByQuality resultByQuality = null;

            DataTable itemTable = new DataTable("items");

            DataColumn cNum = new DataColumn("Num", typeof(string));
            itemTable.PrimaryKey = new DataColumn[] { itemTable.Columns.Add(cNum.ColumnName) };

            itemTable.Columns.Add("Name", typeof(string));
            itemTable.Columns.Add("Material", typeof(string));
            itemTable.Columns.Add("Quality", typeof(string));
            itemTable.Columns.Add("Location type", typeof(string));
            itemTable.Columns.Add("Location", typeof(string));
            itemTable.Columns.Add("Position", typeof(string));

            // When no object matches, the result can be null, or contain only empty lists.
            foreach (ItemID itemID in itemIDs)
            {
                resultByQuality  = _gnomanEmpire.Fortress.StockManager.ItemsByQuality(itemID);
                if (resultByQuality != null)
                    searchResult = resultByQuality.ItemsOfQualityOrHigher(quality).Items;
                else
                    searchResult = null;
                if (searchResult != null)
                {
                    foreach (var valuePair in searchResult)
                    {
                        List<Game.Item> list = valuePair.Value;
                        foreach (Game.Item item in list)
                        {
                            DataRow tmpRow = itemTable.NewRow();
                            int col = 0;

                            // Base attributes
                            tmpRow[col++] = item.ID;
                            tmpRow[col++] = Item.GroupName(item.ItemID);// item.Name();
                            tmpRow[col++] = item.MaterialName();
                            tmpRow[col++] = item.Quality;

                            // Parent type and parent
                            if (item.Parent != null)
                            {
                                if (item.Parent is Game.Character)
                                {
                                    tmpRow[col++] = "Gnome";
                                }
                                else if (item.Parent is Game.StorageContainer)
                                {
                                    tmpRow[col++] = "Container";
                                }
                                else
                                {
                                    tmpRow[col++] = "?"; // Default catcher. Could be monster?
                                }
                                tmpRow[col++] = item.Parent.Name();
                            }
                            else
                            {
                                tmpRow[col++] = "Ground";
                                tmpRow[col++] = "";
                            }

                            tmpRow[col++] = item.Position.ToString();

                            itemTable.Rows.Add(tmpRow);
                        }
                    }
                }
            }

            return itemTable;
        }

        public Result Initialize()
        {
            Result result = new Result(false, "");
            try
            {
                Console.WriteLine("[Game Model] Initialize: 7z Setup");
                SevenZip.SevenZipExtractor.SetLibraryPath("7z.dll");

                Console.WriteLine("[Game Model] Initialize: Running reflection");
                MethodInfo initMethod = typeof(GnomanEmpire).GetMethod("Initialize", BindingFlags.NonPublic | BindingFlags.Instance);
                initMethod.Invoke(GnomanEmpire.Instance, null);

                // Stop the music playing (it starts automatically when the Game object is initialized)
                GnomanEmpire.Instance.AudioManager.SetMusicVolume(0);
                GnomanEmpire.Instance.AudioManager.SetSFXVolume(0);

                // Exit the full screen mode if needed
                if (GnomanEmpire.Instance.Graphics.IsFullScreen)
                    GnomanEmpire.Instance.Graphics.ToggleFullScreen();

                Console.WriteLine("[Game Model] Initialize: Setting SaveFolderPath");
                mSaveGameFolder = GnomanEmpire.SaveFolderPath("Worlds\\");
                mSettingsFolder = GnomanEmpire.SaveFolderPath();

                Console.WriteLine("[Game Model] Initialize: Initializing Character Skills");
                InitializeCharacterAttributes();
                InitializeCharacterSkills();

                result.Success = true;
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                Console.WriteLine("[Game Model] Initialize: Exception encountered \"{0}\"", ex.InnerException.Message);
                result.ErrorMessage = "InvocationException: " + ex.InnerException.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Game Model] Initialize: Exception encountered \"{0}\"", ex.Message);
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private void InitializeCharacterAttributes()
        {
            // Setup temp array's to be modified and setup correctly
            ArrayList attrNamesTemp = new ArrayList(Enum.GetNames(typeof(CharacterAttributeType)));
            ArrayList attrTypesTemp = new ArrayList(Enum.GetValues(typeof(CharacterAttributeType)));

            // The following values are in the CharacterSkillType Array however we do not want them.
            int index = -1;

            index = attrNamesTemp.IndexOf("Count");
            attrNamesTemp.RemoveAt(index);
            attrTypesTemp.RemoveAt(index);

            // Convert the temp arrays and store them in the class attributes.
            attributeTypes = (CharacterAttributeType[])attrTypesTemp.ToArray(typeof(CharacterAttributeType));
            attributeNames = (string[])attrNamesTemp.ToArray(typeof(string));

            // Clean up
            attrTypesTemp = null;
            attrNamesTemp = null;
        }

        /// <summary>
        /// After character skills have been loaded, compute the average and standard deviation values for each skill.
        /// Called by Load_Character_Skills().
        /// </summary>
        private void ComputeAverageSkills()
        {
            if (mCharSkills == null)
            {
                return;
            }

            // (Re-)Initialize the maps
            mAverageCharSkills = new Dictionary<string, double>();
            mStdDevCharSkills = new Dictionary<string, double>();

            foreach (string attrName in attributeNames)
            {
                ComputeAverageAndSrdDev(attrName);
            }

            foreach (string skillName in skillNames)
            { 
                ComputeAverageAndSrdDev(skillName);
            }

        }

        /// <summary>
        /// Compute the average and standard deviation values of the "attrName" column of the mCharSkills data table, and
        /// store it in the mAverageCharSkills and mStdDevCharSkills dictionaries.
        /// </summary>
        /// <param name="attrName">The name of attribute, that is, the column name</param>
        private void ComputeAverageAndSrdDev(string attrName)
        {
            // Generic query on the mCharSkills DataTable, return list of values for this attribute (= attrName)
            IEnumerable<Double> valuesQuery = from gnomeRow in mCharSkills.AsEnumerable() select Convert.ToDouble(gnomeRow.Field<Int32>(attrName));

            //Compute the Average      
            double avg = valuesQuery.Average();
            //Perform the Sum of (value-avg)^2      
            double sum = valuesQuery.Sum(d => Math.Pow(d - avg, 2));
            //Put it all together      
            double stddev = Math.Sqrt((sum) / (valuesQuery.Count() - 1));

            // Store results
            mAverageCharSkills.Add(attrName, avg);
            mStdDevCharSkills.Add(attrName, stddev);
        }

        private void InitializeCharacterSkills()
        {
            // Setup temp array's to be modified and setup correctly
            ArrayList skillNamesTemp = new ArrayList(Enum.GetNames(typeof(CharacterSkillType)));

            // The following values are in the CharacterSkillType Array however we do not want them.
            int index = -1;

            index = skillNamesTemp.IndexOf("LaborStart");
            skillNamesTemp.RemoveAt(index);

            index = skillNamesTemp.IndexOf("LaborEnd");
            skillNamesTemp.RemoveAt(index);

            index = skillNamesTemp.IndexOf("Count");
            skillNamesTemp.RemoveAt(index);

            index = skillNamesTemp.IndexOf("Discipline");
            skillNamesTemp.RemoveAt(index);

            // Reorder skills. Put combat skills first and then order the task skills correctly.
            skillNamesTemp.Remove("Bonecarving");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Tinkering"), "Bonecarving");

            skillNamesTemp.Remove("Medic");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Construction"), "Medic");

            skillNamesTemp.Remove("Caretaker");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Construction"), "Caretaker");

            skillNamesTemp.Remove("NaturalAttack");
            skillNamesTemp.Remove("Brawling");
            skillNamesTemp.Remove("Sword");
            skillNamesTemp.Remove("Axe");
            skillNamesTemp.Remove("Hammer");
            skillNamesTemp.Remove("Crossbow");
            skillNamesTemp.Remove("Gun");
            skillNamesTemp.Remove("Shield");
            skillNamesTemp.Remove("Dodge");
            skillNamesTemp.Remove("Armor");

            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Mining"), "NaturalAttack");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Mining"), "Brawling");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Mining"), "Sword");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Mining"), "Axe");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Mining"), "Hammer");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Mining"), "Crossbow");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Mining"), "Gun");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Mining"), "Shield");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Mining"), "Dodge");
            skillNamesTemp.Insert(skillNamesTemp.IndexOf("Mining"), "Armor");

            // Convert the temp arrays and store them in the class attributes.
            skillTypes = new CharacterSkillType[skillNamesTemp.Count];
            for (int i = 0; i < skillNamesTemp.Count; i++)
            {
                // Convert skills names to skill enums using the static Enum.Parse() method.
                skillTypes[i] = (CharacterSkillType) Enum.Parse(typeof(CharacterSkillType), skillNamesTemp[i].ToString());
            }

            // Final changes: name changes must be done after converting names to official Enum values
            // The following values have different names and we are renaming to show it correctly
            index = skillNamesTemp.IndexOf("NaturalAttack");
            skillNamesTemp[index] = "Fighting";

            skillNames = (string[])skillNamesTemp.ToArray(typeof(string));

            // Clean up
            skillNamesTemp = null;
        }

        /// <summary>
        /// Load a saved game file into memory. Must be done at least once before calling the other Load methods.
        /// </summary>
        /// <param name="saveGame">The complete file path of the saved game to load.</param>
        /// <returns>A Result object with a true/false success and an error message.</returns>
        public Result LoadGame(string saveGame)
        {
            Result result = new Result(false, "");

            try
            {
                Console.WriteLine("[Game Model] LoadGame: Loading world {0}", saveGame);
                GnomanEmpire.Instance.LoadGame(saveGame, false);
                Console.WriteLine("[Game Model] LoadGame Instance.LoadGame successful, now setting _gnomanEmpire");
                _gnomanEmpire = GnomanEmpire.Instance;

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public Result Load_Character_Stats()
        {
            Result result = new Result(false, "");

            if (_gnomanEmpire == null)
            {
                result.ErrorMessage = "You must LoadGame prior to Load_Character_Skills";
                return result;
            }

            try
            {
                Console.WriteLine("[Game Model] Load_Character_Stats: Adding Character Information");

                if (mCharStats != null)
                {
                    mCharStats.Clear();
                    mCharStats.Dispose();
                    mCharStats = null;
                }

                mCharStats = new DataTable("mCharStats");

                // Add the first column, the character ID ("Num") and declare it as the primary key.
                DataColumn cNum = new DataColumn("Num", typeof(string));
                mCharStats.PrimaryKey = new DataColumn[] { mCharStats.Columns.Add(cNum.ColumnName) };

                // Add other columns and set them as read-only.
                mCharStats.Columns.Add("Name", typeof(string));

                // Body stats
                mCharStats.Columns.Add("Hunger", typeof(int));
                mCharStats.Columns.Add("Thirst", typeof(int));
                mCharStats.Columns.Add("Blood Level", typeof(int));
                mCharStats.Columns.Add("Rest Level", typeof(int));

                foreach (var Char in _gnomanEmpire.World.AIDirector.PlayerFaction.Members)
                {
                    Console.WriteLine("[Game Model] Load_Character_Stats: Adding Character: {0}",Char.Value.Name());
                    DataRow tmpRow = mCharStats.NewRow();
                    int col = 0;
                    tmpRow[col++] = Char.Key;
                    tmpRow[col++] = Char.Value.Name();

                    // Body stats
                    tmpRow[col++] = Char.Value.Body.HungerLevel;
                    tmpRow[col++] = Char.Value.Body.ThirstLevel;
                    tmpRow[col++] = Char.Value.Body.BloodLevel;
                    tmpRow[col++] = Char.Value.Body.RestLevel;
                    mCharStats.Rows.Add(tmpRow);
                }

                result.Success = true;
                Console.WriteLine("[Game Model] Load_Character_Stats: Adding Character Information Completed");
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public Result Load_Character_Skills()
        {
            Result result = new Result();

            if (_gnomanEmpire == null)
            {
                result.ErrorMessage = "You must LoadGame prior to Load_Character_Skills";
                return result;
            }

            try
            {
                if (mCharSkills != null)
                {
                    mCharSkills.Clear();
                    mCharSkills.Dispose();
                    mCharSkills = null;
                }

                mCharSkills = new DataTable("mCharSkills");

                DataColumn cNum = new DataColumn("Num", typeof(string));
                mCharSkills.PrimaryKey = new DataColumn[] { mCharSkills.Columns.Add(cNum.ColumnName) };

                mCharSkills.Columns.Add("Name", typeof(string));
                mCharSkills.Columns.Add("Profession", typeof(string));
                mCharSkills.Columns.Add("Current Job", typeof(string));

                // Attribute columns
                foreach (string attrName in attributeNames)
                {
                    mCharSkills.Columns.Add(attrName, typeof(int));
                }

                // Skill columns
                foreach (string name in skillNames)
                {
                    mCharSkills.Columns.Add(name, typeof(int));
                }

                // Now that columns have been defined, iterate through gnomes and add one row per gnome.
                foreach (var Char in _gnomanEmpire.World.AIDirector.PlayerFaction.Members)
                {
                    DataRow tmpRow = mCharSkills.NewRow();
                    int col = 0;
                    tmpRow[col++] = Char.Key; // Gnome ID
                    tmpRow[col++] = Char.Value.Name(); // Gnome name
                    tmpRow[col++] = Char.Value.Title(); // Gnome profession
                    if (Char.Value.Job == null)
                    {
                        if (Char.Value.Body.IsSleeping == true)
                        {
                            tmpRow[col++] = "Sleeping";
                        }
                        else
                        {
                            tmpRow[col++] = "Idle";
                        }
                    }
                    else
                    {
                        tmpRow[col++] = Char.Value.Job.JobName();
                    }

                    // Character attributes (Note: values are float, actually %)
                    foreach (CharacterAttributeType attrType in attributeTypes)
                    {
                        tmpRow[col++] = Char.Value.AttributeLevel(attrType) * 100;
                    }

                    // Character skills
                    foreach (CharacterSkillType skill in skillTypes)
                    {
                        tmpRow[col++] = Char.Value.SkillLevel(skill);
                    }

                    mCharSkills.Rows.Add(tmpRow);
                }

                mCharSkills.Columns[0].ReadOnly = true;
                mCharSkills.Columns[1].ReadOnly = true;
                mCharSkills.Columns[2].ReadOnly = true;
                mCharSkills.Columns[3].ReadOnly = true;

                // Compute the average and standard deviation of all attributes and skills
                ComputeAverageSkills();

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                return result;
            }

            return result;
        }

        public Result Load_Kingdom_Overview()
        {
            Result result = new Result();
            Console.WriteLine("[Game Model] Loading Kingdom Overview...");
            if (_gnomanEmpire == null)
            {
                Console.WriteLine("[Game Model] LoadGame not called 1st");
                result.ErrorMessage = "You must LoadGame prior to Load_Kingdom_Overview";
                result.Success = false;
                return result;
            }

            try
            {
                mKingdomOverview = new Dictionary<string, string>();
                mKingdomOverview["KingdomName"] = _gnomanEmpire.World.AIDirector.PlayerFaction.Name;
                mKingdomOverview["TotalWealth"] = _gnomanEmpire.Fortress.TotalWealth.ToString();
                mKingdomOverview["GameDate"]    = _gnomanEmpire.Region.DayOfSeasonString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Game Model] Exception Caught: {0}",ex.Message);
                result.ErrorMessage = ex.Message;
                result.Success = false;
                return result;
            }
            Console.WriteLine("[Game Model] Loading Kingdom Overview Completed");
            return result;
        }

        // Public Accessors
        public string getSavedGameFolder()
        {
            if (mSaveGameFolder != null)
            {
                return mSaveGameFolder;
            }
            else
            {
                return "";
            }
        }

        public string getSettingsFolder()
        {
            if (mSettingsFolder != null)
            {
                return mSettingsFolder;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Return the average value associated with an attribute or skill name.
        /// </summary>
        /// <param name="attrName">Attribute or skill name, that is, column header in the getCharSkills() data table</param>
        /// <returns></returns>
        public double getAverageAttribute(string attrName)
        {
            return mAverageCharSkills[attrName];
        }

        /// <summary>
        /// Return the standard deviation value associated with an attribute or skill name.
        /// </summary>
        /// <param name="attrName">Attribute or skill name, that is, column header in the getCharSkills() data table</param>
        /// <returns></returns>
        public double getStdDevAttribute(string attrName)
        {
            return mStdDevCharSkills[attrName];
        }

        public DataTable getCharSkills()
        {
            return mCharSkills;
        }

        public DataTable getCharStats()
        {
            return mCharStats;
        }

        public Dictionary<string, string> getKingdomOverview()
        {
            if (mKingdomOverview != null)
            {
                return mKingdomOverview;
            }
            else
            {
                Load_Kingdom_Overview();
                return mKingdomOverview;
            }
        }

        public bool isSkillUsedByProfession(string profTitle, string skillName)
        {
            // Find the profession
            List<Profession> professions = _gnomanEmpire.Fortress.Professions;
            foreach (Profession profession in professions)
            {
                if (profession.Title == profTitle)
                {
                    int index = Array.IndexOf(skillNames, skillName);
                    return profession.AllowedSkills.IsSkillAllowed(skillTypes[index]);
                }
            }
            return false;
        }

        #endregion // Methods
    }
}
