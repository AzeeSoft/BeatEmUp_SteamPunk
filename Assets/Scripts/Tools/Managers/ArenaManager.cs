using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaManager : MonoBehaviour {

    [Serializable]
    public class CharacterSpawnData
    {
        public Transform transform;
        public GameObject characterPrefab;
        public CharacterInputController characterInputController;
        public CharacterHUDInfo characterHudInfo;
    }

    public CharacterInputController playerKM;
    public CharacterInputController playerJ;
    public CharacterInputController ai;

    public CharacterHUDInfo hud1;
    public CharacterHUDInfo hud2;

    public GameObject monkPrefab;
    public GameObject benjaminPrefab;

    [HideInInspector]
    public ArenaCamera arenaCamera;

    public List<CharacterSpawnData> characterSpawnDataList;

    public Text readyText;

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

        arenaCamera = FindObjectOfType<ArenaCamera>();
    }

	// Use this for initialization
	void Start ()
	{
	    Time.timeScale = 1;
        StartCoroutine(StartAfterPause());
	}

    IEnumerator StartAfterPause()
    {
        readyText.text = "Ready";
        yield return new WaitForSeconds(2f);
        readyText.text = "Set";
        yield return new WaitForSeconds(1f);
        readyText.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        readyText.gameObject.SetActive(false);

        if (CurrentSettings.instance)
        {
            if (CurrentSettings.instance.multiplayer)
            {
                characterSpawnDataList[0].characterInputController = playerKM;
                characterSpawnDataList[1].characterInputController = playerJ;
            }
            else
            {
                characterSpawnDataList[0].characterInputController = playerKM;
                characterSpawnDataList[1].characterInputController = ai;
            }

            if (CurrentSettings.instance.player1 == CurrentSettings.characters.Benjamin)
            {
                characterSpawnDataList[0].characterPrefab = benjaminPrefab;
            }

            if (CurrentSettings.instance.player1 == CurrentSettings.characters.LiTian)
            {
                characterSpawnDataList[0].characterPrefab = monkPrefab;
            }

            if (CurrentSettings.instance.player2 == CurrentSettings.characters.Benjamin)
            {
                characterSpawnDataList[1].characterPrefab = benjaminPrefab;
            }

            if (CurrentSettings.instance.player2 == CurrentSettings.characters.LiTian)
            {
                characterSpawnDataList[1].characterPrefab = monkPrefab;
            }
        }

        SpawnCharacters();

        GameManager.instance.ResetTimer();
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
