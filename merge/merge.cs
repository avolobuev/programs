using System;
using System.Collections.Generic;
using System.IO;

namespace MergeNmsp
{
    class Merge
    {
        //composing
        static void _work(string sInPath, string sOutPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(@sInPath);
                using (Stream _o = File.OpenWrite(@sOutPath))
                {
                    foreach (var item in dir.GetFiles("*.*"))
                    {
                        Console.WriteLine("...processing {0}", item.Name);
                        using (Stream _i = File.OpenRead(@item.FullName))
                        {
                            _i.CopyTo(_o);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error occured:");
                Console.WriteLine(e.Message);
            }
        }

        //showing help information
        static void _printHelp()
        {
            Console.WriteLine("SYNTAX:");
            Console.WriteLine();
            Console.WriteLine("merge");
            Console.WriteLine("\t [ -h | -i=<path> -o=<path> ]");
            Console.WriteLine("DETAILS:");
            Console.WriteLine("\t -h - help information");
            Console.WriteLine("\t -i - absolute or relative(to .exe) path to files for merging");
            Console.WriteLine("\t -o - absolute or relative(to .exe) path to output file");
            Console.WriteLine("EXAMPLES: ");
            Console.WriteLine("\t merge -i=in -o=out\\in.txt");
            Console.WriteLine("\t merge -i=C:\\in -o=C:\\out\\in.tx");
            Console.WriteLine("\t merge -i=.\\in -o=.\\out\\in.txt");
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 0)
                {
                    if (args[0].Equals("-h"))
                    {
                        _printHelp();
                    }
                    else
                    {
                        if (args.Length == 2)
                        {
                            string sInFileName = string.Empty;
                            string sOutFileName = string.Empty;

                            for (int i = 0; i < args.Length; i++)
                            {
                                var temp = args[i].Split('=');
                                if (temp.Length != 2)
                                {
                                    throw new Exception("Not valid parameter: " + args[i]);
                                }
                                else
                                {
                                    switch (temp[0])
                                    {
                                        case "-o":
                                            {
                                                if (!Directory.Exists((new FileInfo(@temp[1]).DirectoryName)))
                                                    throw new Exception("Not valid path to files: " + temp[1]);
                                                sOutFileName = temp[1].Replace("\"", "");
                                                break;
                                            }
                                        case "-i":
                                            {
                                                if (!Directory.Exists(@temp[1]))
                                                    throw new Exception("Not valid path for output file: " + temp[1]);
                                                sInFileName = temp[1].Replace("\"", "");
                                                break;
                                            }
                                        default:
                                            {
                                                throw new Exception("Not valid option: " + temp[1]);
                                            }
                                    }
                                }
                            }
                            Console.WriteLine("Wait...");
                            _work(@sInFileName, @sOutFileName);
                            Console.WriteLine("Process's over!");
                        }
                        else
                        {
                            throw new Exception("Not valid options");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error occured:");
                Console.WriteLine(e.Message);
            }
        }

    }
}
