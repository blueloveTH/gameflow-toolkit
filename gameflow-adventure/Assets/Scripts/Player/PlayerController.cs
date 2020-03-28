using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerModel.main.actor2d.onUpdate += Actor_OnUpdate;
    }

    private void Actor_OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        PlayerModel.main.actor2d.Move(new Vector2(h, v));
    }

}
