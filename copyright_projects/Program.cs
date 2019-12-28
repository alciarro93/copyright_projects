// <copyright file=Program.cs Copyright (c) 2019 All Rights Reserved </copyright>
// <author> Alessio Ciarrocchi </author>
// <date> 28/12/2019 04:40:40 </date>

using System;
using System.IO;

namespace copyright_projects
{
    class Program
    {
        static void Main(string[] args)
        {
            string fullPath = "";
            string author = "";
            string company = "";

            try
            {
                Console.WriteLine("Bevenuto in copyright_projects (atm only .NET projects are supported)\n" +
                "Gli input contrassegnati da (*) sono OBBLIGATORI, gli altri sono facoltativi, premere INVIO per skippare l'inserimento.\n");

                
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
            }
            catch (Exception)
            {
                Environment.Exit(0);
            }

            string[] files = Directory.GetFiles(fullPath, "*.*cs", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (file != "AssemblyInfo.cs")
                {
                    string tempFile = Path.GetFullPath(file) + ".tmp";

                    using (StreamReader reader = new StreamReader(file))
                    {
                        using (StreamWriter writer = new StreamWriter(tempFile))
                        {
                            writer.WriteLine("// <copyright file=" + Path.GetFileName(file) + (!string.IsNullOrEmpty(company) ? " company=" + company : "") +
                                              " Copyright (c) " + DateTime.Now.Year + " All Rights Reserved </copyright>\n" +
                                              "// <author> " + author + " </author>\n" +
                                              "// <date> " + DateTime.Now + " </date>\n");

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
                }
            }

        }
    }
}
