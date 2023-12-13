using UnityEngine;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static T GetRandomValueFromArray<T>(this T[] arr)
    {
        return arr[Random.Range(0, arr.Length)];
    }
}