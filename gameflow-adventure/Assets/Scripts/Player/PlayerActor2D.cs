using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor2D : RigidbodyActor2D
{
    public float maxSpeed = 2;

    protected override void InternalUpdate()
    {


    }

    public void Move(Vector2 axis)
    {
        if (axis.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(axis.x), 1, 1);
        velocity = axis * maxSpeed;
    }
}
