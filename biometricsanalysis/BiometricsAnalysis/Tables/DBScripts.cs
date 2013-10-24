using BiometricsAnalysis.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BiometricsAnalysis.Tables
{
    /// <summary>
    /// <author>GABRIEL</author>>
    /// </summary>
    class DBScripts
    {


        Database_Utils databaseConnection;
        string K2_BA = "K2_BA"; //
        public DBScripts()
        {
            databaseConnection = new Database_Utils();
            //faceRecognitionAddEntries("D:\\CASIA-3D-FaceV1\\CASIA-3D-FaceV1\\3D-Face-BMP");
        }
        public void faceRecognitionAddEntries(string root)
        {
            Stack<string> dirs = new Stack<string>(20);
            if (!System.IO.Directory.Exists(root))
            {
                throw new ArgumentException();
            }
            dirs.Push(root);
            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                    foreach (string s in subDirs)
                    {


                        string[] files = null;
                        try
                        {
                            files = System.IO.Directory.GetFiles(s);
                        }catch (Exception e){
                            Console.WriteLine(e.Message);
                            continue;
                        }

                        //Insert entry with the name of the folder
                        String folderName = s.Substring(s.LastIndexOf('\\') + 1);
                        int EntryId = databaseConnection.insert_entry(folderName, folderName);
                        foreach (string file in files)
                        {
                            try
                            {
                                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                                String imagePath = root + "\\" + folderName + "\\" + fi.Name;
                                BitmapImage image = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                                Face f = new Face(-1, EntryId, 1f, false, "generic", image);

                                databaseConnection.insert_face(f);
                                Console.WriteLine("{0}: {1}, {2}", fi.Name, fi.Length, fi.CreationTime);
                            }
                            catch (System.IO.FileNotFoundException e)
                            {
                                Console.WriteLine(e.Message);
                                continue;
                            }
                        }
                        Console.WriteLine(s);
                    }
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }


                dirs.Clear();
            }
        }
    }
}
