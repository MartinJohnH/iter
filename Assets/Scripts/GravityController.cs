using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GravityController : MonoBehaviour
{
    public enum Direction
    {
        Forward, Back, Left, Right, Up, Down
    }

    public Direction direction = Direction.Down;
    public float acceleration = 9.8f;
    
    private HashSet<GravityListener> gravityListeners = new HashSet<GravityListener>();
    private Direction _lastDirection = Direction.Down;

    private void Start()
    {
        _lastDirection = direction;
    }

    private void FixedUpdate()
    {
        if (!direction.Equals(_lastDirection))
        {
            Vector3 g = Vector3.down * acceleration;
            switch (direction)
            {
                case Direction.Forward:
                    g = Vector3.forward * acceleration;
                    break;
                case Direction.Back:
                    g = Vector3.back * acceleration;
                    break;
                case Direction.Left:
                    g = Vector3.left * acceleration;
                    break;
                case Direction.Right:
                    g = Vector3.right * acceleration;
                    break;
                case Direction.Up:
                    g = Vector3.up * acceleration;
                    break;
                case Direction.Down:
                    g = Vector3.down * acceleration;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _lastDirection = direction;
            Physics.gravity = g;
            foreach (GravityListener listener in gravityListeners)
            {
                listener.OnGravityChange(g);
            }
        }
    }

    public void register(GravityListener gravityListener)
    {
        gravityListeners.Add(gravityListener);
    }

    public void unregister(GravityListener gravityListener)
    {
        gravityListeners.Remove(gravityListener);
    }
}
