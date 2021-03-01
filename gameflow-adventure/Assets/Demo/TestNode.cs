using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFlow;

public class TestNode : InteractionNode
{
    void Start()
    {
        var s = Signal("123/516").AddData("key", "value").Debug();
        s["hit"] = 50;
        s["ko"] = 30;
        Emit(s, gameObject);
    }

    [SlotMethod("123")]
    private void OnSignal(Signal s)
    {
        print("");
    }
}
