using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("Bevenuto in copyright_projects (only .NET projects supported)\n" +
                "Gli input contrassegnati da (*) sono OBBLIGATORI, gli altri sono facoltativi, premere INVIO per skippare l'inserimento.\n");

                
                while (string.IsNullOrEmpty(fullPath))
                {
                    Console.Write("\nPercorso completo progetto(*):  ");
                    fullPath = Console.ReadLine().Trim();
                }

                
                while (string.IsNullOrEmpty(author))
                {
                    Console.Write("\nNome completo autore/i(*):  ");
                    author = Console.ReadLine().Trim();
                }
                
                Console.Write("\nNome azienda:  ");
                company = Console.ReadLine();
            }
            catch (Exception)
            {
                Environment.Exit(0);
            }

            string[] files = Directory.GetFiles(fullPath, "*.*cs", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string tempFile = Path.GetFullPath(file) + ".tmp";

                using (StreamReader reader = new StreamReader(file))
                {
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        writer.WriteLine("// <copyright file=" + Path.GetFileNameWithoutExtension(file) + (!string.IsNullOrEmpty(company) ? " company=" + company : "") +
                                          " Copyright (c) " + DateTime.Now.Year + " All Rights Reserved </copyright>\n" +
                                          "// <author> " + author + " </author>\n" +
                                          "// <date> " + DateTime.Now + " </date>\n\n");
            

                        string line = string.Empty;
                        while ((line = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
                File.Delete(file);
                File.Move(tempFile, file);
            }
        }
    }
}
