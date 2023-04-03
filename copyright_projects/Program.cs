// <copyright file=Program.cs Copyright (c) 2019 All Rights Reserved </copyright>
// <author> Alessio Ciarrocchi </author>
// <date> 30/12/2019 09:27:32 </date>

using System;
using System.Collections.Generic;
using System.IO;

namespace copyright_projects
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    string fullPath = "";
                    string author = "";
                    string company = "";
                    Console.Clear();
                    Console.WriteLine("Bevenuto in copyright_projects (atm only .NET projects are supported)\n" +
                "Gli input contrassegnati da (*) sono OBBLIGATORI, gli altri sono facoltativi, premere INVIO per saltare l'inserimento.\n");

                    while (string.IsNullOrEmpty(fullPath))
                    {
                        Console.Write("\nPercorso completo progetto(*): ");
                        fullPath = Console.ReadLine().Trim();
                    }


                    while (string.IsNullOrEmpty(author))
                    {
                        Console.Write("\nNome completo autore/i(*): ");
                        author = Console.ReadLine().Trim();
                    }

                    Console.Write("\nNome azienda: ");
                    company = Console.ReadLine();

                    Console.WriteLine("\nElaborazione in corso...");

                    //string[] files = Directory.GetFiles(fullPath, "*.*cs", SearchOption.AllDirectories);
                    DirectoryInfo dir_info = new DirectoryInfo(fullPath);
                    List<string> file_list = new List<string>();
                    SearchDirectory(dir_info, file_list);

                    int okFile = 0;
                    int skipFile = 0;
                    //file_list is a list of fullpath to files
                    foreach (string file in file_list)
                    {
                        if (CanEditFile(file))
                        {
                            try
                            {
                                string tempFile = file + ".tmp";
                                using (StreamReader reader = new StreamReader(file))
                                {
                                    using (StreamWriter writer = new StreamWriter(tempFile))
                                    {
                                        writer.WriteLine("// <copyright file=" + Path.GetFileName(file) + (!string.IsNullOrEmpty(company) ? " company=" + company : "") +
                                                          " Copyright (c) " + DateTime.Now.Year + " All Rights Reserved </copyright>\n" +
                                                          "// <author> " + author + " </author>\n" +
                                                          "// <date> " + DateTime.UtcNow.ToString() + " UTC+0 </date>\n");

                                        int i = 0;
                                        bool skip = false;
                                        int maxLineSkip = 2;
                                        string line = string.Empty;
                                        while ((line = reader.ReadLine()) != null)
                                        {
                                            if (i == 0 && line.StartsWith("// <copyright file="))
                                            {
                                                skip = true;
                                                maxLineSkip = 3;
                                            }

                                            if (!skip || i > maxLineSkip)
                                            {
                                                writer.WriteLine(line);
                                            }
                                            i++;
                                        }
                                    }
                                }
                                File.Delete(file);
                                File.Move(tempFile, file);
                                okFile++;
                            }
                            catch
                            {
                                skipFile++;
                                continue;
                            }
                        }
                        else
                        {
                            skipFile++;
                        }
                    }
                    Console.WriteLine("Elaborazione conclusa con successo!");
                    Console.WriteLine("\n{0} file modificati, {1} file saltati", okFile, skipFile);
                    Console.WriteLine("Premere INVIO per iniziare da capo...");
                    Console.ReadLine();
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("\nErrore: " + exp.Message);
                Console.WriteLine("Premere INVIO per chiudere l'applicazione...");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Ricerca ricorsiva di tutti i file contenuti nel percorso specificato e nelle sue sotto cartelle.
        /// Se l'utente non ha i permessi di lettura sulla cartella, questa viene saltata.
        /// </summary>
        /// <param name="dir_info">info cartella</param>
        /// <param name="file_list">lista file trovati</param>
        private static void SearchDirectory(DirectoryInfo dir_info, List<string> file_list)
        {
            try
            {
                foreach (DirectoryInfo subdir_info in dir_info.GetDirectories())
                {
                    SearchDirectory(subdir_info, file_list);
                }
            }
            catch
            {
            }
            try
            {
                foreach (FileInfo file_info in dir_info.GetFiles("*.*cs"))
                {
                    file_list.Add(file_info.FullName);
                }
            }
            catch
            {
            }
        }

        private static bool CanEditFile(string file)
        {
            if (file != "AssemblyInfo.cs" && !file.EndsWith("AssemblyInfo.cs"))
            {
                return true;
            }
            return false;
        }

    }
}
