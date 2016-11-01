using System.Collections.Generic;

namespace TankiMinMapsList
{
    class TankiMap
    {
        public TankiMap(string name, List<string> list)
        {
            Name = name;
            Proplibs = list;
        }
        public TankiMap() { }

        public string Name { get; set; }
        public List<string> Proplibs { get; set; }
    }
}
