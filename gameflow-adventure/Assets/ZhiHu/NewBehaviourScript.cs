using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamFlag
{
    Player = 1,     //玩家势力
    Enemy = 2,      //反派势力
    Neutral = 4,    //中立
}

public class NewBehaviourScript : MonoBehaviour
{
    [EnumFlags(typeof(TeamFlag))]
    public int teamMask;
}
