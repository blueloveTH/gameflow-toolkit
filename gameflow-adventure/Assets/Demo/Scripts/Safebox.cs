using GameFlow;
using UnityEngine;

public class Safebox : MonoBehaviour
{
    public enum Flag
    {
        Stage1, Stage2, Stage3, Stage4, Success, Failure
    }

    public FlowDiagram fd;

    void Start()
    {
        fd = FlowDiagram.CreateByEnum<Flag>();

        fd[Flag.Success].onEnter += (x) => fd.isLocked = true;

        var t12 = fd[Flag.Stage1].AddArc(fd[Flag.Stage2]).AddData("code", "1L");
        var t23 = fd[Flag.Stage2].AddArc(fd[Flag.Stage3]).AddData("code", "3R");
        var t35 = fd[Flag.Stage3].AddArc(fd[Flag.Success]).AddData("code", "2L");
        var t24 = fd[Flag.Stage2].AddArc(fd[Flag.Stage4]).AddData("code", "2R");
        var t45 = fd[Flag.Stage4].AddArc(fd[Flag.Success]).AddData("code", "3L");

        fd.onCurrentNodeChange += (x) => print(x.name + " (OnNodeChange)");

        foreach (var item in fd)
        {
            item.onEnter += (x) => print(x.name + " (OnEnter)");
            item.onExit += (x) => print(x.name + " (OnExit)");

            item.enterTaskCreator +=
                () => Task.Delay(2f).OnComplete(() => print("enter task end."));
            item.exitTaskCreator +=
                () => Task.Delay(2f).OnComplete(() => print("exit task end."));
        }
        fd.Enter(Flag.Stage1);
    }

    public void Input(string str)
    {
        var t = fd.currentNode?.FindArc((arc) => arc.GetData<string>("code") == str);
        if (t != null) fd.Enter(t.target);
        else fd.Enter(Flag.Failure);
    }
}
