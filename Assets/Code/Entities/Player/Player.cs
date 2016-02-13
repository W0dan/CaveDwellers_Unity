using System;
using Assets.Code.Entities.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Entities.Player
{
    public class Player : Entity
    {
        public event Action<GameObject> HitSomething = obj => { };

        public Gun Gun;
        public Sprite Sprite;
        public Sprite DeadSprite;
        public SpriteRenderer SpriteRenderer;
        public Camera Camera;

        public int Ammo;
        public Text AmmoText;

        private int _score;
        public Text ScoreText;

        public Text HealthText;

        private bool _died;

        private float _rotationAngle;

        void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            var pickedSomethingUp = false;

            if (triggerCollider.gameObject.tag == "HealthDrop")
            {
                Health += 10;
                if (Health > 100) Health = 100;

                pickedSomethingUp = true;
            }

            if (triggerCollider.gameObject.tag == "AmmoDrop")
            {
                UpdateAmmo(10);

                pickedSomethingUp = true;
            }

            if (pickedSomethingUp)
            {
                var sound = GetComponent<AudioSource>();
                sound.Play();

                Destroy(triggerCollider.gameObject);
            }
        }

        private void UpdateAmmo(int ammo)
        {
            Ammo = Ammo + ammo;
            AmmoText.text = Ammo.ToString("0");
        }

        void Start()
        {
            Gun.HitSomething += Gun_HitSomething;

            StartingHealth = Health;

            UpdateAmmo(0);
        }

        private void Gun_HitSomething(GameObject obj)
        {
            HitSomething(obj);

            if (obj.tag == "Badguy")
            {
                var badguy = obj.GetComponent<Entity>();

                if (badguy.Health <= 0)
                {
                    _score += 100;
                    ScoreText.text = _score.ToString("00000000");
                }
            }
        }

        void Update()
        {
            if (MenuManager.IsPaused)
            {
                return;
            }

            if (_died)
            {
                return;
            }

            var playerTransform = GetComponent<Rigidbody2D>().transform;

            playerTransform.eulerAngles = GetAngleMouse(playerTransform);

            MoveKeyboardWithStrafing(playerTransform);

            if (Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (Ammo > 0)
                {
                    if (Gun.Shoot(playerTransform.position, _rotationAngle))
                    {
                        UpdateAmmo(-1);
                    }
                }
            }

            CameraFollowsPlayer(playerTransform);

            if (Health <= 0)
            {
                Die(playerTransform);
            }

            RenderHealth();
        }

        protected override void RenderHealth(float healthPercent)
        {
            base.RenderHealth(healthPercent);
            HealthText.text = healthPercent.ToString("0") + " %";
        }

        private void CameraFollowsPlayer(Transform playerTransform)
        {
            Camera.transform.position = new Vector3(
                playerTransform.transform.position.x,
                playerTransform.transform.position.y,
                Camera.transform.position.z);
        }

        private Vector3 GetAngleMouse(Transform playerTransform)
        {
            //calculate rotationAngle based on current location and mouse position
            var mousePosition = Input.mousePosition;
            var playerPosition = Camera.WorldToScreenPoint(playerTransform.position);

            var deltaY = playerPosition.y - mousePosition.y;
            var deltaX = playerPosition.x - mousePosition.x;

            _rotationAngle = (float)(Mathf.Rad2Deg * Math.Atan2(deltaY, deltaX)) + 90;

            return new Vector3(0, 0, _rotationAngle);
        }

        private Vector3 GetAngleKeyboard(Transform playerTransform)
        {
            //Rotation met knoppekes
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _rotationAngle += 400 * Time.deltaTime;
                while (_rotationAngle > 360)
                {
                    _rotationAngle -= 360;
                }
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _rotationAngle -= 400 * Time.deltaTime;
                while (_rotationAngle < 0)
                {
                    _rotationAngle += 360;
                }
            }
            return new Vector3(0, 0, _rotationAngle);
        }

        private void MoveKeyboardWithStrafing(Transform playerTransform)
        {
            //forward
            if (Input.GetKey(KeyCode.Z))
            {
                playerTransform.position += MathHelpers.GetForwardVector(_rotationAngle, DistanceToMove());
            }
            //backward
            if (Input.GetKey(KeyCode.S))
            {
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

            SpriteRenderer.sprite = Sprite;
        }

        public void Die(Transform playerTransform)
        {
            _died = true;
            playerTransform.eulerAngles = new Vector3(0, 0, 0);
            SpriteRenderer.sprite = DeadSprite;
        }
    }
}
