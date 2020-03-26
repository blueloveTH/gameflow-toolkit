using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameFlow
{
    [CustomEditor(typeof(BehaviourNode), true)]
    internal sealed class BehaviourNodeDefaultEditor : BehaviourNodeCustomEditor
    {
        protected override bool CanAddChildItem(MonoBehaviour monoBehaviour)
        {
            return monoBehaviour is ParentNode;
        }

        protected override bool CanAddDecoratorItem(MonoBehaviour monoBehaviour)
        {
            return true;
        }
    }

    public abstract class BehaviourNodeCustomEditor : Editor
    {
        private static List<NodeMenuItem> list = new List<NodeMenuItem>();
        private static HashSet<Assembly> loadedAssemblies = new HashSet<Assembly>();

        MonoBehaviour monoBehaviour { get { return target as MonoBehaviour; } }

        private void OnEnable()
        {
            LoadAssemblyData(typeof(BehaviourNode).Assembly);
        }

        public static void LoadAssemblyData(Assembly assembly)
        {
            if (!loadedAssemblies.Add(assembly)) return;
            var types = assembly.GetTypes();

            foreach (var item in types)
            {
                var nodeMenuItem = Attribute.GetCustomAttribute(item, typeof(NodeMenuItem)) as NodeMenuItem;
                if (nodeMenuItem != null)
                {
                    nodeMenuItem.type = item;
                    list.Add(nodeMenuItem);
                }
            }
        }

        public override void OnInspectorGUI()
        {         
            var at = Attribute.GetCustomAttribute(target.GetType(), typeof(NodeMenuItem));
            if (at != null)
            {
                string d = (at as NodeMenuItem).description;
                if (!string.IsNullOrEmpty(d))
                {
                    GUILayout.Box(d);
                }
            }

            base.OnInspectorGUI();

            if (GUILayout.Button(new GUIContent("Add Node"), GUILayout.Height(24)))
            {
                GenericMenu menu = new GenericMenu();

                foreach (var item in list)
                {
                    GenericMenu.MenuFunction2 func2;

                    if (typeof(DecoratorNode).IsAssignableFrom(item.type))
                        func2 = OnDecoratorMenuItem;
                    else
                        func2 = OnAddChildMenuItem;

                    if (func2 == OnAddChildMenuItem && !CanAddChildItem(monoBehaviour)) continue;
                    if (func2 == OnDecoratorMenuItem && !CanAddDecoratorItem(monoBehaviour)) continue;
                    menu.AddItem(new GUIContent(item.path), false, func2, item.type);
                }

                menu.ShowAsContext();
            }
        }

        protected abstract bool CanAddChildItem(MonoBehaviour monoBehaviour);
        protected abstract bool CanAddDecoratorItem(MonoBehaviour monoBehaviour);

        void OnDecoratorMenuItem(object value)
        {
            Type type = (Type)value;
            GameObject obj = new GameObject();
            int siblingIndex = monoBehaviour.transform.GetSiblingIndex();
            obj.transform.parent = monoBehaviour.transform.parent;
            monoBehaviour.transform.parent = obj.transform;
            obj.transform.SetSiblingIndex(siblingIndex);
            obj.AddComponent(type);
            Selection.activeGameObject = obj;
        }

        void OnAddChildMenuItem(object value)
        {
            Type type = (Type)value;
            GameObject obj = new GameObject();
            obj.transform.parent = monoBehaviour.transform;
            obj.AddComponent(type);
            Selection.activeGameObject = obj;
        }
    }
}
