using GameFlow;
using UnityEngine;

public class Safebox : MonoBehaviour
{
    public FlowDiagram fd;

    void Start()
    {
        fd = new FlowDiagram();

        FlowNode stage1 = fd.CreateNode("stage1");
        FlowNode stage2 = fd.CreateNode("stage2");
        FlowNode stage3 = fd.CreateNode("stage3");
        FlowNode stage4 = fd.CreateNode("stage4");
        FlowNode success = fd.CreateNode("success");
        FlowNode failure = fd.CreateNode("failure");

        success.onEnter += (x) => fd.isLocked = true;

        var t12 = stage1.AddArc(stage2).AddData("code", "1L");
        var t23 = stage2.AddArc(stage3).AddData("code", "3R");
        var t35 = stage3.AddArc(success).AddData("code", "2L");
        var t24 = stage2.AddArc(stage4).AddData("code", "2R");
        var t45 = stage4.AddArc(success).AddData("code", "3L");

        fd.onCurrentNodeChange += (x) => print(x.name + " (OnNodeChange)");

        foreach (var item in fd)
        {
            item.onEnter += (x) => print(x.name + " (OnEnter)");
            item.onExit += (x) => print(x.name + " (OnExit)");

            item.enterTaskCreator +=
                () => Task.DelayTask(2f).OnComplete(() => print("enter task end."));
            item.exitTaskCreator +=
                () => Task.DelayTask(2f).OnComplete(() => print("exit task end."));
        }
        fd.Enter("stage1");
    }

    public void Input(string str)
    {
        var t = fd.currentNode?.FindArc((arc) => arc.GetData<string>("code") == str);
        if (t != null) fd.Enter(t.target);
        else fd.Enter("failure");
    }
}
