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
        // Only set after a successfull call to loadGame()
        private GnomanEmpire _gnomanEmpire;

        private string mSaveGameFolder;
        private string mSettingsFolder;
        private int _skillColStart; // This should probably be defined in the LoadCharSkills
        private string[] skillNames;
        private CharacterSkillType[] skills;
        private DataTable mCharSkills;
        private DataTable mCharStats;
        private Dictionary<string, string> mKingdomOverview;

        // Constructor
        public GameModel()
        {
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
                Initialize_Character_Skills();

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

        private void Initialize_Character_Skills()
        {
            // Setup temp array's to be modified and setup correctly
            ArrayList skillNamesTemp = new ArrayList(Enum.GetNames(typeof(CharacterSkillType)));
            ArrayList skillsTemp = new ArrayList(Enum.GetValues(typeof(CharacterSkillType)));

            // The following values are in the CharacterSkillType Array however we do not want them.
            int index = -1;

            index = skillNamesTemp.IndexOf("LaborStart");
            skillNamesTemp.RemoveAt(index);
            skillsTemp.RemoveAt(index);

            index = skillNamesTemp.IndexOf("LaborEnd");
            skillNamesTemp.RemoveAt(index);
            skillsTemp.RemoveAt(index);

            index = skillNamesTemp.IndexOf("Count");
            skillNamesTemp.RemoveAt(index);
            skillsTemp.RemoveAt(index);

            index = skillNamesTemp.IndexOf("Discipline");
            skillNamesTemp.RemoveAt(index);
            skillsTemp.RemoveAt(index);

            // The following values have different names and we are renaming to show it correctly
            index = skillNamesTemp.IndexOf("NaturalAttack");
            skillNamesTemp[index]="Fighting";

            // Convert the temp arrays and store them in the class attributes.
            skills     = (CharacterSkillType[])skillsTemp.ToArray(typeof(CharacterSkillType));
            skillNames = (string[])skillNamesTemp.ToArray(typeof(string));

            // Clean up
            skillsTemp = null;
            skillNamesTemp = null;
        }

        public Result LoadGame(string saveGame)
        {
            Result result = new Result();

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

                DataColumn cNum = new DataColumn("Num", typeof(string));
                mCharStats.PrimaryKey = new DataColumn[] { mCharStats.Columns.Add(cNum.ColumnName) };

                DataColumn cName   = mCharStats.Columns.Add("Name", typeof(string));
                DataColumn cHunger = mCharStats.Columns.Add("Hunger", typeof(int));
                DataColumn cThirst = mCharStats.Columns.Add("Thirst", typeof(int));
                DataColumn cBlood  = mCharStats.Columns.Add("Blood Level", typeof(int));
                DataColumn cRest   = mCharStats.Columns.Add("Rest Level", typeof(int));

                foreach (var Char in _gnomanEmpire.World.AIDirector.PlayerFaction.Members)
                {
                    Console.WriteLine("[Game Model] Load_Character_Stats: Adding Character: {0}",Char.Value.Name());
                    DataRow tmpRow = mCharStats.NewRow();
                    tmpRow[0] = Char.Key;
                    tmpRow[1] = Char.Value.Name();
                    tmpRow[2] = Char.Value.Body.HungerLevel;
                    tmpRow[3] = Char.Value.Body.ThirstLevel;
                    tmpRow[4] = Char.Value.Body.BloodLevel;
                    tmpRow[5] = Char.Value.Body.RestLevel;
                    mCharStats.Rows.Add(tmpRow);
                }
                mCharStats.Columns[0].ReadOnly = true;
                mCharStats.Columns[1].ReadOnly = true;
                mCharStats.Columns[2].ReadOnly = true;
                mCharStats.Columns[3].ReadOnly = true;
                mCharStats.Columns[4].ReadOnly = true;
                mCharStats.Columns[5].ReadOnly = true;

                result.Success = true;
                Console.WriteLine("[Game Model] Load_Character_Stats: Adding Character Information Completed");
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public Result Load_Character_Skills(int skillColStart)
        {
            Result result = new Result();

            this._skillColStart = skillColStart;

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

                DataColumn cName = mCharSkills.Columns.Add("Name", typeof(string));
                DataColumn cProfession = mCharSkills.Columns.Add("Profession", typeof(string));
                DataColumn cJob = mCharSkills.Columns.Add("Current Job", typeof(string));

                foreach (string name in skillNames)
                {
                    mCharSkills.Columns.Add(name, typeof(int));
                }

                foreach (var Char in _gnomanEmpire.World.AIDirector.PlayerFaction.Members)
                {
                    DataRow tmpRow = mCharSkills.NewRow();
                    tmpRow[0] = Char.Key;
                    tmpRow[1] = Char.Value.Name();
                    tmpRow[2] = Char.Value.Title();
                    if (Char.Value.Job == null)
                    {
                        if (Char.Value.Body.IsSleeping == true)
                        {
                            tmpRow[3] = "Sleeping";
                        }
                        else
                        {
                            tmpRow[3] = "Idle";
                        }
                    }
                    else
                    {
                        tmpRow[3] = Char.Value.Job.JobName();
                    }

                    int tmpCol = _skillColStart;
                    foreach (CharacterSkillType skill in skills)
                    {
                        tmpRow[tmpCol] = Char.Value.SkillLevel(skill);
                        tmpCol++;
                    }
                    mCharSkills.Rows.Add(tmpRow);
                }

                mCharSkills.Columns[0].ReadOnly = true;
                mCharSkills.Columns[1].ReadOnly = true;
                mCharSkills.Columns[2].ReadOnly = true;
                mCharSkills.Columns[3].ReadOnly = true;

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
    }
}
