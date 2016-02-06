﻿using Assets.Code.Common;
using UnityEngine;

namespace Assets.Code
{
    public class TileInfo
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsKeystone { get; set; }
        public bool IsSpawnPoint { get; set; }

        //needed for hidden rooms (trigger logic)
        public bool IsHidden { get; set; }
        public bool IsTrigger { get; set; }
        public int TriggerId { get; set; }
        public int TriggeredById { get; set; }

        public GameObject BackgroundTile { get; set; }
        public Transform Badguy { get; set; }

        public string Description { get; set; }
        public Point NextLevelLocation { get; set; }

        public override string ToString()
        {
            return string.Format("({0}, {1}) : {2}", X, Y, Description);
        }
    }
}