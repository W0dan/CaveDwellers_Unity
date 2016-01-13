using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.Entities.BadGuys
{
    public class RandomMoveBehaviour : MoveBehaviour
    {
        private readonly Entity _entity;
        private Vector3 _currentDirection;
        private float _distanceTraveledInThisDirection;

        public RandomMoveBehaviour(Entity entity)
        {
            _currentDirection = Vector3.up;
            _distanceTraveledInThisDirection = 0;

            _entity = entity;
        }

        public override void Move()
        {
            var attackerTransform = _entity.GetComponent<Rigidbody2D>().transform;

            var distanceToMove = DistanceToMove(_entity.Speed);

            _distanceTraveledInThisDirection += distanceToMove;
            attackerTransform.position += _currentDirection * distanceToMove;

            if (_distanceTraveledInThisDirection > 1)
            {
                _distanceTraveledInThisDirection = 0;
                _currentDirection = GetNewDirection();
            }
        }

        private Vector3 GetNewDirection()
        {
            //return Vector3.up;

            var rnd = Random.Range(1, 5);

            switch (rnd)
            {
                case 1: return Vector3.up;
                case 2: return Vector3.left;
                case 3: return Vector3.down;
                case 4: return Vector3.right;
                default:
                    throw new IndexOutOfRangeException("rnd is " + rnd);
            }
        }
    }
}