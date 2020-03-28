using UnityEngine;

public abstract class BaseActor2D : MonoBehaviour
{

    public abstract Vector2 position { get; set; }
    public abstract Vector2 velocity { get; set; }

    public event System.Action onUpdate;

    private void Update()
    {
        InternalUpdate();
        onUpdate?.Invoke();
    }

    protected abstract void InternalUpdate();

    protected abstract void OnEnable();
    protected abstract void OnDisable();
}
