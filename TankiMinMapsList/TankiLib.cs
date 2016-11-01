namespace TankiMinMapsList
{
    class TankiLib
    {
        public TankiLib(string name, int weight)
        {
            Name = name;
            Weight = weight;
        }

        public string Name { get; set; }
        public int Weight { get; set; }
    }
}