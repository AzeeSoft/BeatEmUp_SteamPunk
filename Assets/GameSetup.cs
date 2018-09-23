using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour {

    public enum State
    {
        Main,
        Multiplayer,
        Singleplayer
    }
    public State currentState;

    public GameObject mainModule, singleModule, multiModule;

    private void Start()
    {
        currentState = State.Main;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Main:
                mainModule.gameObject.SetActive(true);
                multiModule.gameObject.SetActive(false);
                singleModule.gameObject.SetActive(false);
                break;
            case State.Multiplayer:
                mainModule.gameObject.SetActive(false);
                multiModule.gameObject.SetActive(true);
                singleModule.gameObject.SetActive(false);
                break;
            case State.Singleplayer:
                mainModule.gameObject.SetActive(false);
                multiModule.gameObject.SetActive(false);
                singleModule.gameObject.SetActive(true);
                break;
        
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("ArenaScene");
    }

    public void PlayerVsPlayer()
    {
        currentState = State.Multiplayer;
        CurrentSettings.instance.multiplayer = true;
    }
    public void PlayerVsAI()
    {
        currentState = State.Singleplayer;
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
