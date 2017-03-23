﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Foundation;

namespace PlantDB.iOS
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");
            
            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            string dbPath = Path.Combine(libFolder, filename);
            CopyDatabaseIfNotExists(dbPath);
            return dbPath;
        }



        private static void CopyDatabaseIfNotExists(string dbPath)
        {
            if (!File.Exists(dbPath))
            {
                var existingDb = NSBundle.MainBundle.PathForResource("miniplant", "db3");
                File.Copy(existingDb, dbPath);
            }
        }
    }
}
