using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public PlayerActor2D actor2d { get; private set; }
    public static PlayerModel main { get; private set; }

    private void Awake()
    {
        main = this;
        actor2d = GetComponent<PlayerActor2D>();
    }
}
