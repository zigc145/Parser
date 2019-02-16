using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Parser
{
    /* Copyright (c) Februar 2019 All rights reserved
     * Author: Žiga Cigole 
     * Email: cigole.ziga@gmail.com
     */

    static class Program
    {
        static StreamReader sr;
        //static string filepath = @"C:\Users\user asus\source\repos\Parser\Parser\PANELBOX.LOG";
        static string filepath;
        static List<string> znacke = new List<string>() { "//SYSTEM NO", "//ROBOT NAME", "//CONTROL POWER", "//SERVO POWER", "//PLAYBACK TIME", "//MOVING TIME",
                                                            "//ENERGY TIME","//MOTOPLUS APP","LANGUAGE", "CONTROL GROUP","APPLICATION","OPTION FUNCTION",
                                                            "CHANGE TRACKING LIST","INITIALIZED FILES","CHANGE TRACKING PARAMETER"};
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Vnesi pot do datoteke: ");
                string filepath = Console.ReadLine();
                sr = new StreamReader(filepath);

                Console.WriteLine("\nIzberi način pridobivanja podatkov: ");
                Console.WriteLine("1. Vrni vrednost glede na značko");
                Console.WriteLine("2. Vrne izpis med dvema značkama\n");

                int x = 0;

                while (!int.TryParse(Console.ReadLine(), out x) || (x!=1 && x!=2))
                {
                    Console.WriteLine("Vnesi številko metode!");
                }
                Console.Clear();

                //Metoda 1 - pridobivanje vrednosti glede na značko
                if (x == 1)
                {
                    Console.WriteLine("Vnesi številko značke za katero želiš pridobiti vrednost: ");

                    //Izpiši vse značke
                    for (int i = 0; i < znacke.Count; i++)
                    {
                        int index = i;
                        index++;
                        Console.WriteLine(index + ": " + znacke.ElementAt(i));
                    }

                    //Pridobi id izbrane znacke
                    int izbranaZnacka;
                    while (int.TryParse(Console.ReadLine(), out izbranaZnacka))
                    {
                        if (checkZnacka(izbranaZnacka))
                        {
                            //Pridobi znacko
                            String tag = znacke.ElementAt(izbranaZnacka - 1);

                            //Izpiši vrednost značke
                            returnValue(filepath, tag);
                            Console.WriteLine();
                            Console.WriteLine("Vnesi številko značke za katero želiš pridobiti vrednost oz vnesi 0 za zaključek: ");
                        }
                        else break;
                    }
                    
                    Console.ReadLine();    
                }

                //Metoda 2 - vrne izpis med dvema značkama
                else if(x==2)
                {
                    //Izpiši vse značke
                    for (int i = 0; i < znacke.Count; i++)
                    {
                        int index = i;
                        index++;
                        Console.WriteLine(index + ": " + znacke.ElementAt(i));
                    }
                    Console.WriteLine("\nVnesi id prve značke:");
                    int izbranaZnacka1,izbranaZnacka2;
                    string firstTag = "";
                    string secondTag = "";
                        
                    while (!int.TryParse(Console.ReadLine(), out izbranaZnacka1))
                    {
                        Console.WriteLine("Vnesi id prve značke:");
                    }

                    if (checkZnacka(izbranaZnacka1))
                    {
                        firstTag = znacke.ElementAt(izbranaZnacka1 - 1);

                        Console.WriteLine("Vnesi id druge značke:");
                        while (!int.TryParse(Console.ReadLine(), out izbranaZnacka2) || izbranaZnacka1==izbranaZnacka2)
                        {
                            Console.WriteLine("Vnesi id druge značke:");
                        }
                        if (checkZnacka(izbranaZnacka2))
                        {
                            secondTag = znacke.ElementAt(izbranaZnacka2 - 1);

                            Console.WriteLine("Text med " + firstTag + " in " + secondTag + ":\n");
                            returnBetween(filepath, firstTag, secondTag);
                            //Console.ReadLine();
                            Environment.ExitCode=0;
                        }
                    }
                    Console.ReadLine();
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("Datoteka ni najdena!");
                Console.ReadLine();
                Environment.Exit(3);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
                Environment.Exit(3);
            }
        }

        public static string returnJSON(string tag)
        {
            if(tag.Equals("//SYSTEM NO"))
            {

            }
            return "";
        }

        public static bool In<T>(this T obj, params T[] args)
        {
            return args.Contains(obj);
        }

        public static void returnValue(string path,string tag)
        {
            String line;
            sr = new StreamReader(path);
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.StartsWith(tag))
                {
                    //SYSTEM NO
                    if (tag.Equals("//SYSTEM NO"))
                    {
                        Console.WriteLine("//SYSTEM NO");
                        line = line.Split(":")[1].Trim();
                        Console.WriteLine(line);
                    }
                    //ROBOT NAME //CONTROL POWER //SERVO POWER //PLAYBACK TIME //MOVING TIME //ENERGY TIME
                    else if (tag.In("//ROBOT NAME", "//CONTROL POWER", "//SERVO POWER", "//PLAYBACK TIME", "//MOVING TIME", "//ENERGY TIME"))
                    {
                        Console.WriteLine(tag);
                        while ((line = sr.ReadLine()) != null && (line.StartsWith("TOTAL") || line.StartsWith("R")))
                        {
                            Console.WriteLine(line);
                        }
                    }
                    //MOTOPLUS APP
                    else if (tag.Equals("//MOTOPLUS APP"))
                    {
                        Console.WriteLine(tag);
                        while ((line = sr.ReadLine()) != null && line.StartsWith("0"))
                        {
                            Console.WriteLine(line);
                        }
                    }
                    //LANGUAGE
                    else if (tag.Equals("LANGUAGE"))
                    {
                        Console.WriteLine(tag);
                        line = sr.ReadLine();
                        line = sr.ReadLine();
                        while ((line = sr.ReadLine()) != null && line.StartsWith("LANGUAGE"))
                        {
                            Console.WriteLine(line);
                        }
                    }
                    //CONTROL GROUP
                    else if (tag.Equals("CONTROL GROUP"))
                    {
                        Console.WriteLine(tag);
                        line = sr.ReadLine();
                        line = sr.ReadLine();
                        while ((line = sr.ReadLine()) != null && !line.StartsWith("===="))
                        {
                            Console.WriteLine(line);
                        }
                    }
                    //APPLICATION
                    else if (tag.Equals("APPLICATION"))
                    {
                        Console.WriteLine(tag);
                        line = sr.ReadLine();
                        line = sr.ReadLine();
                        while ((line = sr.ReadLine()) != null && !line.StartsWith("OPTION FUNCTION"))
                        {
                            Console.WriteLine(line);
                        }
                    }
                    //OPTION FUNCTION
                    else if (tag.Equals("OPTION FUNCTION"))
                    {
                        Console.WriteLine("Vnesi dodaten parameter ali nadaljuj (Enter)");
                        Console.WriteLine(tag);
                        string extraTag = Console.ReadLine();
                        if (extraTag.Length > 0)
                        {
                            while ((line = sr.ReadLine()) != null && !line.ToUpper().StartsWith(extraTag.ToUpper())) { }
                            Console.WriteLine(line);
                        }
                        else
                        {
                            line = sr.ReadLine();
                            line = sr.ReadLine();
                            while ((line = sr.ReadLine()) != null && !line.StartsWith("===="))
                            {
                                Console.WriteLine(line);
                            }
                        }
                    }
                    //CHANGE TRACKING LIST
                    else if (tag.Equals("CHANGE TRACKING LIST"))
                    {
                        Console.WriteLine(tag);
                        while ((line = sr.ReadLine()) != null && !line.StartsWith("======="))
                        {
                            Console.WriteLine(line);
                        }
                    }
                    //INITIALIZED FILES
                    else if (tag.Equals("INITIALIZED FILES"))
                    {
                        Console.WriteLine(tag);
                        while ((line = sr.ReadLine()) != null && !line.StartsWith("======="))
                        {
                            Console.WriteLine(line);
                        }
                    }
                    //CHANGE TRACKING PARAMETER
                    else if (tag.Equals("CHANGE TRACKING PARAMETER"))
                    {
                        Console.WriteLine(tag);
                        while ((line = sr.ReadLine()) != null && !line.StartsWith("======="))
                        {
                            Console.WriteLine(line);
                        }
                    }

                }
            }
        }

        public static void returnBetween(string path,string first, string second)
        {
            String line;
            sr = new StreamReader(path);
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.StartsWith(first))
                {
                    if(first.Equals("//SYSTEM NO"))
                    {
                        line = line.Split(":")[1].Trim();
                        Console.WriteLine(line);
                    }
                    while ((line = sr.ReadLine()) != null && !line.StartsWith(second))
                    {
                        Console.WriteLine(line);
                    }
                }
            }
        }

        public static bool checkZnacka(int znackaId)
        {
            //če je značka 0 zaključi program
            if (znackaId == 0)
            {
                Console.WriteLine("Program se bo zaključil!");
                Environment.ExitCode = 2;
                return false;
            }
            //če značka ni najdena zaključi program
            else if (znackaId > znacke.Count || znackaId<0)
            {
                Console.WriteLine("Značka ni najdena, program se bo zaključil!");
                Environment.ExitCode = 1;
                return false;
            }
            return true;
        }
    }
}
