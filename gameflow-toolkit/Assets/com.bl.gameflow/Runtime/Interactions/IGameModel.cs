using UnityEngine;

namespace GameFlow
{
    public interface IGameModel
    {
        MonoBehaviour monoBehaviour { get; }
    }

    public abstract class GameModel : MonoBehaviour
    {
        protected InteractionHeader header { get; private set; }

        protected virtual void Awake()
        {
            header = gameObject.AddComponent<InteractionHeader>();
        }
    }
}
