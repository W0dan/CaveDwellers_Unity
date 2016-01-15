namespace Assets.Code.Entities.BadGuys
{
    public class BigGreenMonster : Entity
    {
        private MeleeAttackBehaviour _attackBehaviour;
        private MoveBehaviour _moveBehaviour;

        void Start()
        {
            _attackBehaviour = new MeleeAttackBehaviour(this, 15, 40, 0.2f, 2);
            _moveBehaviour = new RandomMoveBehaviour(this);
        }

        void Update()
        {
            _moveBehaviour.Move();
            _attackBehaviour.TryAttack();
        }
    }
}