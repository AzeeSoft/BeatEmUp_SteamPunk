using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseManger : MonoBehaviour 
{
	public RectTransform pauseMenuRoot;
	public Text timerText;
	public Image controllerImage;

	void Awake ()
	{
		ResumeGame();
	}

	void PauseGame ()
	{
		pauseMenuRoot.gameObject.SetActive(true);
		timerText.gameObject.SetActive(false);
		Time.timeScale = 0;
	}
	
	public void ResumeGame ()
	{
		pauseMenuRoot.gameObject.SetActive(false);
		timerText.gameObject.SetActive(true);
		Time.timeScale = 1;
	}

	public void QuitGame ()
	{
		Application.Quit();
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
