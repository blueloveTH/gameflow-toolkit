using UnityEngine;

namespace GameFlow
{
    public enum BehaviourStatus
    {
        Failure = -1,
        Running = 0,
        Success = 1,
    }

    public class BehaviourTree : MonoBehaviour
    {
        [SerializeField] bool _playOnStart = true;
        [SerializeField] bool _restartWhenComplete;
        [SerializeField] float tickRate = 0.1f;
        public bool restartWhenComplete {
            get { return _restartWhenComplete; }
            set { _restartWhenComplete = value; }
        }

        private IGameModel _model;
        public IGameModel model {
            get {
                if (_model == null)
                {
                    if (transform.parent == null) return null;
                    var r = transform.parent.GetComponents<MonoBehaviour>();
                    _model = System.Array.Find(r, (x) => x is IGameModel) as IGameModel;
                }
                return _model;
            }
        }

        public event System.Action OnEnd;
        public bool IsPlaying { get; private set; }

        private BehaviourNode _entry;
        public BehaviourNode entry {
            get {
                if (_entry == null)
                    _entry = transform.GetChild(0).GetComponent<BehaviourNode>();
                return _entry;
            }
        }

        protected void Start()
        {
            if (_playOnStart) Begin();
        }

        public void Begin()
        {
            if (IsPlaying) return;
            entry.Init();
            IsPlaying = true;
        }

        private float restTime = 0;

        private void Update()
        {
            if (!IsPlaying) return;

            if (tickRate <= 0)
            {
                TreeUpdate();
                return;
            }

            if (restTime <= 0)
            {
                TreeUpdate();
                restTime = tickRate;
            }
            else
            {
                restTime -= Time.deltaTime;
            }
        }

        private void TreeUpdate()
        {
            if (entry.Tick() != BehaviourStatus.Running) End();
        }

        public void End()
        {
            if (!IsPlaying) return;
            IsPlaying = false;
            OnEnd?.Invoke();
            if (restartWhenComplete) Begin();
        }

        public PlayTask NewPlayTask() { return new PlayTask(this); }

        public class PlayTask : Task
        {
            public BehaviourTree tree { get; private set; }
            internal PlayTask(BehaviourTree tree)
            {
                this.tree = tree;
                tree.OnEnd += Tree_OnEnd;
            }

            private void Tree_OnEnd()
            {
                Complete();
            }

            protected override void OnPlay()
            {
                base.OnPlay();
                tree.gameObject.SetActive(true);
                tree.Begin();
            }
        }
    }
}