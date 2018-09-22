using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Character Data")]
public class CharacterData : ScriptableObject
{
    public string name;
    public Color primaryColor;
    public Image image;
}
