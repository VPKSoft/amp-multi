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
        /// <summary>
        /// Gets or sets a value of to which level the database is updated to.
        /// </summary>
        public static int DBUpdateRequiredLevel
        {
            get
            {
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                return int.Parse(vnml["DBUpdateLevel", "value", 0.ToString()].ToString());
            }

            set
            {
                VPKNml vnml = new VPKNml();
                Paths.MakeAppSettingsFolder();
                vnml.Load(Paths.GetAppSettingsFolder() + "settings.vnml");
                vnml["DBUpdateLevel", "value"] = value;
                vnml.Save(Paths.GetAppSettingsFolder() + "settings.vnml");
            }
        }
    }
}
