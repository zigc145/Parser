using Newtonsoft.Json;
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
        static string filepath = @"C:\Users\user asus\source\repos\Parser\Parser\PANELBOX.LOG";
        //static string filepath;
        static List<string> znacke = new List<string>() { "//SYSTEM NO", "//ROBOT NAME", "//CONTROL POWER", "//SERVO POWER", "//PLAYBACK TIME", "//MOVING TIME",
                                                            "//OPERATING TIME","//ENERGY TIME","//MOTOPLUS APP","LANGUAGE", "CONTROL GROUP","APPLICATION",
                                                            "OPTION FUNCTION","CHANGE TRACKING LIST","INITIALIZED FILES","CHANGE TRACKING PARAMETER"};
        static void Main(string[] args)
        {
            /*
            if (args.Length < 2)
            {
                Console.WriteLine("Napačno število argumentov");
                Environment.Exit(3);
            }
            //Prva metoda
            else if (args.Length == 2)
            {
                filepath = args[0];
                String tag = args[1].ToUpper();
                Output a = returnJSON(filepath, tag);
                string json=JsonConvert.SerializeObject(a);
                //returnValue(filepath, tag);
            }
            //Druga metoda
            else if (args.Length == 3)
            {
                filepath = args[0];
                String firstTag = args[1].ToUpper();
                String secondTag = args[2].ToUpper();
                returnBetween(filepath, firstTag, secondTag);
            }
            
            else
            {*/
                //Next
                try
                {
                    sr = new StreamReader(filepath);

                    for (int i = 0; i < znacke.Count; i++)
                    {
                        //returnValue(filepath, znacke[i]);
                        Console.WriteLine(returnJSON(filepath, znacke[i]));
                    }
                    Console.ReadLine();
                }
                catch (FileNotFoundException e)
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
        public static string returnJSON(string path, string tag)
        {
            String line;
            String key="";
            String value="";
            String JSON="";
            sr = new StreamReader(path);
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.ToUpper().StartsWith(tag))
                {
                    //SYSTEM NO
                    if (tag.Equals("//SYSTEM NO"))
                    {
                        key = "\"systemNo\"";
                        value = line.Split(":")[1].Trim();
                        JSON = "{\n" + key + ":\"" + value + "\",";
                    }
                    //ROBOT NAME
                    else if (tag.Equals("//ROBOT NAME"))
                    {
                        key = "\"robotNameList\"";
                        JSON += key + ":[{";
                        while ((line = sr.ReadLine()) != null && line.ToUpper().StartsWith("R"))
                        {
                            string robotTag = line.Split(":")[0].Trim();
                            string robotName = line.Split(":")[1].Split(" ")[1];
                            string[] vrstica = line.Split(":")[1].Split(" ");
                            string robotInfo = vrstica[vrstica.Length - 1];
                            JSON += "\n\"robotTag\":\"" + robotTag + "\"," + "\n\"robotName\":\"" + robotName + "\"," + "\n\"robotInfo\":\"" + robotInfo + "\"\n},";
                        }
                        JSON = JSON.Substring(0, JSON.Length - 1) + "\n],";
                    }
                    //CONTROL POWER
                    else if (tag.Equals("//CONTROL POWER"))
                    {
                        key = "\"controlPowerTotal\"";
                        while ((line = sr.ReadLine()) != null && line.ToUpper().StartsWith("TOTAL"))
                        {
                            value = line.Split(":", 2)[1].Trim();
                        }
                        JSON += key + ":\"" + value + "\",";
                    }
                    //SERVO POWER
                    else if (tag.Equals("//SERVO POWER"))
                    {
                        key = "\"servoPowerTotal\"";
                        while ((line = sr.ReadLine()) != null && line.ToUpper().StartsWith("TOTAL"))
                        {
                            value = line.Split(":", 2)[1].Trim();
                        }
                        JSON += key + ":\"" + value + "\",";
                        string list = "\n\"servoPowerRobotList\":[{\n";
                        if (line.ToUpper().StartsWith("R"))
                        {
                            JSON += list;
                        }
                        while (line.ToUpper().StartsWith("R"))
                        {
                            key = line.Split(":")[0].Trim();
                            value = line.Split(":", 2)[1].Trim();
                            JSON += "\"robotTag\":\"" + key + "\",\n\"servoPower\":\"" + value + "\"\n},";
                            line = sr.ReadLine();
                        }
                        JSON = JSON.Substring(0, JSON.Length - 1) + "\n],";
                    }
                    //PLAYBACK TIME
                    else if (tag.Equals("//PLAYBACK TIME"))
                    {
                        key = "\"playBackTimeTotal\"";
                        while ((line = sr.ReadLine()) != null && line.ToUpper().StartsWith("TOTAL"))
                        {
                            value = line.Split(":", 2)[1].Trim();
                        }
                        JSON += key + ":\"" + value + "\",";
                        string list = "\n\"playBackTimeRobotList\":[{\n";
                        if (line.ToUpper().StartsWith("R"))
                        {
                            JSON += list;
                        }
                        while (line.ToUpper().StartsWith("R"))
                        {
                            key = line.Split(":")[0].Trim();
                            value = line.Split(":", 2)[1].Trim();
                            JSON += "\"robotTag\":\"" + key + "\",\n\"playBackTime\":\"" + value + "\"\n},";
                            line = sr.ReadLine();
                        }
                        JSON = JSON.Substring(0, JSON.Length - 1) + "\n],";
                    }
                    //MOVING TIME
                    else if (tag.Equals("//MOVING TIME"))
                    {
                        key = "\"movingTimeTotal\"";
                        while ((line = sr.ReadLine()) != null && line.ToUpper().StartsWith("TOTAL"))
                        {
                            value = line.Split(":", 2)[1].Trim();
                        }
                        JSON += key + ":\"" + value + "\",";
                        string list = "\n\"movingTimeRobotList\":[{\n";
                        if (line.ToUpper().StartsWith("R"))
                        {
                            JSON += list;
                        }
                        while (line.ToUpper().StartsWith("R"))
                        {
                            key = line.Split(":")[0].Trim();
                            value = line.Split(":", 2)[1].Trim();
                            JSON += "\"robotTag\":\"" + key + "\",\n\"movingTime\":\"" + value + "\"\n},";
                            line = sr.ReadLine();
                        }
                        JSON = JSON.Substring(0, JSON.Length - 1) + "\n],";
                    }
                    //ENERGY TIME
                    else if (tag.Equals("//ENERGY TIME"))
                    {
                        key = "\"energyTimeTotal\"";
                        while ((line = sr.ReadLine()) != null && line.ToUpper().StartsWith("TOTAL"))
                        {
                            value = line.Split(":", 2)[1].Trim();
                        }
                        JSON += key + ":\"" + value + "\",";
                    }
                    //MOTOPLUS APP
                    else if (tag.Equals("//MOTOPLUS APP"))
                    {
                        string list = "\n\"motoPlusAppList\":[{\n";
                        JSON += list;

                        while ((line = sr.ReadLine()) != null && line.ToUpper().StartsWith("0"))
                        {
                            int lastNumber = line.LastIndexOfAny("0123456789".ToCharArray());
                            key = line.Substring(0, lastNumber + 1).Split(":", 2)[1];
                            //key = line.Split(":", 2)[1].Trim();
                            value = line.Substring(lastNumber + 1).Trim();
                            JSON += "\"motoPlusAppTag\":\"" + key + "\",\n\"motoPlusAppInfo\":\"" + value + "\"\n},";
                        }
                        JSON = JSON.Substring(0, JSON.Length - 1) + "\n],";

                    }
                    //LANGUAGE
                    else if (tag.Equals("LANGUAGE"))
                    {
                        string list = "\n\"languageList\":[{\n";
                        JSON += list;
                        line = sr.ReadLine();
                        line = sr.ReadLine();
                        while ((line = sr.ReadLine()) != null && line.ToUpper().StartsWith("LANGU"))
                        {
                            key = line.Split(" ", 2)[0].Trim();
                            value = line.Split(" ", 2)[1].Trim();
                            JSON += "\"" + key + "\":\"" + value + "\"\n},\n{\n";
                        }
                        JSON = JSON.Substring(0, JSON.Length - 4) + "\n],";
                    }
                    //CONTROL GROUP
                    else if (tag.Equals("CONTROL GROUP"))
                    {
                        string list = "\n\"controlGroupList\":[{\n";
                        JSON += list;
                        while ((line = sr.ReadLine()) != null && !line.ToUpper().StartsWith("CONNECT"))
                        { }
                        while ((line = sr.ReadLine()) != null && !line.StartsWith("===="))
                        {
                            string controlTag = line.Split(":", 2)[0].Trim();
                            string controlName = line.Split(":", 2)[1].Trim().Split(" ")[0];
                            int splitLength = line.Split(":", 2)[1].Trim().Split(" ", 2).Length;
                            var controlInfo = splitLength > 1 ? line.Split(":", 2)[1].Trim().Split(" ", 2)[1].Trim() : "";
                            JSON += "\"controlTag\":\"" + controlTag + "\",\n\"controlName\":\"" + controlName + "\",\n\"controlInfo\":\"" + controlInfo + "\"\n},\n{\n";
                        }
                        JSON = JSON.Substring(0, JSON.Length - 4) + "\n],";
                    }
                    //APPLICATION
                    else if (tag.Equals("APPLICATION"))
                    {
                        while((line = sr.ReadLine())==null || !line.ToUpper().StartsWith("APP"))
                        {}
                        string application = sr.ReadLine().Trim();
                        JSON += "\"application\":\"" + application + "\",\n\"robotApplicationList\":[{\n";
                        do
                        {
                            line = sr.ReadLine();
                            if (line.ToUpper().StartsWith("ROBOT"))
                            {
                                int last = line.Trim().LastIndexOf(" ");
                                string robotTag = line.Substring(last+1).Trim();
                                line = sr.ReadLine();
                                line = line.Split(":")[1].Trim();
                                last = line.LastIndexOf(" ");
                                string applicationName = line.Substring(0, last).Trim();
                                string applicationInfo=line.Substring(last).Trim();
                                JSON += "\"robotTag\":\"" + robotTag + "\",\n\"applicationName\":\""+ applicationName+"\",\n\"applicationInfo\":\""+applicationInfo+"\"\n},";
                            }
                        } while (line.ToUpper().StartsWith("CIO"));
                        JSON = JSON.Substring(0, JSON.Length - 1) + "\n],";

                        line = sr.ReadLine();
                        line = sr.ReadLine();
                        string cioLadder = line.Trim();
                        JSON += "\n\"cioLadder\":\"" + cioLadder+"\",";            
                        
                    }
                    //OPTION FUNCTION
                    else if (tag.Equals("OPTION FUNCTION"))
                    {
                        string list = "\n\"optionFunctionList\":[{\n";
                        JSON += list;
                        while(!(line=sr.ReadLine()).StartsWith("*"))
                        {}

                        string optionFunction = line.Split(" ",2)[1].Trim();
                        string optionFunctionName = sr.ReadLine().Split(" ",2)[1].Trim();
                        JSON += "\"optionFunction\":\"" + optionFunction +"\"\n},\n{\n\"optionFunctionName\":\"" + optionFunctionName + "\"\n},";
                        while ((line = sr.ReadLine()).StartsWith("*"))
                        {
                            optionFunctionName = line.Split(" ", 2)[1].Trim();
                            JSON += "\n{\n\"optionFunctionName\":\"" + optionFunctionName + "\"\n},";
                        }
                        JSON = JSON.Substring(0, JSON.Length - 1) + "\n],";
                    }
                    //CHANGE TRACKING LIST
                    else if (tag.Equals("CHANGE TRACKING LIST"))
                    {
                        string list = "\n\"changeTrackingList\":[{\n";
                        JSON += list;
                        while ((line = sr.ReadLine()) != null && !line.ToUpper().StartsWith("==="))
                        {
                            string changeTrackingFile = line.Trim();
                            JSON += "\"changeTrackingFile\":\"" + changeTrackingFile + "\"\n},\n{\n";
                        }
                        JSON = JSON.Substring(0, JSON.Length - 4) + "\n],";
                    }
                    //INITIALIZED FILES
                    else if (tag.Equals("INITIALIZED FILES"))
                    {
                        string list = "\n\"initializedFileList\":[{\n";
                        JSON += list;
                        while ((line = sr.ReadLine()) != null && !line.ToUpper().StartsWith("==="))
                        {
                            string initializedFile = line.Trim();
                            JSON += "\"initializedFile\":\"" + initializedFile + "\"\n},\n{\n";
                        }
                        JSON = JSON.Substring(0, JSON.Length - 4) + "\n],";
                    }
                    //CHANGE TRACKING PARAMETER
                    else if (tag.Equals("CHANGE TRACKING PARAMETER"))
                    {
                        string list = "\n\"changeTrackingParameterList\":[{\n";
                        JSON += list;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string changeTrackingParameter = line.Trim();
                            JSON += "\"changeTrackingParameter\":\"" + changeTrackingParameter + "\"\n},\n{\n";
                        }
                        JSON = JSON.Substring(0, JSON.Length - 4) + "\n]\n}";
                    } 
                }
            }
            return JSON;
        }

        public static bool In<T>(this T obj, params T[] args)
        {
            return args.Contains(obj);
        }

        public static void returnValue(string path, string tag)
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
                    else if (tag.Equals("//OPERATING TIME"))
                    {
                        Console.WriteLine(tag);
                        while ((line = sr.ReadLine()) != null && (line.StartsWith("TOOL")))
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

        public static void returnBetween(string path, string first, string second)
        {
            String line;
            sr = new StreamReader(path);
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.StartsWith(first))
                {
                    if (first.Equals("//SYSTEM NO"))
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
            else if (znackaId > znacke.Count || znackaId < 0)
            {
                Console.WriteLine("Značka ni najdena, program se bo zaključil!");
                Environment.ExitCode = 1;
                return false;
            }
            return true;
        }
    }
}
