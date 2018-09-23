using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;

	public float startTime = 60f;

	public Text youLoseText;
	public Text timerText;
	public Text youWinText;

	bool gameOver;

	public RectTransform endGame;
	public RectTransform credits;
	
	private void Awake()
    {
        if (instance == null) instance = this;

        else Destroy(gameObject);
    }
	
	IEnumerator Start()
	{
        youLoseText.text = "";

		yield return new WaitForSeconds(1f);
        gameOver = false;
		StartCoroutine("Timer");
	}

	public IEnumerator YouLose()
    {
		while (enabled)
		{
			youLoseText.text = "Sad, You Lose";
			StopCoroutine("Timer");
			yield return new WaitForSeconds (2f);
			SceneManager.LoadScene("Start Screen");
		}
    }

	IEnumerator Timer()
	{
		while (enabled)
		{
			timerText.text = "Time: " + (int)(startTime - Time.time);
			yield return new WaitForSeconds(1);
			//If timer == 0 {player with greatest health; StartCouroutine("Game Over);}
		}
	}

	public IEnumerator GameOver ()
	{
		while (enabled)
		{
			youWinText.text = "You Win!";
			StopCoroutine("Timer");
			yield return new WaitForSeconds (2f);			
			credits.gameObject.SetActive(true);
		}
	}
}