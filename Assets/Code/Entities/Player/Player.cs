using System;
using Assets.Code.Common;
using Assets.Code.Entities.Weapons;
using UnityEngine;

namespace Assets.Code.Entities.Player
{
    public class Player : Entity
    {
        public Gun Gun;

        public Sprite ForwardSprite;
        public Sprite BackwardSprite;
        public Sprite LeftSprite;
        public Sprite RightSprite;

        public Sprite DeadSprite;

        public SpriteRenderer SpriteRenderer;

        public Camera Camera;

        //private Direction _direction;
        private bool _died;

        private float _rotationAngle;
        //private Vector3 _directionVector;

        void Start()
        {
            StartingHealth = Health;
        }

        void Update()
        {
            if (_died)
            {
                return;
            }

            var playerTransform = GetComponent<Rigidbody2D>().transform;

            //var directionVector = MoveKeyboard(playerTransform);

            MoveKeyboardWithStrafing(playerTransform);

            if (Input.GetKey(KeyCode.Space))
            {
                Gun.Shoot(playerTransform.position, _rotationAngle);
            }

            CameraFollowsPlayer(playerTransform);

            if (Health <= 0)
            {
                Die();
            }

            RenderHealth();
        }

        private void CameraFollowsPlayer(Transform playerTransform)
        {
            Camera.transform.position = new Vector3(
                playerTransform.transform.position.x,
                playerTransform.transform.position.y,
                Camera.transform.position.z);
        }

        private Vector3 MoveKeyboardAndMouse(Transform playerTransform)
        {
            ////calculate directionVector based on current location and mouse position
            //var mousePosition = Input.mousePosition;

            //Console.WriteLine(mousePosition);

            //var direction = mousePosition - playerTransform.position;
            //var rotation = Quaternion.LookRotation(direction);
            ////playerTransform.rotation = Quaternion.LookRotation(direction);
            //// and afterward, if you want to constrain the rotation to a particular axis- in this case Y:
            ////playerTransform.eulerAngles = new Vector3(0f, 0f, playerTransform.eulerAngles.z);

            //Vector3 axis;
            //float angle;

            //rotation.ToAngleAxis(out angle, out axis);

            //playerTransform.Rotate(playerTransform.position, angle);
            //playerTransform.eulerAngles = new Vector3(0f, 0f, playerTransform.eulerAngles.z);
            ////playerTransform.

            ////playerTransform.localRotation = 

            ////playerTransform.position+=

            return Vector3.up;
        }

        private void MoveKeyboardWithStrafing(Transform playerTransform)
        {
            //Rotation met knoppekes
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _rotationAngle += 400 * Time.deltaTime;
                while (_rotationAngle > 360)
                {
                    _rotationAngle -= 360;
                }
                playerTransform.eulerAngles = new Vector3(0, 0, _rotationAngle);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _rotationAngle -= 400 * Time.deltaTime;
                while (_rotationAngle < 0)
                {
                    _rotationAngle += 360;
                }
                playerTransform.eulerAngles = new Vector3(0, 0, _rotationAngle);
            }

            //forward
            if (Input.GetKey(KeyCode.Z))
            {
                playerTransform.position += MathHelpers.GetForwardVector(_rotationAngle, DistanceToMove());
            }
            //backward
            if (Input.GetKey(KeyCode.S))
            {
                //var distanceToMoveUp = -(float)Math.Cos(_rotationAngle * Mathf.Deg2Rad) * DistanceToMove();
                //var distanceToMoveLeft = (float)Math.Sin(_rotationAngle * Mathf.Deg2Rad) * DistanceToMove();

                //playerTransform.position += new Vector3(distanceToMoveLeft, distanceToMoveUp);

                playerTransform.position += MathHelpers.GetBackwardVector(_rotationAngle, DistanceToMove());
            }
            //left
            if (Input.GetKey(KeyCode.D))
            {
                var distanceToMoveUp = (float)Math.Sin(_rotationAngle * Mathf.Deg2Rad) * DistanceToMove();
                var distanceToMoveLeft = (float)Math.Cos(_rotationAngle * Mathf.Deg2Rad) * DistanceToMove();

                playerTransform.position += new Vector3(distanceToMoveLeft, distanceToMoveUp);
            }
            //right
            if (Input.GetKey(KeyCode.Q))
            {
                var distanceToMoveUp = -(float)Math.Sin(_rotationAngle * Mathf.Deg2Rad) * DistanceToMove();
                var distanceToMoveLeft = -(float)Math.Cos(_rotationAngle * Mathf.Deg2Rad) * DistanceToMove();

                playerTransform.position += new Vector3(distanceToMoveLeft, distanceToMoveUp);
            }

            SpriteRenderer.sprite = ForwardSprite;
        }

        //private Vector3 MoveKeyboard(Transform playerTransform)
        //{
        //    var directionVector = GetCurrentDirectionVector(_direction);

        //    if (Input.GetKey(KeyCode.UpArrow))
        //    {
        //        _direction = Direction.Up;
        //        directionVector = Vector3.up;
        //        playerTransform.position += directionVector * DistanceToMove();
        //    }
        //    if (Input.GetKey(KeyCode.DownArrow))
        //    {
        //        _direction = Direction.Down;
        //        directionVector = Vector3.down;
        //        playerTransform.position += directionVector * DistanceToMove();
        //    }
        //    if (Input.GetKey(KeyCode.LeftArrow))
        //    {
        //        _direction = Direction.Left;
        //        directionVector = Vector3.left;
        //        playerTransform.position += directionVector * DistanceToMove();
        //    }
        //    if (Input.GetKey(KeyCode.RightArrow))
        //    {
        //        _direction = Direction.Right;
        //        directionVector = Vector3.right;
        //        playerTransform.position += directionVector * DistanceToMove();
        //    }

        //    SpriteRenderer.sprite = GetSprite(_direction);
        //    return directionVector;
        //}

        private Sprite GetSprite(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return ForwardSprite;
                case Direction.Right:
                    return RightSprite;
                case Direction.Down:
                    return BackwardSprite;
                case Direction.Left:
                    return LeftSprite;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        private static Vector3 GetCurrentDirectionVector(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Vector3.up;
                case Direction.Right:
                    return Vector3.right;
                case Direction.Down:
                    return Vector3.down;
                case Direction.Left:
                    return Vector3.left;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        protected Vector3 GetDirection()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                return Vector3.up;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                return Vector3.down;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                return Vector3.left;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                return Vector3.right;
            }

            return new Vector3();
        }

        public void Die()
        {
            _died = true;
            SpriteRenderer.sprite = DeadSprite;
        }
    }
}
