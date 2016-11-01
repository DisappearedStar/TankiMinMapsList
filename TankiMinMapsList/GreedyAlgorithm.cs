using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;

namespace TankiMinMapsList
{
    static class GreedyAlgorithm
    {
        private static string PathToMaps;
        private static string ProplibsXml;
        private static double ProplibsCounter;

        private static List<TankiMap> Maps;
        private static List<TankiLib> Libs;
        private static CurrentMap Current;

        static GreedyAlgorithm()
        {
            PathToMaps = @"maps\";
            ProplibsXml = @"\proplibs.xml";
            Maps = new List<TankiMap>();
            Libs = new List<TankiLib>();
            Current = CurrentMap.Instance;

            //initializing Maps and Libs
            List<string> temp;
            foreach (string map in GetMapsNames(PathToMaps))
            {
                temp = GetLibsFromXml(PathToMaps + map + ProplibsXml).ToList(); //load all libs of this map
                Maps.Add(new TankiMap(map, temp)); //add a new map
                foreach (string s in temp)
                {
                    if(Libs.Any(lib => lib.Name == s)) //if Libs contains a lib with specified name
                    {
                        Libs.Find(lib => lib.Name == s).Weight++; //increment its weight
                    }
                    else
                    {
                        Libs.Add(new TankiLib(s, 1)); //add a new lib with specified name and weight=1
                    }
                }
            }
            Current.Weight = Current.MaxWeight = Maps.Count;
            ProplibsCounter = Libs.Count;
        }

        public static void Do()
        {
            while (ProplibsCounter > 0)
            {
                foreach (TankiMap map in Maps)
                {
                    double counter = 0.0;
                    int w = Current.MaxWeight;
                    foreach (string s in map.Proplibs)
                    {
                        if (Libs.Any(lib => lib.Name == s))
                        {
                            counter++;
                            if(Libs.Find(lib => lib.Name == s).Weight <= w)
                            {
                                w = Libs.Find(lib => lib.Name == s).Weight;
                            }
                        }
                    }
                    //Console.WriteLine($"{map.Name,15}{"  Weight=",9}{w,2}{"  Ratio=",8}{Math.Round(counter / ProplibsCounter, 4, MidpointRounding.AwayFromZero),6}{"  Libs=",7}{map.Proplibs.Count,2}");

                    if (w < Current.Weight)
                    {
                        Current.Set(map.Name, map.Proplibs, counter / ProplibsCounter, w, map.Proplibs.Count);
                    }
                    else if (w == Current.Weight)
                    {
                        if ((counter / ProplibsCounter) > Current.Ratio) 
                        {
                            Current.Set(map.Name, map.Proplibs, counter / ProplibsCounter, w, map.Proplibs.Count);
                        }
                        
                        else if ((counter / ProplibsCounter) == Current.Ratio) 
                        {
                            if (map.Proplibs.Count < Current.LibCount)
                            {
                                Current.Set(map.Name, map.Proplibs, counter / ProplibsCounter, w, map.Proplibs.Count);
                            }
                        }
                    }
                }
                Console.WriteLine("=====================================================");
                Console.WriteLine(Current.Name + " Weight=" + Current.Weight + " Ratio=" + Current.Ratio + " LibsCounter=" + Current.LibCount);
                Current.Ratio = 0.0;

                //removing current proplibs from libs' list
                foreach (string curlib in Current.Proplibs)
                {
                    if (Libs.Any(lib => lib.Name == curlib))
                    {
                        Libs.Remove(Libs.Find(lib => lib.Name == curlib));
                    }
                }

                //removing unnecessary maps from maps' list
                for (int i = Maps.Count - 1; i >= 0; i--)
                {
                    if(Maps[i].Proplibs.Intersect(Libs.Select(lib => lib.Name)).ToList().Count == 0) Maps.RemoveAt(i);
                }
                Current.Name = "";

                /* removing unnecessary libs from each map's libs list
                foreach (TankiMap map in Maps)
                {
                    List<string> t = new List<string>();
                    foreach (string lib in map.Proplibs)
                    {
                        if (CurrentProplibs.Any(l => l != lib))
                        {
                            t.Add(lib);
                        }
                    }
                    map.Proplibs = t;
                }
                */

                ProplibsCounter = Libs.Count;
                Current.Weight = Current.MaxWeight;
                Current.LibCount = Libs.Count;
                Console.WriteLine(ProplibsCounter + " libs are left");
                Console.WriteLine("=====================================================");
                //Console.ReadLine();
            }
            Console.WriteLine("Done");
        }

        private static IEnumerable<string> GetMapsNames(string path)
        {
            foreach(string dir in Directory.GetDirectories(path))//get the list of maps' paths (example: "maps/Arena")
            {
                yield return dir.Remove(0, path.Length); //remove "maps/" from every path and return them
            }
        }

        private static IEnumerable<string> GetLibsFromXml(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path); //the content of proplibs.xml is loaded
            foreach (XmlNode node in doc.GetElementsByTagName("library"))//get all "library" nodes
            {
                yield return node.Attributes["name"].Value; //return libs' names only
            }
        }
    }
}
