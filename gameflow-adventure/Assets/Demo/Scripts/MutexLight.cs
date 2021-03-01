using GameFlow;
using System.Collections.Generic;
using UnityEngine;

public class MutexLight : MonoBehaviour
{
    public List<FlowMachine> diagrams = new List<FlowMachine>();

    void Start()
    {
        var collection = GetComponentsInChildren<LightController1>();
        foreach (var item in collection)
        {
            diagrams.Add(item.fd);
            item.fd.onNodeChange += Fd_OnCurrentNodeChange;
        }
    }

    private void Fd_OnCurrentNodeChange(FlowNode node)
    {
        if (node.name == "1")
        {
            foreach (var item in diagrams)
                if (item.currentNode != node && item.currentNode.name == "1")
                    item.EnterNext();
        }
    }
}
