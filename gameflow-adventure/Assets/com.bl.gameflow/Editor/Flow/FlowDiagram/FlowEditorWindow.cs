//using Bluel.UnityEditor;
//using STL.Container;
//using UnityEditor;
//using UnityEngine;

//namespace GameFlow
//{
//    public class FSMEditorWindow : NodeBasedEditor
//    {
//        public static FSMEditorWindow main { get; private set; }
//        public static StateMachine fsm { get; private set; }

//        public DualMap<Node, FSMState> dMap = new DualMap<Node, FSMState>();

//        public static void Open(StateMachine paramFsm)
//        {   
//            if (paramFsm == null) return;
//            main?.Close();
//            fsm = paramFsm;

//            main = GetWindow<FSMEditorWindow>();
//            main.titleContent = new GUIContent("FSM Editor Window");
//        }

//        protected override void OnEnable()
//        {
//            base.OnEnable();
//            BuildFlow();
//        }

//        private void BuildFlow()
//        {
//            //Build nodes
//            foreach (var s in fsm.GetStates())
//                InitNode(s, s._editorWindowPos);

//            //Build connections
//            foreach (var s in fsm.GetStates())
//            {
//                foreach (var conn in s.GetTransitions())
//                {
//                    if (!conn.source || !conn.target) continue;
//                    var o = dMap.B2A(conn.source);
//                    var i = dMap.B2A(conn.target);
//                    AddConnection(o, i);
//                }
//            }
//        }

//        protected override void OnGUI()
//        {
//            if (!fsm || EditorApplication.isPlayingOrWillChangePlaymode)
//            {
//                Close();
//                return;
//            }
//            base.OnGUI();
//        }

//        protected override void ProcessContextMenu(Event e)
//        {
//            GenericMenu genericMenu = new GenericMenu();
//            genericMenu.AddItem(new GUIContent("New State"), false, () => NewState(e));
//            genericMenu.AddItem(new GUIContent("Refresh"), false, Refresh);
//            genericMenu.ShowAsContext();
//        }

//        private static void HardRefresh()
//        {
//            Open(fsm);
//        }

//        public override void Reset()
//        {
//            base.Reset();
//            foreach (var item in dMap)
//            {
//                if (!item.Value) continue;
//                Undo.RecordObject(item.Value, "");
//                item.Value._editorWindowPos = item.Key.rect.position;
//            }
//            dMap.Clear();
//        }

//        private void Refresh()
//        {
//            Reset();
//            BuildFlow();
//        }

//        private void NewState(Event e)
//        {
//            FSMDefaultState state = fsm.NewState("New State");
//            InitNode(state, e.mousePosition);
//            if (state != null) Selection.activeObject = state;
//        }

//        private Node InitNode(FSMState s, Vector2 position)
//        {
//            var node = AddNode(Vector2.zero);
//            node.title = s.id;
//            dMap.Add(node, s);
//            node.rect.position = position;
//            return node;
//        }

//        protected override void OnDestroy()
//        {
//            if (!fsm) return;
//            Reset();
//            fsm = null;
//        }

//        protected override Node CreateNode(Vector2 position)
//        {
//            Node newNode = new FSMEditorNode(position, new Vector2(120, 40),
//                nodeStyle, selectedNodeStyle);
//            return newNode;
//        }
//    }
//}