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

        arenaCamera = FindObjectOfType<ArenaCamera>();
    }

	// Use this for initialization
	void Start () {
	    SpawnCharacters();
    }
	
	// Update is called once per frame
	void Update () {
		
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
}
