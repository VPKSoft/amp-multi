﻿#region License
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

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using TagLib;
using VPKSoft.RandomizationUtils;
using File = TagLib.File;

namespace amp.UtilityClasses
{
    public class MusicFile
    {
        private readonly string fileName;
        private readonly string filePath;
        private readonly string fileExt;
        private readonly string songName;
        private readonly string fullPath;
        private string tAlbum;
        private string tArtist;
        private readonly long fileSize;
        // ReSharper disable once IdentifierTypo
        private string tagstr;
        private uint tYear;
        private uint tTrack;
        private string tTitle;
        private int id;
        private int queueIndex = -1;
        private int visualIndex;
        private bool picLoaded;
        private bool tagLoaded;
        private float volume = 1.0F;
        private int rating = 500;
        private Tag tag;
        private string overrideName = string.Empty;
        public int Duration;
        public IPicture[] Pictures;

        public string FileNameNoPath => fileName;

        public string TagString
        {
            get => tagstr;

            set => tagstr = value;
        }

        public void LoadPic()
        {
            if (picLoaded)
            {
                return;
            }
            try
            {
                using (File tagFile = File.Create(fullPath))
                {
                    Pictures = tagFile.Tag.Pictures;
                }
            }
            catch
            {
                // Do nothing..
            }
            picLoaded = true;
        }

        public void LoadTag(bool force = false)
        {
            if (tagLoaded && !force)
            {
                return;
            }
            try
            {                
                using (File tagFile = File.Create(fullPath))
                {
                    tAlbum = tagFile.Tag.Album;
                    if (tagFile.Tag.AlbumArtists.Length > 0)
                    {
                        tArtist = tagFile.Tag.AlbumArtists[0];
                    }
                    Duration = (int)Math.Round(tagFile.Properties.Duration.TotalSeconds);
                    tYear = tagFile.Tag.Year;
                    tTitle = tagFile.Tag.Title;
                    tTrack = tagFile.Tag.Track;
                    tag = tagFile.Tag;
                }
                tagstr = GetTagString(tag);
            }
            catch
            {
                // ignored..
            }

            tagLoaded = true;
        }

        public void GetTagFromDataReader(SQLiteDataReader dr)
        {
            tArtist = dr.GetString(9);
            tAlbum = dr.GetString(10);

            if (uint.TryParse(dr.GetString(11), out _))
            {
                tTrack = uint.Parse(dr.GetString(11));
            }

            if (uint.TryParse(dr.GetString(12), out _))
            {
                tYear = uint.Parse(dr.GetString(12));
            }

            tTitle = dr.GetString(13);
        }

        private string GetTagString(Tag strTag, bool goDeep = false)
        {
            string retval = string.Empty;
            PropertyInfo[] pis = strTag.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.PropertyType != typeof(string) &&
                    pi.PropertyType != typeof(uint) &&
                    pi.PropertyType != typeof(UInt32) &&
                    pi.PropertyType != typeof(string[]) && 
                    pi.PropertyType != typeof(Tag[]))
                {
                    continue;
                }

                try
                {
                    if (pi.GetValue(tag) == null)
                    {
                        continue;
                    }
                }
                catch
                {
                    continue;
                }

                if (pi.PropertyType == typeof(Tag[]) && goDeep)
                {
                    foreach (Tag tmpTag in ((Tag[]) pi.GetValue(tag)))
                    {
                        retval += GetTagString(tmpTag);
                    }
                }
                if (pi.PropertyType == typeof(string[]))
                {
                    foreach (string str in ((string[]) pi.GetValue(tag)))
                    {
                        retval += str;
                    }
                }
                else
                {
                    retval += pi.GetValue(tag).ToString();
                }
            }
            retval = retval.TrimStart("TagLib.Tag".ToCharArray());
            return retval;
        }

        private int changeQueryCount;
        private bool valueChanged;

        public bool SongChanged
        {
            set
            {
                valueChanged = value;
                if (value)
                {
                    changeQueryCount = 0;
                }
            }

            get
            {
                changeQueryCount++;
                if (changeQueryCount >= 2)
                {
                    bool btmp = valueChanged;
                    valueChanged = false;
                    return btmp;
                }

                return valueChanged;
            }
        }



        public string OverrideName
        {
            get => overrideName;

            set
            {
                if (value != string.Empty)
                {
                    SongChanged = true;
                    overrideName = value;
                }
            }
        }

        public bool RatingChanged { get; set; }

        public int Rating
        {
            get => rating;

            set
            {
                if (value < 0 || value > 1000)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                rating = value;
                SongChanged = true;
            }
        }

        public float Volume
        {
            get => volume;

            set
            {
                if (value < 0 || value > 2)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                volume = value;
                SongChanged = true;
            }
        }


        public int ID
        {
            get => id;

            set => id = value;
        }

        public string Artist => (tArtist ?? string.Empty).Trim();

        public string Album => (tAlbum ?? string.Empty).Trim();

        public string Title => (tTitle ?? string.Empty).Trim();

        public string Year => tYear == 0 ? "????" : tYear.ToString();

        public string Track => tTrack == 0 ? string.Empty : tTrack.ToString();

        public string FullFileName => fullPath;

        public long FileSize => fileSize;

        public int QueueIndex
        {
            get => queueIndex;
            set
            {
                queueIndex = value;
                queueChanged = true;
            }
        }

        public int AlternateQueueIndex { get; set; } = -1;

        public int VisualIndex
        {
            get => visualIndex;

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                visualIndex = value;
            }
        }

        public static void RemoveById(ref List<MusicFile> files, int id)
        {
            for (int i = files.Count - 1; i >= 0; i--)
            {
                if (files[i].ID == id)
                {
                    files.RemoveAt(i);
                }
            }
        }

        private static bool queueChanged;

        public static bool QueueChanged
        {
            get
            {
                bool tmp = queueChanged;
                queueChanged = false;
                return tmp;
            }
        }

        public void QueueInsert(ref List<MusicFile> files, bool filtered, int mfIndex = -1) // 14.10.17
        {
            if (QueueIndex > 0)
            {
                Queue(ref files);
            }

            if ((filtered || mfIndex != -1) && QueueIndex == 0)
            {
                foreach (MusicFile mf in files)
                {
                    if (mf.QueueIndex > 0)
                    {
                        mf.QueueIndex++; //::QUEUE
                    }
                }
                QueueIndex = 1; //::QUEUE
                return;
            }


            int iQueue = 0;

            int idx = files.IndexOf(this); // hope nobody does sort the list !!
            int idFirst = -1, idLast = -1;

            for (int i = 0; i < files.Count; i++)
            {
                if (i == idx)
                {
                    continue;
                }

                if (files[i].QueueIndex > iQueue && i < idx)
                {
                    iQueue = files[i].QueueIndex;
                    idFirst = i;
                }
                else if (files[i].QueueIndex > iQueue && i > idx)
                {
                    iQueue = files[i].QueueIndex;
                    idLast = i;
                }
            }

            if (idLast < idx && idFirst > idx)
            {
                QueueIndex = files[idFirst].QueueIndex; //::QUEUE
            }
            else if (idFirst >= 0 && idFirst < idx)
            {
                QueueIndex = files[idFirst].QueueIndex + 1; //::QUEUE
            }
            else 
            {
                QueueIndex = QueueIndex == 0 ? 1 : iQueue; //::QUEUE
            }

            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].QueueIndex >= QueueIndex && idx != i)
                {
                    files[i].QueueIndex++; //::QUEUE
                }
            }
        }

        public void Queue(ref List<MusicFile> files)
        {
            int iQueue = 0;
            foreach (MusicFile mf in files)
            {
                if (mf.QueueIndex > iQueue)
                {
                    iQueue = mf.QueueIndex;
                }
            }
            if (QueueIndex >= 1)
            {
                int tmp = QueueIndex;
                QueueIndex = 0;
                foreach (MusicFile mf in files)
                {
                    if (mf.QueueIndex > tmp)
                    {
                        mf.QueueIndex--; //::QUEUE
                    }
                }
                return;
            }
            QueueIndex = iQueue + 1;
            //::QUEUE
        }

        #region AlternateQueue

        public void QueueInsertAlternate(ref List<MusicFile> files, bool filtered, int mfIndex = -1) // 14.10.17
        {
            if (AlternateQueueIndex > 0)
            {
                QueueAlternate(ref files);
            }

            if ((filtered || mfIndex != -1) && QueueIndex == 0)
            {
                foreach (MusicFile mf in files)
                {
                    if (mf.AlternateQueueIndex > 0)
                    {
                        mf.AlternateQueueIndex++; //::QUEUE
                    }
                }
                AlternateQueueIndex = 1; //::QUEUE
                return;
            }


            int iQueue = 0;

            int idx = files.IndexOf(this); // hope nobody does sort the list !!
            int idFirst = -1, idLast = -1;

            for (int i = 0; i < files.Count; i++)
            {
                if (i == idx)
                {
                    continue;
                }

                if (files[i].AlternateQueueIndex > iQueue && i < idx)
                {
                    iQueue = files[i].AlternateQueueIndex;
                    idFirst = i;
                }
                else if (files[i].AlternateQueueIndex > iQueue && i > idx)
                {
                    iQueue = files[i].AlternateQueueIndex;
                    idLast = i;
                }
            }

            if (idLast < idx && idFirst > idx)
            {
                AlternateQueueIndex = files[idFirst].AlternateQueueIndex; //::QUEUE
            }
            else if (idFirst >= 0 && idFirst < idx)
            {
                AlternateQueueIndex = files[idFirst].AlternateQueueIndex + 1; //::QUEUE
            }
            else
            {
                AlternateQueueIndex = AlternateQueueIndex == 0 ? 1 : iQueue; //::QUEUE
            }

            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].AlternateQueueIndex >= AlternateQueueIndex && idx != i)
                {
                    files[i].AlternateQueueIndex++; //::QUEUE
                }
            }
        }

        public void QueueAlternate(ref List<MusicFile> files)
        {
            int iQueue = 0;
            foreach (MusicFile mf in files)
            {
                if (mf.AlternateQueueIndex > iQueue)
                {
                    iQueue = mf.AlternateQueueIndex;
                }
            }
            if (AlternateQueueIndex >= 1)
            {
                int tmp = AlternateQueueIndex;
                AlternateQueueIndex = 0;
                foreach (MusicFile mf in files)
                {
                    if (mf.AlternateQueueIndex > tmp)
                    {
                        mf.AlternateQueueIndex--; //::QUEUE
                    }
                }
                return;
            }
            AlternateQueueIndex = iQueue + 1;
            //::QUEUE
        }

        #endregion

        #region PlayListNaming
        public static string FormulaReplace(string formula, string value, string formulaStr)
        {
            int formulaStart = formula.IndexOf(formulaStr, StringComparison.Ordinal);

            
            if (formulaStr + "#" == "#ARTIST#" ||
                formulaStr + "#" == "#ALBUM#" ||
                formulaStr + "#" == "#TITLE#" ||
                formulaStr + "#" == "#QUEUE#" ||
                formulaStr + "#" == "#ALTERNATE_QUEUE#" ||
                formulaStr + "#" == "#RENAMED#" ||
                formulaStr + "#" == "#TRACKNO#")
            {
                formula = formula.Replace(formulaStr + "#", value);
                return formula;
            }
            
            int stringPos = -1;
            int formulaEnd = -1;
            string formulaPart = formula;
            for (int i = formulaStart + 1;  i < formulaPart.Length && (stringPos == -1 || formulaEnd == -1); i++)
            {
                if (formulaPart[i] == '?' && stringPos == -1)
                {
                    stringPos = i;
                }

                if (formulaPart[i] == '#' && formulaEnd == -1)
                {
                    formulaEnd = i;
                }
            }

            if (stringPos != -1 && formulaEnd != -1 && stringPos <= formulaEnd)
            {
                string stringPart = formulaPart.Substring(stringPos + 1, formulaEnd - stringPos - 1);
                string startPart = formula.Substring(0, formulaStart);
                string endPart = formula.Substring(formulaEnd + 1);

                string[] tmpParts = stringPart.Split('^');

                string stringPartStart = tmpParts.Length == 1 ? string.Empty :
                    (tmpParts.Length > 1 ? tmpParts[0] : string.Empty);
                string stringPartEnd = tmpParts.Length > 1 ? tmpParts[1] : stringPart;

                formula = string.IsNullOrEmpty(value) ?
                    startPart + endPart :
                    startPart + stringPartStart + value + stringPartEnd + endPart;
            }           
            else
            {
                return string.Empty;
            }
            return formula;
        }

        public enum FormulaType
        {
            None,
            Artist,
            Album,
            TrackNo,
            Title,
            QueueIndex,
            AlternateQueueIndex,
            Renamed
        }


        internal static string GetNextFormula(string formula, out FormulaType formulaType)
        {
            int startIndex = formula.IndexOf('#');
            int endIndex = -1;
            for (int i = startIndex + 1; i < formula.Length; i++)
            {
                if (formula[i] == '#')
                {
                    endIndex = i;
                    break;
                }
            }

            if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
            {
                string result = formula.Substring(startIndex, endIndex - startIndex);
                if (result.StartsWith("#ARTIST"))
                {
                    formulaType = FormulaType.Artist;
                }
                else if (result.StartsWith("#ALBUM"))
                {
                    formulaType = FormulaType.Album;
                }
                else if (result.StartsWith("#TRACKNO"))
                {
                    formulaType = FormulaType.TrackNo;
                }
                else if (result.StartsWith("#TITLE"))
                {
                    formulaType = FormulaType.Title;
                }
                else if (result.StartsWith("#QUEUE"))
                {
                    formulaType = FormulaType.QueueIndex;
                }
                else if (result.StartsWith("#ALTERNATE_QUEUE"))
                {
                    formulaType = FormulaType.AlternateQueueIndex;
                }
                else if (result.StartsWith("#RENAMED"))
                {
                    formulaType = FormulaType.Renamed;
                }
                else
                {
                    formulaType = FormulaType.None;
                    return string.Empty;
                }
                return result;
            }

            formulaType = FormulaType.None;
            return string.Empty;
        }

        public static string GetString(string formula, string artist, string album, int trackNo, 
            string title, string songName, int queueIndex, int alternateQueueIndex, string overrideName, string onError, out bool error)
        {
            try
            {
                string formulaStr;
                while ((formulaStr = GetNextFormula(formula, out var formulaType)) != string.Empty)
                {
                    if (formulaType == FormulaType.Artist)
                    {
                        formula = FormulaReplace(formula, artist == string.Empty ? string.Empty : artist, formulaStr);
                    }
                    else if (formulaType == FormulaType.Album)
                    {
                        formula = FormulaReplace(formula, album == string.Empty ? string.Empty : album, formulaStr);
                    }
                    else if (formulaType == FormulaType.TrackNo)
                    {
                        formula = FormulaReplace(formula, trackNo <= 0 ? string.Empty : trackNo.ToString(), formulaStr);
                    }
                    else if (formulaType == FormulaType.Title)
                    {
                        formula = FormulaReplace(formula, title.Trim() == string.Empty ? songName : title, formulaStr);
                    }
                    else if (formulaType == FormulaType.QueueIndex)
                    {
                        formula = FormulaReplace(formula, queueIndex <= 0 ? string.Empty : queueIndex.ToString(), formulaStr);
                    }
                    else if (formulaType == FormulaType.AlternateQueueIndex)
                    {
                        formula = FormulaReplace(formula, alternateQueueIndex <= 0 ? string.Empty : alternateQueueIndex.ToString(), formulaStr);
                    }
                    else if (formulaType == FormulaType.Renamed)
                    {
                        formula = FormulaReplace(formula, overrideName == string.Empty ? songName : overrideName, formulaStr);
                    }
                }
                error = false;
                return formula;
            }
            catch
            {
                error = true;
                return onError;
            }
        }

        private int TrackInt
        {
            get
            {
                if (int.TryParse(Track, out _))
                {
                    return int.Parse(Track);
                }

                return 0;
            }
        }

        #region WeightedRandomization
        // ReSharper disable once InconsistentNaming
        internal int SKIPPED_EARLY { get; set; } = 0;
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once IdentifierTypo
        internal int NPLAYED_RAND { get; set; } = 0;
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once IdentifierTypo
        internal int NPLAYED_USER { get; set; } = 0;

        internal static Random Random = new Random();

        internal static bool InRange(double value, double randomValue, double min, double max, double tolerancePercentage)
        {
            double range = (max - min) / 100 * tolerancePercentage;
            return value <= randomValue + range && value >= randomValue - range;
        }

        internal static int RandomWeighted(List<MusicFile> musicFiles)
        {
            if (musicFiles == null || musicFiles.Count == 0)
            {
                return -1;
            }

            List<MusicFile> results = new List<MusicFile>();

            double valueMin = musicFiles.Min(f => f.Rating);
            double valueMax = musicFiles.Max(f => f.Rating);

            double biased = BiasedRandom.RandomBiased(valueMin, valueMax, Settings.Settings.BiasedRating);

            if (Settings.Settings.BiasedRatingEnabled)
            {
                results.AddRange(musicFiles.FindAll(f =>
                    InRange(f.Rating, biased, valueMin, valueMax, Settings.Settings.Tolerance)));
            }

            valueMin = musicFiles.Min(f => f.NPLAYED_USER);
            valueMax = musicFiles.Max(f => f.NPLAYED_USER);
            biased = BiasedRandom.RandomBiased(valueMin, valueMax, Settings.Settings.BiasedPlayedCount);

            if (Settings.Settings.BiasedPlayedCountEnabled)
            {
                results.AddRange(musicFiles.FindAll(f =>
                    InRange(f.NPLAYED_USER, biased, valueMin, valueMax, Settings.Settings.Tolerance)));
            }

            valueMin = musicFiles.Min(f => f.NPLAYED_RAND);
            valueMax = musicFiles.Max(f => f.NPLAYED_RAND);
            biased = BiasedRandom.RandomBiased(valueMin, valueMax, Settings.Settings.BiasedRandomizedCount);

            if (Settings.Settings.BiasedRandomizedCountEnabled)
            {
                results.AddRange(musicFiles.FindAll(f =>
                   InRange(f.NPLAYED_RAND, biased, valueMin, valueMax, Settings.Settings.Tolerance)));
            }

            valueMin = musicFiles.Min(f => f.SKIPPED_EARLY);
            valueMax = musicFiles.Max(f => f.SKIPPED_EARLY);
            biased = BiasedRandom.RandomBiased(valueMin, valueMax, Settings.Settings.BiasedSkippedCount);

            if (Settings.Settings.BiasedSkippedCountEnabled)
            {
                results.AddRange(musicFiles.FindAll(f =>
                   InRange(f.SKIPPED_EARLY, biased, valueMin, valueMax, Settings.Settings.Tolerance)));
            }

            int result = -1;

            if (results.Count > 0)
            {
                int tmpIndex = Random.Next(results.Count);
                result = musicFiles.FindIndex(f => f.ID == results[tmpIndex].ID);
            }

            if (result == -1)
            {
                result = Random.Next(musicFiles.Count);
            }
            return result;
        }

        #endregion


        public string ToString(bool queue)
        {
            if (overrideName != string.Empty)
            {
                return GetString(Settings.Settings.AlbumNamingRenamed, Artist, Album, TrackInt, Title,
                    songName, queue ? queueIndex : 0, AlternateQueueIndex, overrideName, ToStringOld(queue), out _);
            }

            return GetString(Settings.Settings.AlbumNaming, Artist, Album, TrackInt, Title,
                songName, queue ? queueIndex : 0, AlternateQueueIndex, overrideName, ToStringOld(queue), out _);
        }



        public override string ToString()
        {
            return ToString(true);
        }

        private string ToStringOld(bool queue)
        {
            string alternateQueue = AlternateQueueIndex > 0 && queue ? " [*=" + AlternateQueueIndex + "]" : string.Empty;
            if (overrideName != string.Empty)
            {
                return overrideName + ((queueIndex >= 1 && queue) ? " [" + queueIndex + "]" : "") + alternateQueue;
            }

            return
                (Artist == string.Empty ? string.Empty : Artist + " - ") +
                (Album == string.Empty ? string.Empty : Album + " - ") +
                (Title.Length > 0 ? Title : songName) +
                (queueIndex >= 1 ? " [" + queueIndex + "]" : "") + alternateQueue;
        }

        public string SongName => ToString();

        public string SongNameNoQueue => ToString(false);

        #endregion

        public string GetFileName()
        {
            return fullPath;
        }

        public MusicFile(string fullPath): this(fullPath, 0)
        {
        }

        public bool Match(string search)
        {
            if (search.Trim() == string.Empty)
            {
                return true;
            }
            search = search.ToUpper().Trim();
            bool bfound1 = Artist.ToUpper().Contains(search) ||
                           Album.ToUpper().Contains(search) ||
                           Title.ToUpper().Contains(search) ||
                           Year.ToUpper().Contains(search) ||
                           Track.ToUpper().Contains(search) ||
                           FullFileName.ToUpper().Contains(search) ||
                           OverrideName.ToUpper().Contains(search) ||
                           tagstr.ToUpper().Contains(search);

            string[] search2 = search.Split(' ');
            if (search2.Length <= 1 || bfound1)
            {
                return bfound1;
            }
            bool bfound2 = true;
            foreach(string str in search2)
            {
                var tmpStr = str.ToUpper();
                bfound2 &= Artist.ToUpper().Contains(tmpStr) ||
                           Album.ToUpper().Contains(tmpStr) ||
                           Title.ToUpper().Contains(tmpStr) ||
                           Year.ToUpper().Contains(tmpStr) ||
                           Track.ToUpper().Contains(search) ||
                           FullFileName.ToUpper().Contains(tmpStr) ||
                           OverrideName.ToUpper().Contains(search) ||
                           tagstr.ToUpper().Contains(tmpStr);
            }
            return bfound2;
        }

        public MusicFile(string fullPath, int id)
        {
            this.id = id;
            fileName = Path.GetFileName(fullPath);
            filePath = Path.GetFullPath(fullPath);
            songName = Path.GetFileNameWithoutExtension(fullPath);
            fileExt = Path.GetExtension(fullPath);
            FileInfo fInfo = new FileInfo(fullPath);
            fileSize = fInfo.Length;
            tAlbum = string.Empty;
            tArtist = string.Empty;
            tagstr = string.Empty;
            tYear = 0;
            tTitle = string.Empty;
            this.fullPath = fullPath;
            tTrack = 0;
            Duration = 0;
        }

        public MusicFile(MusicFile mf)
        {
            id = mf.id;
            fileName = mf.fileName;
            filePath = mf.filePath;
            songName = mf.songName;
            fileExt = mf.fileExt;
            fileSize = mf.fileSize;
            tAlbum = mf.tAlbum;
            tArtist = mf.tArtist;
            tagstr = mf.tagstr;
            tYear = mf.tYear;
            tTitle = mf.tTitle;
            fullPath = mf.fullPath;
            tTrack = mf.tTrack;
            Duration = mf.Duration;
        }
    }
}
