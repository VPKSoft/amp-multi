#region License
/*
MIT License

Copyright(c) 2019 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System.Globalization;
using VPKSoft.Utils;

namespace amp.UtilityClasses.Settings
{
    public static class Settings
    {
        private static int _DbUpdateRequiredLevel = -1;
        /// <summary>
        /// Gets or sets a value of to which level the database is updated to.
        /// </summary>
        public static int DbUpdateRequiredLevel
        {
            get
            {
                if (_DbUpdateRequiredLevel != -1)
                {
                    return _DbUpdateRequiredLevel;
                }

                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                int result = int.Parse(vnml["DBUpdateLevel", "value", 0.ToString()].ToString());

                if (_DbUpdateRequiredLevel != -1)
                {
                    vnml["DBUpdateLevel", "value"] = result;
                    vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
                }


                _DbUpdateRequiredLevel = result;
                return result;
            }

            set
            {
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["DBUpdateLevel", "value"] = value;
                _DbUpdateRequiredLevel = value;
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
        private static bool? _BiasedRandom;
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
                // ReSharper disable once CompareOfFloatsByEqualityOperator
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
                        double.Parse(vnml["biasedRandom", "biasedPlayedCount", (-1.0).ToString(CultureInfo.InvariantCulture)].ToString(),
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
                        double.Parse(vnml["biasedRandom", "biasedPlayedCount", (-1.0).ToString(CultureInfo.InvariantCulture)].ToString(),
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
                        double.Parse(vnml["biasedRandom", "biasedSkippedCount", (-1.0).ToString(CultureInfo.InvariantCulture)].ToString(),
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
        private static CultureInfo _Culture;

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
