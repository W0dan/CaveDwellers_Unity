using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.Entities.BadGuys.Attacking
{
    public class MeleeAttackBehaviour
    {
        private readonly Entity _attacker;
        private readonly int _minDamage;
        private readonly int _maxDamage;
        private readonly float _rechargeTime;
        private bool _canAttack;
        private Entity _target;
        private readonly int _range;

        public MeleeAttackBehaviour(Entity attacker, int minDamage, int maxDamage, float rechargeTime, int range)
        {
            _range = range;
            _attacker = attacker;
            _minDamage = minDamage;
            _maxDamage = maxDamage;
            _rechargeTime = rechargeTime;

            _canAttack = true;
        }

        public void TryAttack()
        {
            if (_target == null)
            {
                var targetGameObject = GameObject.FindGameObjectWithTag("Player");
                _target = targetGameObject.GetComponent<Entity>();
            }

            var distance = Vector2.Distance(_attacker.transform.position, _target.transform.position);
            if (distance <= _range && _canAttack)
            {
                Attack(_target);
                _attacker.StartCoroutine(WaitForAttack());
            }
        }
        
        private IEnumerator WaitForAttack()
        {
            _canAttack = false;
            yield return new WaitForSeconds(_rechargeTime);
            _canAttack = true;
        }

        private void Attack(Entity target)
        {
            var take = Random.Range(_minDamage, _maxDamage);

            target.TakeHealth(take);
        }
    }
}