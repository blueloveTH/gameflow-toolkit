using GameFlow;
using UnityEngine;

public class Safebox : MonoBehaviour
{
    public enum Flag
    {
        Stage1, Stage2, Stage3, Stage4, Success, Failure
    }

    public FlowMachine fd;

    void Start()
    {
        fd = FlowMachine.CreateByEnum<Flag>();

        fd[Flag.Success].onEnter += (x) => fd.isLocked = true;

        var t12 = fd.Connect(Flag.Stage1, Flag.Stage2).AddData("code", "1L");
        var t23 = fd.Connect(Flag.Stage2, Flag.Stage3).AddData("code", "3R");
        var t35 = fd.Connect(Flag.Stage3, Flag.Success).AddData("code", "2L");
        var t24 = fd.Connect(Flag.Stage2, Flag.Stage4).AddData("code", "2R");
        var t45 = fd.Connect(Flag.Stage4, Flag.Success).AddData("code", "3L");

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
        var arc = fd.currentNode?.FindArc((a) => a.GetData<string>("code") == str);
        if (arc != null) fd.Enter(arc.target);
        else fd.Enter(Flag.Failure);
    }
}
