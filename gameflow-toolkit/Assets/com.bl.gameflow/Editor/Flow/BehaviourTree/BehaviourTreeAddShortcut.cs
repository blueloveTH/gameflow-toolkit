//using UnityEditor;
//using UnityEngine;

//namespace GameFlow
//{
//    internal class BehaviourTreeAddShortcut : Editor
//    {
//        [MenuItem("GameObject/GameFlow/BehaviourTree", false, 17)]
//        public static void Display()
//        {
//            var t = Selection.activeTransform;
//            GameObject obj = new GameObject("BehaviourTree(Clone)");
//            obj.AddComponent<BehaviourTree>();
//            if (t != null)
//                obj.transform.SetParent(t);
//            Selection.activeGameObject = obj;
//        }
//    }
//}