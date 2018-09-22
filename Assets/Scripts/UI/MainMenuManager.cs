using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour 
{
	public static MainMenuManager instance;
	public RectTransform startScreen;
	public RectTransform fileSelectScreen;
	public RectTransform newGameScreen;

	public Selectable startScreenSelectable;
	public Selectable fileScreenSelectable;

	public enum Screen
	{
		Start, 
		FileSelect,
		NewGame
	}

	public Screen currentScreen = Screen.Start;
	
	void Awake ()
	{
		if(instance == null) instance = this;
	
		ShowStartScreen (); 
		deleteFilePrompt.SetActive(false);
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Cancel"))
		{
			switch (currentScreen)
			{
				case Screen.FileSelect:
					ShowStartScreen();
					break;
				case Screen.NewGame:
					ShowFileSelectScreen();
					break;
				default:
					break;
			}
		}
	}

	public void ShowStartScreen ()
	{
		currentScreen = Screen.Start;
		EventSystem.current.SetSelectedGameObject(null);
		startScreen.gameObject.SetActive(true);
		fileSelectScreen.gameObject.SetActive(false);
		newGameScreen.gameObject.SetActive(false);
		startScreenSelectable.Select();
	}

	
	public void ShowFileSelectScreen ()
	{
		currentScreen = Screen.FileSelect;
		EventSystem.current.SetSelectedGameObject(null);
		startScreen.gameObject.SetActive(false);
		fileSelectScreen.gameObject.SetActive(true);
		newGameScreen.gameObject.SetActive(false);
		fileScreenSelectable.Select();
	}

	
	public void ShowNewGameScreen ()
	{
		currentScreen = Screen.NewGame;
		EventSystem.current.SetSelectedGameObject(null);
		startScreen.gameObject.SetActive(false);
		fileSelectScreen.gameObject.SetActive(false);
		newGameScreen.gameObject.SetActive(true);
	}

	private FileControllerManager fileSelect;
	public GameObject deleteFilePrompt;
	public void PromptDelete (FileControllerManager fileSelect)
	{
		this.fileSelect = fileSelect;
		deleteFilePrompt.SetActive(true);
	}

	public void ConfirmDelete (bool confirm)
	{
		deleteFilePrompt.SetActive (false);
		if (confirm)
		
		{
			fileSelect.ConfirmDelete ();
		}
		
	}

}

