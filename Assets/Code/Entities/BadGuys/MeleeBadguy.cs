using Assets.Code.Entities.BadGuys.Attacking;
using Assets.Code.Entities.BadGuys.Moving;
using UnityEngine;

namespace Assets.Code.Entities.BadGuys
{
    public class MeleeBadguy : Entity
    {
        private MeleeAttackBehaviour _attackBehaviour;
        private MoveBehaviour _moveBehaviour;

        public int MinDamage;
        public int MaxDamage;
        public float RechargeTime;
        public int Range;

        void Start()
        {
            StartingHealth = Health;

            _attackBehaviour = new MeleeAttackBehaviour(this, MinDamage, MaxDamage, RechargeTime, Range);
            _moveBehaviour = new RandomMoveBehaviour(this);
        }

        void Update()
        {
            _moveBehaviour.Move();
            _attackBehaviour.TryAttack();

            RenderHealth();
        }
    }
}
