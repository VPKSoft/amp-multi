#region license
/*
This file is public domain.
You may freely do anything with it.

Copyright (c) VPKSoft 2018
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace amp
{
    public class ScriptRunner
    {
        private class DBScriptBlock
        {
            public List<string> SQLBlock = new List<string>();
            public int DBVer = 0;
        }

        public static bool RunScript(string sqliteDatasource)
        {
            try
            {
                int dbVersion = 0;
                List<DBScriptBlock> sqlBlocks = new List<DBScriptBlock>();
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + sqliteDatasource + ";Pooling=true;FailIfMissing=false"))
                {
                    conn.Open();


                    int DBVer = 0;
                    string line;

                    using (SQLiteCommand command = new SQLiteCommand(conn))
                    {
                        using (StreamReader sr = new StreamReader(Program.AppInstallDir + "script.sql_script"))
                        {
                            while (!sr.EndOfStream)
                            {
                                while (!(line = sr.ReadLine()).StartsWith("--VER " + DBVer)) { }

                                DBScriptBlock scriptBlock = new DBScriptBlock();
                                scriptBlock.DBVer = Convert.ToInt32(line.Split(' ')[1]);

                                while (!(line = sr.ReadLine()).EndsWith("--ENDVER " + DBVer))
                                {
                                    scriptBlock.SQLBlock.Add(line);
                                }
                                DBVer++;
                                sqlBlocks.Add(scriptBlock);
                            }
                        }
                    }

                    using (SQLiteCommand command = new SQLiteCommand(conn))
                    {
                        try 
                        {
                            command.CommandText = "SELECT MAX(DBVERSION) AS VER FROM DBVERSION; ";
                            using (SQLiteDataReader dr = command.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    dbVersion = dr.GetInt32(0) + 1;
                                }
                                else
                                {
                                    dbVersion = 0;
                                }
                            }
                        }
                        catch 
                        {
                            dbVersion = 0;
                        }
                    }

                    for (int i = dbVersion; i < sqlBlocks.Count; i++)
                    {
                        string exec = string.Empty;
                        foreach (string sqLine in sqlBlocks[i].SQLBlock)
                        {
                            exec += sqLine + Environment.NewLine;
                        }
                        try
                        {
                            using (SQLiteCommand command = new SQLiteCommand(conn))
                            {
                                command.CommandText = exec;
                                command.ExecuteNonQuery();
                            }
                        } 
                        catch
                        {

                        }
                        exec =  "INSERT INTO DBVERSION(DBVERSION) " + Environment.NewLine +
                                "SELECT " + sqlBlocks[i].DBVer + " " + Environment.NewLine +
                                "WHERE NOT EXISTS(SELECT 1 FROM DBVERSION WHERE DBVERSION = " + sqlBlocks[i].DBVer + "); " + Environment.NewLine;
                        using (SQLiteCommand command = new SQLiteCommand(conn))
                        {
                            command.CommandText = exec;
                            command.ExecuteNonQuery();
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
