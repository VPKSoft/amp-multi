#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System.Globalization;
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
                string result = vnml["AlbumNaming", "value", "    #ARTIST? - ##ALBUM? - ##TRACKNO?(^) ##TITLE?##QUEUE? [^]##ALTERNATE_QUEUE?[ *=^]#"].ToString();

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
                string result = vnml["AlbumNamingRenamed", "value", "    #RENAMED?##QUEUE? [^]##ALTERNATE_QUEUE?[ *=^]#"].ToString();

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

        #region BiasedRandomization
        private static bool? _BiasedRandom = null;
        public static bool BiasedRandom
        {
            get
            {
                if (_BiasedRandom == null)
                {
                    VPKNml vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                    _BiasedRandom = bool.Parse(vnml["biasedRandom", "enabled", false.ToString()].ToString());
                }
                return (bool)_BiasedRandom;
            }

            set
            {
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["biasedRandom", "enabled"] = value;
                vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
                _BiasedRandom = value;
            }
        }

        private static double _Tolerance = -1;
        public static double Tolerance
        {
            get
            {
                if (_Tolerance == -1)
                {
                    VPKNml vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                    _Tolerance =
                        double.Parse(vnml["biasedRandom", "tolerance", (10.0).ToString(CultureInfo.InvariantCulture)].ToString(),
                        CultureInfo.InvariantCulture);
                }
                return _Tolerance;
            }

            set
            {
                _Tolerance = value;
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["biasedRandom", "tolerance"] = _Tolerance.ToString(CultureInfo.InvariantCulture);
                vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
            }
        }

        private static double _BiasedRating = -1;
        public static double BiasedRating
        {
            get
            {
                if (_BiasedRating == -1)
                {
                    VPKNml vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                    _BiasedRating = 
                        double.Parse(vnml["biasedRandom", "biasedRating", (50.0).ToString(CultureInfo.InvariantCulture)].ToString(), 
                        CultureInfo.InvariantCulture);
                }
                return _BiasedRating;
            }

            set
            {
                _BiasedRating = value;
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["biasedRandom", "biasedRating"] = _BiasedRating.ToString(CultureInfo.InvariantCulture);
                vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
            }
        }

        public static bool BiasedRatingEnabled
        {
            get
            {
                return BiasedRating >= 0;
            }

            set
            {
                if (!value)
                {
                    BiasedRating = -1;
                }
            }
        }

        private static double _BiasedPlayedCount = -1;
        public static double BiasedPlayedCount
        {
            get
            {
                if (_BiasedPlayedCount == -1)
                {
                    VPKNml vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");

                    _BiasedPlayedCount =
                        double.Parse(vnml["biasedRandom", "biasedPlayedCount", (50.0).ToString(CultureInfo.InvariantCulture)].ToString(),
                        CultureInfo.InvariantCulture);

                }
                return _BiasedPlayedCount;
            }

            set
            {
                _BiasedPlayedCount = value;
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["biasedRandom", "biasedPlayedCount"] = _BiasedPlayedCount.ToString(CultureInfo.InvariantCulture);
                vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
            }
        }

        public static bool BiasedPlayedCountEnabled
        {
            get
            {
                return BiasedPlayedCount >= 0;
            }

            set
            {
                if (!value)
                {
                    BiasedPlayedCount = -1;
                }
            }
        }

        private static double _BiasedRandomizedCount = -1;
        public static double BiasedRandomizedCount
        {
            get
            {
                if (_BiasedRandomizedCount == -1)
                {
                    VPKNml vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");

                    _BiasedRandomizedCount =
                        double.Parse(vnml["biasedRandom", "biasedPlayedCount", (50.0).ToString(CultureInfo.InvariantCulture)].ToString(),
                        CultureInfo.InvariantCulture);
                }
                return _BiasedRandomizedCount;
            }

            set
            {
                _BiasedRandomizedCount = value;
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["biasedRandom", "biasedRandomizedCount"] = _BiasedRandomizedCount.ToString(CultureInfo.InvariantCulture);
                vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
            }
        }

        public static bool BiasedRandomizedCountEnabled
        {
            get
            {
                return BiasedRandomizedCount >= 0;
            }

            set
            {
                if (!value)
                {
                    BiasedRandomizedCount = -1;
                }
            }
        }

        private static double _BiasedSkippedCount = -1;
        public static double BiasedSkippedCount
        {
            get
            {
                if (_BiasedSkippedCount == -1)
                {
                    VPKNml vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");

                    _BiasedSkippedCount =
                        double.Parse(vnml["biasedRandom", "biasedSkippedCount", (50.0).ToString(CultureInfo.InvariantCulture)].ToString(),
                        CultureInfo.InvariantCulture);

                }
                return _BiasedSkippedCount;
            }

            set
            {
                _BiasedSkippedCount = value;
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["biasedRandom", "biasedSkippedCount"] = _BiasedSkippedCount.ToString(CultureInfo.InvariantCulture);
                vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
            }
        }

        public static bool BiasedSkippedCountEnabled
        {
            get
            {
                return BiasedSkippedCount >= 0;
            }

            set
            {
                if (!value)
                {
                    BiasedSkippedCount = -1;
                }
            }
        }
        #endregion

        // the current language (Culture) to be used with the software..
        private static CultureInfo _Culture = null;

        /// <summary>
        /// Gets or sets the current language (Culture) to be used with the software's localization.
        /// </summary>
        public static CultureInfo Culture
        {
            get
            {
                if (_Culture == null)
                {
                    VPKNml vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                    string culture = vnml["culture", "value", "en-US"].ToString();

                    vnml["culture", "value"] = culture;
                    vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
                    _Culture = new CultureInfo(culture);
                }
                return _Culture;
            }

            set
            {
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["culture", "value"] = value.Name;
                vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
                _Culture = value;
            }
        }
    }
}
