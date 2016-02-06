using System;
using UnityEngine;

namespace Assets.Code.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public event Action<Entity> Died = entity => { };

        public float SizeFactor = 1;

        public SpriteRenderer HealthRenderer;
        public Sprite HealthFull;
        public Sprite Health75;
        public Sprite Health50;
        public Sprite HealthLow;

        public float Speed;
        public int Health;
        protected int StartingHealth;

        protected float DistanceToMove()
        {
            return (float)Math.Round(Speed * Time.deltaTime, 2);
        }

        public void TakeHealth(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Health = 0;
                Died(this);
            }
        }

        protected virtual float CalculateHealthbarWidth(float health)
        {
            return health / 5;
        }

        private float GetHealthPercent()
        {
            return (float)Health / StartingHealth * 100;
        }

        protected void RenderHealth()
        {
            var healthPercent = GetHealthPercent();

            RenderHealth(healthPercent);
        }

        protected virtual void RenderHealth(float healthPercent)
        {
            HealthRenderer.transform.localScale = new Vector3(CalculateHealthbarWidth(healthPercent), 2/SizeFactor);
            if (healthPercent > 75)
            {
                HealthRenderer.sprite = HealthFull;
            }
            if (healthPercent <= 75 && healthPercent > 50)
            {
                HealthRenderer.sprite = Health75;
            }
            if (healthPercent <= 50 && healthPercent > 25)
            {
                HealthRenderer.sprite = Health50;
            }
            if (healthPercent <= 25)
            {
                HealthRenderer.sprite = HealthLow;
            }
        }
    }
}
