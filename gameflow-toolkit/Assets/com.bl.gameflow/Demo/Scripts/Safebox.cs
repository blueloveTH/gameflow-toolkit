using GameFlow;
using UnityEngine;

public class Safebox : MonoBehaviour
{
    public FlowDiagram fd;

    void Start()
    {
        fd = FlowDiagram.Empty();

        FlowNode stage1 = fd.AddNode("stage1");
        FlowNode stage2 = fd.AddNode("stage2");
        FlowNode stage3 = fd.AddNode("stage3");
        FlowNode stage4 = fd.AddNode("stage4");
        FlowNode success = fd.AddNode("success");
        FlowNode failure = fd.AddNode("failure");

        success.onEnter += (x) => fd.isLocked = true;

        var t12 = stage1.AddArc(stage2).AddKeyValue("code", "1L");
        var t23 = stage2.AddArc(stage3).AddKeyValue("code", "3R");
        var t35 = stage3.AddArc(success).AddKeyValue("code", "2L");
        var t24 = stage2.AddArc(stage4).AddKeyValue("code", "2R");
        var t45 = stage4.AddArc(success).AddKeyValue("code", "3L");

        fd.onCurrentNodeChange += (x) => print(x.name + " (OnNodeChange)");

        foreach (var item in fd)
        {
            item.onEnter += (x) => print(x.name + " (OnEnter)");
            item.onExit += (x) => print(x.name + " (OnExit)");

            item.enterTaskCreator +=
                () => Task.DelayTask(2f).OnCompleteAdd(() => print("enter task end."));
            item.exitTaskCreator +=
                () => Task.DelayTask(2f).OnCompleteAdd(() => print("exit task end."));
        }
        fd.Enter("stage1");
    }

    public void Input(string str)
    {
        var t = fd.currentNode?.FindArc((arc) => arc.GetValue<string>("code") == str);
        if (t != null) fd.Enter(t.target);
        else fd.Enter("failure");
    }
}
