using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Parser
{
    static class Program
    {
        static StreamReader sr;
        static List<string> znacke = new List<string>() { "//SYSTEM NO", "//ROBOT NAME", "//CONTROL POWER", "//SERVO POWER", "//PLAYBACK TIME", "//MOVING TIME",
                                                            "//OPERATING TIME","//ENERGY TIME","//MOTOPLUS APP","LANGUAGE", "CONTROL GROUP","APPLICATION",
                                                            "OPTION FUNCTION","CHANGE TRACKING LIST","INITIALIZED FILES","CHANGE TRACKING PARAMETER"};
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1)
                {
                    //string filepath = @"C:\Users\user asus\source\repos\Parser\Parser\PANELBOX2.LOG";
                    string filepath = args[0];
                    int lastOccurance = filepath.LastIndexOf("\\");
                    string newFilePath = filepath.Substring(0, lastOccurance) + "\\jsonFile.json";

                    //Zbriši datoteko če obstaja
                    if (File.Exists(newFilePath))
                    {
                        File.Delete(newFilePath);
                    }
                    using (var x = File.Create(newFilePath))
                    {
                        x.Close();
                    }

                    sr = new StreamReader(filepath);

                    for (int i = 0; i < znacke.Count; i++)
                    {
                        Console.Write(returnJSON(filepath, newFilePath, znacke[i]));
                    }
                    Console.WriteLine("\nJSON uspešno generiran");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Napačno število argumentov");
                    Console.ReadKey();
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
        }
        public static string returnJSON(string path,string newFilePath, string tag)
        {
            String line;
            String key="";
            String value="";
            String JSON="";
            try
            {
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
                            key = "\n\"robotNameList\"";
                            JSON += key + ":\n[";
                            while ((line = sr.ReadLine()) != null && line.ToUpper().StartsWith("R"))
                            {
                                string robotTag = line.Split(":")[0].Trim();
                                string robotName = line.Split(":")[1].Split(" ")[1];
                                string[] vrstica = line.Split(":")[1].Split(" ");
                                string robotInfo = vrstica[vrstica.Length - 1];
                                JSON += "{\"robotTag\":\"" + robotTag + "\"," + "\n\"robotName\":\"" + robotName + "\"," + "\n\"robotInfo\":\"" + robotInfo + "\"\n},";
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
                            string list = "\n\"servoPowerRobotList\":[";
                            if (line.ToUpper().StartsWith("R"))
                            {
                                JSON += list;
                            }
                            while (line.ToUpper().StartsWith("R"))
                            {
                                key = line.Split(":")[0].Trim();
                                value = line.Split(":", 2)[1].Trim();
                                JSON += "{\n\"robotTag\":\"" + key + "\",\n\"servoPower\":\"" + value + "\"\n},";
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
                            string list = "\n\"playBackTimeRobotList\":[";
                            if (line.ToUpper().StartsWith("R"))
                            {
                                JSON += list;
                            }
                            while (line.ToUpper().StartsWith("R"))
                            {
                                key = line.Split(":")[0].Trim();
                                value = line.Split(":", 2)[1].Trim();
                                JSON += "{\n\"robotTag\":\"" + key + "\",\n\"playBackTime\":\"" + value + "\"\n},";
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
                            string list = "\n\"movingTimeRobotList\":[";
                            if (line.ToUpper().StartsWith("R"))
                            {
                                JSON += list;
                            }
                            while (line.ToUpper().StartsWith("R"))
                            {
                                key = line.Split(":")[0].Trim();
                                value = line.Split(":", 2)[1].Trim();
                                JSON += "{\n\"robotTag\":\"" + key + "\",\n\"movingTime\":\"" + value + "\"\n},";
                                line = sr.ReadLine();
                            }
                            JSON = JSON.Substring(0, JSON.Length - 1) + "\n],";
                        }
                        
                        //OPERATING TIME
                        else if (tag.Equals("//OPERATING TIME"))
                        {
                            key = "\"operatingTimeTotal\"";
                            while ((line = sr.ReadLine()) != null && line.ToUpper().StartsWith("TOOL"))
                            {
                                value = line.Split(":", 2)[1].Trim();
                            }
                            JSON += key + ":\"" + value + "\",";
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
                            while ((line = sr.ReadLine()) == null || !line.ToUpper().StartsWith("APP"))
                            { }
                            string application = sr.ReadLine().Trim();
                            JSON += "\"application\":\"" + application + "\",\n\"robotApplicationList\":[{\n";
                            do
                            {
                                line = sr.ReadLine();
                                if (line.ToUpper().StartsWith("ROBOT"))
                                {
                                    int last = line.Trim().LastIndexOf(" ");
                                    string robotTag = line.Substring(last + 1).Trim();
                                    line = sr.ReadLine();
                                    line = line.Split(":")[1].Trim();
                                    last = line.LastIndexOf(" ");
                                    string applicationName = line.Substring(0, last).Trim();
                                    string applicationInfo = line.Substring(last).Trim();
                                    JSON += "\"robotTag\":\"" + robotTag + "\",\n\"applicationName\":\"" + applicationName + "\",\n\"applicationInfo\":\"" + applicationInfo + "\"\n},";
                                }
                            } while (line.ToUpper().StartsWith("CIO"));
                            JSON = JSON.Substring(0, JSON.Length - 1) + "\n],";

                            line = sr.ReadLine();
                            line = sr.ReadLine();
                            string cioLadder = line.Trim();
                            JSON += "\n\"cioLadder\":\"" + cioLadder + "\",";

                        }
                        //OPTION FUNCTION
                        else if (tag.Equals("OPTION FUNCTION"))
                        {
                            string list = "\n\"optionFunctionList\":[";
                            JSON += list;
                            while (!(line = sr.ReadLine()).StartsWith("*"))
                            { }
                            do
                            {
                                string optionFunction = line.Split(" ", 2)[1].Trim();
                                JSON += "\n{\n\"optionFunction\":\"" + optionFunction + "\"\n},";
                            }
                            while ((line = sr.ReadLine()).StartsWith("*"));
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
            } catch(Exception e)
            {
                Console.WriteLine("Napaka pri branju datoteke");
            }
            
            using (StreamWriter s = new StreamWriter(newFilePath, true))
            {
                s.WriteLine(JSON);
            }
            
            return JSON;
        }
    }
}
