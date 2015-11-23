using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float Speed;

    public int Health;

	void Start () {
	
	}
	
	void Update () {
	
	}

    protected float DistanceToMove()
    {
        return (float)Math.Round(Speed * Time.deltaTime, 2);
    }

    public void TakeHealth(int damage)
    {
        Health -= damage;
    }
}
