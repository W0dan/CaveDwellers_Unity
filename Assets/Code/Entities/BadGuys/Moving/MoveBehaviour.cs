using System;
using UnityEngine;

namespace Assets.Code.Entities.BadGuys.Moving
{
    public abstract class MoveBehaviour
    {
        public abstract void Move();

        protected float DistanceToMove(float speed)
        {
            return (float)Math.Round(speed * Time.deltaTime, 2);
        }
    }
}