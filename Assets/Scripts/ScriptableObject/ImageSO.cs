using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Image", menuName = "Scriptable Object/Image")]
public class ImageSO : ScriptableObject
{
    public Sprites[] Images;
}

[Serializable]
public class Sprites
{
    public Sprite[] SpriteList;
}