#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using VPKSoft.Utils;

namespace amp
{
    public static class Settings
    {
        private static int _DBUpdateRequiredLevel = -1;
        /// <summary>
        /// Gets or sets a value of to which level the database is updated to.
        /// </summary>
        public static int DBUpdateRequiredLevel
        {
            get
            {
                if (_DBUpdateRequiredLevel != -1)
                {
                    return _DBUpdateRequiredLevel;
                }

                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                int result = int.Parse(vnml["DBUpdateLevel", "value", 0.ToString()].ToString());

                if (_DBUpdateRequiredLevel != -1)
                {
                    vnml["DBUpdateLevel", "value"] = result;
                    vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
                }


                _DBUpdateRequiredLevel = result;
                return result;
            }

            set
            {
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["DBUpdateLevel", "value"] = value;
                _DBUpdateRequiredLevel = value;
                vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
            }
        }

        private static string _AlbumNaming = string.Empty; 
        public static string AlbumNaming
        {
            get
            {
                if (_AlbumNaming != string.Empty)
                {
                    return _AlbumNaming;
                }

                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                string result = vnml["AlbumNaming", "value", "    #ARTIST? - ##ALBUM? - ##TRACKNO?(^) ##TITLE? - ##QUEUE?[^]##ALTERNATE_QUEUE?[*=^]#"].ToString();

                if (_AlbumNaming == string.Empty)
                {
                    vnml["AlbumNaming", "value"] = result;
                    vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
                }

                _AlbumNaming = result;
                return result;
            }

            set
            {
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["AlbumNaming", "value"] = value;
                _AlbumNaming = value;
                vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
            }
        }

        private static string _AlbumNamingRenamed = string.Empty;
        public static string AlbumNamingRenamed
        {
            get
            {
                if (_AlbumNamingRenamed != string.Empty)
                {
                    return _AlbumNamingRenamed;
                }

                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                string result = vnml["AlbumNamingRenamed", "value", "    #RENAMED? ##QUEUE?[^]##ALTERNATE_QUEUE?[*=^]#"].ToString();

                if (_AlbumNamingRenamed == string.Empty)
                {
                    vnml["AlbumNamingRenamed", "value"] = result;
                    vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
                }

                _AlbumNamingRenamed = result;
                return result;
            }

            set
            {
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["AlbumNamingRenamed", "value"] = value;
                _AlbumNamingRenamed = value;
                vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
            }
        }
    }
}
