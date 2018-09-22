﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseManger : MonoBehaviour 
{
	public RectTransform pauseMenuRoot;

	void Awake ()
	{
		ResumeGame();
	}

	void PauseGame ()
	{
		pauseMenuRoot.gameObject.SetActive(true);
		Time.timeScale = 0;
	}
	
	public void ResumeGame ()
	{
		pauseMenuRoot.gameObject.SetActive(false);
		Time.timeScale = 1;
	}

	public void QuitGame ()
	{
		Application.Quit();
	}

	void Controls ()
	{
		//Game Controls
	}

	void Update ()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			if (pauseMenuRoot.gameObject.activeSelf) ResumeGame();
			else PauseGame();
		}
	} 
}