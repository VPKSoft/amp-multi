#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Data.SQLite;
using VPKSoft.RandomizationUtils;

namespace amp
{
    public class MusicFile
    {
        private string fileName;
        private string filePath;
        private string fileExt;
        private string songName;
        private string fullPath;
        private string tAlbum;
        private string tArtist;
        private long fileSize;
        private string tagstr;
        private uint tYear;
        private uint tTrack;
        private string tTitle;
        private int id;
        private int queueIndex = -1;
        private int alternateQueueIndex = -1;
        private int lastQueueIndex = 0;
        private int visualIndex = 0;
        private bool picLoaded = false;
        private bool tagLoaded = false;
        private float volume = 1.0F;
        private int rating = 500;
        private bool ratingChanged = false;
        private TagLib.Tag tag = null;
        private string overrideName = string.Empty;
        public int Duration = 0;
        public TagLib.IPicture[] Pictures;

        public string FileNameNoPath
        {
            get
            {
                return fileName;
            }
        }

        public string TagString
        {
            get
            {
                return tagstr;
            }

            set
            {
                tagstr = value;
            }
        }

        public void LoadPic()
        {
            if (picLoaded)
            {
                return;
            }
            try
            {
                using (TagLib.File tagFile = TagLib.File.Create(fullPath))
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
                using (TagLib.File tagFile = TagLib.File.Create(fullPath))
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

        private string GetTagString(TagLib.Tag strTag, bool goDeep = false)
        {
            string retval = string.Empty;
            PropertyInfo[] pis = strTag.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (pi.PropertyType != typeof(string) &&
                    pi.PropertyType != typeof(uint) &&
                    pi.PropertyType != typeof(UInt32) &&
                    pi.PropertyType != typeof(string[]) && 
                    pi.PropertyType != typeof(TagLib.Tag[]))
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

                if (pi.PropertyType == typeof(TagLib.Tag[]) && goDeep)
                {
                    foreach (TagLib.Tag tmpTag in (pi.GetValue(tag) as TagLib.Tag[]))
                    {
                        retval += GetTagString(tmpTag);
                    }
                }
                if (pi.PropertyType == typeof(string[]))
                {
                    foreach (string str in (pi.GetValue(tag) as string[]))
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

        private int changeQueryCount = 0;
        private bool valueChanged = false;

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
                else
                {
                    return valueChanged;
                }
            }
        }



        public string OverrideName
        {
            get
            {
                return overrideName;
            }
            
            set
            {
                if (value != string.Empty)
                {
                    SongChanged = true;
                    overrideName = value;
                }
            }
        }

        public bool RatingChanged
        {
            get
            {
                return ratingChanged;
            }

            set
            {
                ratingChanged = value;
            }
        }

        public int Rating
        {
            get
            {
                return rating;
            }

            set
            {
                if (value < 0 || value > 1000)
                {
                    throw new ArgumentOutOfRangeException("Rating");
                }
                else
                {
                    rating = value;
                    SongChanged = true;
                }
            }
        }

        public float Volume
        {
            get
            {
                return volume;
            }

            set
            {
                if (value < 0 || value > 2)
                {
                    throw new ArgumentOutOfRangeException("volume");
                }
                else
                {
                    volume = value;
                    SongChanged = true;
                }
            }
        }


        public int ID
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Artist
        {
            get
            {
                return (tArtist ?? string.Empty).Trim();
            }
        }

        public string Album
        {
            get
            {
                return (tAlbum ?? string.Empty).Trim();
            }
        }

        public string Title
        {
            get
            {
                return (tTitle ?? string.Empty).Trim();
            }
        }

        public string Year
        {
            get
            {
                return tYear == 0 ? "????" : tYear.ToString();
            }
        }

        public string Track
        {
            get
            {
                return tTrack == 0 ? string.Empty : tTrack.ToString();
            }
        }

        public string FullFileName
        {
            get
            {
                return fullPath;
            }
        }

        public long FileSize
        {
            get
            {
                return fileSize;
            }
        }

        public int QueueIndex
        {
            get
            {
                return queueIndex;
            }
            set
            {
                lastQueueIndex = queueIndex;
                queueIndex = value;
                queueChanged = true;
            }
        }

        public int AlternateQueueIndex
        {
            get
            {
                return alternateQueueIndex;
            }
            set
            {
                alternateQueueIndex = value;
            }
        }
        public int LastQueueIndex
        {
            get
            {
                return lastQueueIndex;
            }
        }

        public int VisualIndex
        {
            get
            {
                return visualIndex;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    visualIndex = value;
                }
            }
        }

        public static void RemoveByID(ref List<MusicFile> files, int id)
        {
            for (int i = files.Count - 1; i >= 0; i--)
            {
                if (files[i].ID == id)
                {
                    files.RemoveAt(i);
                }
            }
        }

        internal int GetFirstQueueIndex(ref List<MusicFile> files)
        {
            int qIdx = int.MaxValue;
            foreach (MusicFile mf in files)
            {
                if (mf.QueueIndex > 0 && mf.QueueIndex < qIdx)
                {
                    qIdx = mf.QueueIndex;
                }
            }
            return qIdx == int.MaxValue ? 1 : qIdx;
        }

        private static bool queueChanged = false;

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
        internal int GetFirstAlternateQueueIndex(ref List<MusicFile> files)
        {
            int qIdx = int.MaxValue;
            foreach (MusicFile mf in files)
            {
                if (mf.AlternateQueueIndex > 0 && mf.AlternateQueueIndex < qIdx)
                {
                    qIdx = mf.AlternateQueueIndex;
                }
            }
            return qIdx == int.MaxValue ? 1 : qIdx;
        }

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
            int formulaStart = formula.IndexOf(formulaStr);

            
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

                string stringPartStart = tmpParts != null && tmpParts.Length == 1 ? string.Empty :
                    (tmpParts.Length > 1 ? tmpParts[0] : string.Empty);
                string stringPartEnd = tmpParts != null && tmpParts.Length > 1 ? tmpParts[1] : stringPart;

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
            TrackNO,
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
                    formulaType = FormulaType.TrackNO;
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
            else
            {
                formulaType = FormulaType.None;
                return string.Empty;
            }
        }

        public static string GetString(string formula, string artist, string album, int trackNo, 
            string title, string songName, int queueIndex, int alternateQueueIndex, string overrideName, string onError, out bool error)
        {
            try
            {
                FormulaType formulaType = FormulaType.None;
                string formulaStr;
                while ((formulaStr = GetNextFormula(formula, out formulaType)) != string.Empty)
                {
                    if (formulaType == FormulaType.Artist)
                    {
                        formula = FormulaReplace(formula, artist == string.Empty ? string.Empty : artist, formulaStr);
                    }
                    else if (formulaType == FormulaType.Album)
                    {
                        formula = FormulaReplace(formula, album == string.Empty ? string.Empty : album, formulaStr);
                    }
                    else if (formulaType == FormulaType.TrackNO)
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
                else
                {
                    return 0;
                }
            }
        }

        #region WeightedRandomization
        internal int SKIPPED_EARLY { get; set; } = 0;
        internal int NPLAYED_RAND { get; set; } = 0;
        internal int NPLAYED_USER { get; set; } = 0;

        internal static Random random = new Random();

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
            double ValueMax = musicFiles.Max(f => f.Rating);

            double biased = BiasedRandom.RandomBiased(valueMin, ValueMax, Settings.BiasedRating);

            if (Settings.BiasedRatingEnabled)
            {
                results.AddRange(musicFiles.FindAll(f =>
                    InRange(f.Rating, biased, valueMin, ValueMax, Settings.Tolerance)));
            }

            valueMin = musicFiles.Min(f => f.NPLAYED_USER);
            ValueMax = musicFiles.Max(f => f.NPLAYED_USER);
            biased = BiasedRandom.RandomBiased(valueMin, ValueMax, Settings.BiasedPlayedCount);

            if (Settings.BiasedPlayedCountEnabled)
            {
                results.AddRange(musicFiles.FindAll(f =>
                    InRange(f.NPLAYED_USER, biased, valueMin, ValueMax, Settings.Tolerance)));
            }

            valueMin = musicFiles.Min(f => f.NPLAYED_RAND);
            ValueMax = musicFiles.Max(f => f.NPLAYED_RAND);
            biased = BiasedRandom.RandomBiased(valueMin, ValueMax, Settings.BiasedRandomizedCount);

            if (Settings.BiasedRandomizedCountEnabled)
            {
                results.AddRange(musicFiles.FindAll(f =>
                   InRange(f.NPLAYED_RAND, biased, valueMin, ValueMax, Settings.Tolerance)));
            }

            valueMin = musicFiles.Min(f => f.SKIPPED_EARLY);
            ValueMax = musicFiles.Max(f => f.SKIPPED_EARLY);
            biased = BiasedRandom.RandomBiased(valueMin, ValueMax, Settings.BiasedSkippedCount);

            if (Settings.BiasedSkippedCountEnabled)
            {
                results.AddRange(musicFiles.FindAll(f =>
                   InRange(f.SKIPPED_EARLY, biased, valueMin, ValueMax, Settings.Tolerance)));
            }

            int result = -1;

            if (results != null && results.Count > 0)
            {
                int tmpIndex = random.Next(results.Count);
                result = musicFiles.FindIndex(f => f.ID == results[tmpIndex].ID);
            }

            if (result == -1)
            {
                result = random.Next(musicFiles.Count);
            }
            return result;
        }

        #endregion


        public string ToString(bool queue)
        {
            if (overrideName != string.Empty)
            {
                return GetString(Settings.AlbumNamingRenamed, Artist, Album, TrackInt, Title,
                    songName, queue ? queueIndex : 0, AlternateQueueIndex, overrideName, ToStringOld(queue), out _);
            }
            else
            {
                return GetString(Settings.AlbumNaming, Artist, Album, TrackInt, Title,
                    songName, queue ? queueIndex : 0, AlternateQueueIndex, overrideName, ToStringOld(queue), out _);
            }
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
            else
            {
                return
                    (Artist == string.Empty ? string.Empty : Artist + " - ") +
                    (Album == string.Empty ? string.Empty : Album + " - ") +
                    (Title.Length > 0 ? Title : songName) +
                    (queueIndex >= 1 ? " [" + queueIndex + "]" : "") + alternateQueue;
            }
        }

        public string SongName
        {
            get
            {
                return ToString();
            }
        }

        public string SongNameNoQueue
        {
            get
            {
                return ToString(false);
            }
        }
        #endregion

        public string GetFileName()
        {
            return fullPath;
        }

        public MusicFile(string FullPath): this(FullPath, 0)
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
            string tmpStr;
            foreach(string str in search2)
            {
                tmpStr = str.ToUpper();
                bfound2 &= Artist.ToUpper().Contains(tmpStr) ||
                           Album.ToUpper().Contains(tmpStr) ||
                           Title.ToUpper().Contains(tmpStr) ||
                           Year.ToUpper().Contains(tmpStr) ||
                           Track.ToUpper().Contains(search) ||
                           FullFileName.ToUpper().Contains(tmpStr) ||
                           OverrideName.ToUpper().Contains(search) ||
                           tagstr.ToUpper().Contains(tmpStr);
            }
            return bfound1 || bfound2;
        }

        public MusicFile(string FullPath, int id)
        {
            this.id = id;
            fileName = Path.GetFileName(FullPath);
            filePath = Path.GetFullPath(FullPath);
            songName = Path.GetFileNameWithoutExtension(FullPath);
            fileExt = Path.GetExtension(FullPath);
            FileInfo fInfo = new FileInfo(FullPath);
            fileSize = fInfo.Length;
            tAlbum = string.Empty;
            tArtist = string.Empty;
            tagstr = string.Empty;
            tYear = 0;
            tTitle = string.Empty;
            fullPath = FullPath;
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
