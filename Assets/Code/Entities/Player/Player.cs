using System;
using Assets.Code.Common;
using UnityEngine;

public class Player : Entity
{
    public Gun Gun;

    public Sprite ForwardSprite;
    public Sprite BackwardSprite;
    public Sprite LeftSprite;
    public Sprite RightSprite;

    public SpriteRenderer SpriteRenderer;

    private Direction _direction;

    void Start()
    {
    }

    void Update()
    {
        var playerTransform = GetComponent<Rigidbody2D>().transform;
        var directionVector = GetCurrentDirectionVector(_direction);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _direction = Direction.Up;
            directionVector = Vector3.up;
            playerTransform.position += directionVector * DistanceToMove();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _direction = Direction.Down;
            directionVector = Vector3.down;
            playerTransform.position += directionVector * DistanceToMove();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _direction = Direction.Left;
            directionVector = Vector3.left;
            playerTransform.position += directionVector * DistanceToMove();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _direction = Direction.Right;
            directionVector = Vector3.right;
            playerTransform.position += directionVector * DistanceToMove();
        }

        SpriteRenderer.sprite = GetSprite(_direction);

        if (Input.GetKey(KeyCode.Space))
        {
            Gun.Shoot(playerTransform.position+directionVector, directionVector);
        }

        if (Health <= 0)
        {
            Die();
        }
    }

    private Sprite GetSprite(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return ForwardSprite;
            case Direction.Right:
                return RightSprite;
            case Direction.Down:
                return BackwardSprite;
            case Direction.Left:
                return LeftSprite;
            default:
                throw new ArgumentOutOfRangeException("direction");
        }
    }

    private Vector3 GetCurrentDirectionVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector3.up;
            case Direction.Right:
                return Vector3.right;
            case Direction.Down:
                return Vector3.down;
            case Direction.Left:
                return Vector3.left;
            default:
                throw new ArgumentOutOfRangeException("direction");
        }
    }

    protected Vector3 GetDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            return Vector3.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            return Vector3.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            return Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            return Vector3.right;
        }

        return new Vector3();
    }

    public void Die()
    {
        print("I' ve been killed");
    }
}
