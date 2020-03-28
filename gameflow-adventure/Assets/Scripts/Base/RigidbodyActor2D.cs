using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class RigidbodyActor2D : BaseActor2D
{
    protected Rigidbody2D r2d { get; private set; }

    public float gravityScale {
        get { return r2d.gravityScale; }
        set { r2d.gravityScale = value; }
    }

    protected virtual void Awake()
    {
        r2d = GetComponent<Rigidbody2D>();
    }

    public override Vector2 position {
        get { return r2d.position; }
        set { r2d.position = value; }
    }

    public override Vector2 velocity {
        get { return r2d.velocity; }
        set { r2d.velocity = value; }
    }

    protected override void OnEnable()
    {
        r2d.simulated = true;
    }

    protected override void OnDisable()
    {
        r2d.simulated = false;
    }
}
