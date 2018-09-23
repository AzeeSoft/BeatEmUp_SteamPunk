using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour 
{
	public static MainMenuManager instance;

	public RectTransform startScreen;
	public RectTransform controlScreen;

	public Selectable startScreenSelectable;

	public Button continueButton;

	public enum Screen
	{
		Start, 
		Controls,
	}

	public Screen currentScreen = Screen.Start;
	
	void Awake ()
	{
		if(instance == null) instance = this;
	
		ShowStartScreen (); 
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Cancel"))
		{
			switch(currentScreen)
			{
				case Screen.Start:
					ShowStartScreen();
					break;

				case Screen.Controls:
					ShowControlScreen();
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
		controlScreen.gameObject.SetActive(false);
		startScreenSelectable.Select();
	}

	public void ShowControlScreen ()
	{
		currentScreen = Screen.Controls;
		EventSystem.current.SetSelectedGameObject(null);
		controlScreen.gameObject.SetActive(true);
		startScreen.gameObject.SetActive(false);
	}

	public void Continue ()
	{
		SceneManager.LoadScene("createUI");
	}
}

