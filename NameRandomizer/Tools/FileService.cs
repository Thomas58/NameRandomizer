using NameRandomizer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace NameRandomizer.Tools
{
    class FileService
    {
        private static string File = "save.list";
        private static string TempFile = "temp.list";

        public static void SaveFile(EntryList list)
        {
            lock (File)
            {
                FileInfo TempFileInfo = new FileInfo(Environment.CurrentDirectory + Path.DirectorySeparatorChar + TempFile);
                if (TempFileInfo.Exists)
                    TempFileInfo.Delete();
                XmlSerializer serializer = new XmlSerializer(list.GetType());
                using (FileStream stream = TempFileInfo.OpenWrite())
                {
                    try
                    {
                        serializer.Serialize(stream, list);
                    }
                    catch (IOException e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
                FileInfo FileInfo = new FileInfo(Environment.CurrentDirectory + Path.DirectorySeparatorChar + File);
                if (FileInfo.Exists)
                    FileInfo.Delete();
                TempFileInfo.MoveTo(FileInfo.FullName);
            }
        }

        public static EntryList LoadFile()
        {
            lock (File)
            {
                EntryList list = null;
                FileInfo FileInfo = new FileInfo(Environment.CurrentDirectory + Path.DirectorySeparatorChar + File);
                if (!FileInfo.Exists)
                {
                    FileInfo.Create();
                    return new EntryList();
                }
                XmlSerializer serializer = new XmlSerializer(typeof(EntryList));
                using (FileStream stream = FileInfo.OpenRead())
                {
                    try
                    {
                        list = (EntryList)serializer.Deserialize(stream);
                    }
                    catch (Exception e) when (e is IOException || e is InvalidOperationException)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
                return list;
            }
        }
    }
}
