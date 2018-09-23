using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour {

    [Serializable]
    public struct CharacterSpawnData
    {
        public Transform transform;
        public GameObject characterPrefab;
        public CharacterInputController characterInputController;
        public CharacterHUDInfo characterHudInfo;
    }

    [HideInInspector]
    public ArenaCamera arenaCamera;

    public List<CharacterSpawnData> characterSpawnDataList;

    private static ArenaManager instance;
    public static ArenaManager Instance
    {
        get { return instance; }
    }

    [HideInInspector]
    public List<CharacterModel> characterModels;

    void Awake()
    {
        instance = this;

        Time.timeScale = 1;

        arenaCamera = FindObjectOfType<ArenaCamera>();
    }

	// Use this for initialization
	void Start () {
	    SpawnCharacters();

        AudioManager.instance.PlayMusic(AudioManager.AudioData.GameplayTheme, true);
    }
	
	// Update is called once per frame
	void Update () {
		CheckForGameOver();
	}

    void SpawnCharacters()
    {
        foreach (CharacterSpawnData characterSpawnData in characterSpawnDataList)
        {
            GameObject characterGameObject = Instantiate(characterSpawnData.characterPrefab, characterSpawnData.transform.position,
                characterSpawnData.transform.rotation);

            if (characterSpawnData.characterInputController)
            {
                Instantiate(characterSpawnData.characterInputController, characterGameObject.transform);
            }

            CharacterModel characterModel = characterGameObject.GetComponent<CharacterModel>();
            characterModels.Add(characterModel);

            characterSpawnData.characterHudInfo.AttachCharacter(characterModel);
        }
    }

    public void CheckForGameOver()
    {
        if (characterModels.Count == 0 || GameManager.instance.gameOver)
        {
            return;
        }

        int charsAlive = 0;
        CharacterModel currentWinner = null;

        foreach (CharacterModel characterModel in characterModels)
        {
            CharacterHealthController characterHealthController =
                characterModel.GetComponent<CharacterHealthController>();

            if (!characterHealthController.isDead)
            {
                if (charsAlive == 0)
                {
                    currentWinner = characterModel;
                }
                else
                {
                    currentWinner = null;
                }

                charsAlive++;
            }
        }

        if (charsAlive == 0)
        {
            // Game Tie
            // Forcing a winner... Yep.. that's right... Tough times!!
            currentWinner = characterModels[0];
            GameManager.instance.ShowGameOver(currentWinner);
        } else if (charsAlive == 1)
        {
            // We have a winner
            GameManager.instance.ShowGameOver(currentWinner);
        }
    }

    public CharacterModel GetWinningPlayer()
    {
        float maxhealth = 0;
        CharacterModel winner = null;
        foreach (CharacterModel characterModel in characterModels)
        {
            CharacterHealthController characterHealthController =
                characterModel.GetComponent<CharacterHealthController>();

            if (characterHealthController.health > maxhealth)
            {
                maxhealth = characterHealthController.health;
                winner = characterModel;
            }
        }

        return winner;
    }
}
