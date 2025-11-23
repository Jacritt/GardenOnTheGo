// GrowthStage.cs
using System;
using UnityEngine;

[Serializable]
public class GrowthStage
{
    public string stageName;
    public Sprite sprite;
    [TextArea] public string description;

    public int waterRequired = 1;
    public int daysRequired = 5;
    public int minigamesRequired = 10;
}
