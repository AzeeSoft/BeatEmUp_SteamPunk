using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManger : MonoBehaviour 
{
	public RectTransform pauseMenuRoot;
	public Text timerText;
	public Image controllerImage;

    public GameObject controllerPanel;

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
//		Application.Quit();
        SceneManager.LoadScene("StartScreen");
	}

	void Update ()
	{
		if (Input.GetButtonDown("Cancel"))
		{
            controllerPanel.SetActive(false);

			if (pauseMenuRoot.gameObject.activeSelf) ResumeGame();
			else PauseGame();
		}
	} 
}
