//using UnityEditor;
//using UnityEngine;

//namespace GameFlow
//{
//    [CustomEditor(typeof(StateMachine))]
//    public class FSMCustomEditor : Editor
//    {
//        private StateMachine fsm;

//        private void OnEnable()
//        {
//            fsm = target as StateMachine;
//        }


//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();
//            EditorGUILayout.Space();

//            bool isPlaying = EditorApplication.isPlayingOrWillChangePlaymode;

//            if (isPlaying)
//            {
//                if (GUILayout.Button(fsm.currentState == null ? "null" : fsm.currentState.id))
//                {
//                    if (fsm.currentState != null) Selection.activeObject = fsm.currentState;
//                }
//            }
//            else
//            {
//                if (GUILayout.Button("Open Editor Window"))
//                {
//                    FSMEditorWindow.Open(fsm);
//                }
//            }
//        }

//    }
//}
