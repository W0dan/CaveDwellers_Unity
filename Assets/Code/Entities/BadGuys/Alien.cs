namespace Assets.Code.Entities.BadGuys
{
    public class Alien : Entity
    {
        private MeleeAttackBehaviour _attackBehaviour;
        private MoveBehaviour _moveBehaviour;

        void Start()
        {
            _attackBehaviour = new MeleeAttackBehaviour(this, 5, 10, 0.2f);
            _moveBehaviour = new RandomMoveBehaviour(this);
        }

        void Update()
        {
            _moveBehaviour.Move();
            _attackBehaviour.TryAttack();
        }
    }
}