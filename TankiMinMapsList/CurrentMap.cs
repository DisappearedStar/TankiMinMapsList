using System;
using System.Collections.Generic;

namespace TankiMinMapsList
{
    sealed class CurrentMap // singleton
    {
        private static readonly Lazy<CurrentMap> instanceHolder = new Lazy<CurrentMap>(() => new CurrentMap());
        public static CurrentMap Instance { get { return instanceHolder.Value; } } 

        public string Name { get; set; }
        public List<string> Proplibs { get; set; }
        public double Ratio { get; set; }
        public int Weight { get; set; }
        public int MaxWeight { get; set; }
        public int LibCount { get; set; }

        private CurrentMap()
        {
            Ratio = 0.0;
            Weight = 0;
            MaxWeight = 0;
            Name = "";
            Proplibs = new List<string>();
            LibCount = 0;
        }
        public void Set(string _name, List<string> _proplibs, double _ratio, int _weight, int _libcount)
        {
            Name = _name;
            Proplibs = _proplibs;
            Ratio = _ratio;
            Weight = _weight;
            LibCount = _libcount;
        }
    }
}
