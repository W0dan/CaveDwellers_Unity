using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public event Action<Entity> Died = entity => {};

    public float Speed;
    public int Health;

    protected float DistanceToMove()
    {
        return (float)Math.Round(Speed * Time.deltaTime, 2);
    }

    public void TakeHealth(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Died(this);
        }
    }
}
