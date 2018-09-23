using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour {

    public enum State
    {
        Main,
        Multiplayer,
        Singleplayer
    }
    public State currentState;

    public GameObject mainModule, singleModule, multiModule;

    private void Update()
    {
        switch (currentState)
        {
            case State.Multiplayer:

                break;
            case State.Singleplayer:

                break;
        
        }
    }

    public void PlayerVsPlayer()
    {
        CurrentSettings.instance.multiplayer = true;
    }
    public void PlayerVsAI()
    {
        CurrentSettings.instance.multiplayer = false;
    }

    public void Player1Select(string name)
    {
        if(name == "LiTian")
        {
            CurrentSettings.instance.player1 = CurrentSettings.characters.LiTian;
        }
        if(name == "Ben")
        {
            CurrentSettings.instance.player1 = CurrentSettings.characters.Benjamin;
        }
    }

    public void Player2Select(string name)
    {
        if (name == "LiTian")
        {
            CurrentSettings.instance.player2 = CurrentSettings.characters.LiTian;
        }
        if (name == "Ben")
        {
            CurrentSettings.instance.player2 = CurrentSettings.characters.Benjamin;
        }
    }
}
