using System.Collections.Generic;

namespace StatParser
{
    public class GameEntity
    {
        public GameEntity()
        {
            Data = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Using { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}
