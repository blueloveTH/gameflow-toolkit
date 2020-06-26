using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MaskIntAttribute : PropertyAttribute
{
    public System.Type enumType { get; private set; }

    public MaskIntAttribute(System.Type enumType)
    {
        this.enumType = enumType;
    }
}

