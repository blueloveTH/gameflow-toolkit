using GameFlow;
using System.Collections.Generic;
using UnityEngine;

public class MutexLight : MonoBehaviour
{
    public List<FlowDiagram> diagrams = new List<FlowDiagram>();

    void Start()
    {
        var collection = GetComponentsInChildren<LightController1>();
        foreach (var item in collection)
        {
            diagrams.Add(item.fd);
            item.fd.onCurrentNodeChange += Fd_OnCurrentNodeChange;
        }
    }

    private void Fd_OnCurrentNodeChange(FlowNode node)
    {
        if (node == "1")
        {
            foreach (var item in diagrams)
                if (item.currentNode != node && item.currentNode == "1")
                    item.EnterNext();
        }
    }
}
