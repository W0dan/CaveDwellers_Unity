using System.Collections;
using UnityEngine;

namespace Assets.Code.Entities.BadGuys
{
    public class Patrol : Entity
    {
        private Entity _target;
        //public int Distance;

        private bool _canAttack;

        private Vector3 _currentDirection;
        private float _distanceTraveledInThisDirection;

        void Start()
        {
            _currentDirection = Vector3.up;
            _distanceTraveledInThisDirection = 0;
        }

        void Update()
        {
            _canAttack = true;
            var attackerTransform = GetComponent<Rigidbody2D>().transform;

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

            //if (_target == null)
            //{
            //    var targetGameObject = GameObject.FindGameObjectWithTag("Player");
            //    _target = targetGameObject.GetComponent<Entity>();
            //}

            //var targetTransform = _target.GetComponent<Rigidbody2D>().transform;
            //var attackerTransform = GetComponent<Rigidbody2D>().transform;

            //if (targetTransform.position.y > (attackerTransform.position.y + Distance))
            //{
            //    attackerTransform.position += Vector3.up * DistanceToMove();
            //}
            //if (targetTransform.position.y < (attackerTransform.position.y - Distance))
            //{
            //    attackerTransform.position += Vector3.down * DistanceToMove();
            //}
            //if (targetTransform.position.x > (attackerTransform.position.x + Distance))
            //{
            //    attackerTransform.position += Vector3.right * DistanceToMove();
            //}
            //if (targetTransform.position.x < (attackerTransform.position.x - Distance))
            //{
            //    attackerTransform.position += Vector3.left * DistanceToMove();
            //}

            //if (Vector2.Distance(attackerTransform.position, targetTransform.position) <= Distance && _canAttack)
            //{
            //    Attack();
            //    StartCoroutine(WaitForAttack());
            //}
        }

        public void Die()
        {

        }

        public void Attack()
        {
            var take = Random.Range(1, 20);

            _target.TakeHealth(take);
        }

        private IEnumerator WaitForAttack()
        {
            _canAttack = false;
            yield return new WaitForSeconds(2);
            _canAttack = true;
        }
    }
}