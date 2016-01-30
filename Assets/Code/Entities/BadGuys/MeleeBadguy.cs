using Assets.Code.Entities.BadGuys.Attacking;
using Assets.Code.Entities.BadGuys.Moving;

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
            _attackBehaviour = new MeleeAttackBehaviour(this, MinDamage, MaxDamage, RechargeTime, Range);
            _moveBehaviour = new RandomMoveBehaviour(this);
        }

        void Update()
        {
            _moveBehaviour.Move();
            _attackBehaviour.TryAttack();
        }
    }
}
