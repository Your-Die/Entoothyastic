using UnityEngine;

public static class TMPHelper
{
    public static string WrapColor(string text, Color color)
    {
        string hex = ColorUtility.ToHtmlStringRGBA(color);
        return $"<color=#{hex}>{text}</color>";
    }
}
