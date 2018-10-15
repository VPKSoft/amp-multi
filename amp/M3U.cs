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
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace amp
{
    public class M3UEntry
    {
        private string fileName = string.Empty;
        private string fileDesc = string.Empty;

        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        public string FileDesc
        {
            get
            {
                return fileDesc;
            }

            set
            {
                if (value != string.Empty)
                {
                    fileDesc = value;
                }
            }
        }

        public M3UEntry(string FileName, string FileDesc)
        {
            fileName = FileName;
            fileDesc = FileDesc;
        }
    }

    public class M3U
    {
        private Encoding enc = null;
        private string fileName = string.Empty;
        private string fileDir = string.Empty;
        public List<M3UEntry> M3UFiles = new List<M3UEntry>();
        public M3U(string FileName)
        {
            List<string> fileLines = new List<string>();
            fileName = FileName;
            fileDir = Path.GetDirectoryName(fileName).TrimEnd('\\') + "\\";
            if (Path.GetExtension(fileName).ToUpper() == ".m3u".ToUpper())
            {
                enc = Encoding.GetEncoding(1252);
            }
            else if (Path.GetExtension(fileName).ToUpper() == ".m3u8".ToUpper())
            {
                enc = new UTF8Encoding();
            }
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, enc))
                {
                    while (!sr.EndOfStream)
                    {
                        fileLines.Add(sr.ReadLine());
                    }
                }
            }

            if (fileLines.Count == 0)
            {
                return;
            }

            string fname;
            for (int i = 0; i < fileLines.Count; i++)
            {
                if (fileLines[i] == string.Empty)
                {
                    continue;
                }
                else if (fileLines[i].StartsWith("#EXTINF:"))
                {
                    if (i + 1 < fileLines.Count)
                    {
                        if (File.Exists(fileLines[i + 1]))
                        {
                            fname = fileLines[i + 1];
                        }
                        else if (File.Exists(fileDir + fileLines[i + 1]))
                        {
                            fname = fileDir + fileLines[i + 1];
                        }
                        else
                        {
                            continue;
                        }

                        if (File.Exists(fname))
                        {
                            string fileDesc;
                            try
                            {
                                fileDesc = fileLines[i].Split(',')[1];
                            }
                            catch
                            {
                                fileDesc = string.Empty;
                            }
                            M3UFiles.Add(new M3UEntry(fname, fileDesc));
                            i++;
                            continue;
                        }
                    }
                }
                else if (Directory.Exists(fileLines[i]) || Directory.Exists(fileDir + fileLines[i]))
                {
                    if (Directory.Exists(fileLines[i]))
                    {
                        fname = fileLines[i];
                    }
                    else if (Directory.Exists(fileDir + fileLines[i]))
                    {
                        fname = fileDir + fileLines[i];
                    }
                    else
                    {
                        fname = string.Empty;
                    }
                    foreach (string dirFileName in Directory.GetFiles(fname.TrimEnd('\\') + "\\", "*.*", SearchOption.AllDirectories).ToArray())
                    {
                        M3UFiles.Add(new M3UEntry(dirFileName, string.Empty));
                    }
                }
                else if (File.Exists(fileLines[i]) || File.Exists(fileDir + fileLines[i]))
                {
                    if (File.Exists(fileLines[i]))
                    {
                        fname = fileLines[i];
                    }
                    else if (File.Exists(fileDir + fileLines[i]))
                    {
                        fname = fileDir + fileLines[i];
                    }
                    else
                    {
                        fname = string.Empty;
                    }
                    M3UFiles.Add(new M3UEntry(fname, string.Empty));
                }
            }
            for (int i = M3UFiles.Count - 1; i >= 0; i--)
            {
                if (Path.GetExtension(M3UFiles[i].FileName).ToUpper() != ".mp3".ToUpper() &&
                    Path.GetExtension(M3UFiles[i].FileName).ToUpper() != ".ogg".ToUpper() &&
                    Path.GetExtension(M3UFiles[i].FileName).ToUpper() != ".flac".ToUpper() &&
                    Path.GetExtension(M3UFiles[i].FileName).ToUpper() != ".wma".ToUpper() &&
                    Path.GetExtension(M3UFiles[i].FileName).ToUpper() != ".wav".ToUpper() &&
                    Path.GetExtension(M3UFiles[i].FileName).ToUpper() != ".m4a".ToUpper() && // Added: 01.02.2018
                    Path.GetExtension(M3UFiles[i].FileName).ToUpper() != ".aif".ToUpper() && // Added: 01.02.2018
                    Path.GetExtension(M3UFiles[i].FileName).ToUpper() != ".aac".ToUpper() && // Added: 01.02.2018
                    Path.GetExtension(M3UFiles[i].FileName).ToUpper() != ".aiff".ToUpper()) // Added: 01.02.2018
                {
                    M3UFiles.RemoveAt(i);
                }
            }
        }
    }
}