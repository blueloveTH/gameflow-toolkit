using GameFlow.Tree;
using UnityEngine;


public class TestTree : MonoBehaviour
{
    private void Awake()
    {
        BehaviourTree tree = new BehaviourTree(
            owner: this,
            child: new Sequencer(
                    children: new BehaviourNode[]
                    {
                        new Conditional(
                                predicate: () => true,
                                child: new Lambda(()=>print("1"))
                                ),
                        new Lambda(()=>print("2"))
                    }
                )
            );

        tree.Play();
    }
}
