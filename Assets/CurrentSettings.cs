using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSettings : MonoBehaviour {

    public enum characters
    {
        Benjamin,
        LiTian
    }

    public bool multiplayer;

    public characters player1 = characters.LiTian;
     public characters player2 = characters.Benjamin; 

    public static CurrentSettings instance = null;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
}
