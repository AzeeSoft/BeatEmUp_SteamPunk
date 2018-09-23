using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Character Data")]
public class CharacterInfo : ScriptableObject
{
    public string name;
    public Color primaryColor;
    public Sprite image;
}
