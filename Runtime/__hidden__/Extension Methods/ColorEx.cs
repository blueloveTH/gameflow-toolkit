using UnityEngine;

public static class ColorEx
{
    public static Color OverrideR(this Color c, float r)
    {
        c.r = r;
        return c;
    }

    public static Color OverrideG(this Color c, float g)
    {
        c.g = g;
        return c;
    }

    public static Color OverrideB(this Color c, float b)
    {
        c.b = b;
        return c;
    }

    public static Color OverrideA(this Color c, float a)
    {
        c.a = a;
        return c;
    }


    public static Color32 OverrideR(this Color32 c, byte r)
    {
        c.r = r;
        return c;
    }

    public static Color32 OverrideG(this Color32 c, byte g)
    {
        c.g = g;
        return c;
    }

    public static Color32 OverrideB(this Color32 c, byte b)
    {
        c.b = b;
        return c;
    }

    public static Color32 OverrideA(this Color32 c, byte a)
    {
        c.a = a;
        return c;
    }
}
