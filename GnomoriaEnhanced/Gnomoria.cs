using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;

using Game;
using Game.Common;
using GameLibrary;
using SevenZip;

namespace GnomoriaEnhanced
{
    public class Gnomoria
    {
        #region Variables
        
        // Parent for Logging
        private GnomoriaEnhanced.Main _parent;

        // Private variables set by class and access via methods
        private string m_installLocation;
        private string m_savePath;
        private string m_saveBackupPath;

        #endregion

        public Gnomoria(GnomoriaEnhanced.Main parent)
        {
            _parent = parent;
        }

        public void Invoke()
        {
            SevenZip.SevenZipExtractor.SetLibraryPath("7z.dll");
            try
            {
                _parent.Debug(String.Format("Current Directory: {0}", Directory.GetCurrentDirectory()));
                MethodInfo method = typeof(GnomanEmpire).GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic);
                method.Invoke(GnomanEmpire.Instance, null);
                m_savePath = GnomanEmpire.SaveFolderPath(@"Worlds\");
                m_saveBackupPath = GnomanEmpire.SaveFolderPath(@"Addons\Gnomoria Enhanced\WorldsBackup\");
                GnomanEmpire.Instance.AudioManager.SetMusicVolume(0);
                GnomanEmpire.Instance.AudioManager.SetSFXVolume(0);
                _parent.Info("Gnomoria has been Invoked");
            }
            catch (TargetInvocationException e)
            {
                _parent.Debug(String.Format("TagetInvocationException: {0}", e.Message));
                if (e.InnerException != null)
                {
                    _parent.Debug(String.Format("Inner Exception: {0}", e.InnerException.Message));
                }
            }
        }

        public GnomanEmpire LoadGame(string saveGame)
        {
            try
            {
                Console.WriteLine("[Gnomoria] LoadGame: Loading world {0}", saveGame);
                GnomanEmpire.Instance.LoadGame(saveGame);
                Console.WriteLine("[Gnomoria] LoadGame Instance.LoadGame successful, now setting _gnomanEmpire");
            }
            catch (System.Exception e)
            {
                _parent.Error(String.Format("LoadGame threw exception {0}", e.Message));
                throw e;
            }
            return GnomanEmpire.Instance;
        }

        /// <summary>
        /// This method returns the installed version of Gnomoria as well as sets the m_installLocation
        /// </summary>
        public int[] GetInstalledVersion()
        {
            int[] version = new int[4];
            // version[0] Major
            // version[1] Minor
            // version[2] Build
            // version[3] Revision
            RegistryKey baseRegistryKey = Registry.LocalMachine;
            string subKey = "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
            RegistryKey uninstallKey = baseRegistryKey.OpenSubKey(subKey);
            string[] allApplications = uninstallKey.GetSubKeyNames();
            _parent.Debug("Before allApplications");
            foreach (string applicationSubKeyName in allApplications)
            {
                RegistryKey appKey = baseRegistryKey.OpenSubKey(subKey + "\\" + applicationSubKeyName);
                string appName = (string)appKey.GetValue("DisplayName");
                string appVersion = (string)appKey.GetValue("DisplayVersion");
                //_parent.Debug(String.Format("[{0}] {1}", appName, appVersion));
                //Console.WriteLine(String.Format("[{0}] {1}", appName, appVersion));
                if (appName != null && appName.Contains("Gnomoria"))
                {
                    _parent.Info(String.Format("Found Gnomoria Version: {0}", appVersion));
                    m_installLocation = (string)appKey.GetValue("InstallLocation");
                    string [] strVersion = appVersion.Split('.');
                    version[0] = Convert.ToInt32(strVersion[0]);
                    version[1] = Convert.ToInt32(strVersion[1]);
                    version[2] = Convert.ToInt32(strVersion[2]);
                    if (strVersion.Count() >= 4)
                    {
                        version[3] = Convert.ToInt32(strVersion[3]);
                    }
                    else
                    {
                        version[3] = 0;
                    }
                    break;
                }

            }

            if (version == null)
            {
                _parent.Warn("Could not locate Gnomoria Installation");
                throw new GEException.NotInstalledException();
            }

            return version;
        }

        /// <summary>
        /// This method returns the installation path of Gnomoria, if it is not set it runs GetInstalledVersion and tries to locate it
        /// </summary>
        public string GetGnomoriaPath()
        {
            if (m_installLocation == null)
            {
                _parent.Debug("GetGnomoriaPath m_installLocation is not setting, attempting to locate");
                try
                {
                    GetInstalledVersion();
                    if (m_installLocation == null)
                    {
                        _parent.Warn("Could not location Gnomoria Installation Directory");
                        throw new GEException.NotInstalledException();
                    }
                }
                catch (GEException.NotInstalledException)
                {
                    _parent.Warn("Gnomoria is not installed");
                    throw new GEException.NotInstalledException();
                }
                catch (SystemException e)
                {
                    _parent.Warn("GetGnomoriaPath failed with exception: " + e.Message);
                    throw e;
                }
            }
            return m_installLocation;
        }

        /// <summary>
        /// This method returns the saved game path of Gnomoria
        /// </summary>
        public string GetGnomoriaSavePath()
        {
            if (m_savePath == null)
            {
                throw new GEException.GnomoriaNotInvoked();
            }
            return m_savePath;
        }

        public string GetGnomoriaSaveBackupPath()
        {
            if (m_saveBackupPath == null)
            {
                throw new GEException.GnomoriaNotInvoked();
            }
            return m_saveBackupPath;
        }

        public void BackupWorld(string world, string toFolder)
        {
            Console.WriteLine("Backing up world {0}", world);
            Console.WriteLine("to: {0}", world);
            if (!File.Exists(m_savePath + world))
            {
                throw new FileNotFoundException();
            }
            if (!Directory.Exists(toFolder))
            {
                try
                {
                    Directory.CreateDirectory(toFolder);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            try
            {
                DateTime dt = DateTime.Now;
                string filename = String.Format("{0:yyyyMMddHHmmss}", dt);
                string toFile = toFolder + "\\" + filename + ".sav";
                File.Copy(m_savePath + world, toFile);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        
    }
}
