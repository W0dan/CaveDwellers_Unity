using System;
using UnityEngine;

namespace Assets.Code.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public SpriteRenderer HealthRenderer;
        public Sprite Health75;
        public Sprite Health50;
        public Sprite HealthLow;

        public event Action<Entity> Died = entity => {};

        public float Speed;
        public int Health;

        protected float DistanceToMove()
        {
            return (float)Math.Round(Speed * Time.deltaTime, 2);
        }

        public void TakeHealth(int damage)
        {
            RenderHealth();

            Health -= damage;

            if (Health <= 0)
            {
                Health = 0;
                Died(this);
            }
        }

        protected virtual float CalculateHealthbarWidth(float health)
        {
            //todo: have a reasonable width health bar for bigger monsters
            return health/5;
        }

        private void RenderHealth()
        {
            HealthRenderer.transform.localScale = new Vector3(CalculateHealthbarWidth(Health), 2);
            if (Health <= 75 && Health > 50)
            {
                HealthRenderer.sprite = Health75;
            }
            if (Health <= 50 && Health > 25)
            {
                HealthRenderer.sprite = Health50;
            }
            if (Health <= 25)
            {
                HealthRenderer.sprite = HealthLow;
            }
        }

    }
}
