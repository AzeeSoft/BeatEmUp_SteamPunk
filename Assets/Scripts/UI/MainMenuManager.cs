using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour 
{
	public static MainMenuManager instance;
	public RectTransform startScreen;
	public RectTransform controlScreen;

	public Selectable startScreenSelectable;

	public enum Screen
	{
		Start, 
		Controls,
	}

	public Screen currentScreen = Screen.Start;
	public int index = 0;
	
	void Awake ()
	{
		if(instance == null) instance = this;
	
		ShowStartScreen (); 
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Cancel"))
		{
			switch(index)
			{
				case 0:
				ShowStartScreen();
				break;

				case 1:
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
		startScreenSelectable.Select();
	}

	public void ShowControlScreen ()
	{

	}
}

