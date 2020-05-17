#region License
/*
MIT License

Copyright(c) 2020 Petteri Kautonen

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
    /// <summary>
    /// The settings class for the software.
    /// </summary>
    public static class Settings
    {
        private const string SettingsFileName = "settings.vnml";
        // ReSharper disable once IdentifierTypo
        private static VPKNml vnml;

        // an old "fossil" from the past..
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

                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                int result = int.Parse(vnml["DBUpdateLevel", "value", 0.ToString()].ToString());

                if (_DbUpdateRequiredLevel != -1)
                {
                    vnml["DBUpdateLevel", "value"] = result;
                    vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
                }


                _DbUpdateRequiredLevel = result;
                return result;
            }

            set
            {
                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                vnml["DBUpdateLevel", "value"] = value;
                _DbUpdateRequiredLevel = value;
                vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
            }
        }

        // a field for the LoadEntireFileSizeLimit property..
        private static int? _LoadEntireFileSizeLimit;

        /// <summary>
        /// Gets or sets the limit of a file size to be loaded into the memory for smoother playback.
        /// </summary>
        public static int LoadEntireFileSizeLimit
        {
            get
            {
                if (_LoadEntireFileSizeLimit != null)
                {
                    return (int) _LoadEntireFileSizeLimit;
                }

                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);

                var result = int.Parse(vnml["PlayBack", "loadToMemoryLimit", "-1"].ToString());


                if (_LoadEntireFileSizeLimit == null)
                {
                    vnml["PlayBack", "loadToMemoryLimit"] = result;
                    vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
                    _LoadEntireFileSizeLimit = result;
                }

                return result;
            }

            set
            {
                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                vnml["PlayBack", "loadToMemoryLimit"] = value;
                _LoadEntireFileSizeLimit = value;
                vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
            }
        }

        // a field for the AlbumNaming property..
        private static string _AlbumNaming = string.Empty;

        /// <summary>
        /// Gets or sets the album naming instructions.
        /// </summary>
        public static string AlbumNaming
        {
            get
            {
                if (_AlbumNaming != string.Empty)
                {
                    return _AlbumNaming;
                }

                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                // ReSharper disable once StringLiteralTypo
                string result = vnml["AlbumNaming", "value", "    #ARTIST? - ##ALBUM? - ##TRACKNO?(^) ##TITLE?##QUEUE? [^]##ALTERNATE_QUEUE?[ *=^]#"].ToString();

                if (_AlbumNaming == string.Empty)
                {
                    vnml["AlbumNaming", "value"] = result;
                    vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
                }

                _AlbumNaming = result;
                return result;
            }

            set
            {
                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                vnml["AlbumNaming", "value"] = value;
                _AlbumNaming = value;
                vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
            }
        }

        // a field for the AlbumNamingRenamed property..
        private static string _AlbumNamingRenamed = string.Empty;

        /// <summary>
        /// Gets or sets the renamed album naming instructions.
        /// </summary>
        public static string AlbumNamingRenamed
        {
            get
            {
                if (_AlbumNamingRenamed != string.Empty)
                {
                    return _AlbumNamingRenamed;
                }

                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                string result = vnml["AlbumNamingRenamed", "value", "    #RENAMED?##QUEUE? [^]##ALTERNATE_QUEUE?[ *=^]#"].ToString();

                if (_AlbumNamingRenamed == string.Empty)
                {
                    vnml["AlbumNamingRenamed", "value"] = result;
                    vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
                }

                _AlbumNamingRenamed = result;
                return result;
            }

            set
            {
                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                vnml["AlbumNamingRenamed", "value"] = value;
                _AlbumNamingRenamed = value;
                vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
            }
        }

        #region BiasedRandomization
        // a field for the BiasedRandom property..
        private static bool? _BiasedRandom;

        /// <summary>
        /// Gets or sets a value indicating whether to use biased randomization with randomizing songs.
        /// </summary>
        public static bool BiasedRandom
        {
            get
            {
                if (_BiasedRandom == null)
                {
                    vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                    _BiasedRandom = bool.Parse(vnml["biasedRandom", "enabled", false.ToString()].ToString());
                }
                return (bool)_BiasedRandom;
            }

            set
            {
                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                vnml["biasedRandom", "enabled"] = value;
                vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
                _BiasedRandom = value;
            }
        }

        // a field for the Tolerance property..
        private static double _Tolerance = -1;

        /// <summary>
        /// Gets or sets the tolerance for biased randomization.
        /// </summary>
        public static double Tolerance
        {
            get
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_Tolerance == -1)
                {
                    vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                    _Tolerance =
                        double.Parse(vnml["biasedRandom", "tolerance", (10.0).ToString(CultureInfo.InvariantCulture)].ToString(),
                        CultureInfo.InvariantCulture);
                }
                return _Tolerance;
            }

            set
            {
                _Tolerance = value;
                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                vnml["biasedRandom", "tolerance"] = _Tolerance.ToString(CultureInfo.InvariantCulture);
                vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
            }
        }

        // a field for the BiasedRating property..
        private static double _BiasedRating = -1;

        /// <summary>
        /// Gets or sets the value for randomization with biased rating.
        /// </summary>
        public static double BiasedRating
        {
            get
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_BiasedRating == -1)
                {
                    vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                    _BiasedRating = 
                        double.Parse(vnml["biasedRandom", "biasedRating", (50.0).ToString(CultureInfo.InvariantCulture)].ToString(), 
                        CultureInfo.InvariantCulture);
                }
                return _BiasedRating;
            }

            set
            {
                _BiasedRating = value;
                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                vnml["biasedRandom", "biasedRating"] = _BiasedRating.ToString(CultureInfo.InvariantCulture);
                vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the biased rating in randomization is enabled.
        /// </summary>
        public static bool BiasedRatingEnabled
        {
            get => BiasedRating >= 0;

            set
            {
                if (!value)
                {
                    BiasedRating = -1;
                }
            }
        }

        // a field for the BiasedPlayedCount property..
        private static double _BiasedPlayedCount = -1;

        /// <summary>
        /// Gets or sets the value for randomization with biased played count.
        /// </summary>
        public static double BiasedPlayedCount
        {
            get
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_BiasedPlayedCount == -1)
                {
                    vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);

                    _BiasedPlayedCount =
                        double.Parse(vnml["biasedRandom", "biasedPlayedCount", (-1.0).ToString(CultureInfo.InvariantCulture)].ToString(),
                        CultureInfo.InvariantCulture);

                }
                return _BiasedPlayedCount;
            }

            set
            {
                _BiasedPlayedCount = value;
                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                vnml["biasedRandom", "biasedPlayedCount"] = _BiasedPlayedCount.ToString(CultureInfo.InvariantCulture);
                vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the biased played count in randomization is enabled.
        /// </summary>
        public static bool BiasedPlayedCountEnabled
        {
            get => BiasedPlayedCount >= 0;

            set
            {
                if (!value)
                {
                    BiasedPlayedCount = -1;
                }
            }
        }

        // a field for the BiasedRandomizedCount property..
        private static double _BiasedRandomizedCount = -1;

        /// <summary>
        /// Gets or sets the value for randomization with biased randomized count.
        /// </summary>
        public static double BiasedRandomizedCount
        {
            get
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_BiasedRandomizedCount == -1)
                {
                    vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);

                    _BiasedRandomizedCount =
                        double.Parse(vnml["biasedRandom", "biasedPlayedCount", (-1.0).ToString(CultureInfo.InvariantCulture)].ToString(),
                        CultureInfo.InvariantCulture);
                }
                return _BiasedRandomizedCount;
            }

            set
            {
                _BiasedRandomizedCount = value;
                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                vnml["biasedRandom", "biasedRandomizedCount"] = _BiasedRandomizedCount.ToString(CultureInfo.InvariantCulture);
                vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the biased randomized count in randomization is enabled.
        /// </summary>
        public static bool BiasedRandomizedCountEnabled
        {
            get => BiasedRandomizedCount >= 0;

            set
            {
                if (!value)
                {
                    BiasedRandomizedCount = -1;
                }
            }
        }

        // a field for the BiasedSkippedCount property..
        private static double _BiasedSkippedCount = -1;

        /// <summary>
        /// Gets or sets the value for randomization with biased skipped count.
        /// </summary>
        public static double BiasedSkippedCount
        {
            get
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (_BiasedSkippedCount == -1)
                {
                    vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);

                    _BiasedSkippedCount =
                        double.Parse(vnml["biasedRandom", "biasedSkippedCount", (-1.0).ToString(CultureInfo.InvariantCulture)].ToString(),
                        CultureInfo.InvariantCulture);

                }
                return _BiasedSkippedCount;
            }

            set
            {
                _BiasedSkippedCount = value;
                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                vnml["biasedRandom", "biasedSkippedCount"] = _BiasedSkippedCount.ToString(CultureInfo.InvariantCulture);
                vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the biased skipped count in randomization is enabled.
        /// </summary>
        public static bool BiasedSkippedCountEnabled
        {
            get => BiasedSkippedCount >= 0;

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
                    vnml = new VPKNml();
                    Paths.MakeAppSettingsFolder();
                    vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                    string culture = vnml["culture", "value", "en-US"].ToString();

                    vnml["culture", "value"] = culture;
                    vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
                    _Culture = new CultureInfo(culture);
                }
                return _Culture;
            }

            set
            {
                vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + SettingsFileName);
                vnml["culture", "value"] = value.Name;
                vnml.Save(Paths.GetAppSettingsFolder() + SettingsFileName);
                _Culture = value;
            }
        }
    }
}
