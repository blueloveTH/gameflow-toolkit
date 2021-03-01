using UnityEngine;

public sealed class EnumFlagsAttribute : PropertyAttribute
{
    public System.Type enumType { get; private set; }

    public EnumFlagsAttribute(System.Type enumType)
    {
        this.enumType = enumType;
    }
}

