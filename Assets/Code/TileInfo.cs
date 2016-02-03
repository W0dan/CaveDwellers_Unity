using Assets.Code.Entities;
using UnityEngine;

namespace Assets.Code
{
    internal class TileInfo
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsKeystone { get; set; }
        public bool IsSpawnPoint { get; set; }

        public GameObject BackgroundTile { get; set; }
        public Transform Badguy { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format("({0}, {1}) : {2}", X, Y, Description);
        }
    }
}