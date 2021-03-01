using GameFlow;
using UnityEngine;

public class LightController1 : MonoBehaviour
{
    private SpriteRenderer spRenderer;
    public FlowMachine fd { get; private set; }

    private void Awake()
    {
        spRenderer = GetComponent<SpriteRenderer>();

        fd = FlowMachine.BinaryDiagram();

        fd["1"].onEnter += (x) => spRenderer.color = Color.white;
        fd["0"].onEnter += (x) => spRenderer.color = Color.grey;

        fd.Enter("0");
    }

    private void OnMouseDown()
    {
        fd.EnterNext();
    }
}
