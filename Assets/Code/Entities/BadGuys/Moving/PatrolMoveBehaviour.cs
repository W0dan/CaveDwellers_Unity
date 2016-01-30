using System;
using UnityEngine;

namespace Assets.Code.Entities.BadGuys.Moving
{
    public class PatrolMoveBehaviour : MoveBehaviour
    {
        private readonly Entity _entity;
        private Vector3 _currentDirection;
        private float _distanceTraveledInThisDirection;

        public PatrolMoveBehaviour(Entity entity)
        {
            _currentDirection = Vector3.up;
            _distanceTraveledInThisDirection = 0;

            _entity = entity;
        }

        public override void Move()
        {
            var attackerTransform = _entity.GetComponent<Rigidbody2D>().transform;

            if (_currentDirection == Vector3.up)
            {
                var distanceToMove = DistanceToMove();

                _distanceTraveledInThisDirection += distanceToMove;
                attackerTransform.position += _currentDirection * distanceToMove;

                if (_distanceTraveledInThisDirection > 2)
                {
                    _distanceTraveledInThisDirection = 0;
                    _currentDirection = Vector3.left;
                }
            }

            if (_currentDirection == Vector3.left)
            {
                var distanceToMove = DistanceToMove();

                _distanceTraveledInThisDirection += distanceToMove;
                attackerTransform.position += _currentDirection * distanceToMove;

                if (_distanceTraveledInThisDirection > 2)
                {
                    _distanceTraveledInThisDirection = 0;
                    _currentDirection = Vector3.down;
                }
            }

            if (_currentDirection == Vector3.down)
            {
                var distanceToMove = DistanceToMove();

                _distanceTraveledInThisDirection += distanceToMove;
                attackerTransform.position += _currentDirection * distanceToMove;

                if (_distanceTraveledInThisDirection > 2)
                {
                    _distanceTraveledInThisDirection = 0;
                    _currentDirection = Vector3.right;
                }
            }

            if (_currentDirection == Vector3.right)
            {
                var distanceToMove = DistanceToMove();

                _distanceTraveledInThisDirection += distanceToMove;
                attackerTransform.position += _currentDirection * distanceToMove;

                if (_distanceTraveledInThisDirection > 2)
                {
                    _distanceTraveledInThisDirection = 0;
                    _currentDirection = Vector3.up;
                }
            }
        }


        protected float DistanceToMove()
        {
            return (float)Math.Round(_entity.Speed * Time.deltaTime, 2);
        }
    }
}