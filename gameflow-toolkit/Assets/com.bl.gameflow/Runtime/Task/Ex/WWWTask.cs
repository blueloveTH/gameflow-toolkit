using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

namespace GameFlow
{
    public class WWWTask : Task
    {
        public string url { get; private set; }
        public UnityWebRequest www { get; private set; }

        internal WWWTask(string url)
        {
            this.url = url;
        }

        protected override void OnPlay()
        {
            base.OnPlay();
            StartCoroutine(WWWCoroutine());
        }

        IEnumerator WWWCoroutine()
        {
            www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            Complete();
        }

        protected override void OnComplete()
        {
            base.OnComplete();
            InternalDispose();
        }

        protected override void OnKill()
        {
            base.OnKill();
            InternalDispose();
        }

        private void InternalDispose()
        {
            if (www != null)
            {
                www.Dispose();
                www = null;
            }
        }
    }
}
