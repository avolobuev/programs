using System;
using System.Collections.Generic;
using System.IO;

namespace Splitter
{
    class Program
    {
        //splitting
        static void _work(string sInPath, string sOutPath, uint nLines)
        {
            try
            {
                List<string> vLine = new List<string>();

                uint nStrNo = 0;
                uint nPart = 0;
                string sNewFile = string.Empty;

                using (StreamReader _o = File.OpenText(@sInPath))
                {
                    string sLine;
                    while ((sLine = _o.ReadLine()) != null)
                    {
                        if (nStrNo == nLines)
                        {
                            nStrNo = 0;
                            nPart++;
                            sNewFile = sOutPath + @"\part_" + nPart;
                            Console.WriteLine("...filling {0} file", "part_" + nPart);
                            File.WriteAllLines(@sNewFile, vLine);
                            vLine.Clear();
                        }
                        vLine.Add(sLine);
                        nStrNo++;
                    }
                }
                nPart++;
                sNewFile = sOutPath + @"\part_" + nPart;
                Console.WriteLine("...filling {0} file", "part_" + nPart);

                FileInfo _flast = new FileInfo(@sNewFile);
                using (StreamWriter _i = _flast.AppendText())
                {
                    for (int i = 0; i < vLine.Count; i++)
                    {
                        _i.WriteLine(vLine[i]);
                    }
                }
            }
            catch(Exception e)
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
            Console.WriteLine("splitter");
            Console.WriteLine("\t [ -h | -i=<path> -o=<path> -n=<lines> ]");
            Console.WriteLine("DETAILS:");
            Console.WriteLine("\t -h - help information");
            Console.WriteLine("\t -i - absolute or relative(to .exe) path to input file");
            Console.WriteLine("\t -o - absolute or relative(to .exe) path for output files");
            Console.WriteLine("\t -n - count of lines to split by [1..{0}]", UInt32.MaxValue - 1);
            Console.WriteLine("EXAMPLES: ");
            Console.WriteLine("\t splitter -i=in\\in.txt -o=out -n=1000");
            Console.WriteLine("\t splitter -i=C:\\in\\in.txt -o=C:\\out -n=1000");
            Console.WriteLine("\t splitter -i=.\\in\\in.txt -o=.\\out -n=1000");
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
                        if (args.Length == 3)
                        {
                            string sInFileName = string.Empty;
                            string sOutFileName = string.Empty;
                            uint nLines = 0;

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
                                        case "-i":
                                            {
                                                if (!File.Exists(@temp[1]))
                                                    throw new Exception("Not valid path to input file: " + temp[1]);
                                                sInFileName = temp[1];
                                                break;
                                            }
                                        case "-o":
                                            {
                                                if (!Directory.Exists(@temp[1]))
                                                    throw new Exception("Not valid path for output files: " + temp[1]);
                                                sOutFileName = temp[1];
                                                break;
                                            }
                                        case "-n":
                                            {
                                                if (!UInt32.TryParse(temp[1], out nLines))
                                                    throw new Exception("Not valid value for count of lines: " + temp[1]);
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
                            _work(@sInFileName, @sOutFileName, nLines);
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
