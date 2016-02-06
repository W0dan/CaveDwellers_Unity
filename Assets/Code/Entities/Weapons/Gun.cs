using System;
using System.Diagnostics;
using Assets.Code.Entities.Player;
using UnityEngine;

namespace Assets.Code.Entities.Weapons
{
    public class Gun : MonoBehaviour
    {
        public event Action<GameObject> HitSomething = obj => { };

        public GameObject Explosion;
        public GameObject Pang;
        private Stopwatch _lastShot = null;

        /// <summary>
        /// the gun shoots a bullet when it's ready
        /// </summary>
        /// <returns>true when a shot is fired, false when it is not</returns>
        public bool Shoot(Vector3 playerPosition, float angle)
        {
            var direction = MathHelpers.GetForwardVector(angle, 1);

            Vector2 gunfireOrigin = playerPosition + (direction);
            Vector2 pangLocation = playerPosition + (direction / 2);

            if (!Ready())
            {
                return false;
            }

            var sound = GetComponent<AudioSource>();
            sound.Play();

            _lastShot = Stopwatch.StartNew();

            var pang = Instantiate(Pang, pangLocation, Quaternion.identity);

            Destroy(pang, 0.05f);

            var ray = new Ray2D(gunfireOrigin, direction);

            const float shotDistance = 20;

            var hit = Physics2D.Raycast(ray.origin, ray.direction, shotDistance);
            if (hit.collider == null) return true; //nothing hit, so return

            //we hit something, draw explosion @ impact location
            var vector2 = ray.origin + ray.direction * hit.distance;
            var explosion = Instantiate(Explosion, vector2, Quaternion.identity);

            Destroy(explosion, 0.03f);

            if (hit.collider.attachedRigidbody != null)
            {
                HitSomething(hit.collider.attachedRigidbody.gameObject);
                return true;
            }

            if (hit.collider.gameObject != null)
            {
                HitSomething(hit.collider.gameObject);
            }
            return true;
        }

        private bool Ready()
        {
            if (_lastShot == null) return true;

            if (_lastShot.ElapsedMilliseconds > 200) return true;

            return false;
        }
    }
}
