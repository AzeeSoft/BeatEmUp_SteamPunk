using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;

	public float totalTime = 60f;
	private float startTime = 0f;

	public Text youLoseText;
	public Text timerText;
	public Text youWinText;

	public bool gameOver;

	public RectTransform credits;

    public GameObject YouWinGameObject;
    public GameObject YouLoseGameObject;

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
			timerText.gameObject.SetActive(false);
			yield return new WaitForSeconds (2f);
			SceneManager.LoadScene("Start Screen");
		}
    }

	IEnumerator Timer()
	{
		while (enabled)
		{
		    int timeLeft = (int) (totalTime - (Time.time - startTime));
            if (timeLeft <= 0)
            {
                timeLeft = 0;

                CharacterModel winner = ArenaManager.Instance.GetWinningPlayer();
                if (winner != null)
                {
                    ShowGameOver(winner);
                }
            }

            timerText.text = "Time: " + timeLeft;
			yield return new WaitForSeconds(1);
		}
	}

	public IEnumerator GameOver ()
	{
		while (enabled)
		{
			youWinText.text = "You Win!";
			StopCoroutine("Timer");
			timerText.gameObject.SetActive(false);
			yield return new WaitForSeconds (2f);			
			credits.gameObject.SetActive(true);
		}
	}

    public void ResetTimer()
    {
        startTime = Time.time;
    }

    public void ShowGameOver(CharacterModel winner)
    {
        gameOver = true;
        youWinText.text = winner.CharacterInfo.name + " Wins!!";
        StopCoroutine("Timer");
        StartCoroutine("GameOverDelay");
    }

    public IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(2f);
        timerText.gameObject.SetActive(false);
        Time.timeScale = 0;
        YouWinGameObject.SetActive(true);

        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("StartScreen");
    
    }
}