using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameFlow
{
    [System.Serializable]
    public sealed class Trigger
    {
        public bool value;
        /// <summary>
        /// Test-and-Set(FALSE)
        /// </summary>
        public bool TS()
        {
            if (value)
            {
                value = false;
                return true;
            }
            else return false;
        }
    }

    public class Blackboard : MonoBehaviour
    {
        private Dictionary<string, FieldInfo> fieldsDic = new Dictionary<string, FieldInfo>();

        public event System.Action<string, object> OnValueChange;

        public bool Contains(string name)
        {
            return GetFieldInfo(name) != null;
        }

        private FieldInfo GetFieldInfo(string fieldName)
        {
            FieldInfo info;
            if (!fieldsDic.TryGetValue(fieldName, out info))
            {
                info = GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
                if (info != null) fieldsDic.Add(fieldName, info);
            }
            return info;
        }

        public void SetValue(string fieldName, object value)
        {
            SetValueWithoutEvent(fieldName, value);
            OnValueChange(fieldName, value);
        }

        public void SetValueWithoutEvent(string fieldName, object value)
        {
            GetFieldInfo(fieldName).SetValue(this, value);
        }

        public object GetValue(string fieldName)
        {
            return GetFieldInfo(fieldName).GetValue(this);
        }

        public T GetValue<T>(string fieldName)
        {
            return (T)GetValue(fieldName);
        }

        public T GetValueAndReset<T>(string fieldName, T defaultValue)
        {
            object t = GetValue(fieldName);
            SetValueWithoutEvent(fieldName, defaultValue);
            return (T)t;
        }

        public void SetTrigger(string name)
        {
            SetValue(name, true);
        }

        public bool GetTrigger(string name)
        {
            if (!Contains(name))
                throw new System.Exception("Trigger: " + name + " doesn't exist.");
            bool t = GetValue<bool>(name);
            if (t) SetValue(name, false);
            return t;
        }
    }
}