using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileControllerManager : MonoBehaviour 
{

	public Text nameText;
	public Text statsText;
	public GameObject deleteButton;

	void OnEnable ()
	{
		if (SaveFileExists())
		{
			SaveManager.SaveFile saveFile = SaveManager.Read(transform.GetSiblingIndex());
			nameText.text = saveFile.name;
			statsText.text = string.Format("HP:{0}/{1}\nLVL:{2}", saveFile.health, saveFile.maxHealth, saveFile.scene);
			deleteButton.SetActive (true);
		}

		else 
		{
			nameText.text = "New Game";
			statsText.text = "HP: ---/---\nLVL: ---";
			deleteButton.SetActive (false);
		}
	}

	bool SaveFileExists ()
	{
		int fileID = transform.GetSiblingIndex();
		return (SaveManager.Read(fileID) != null);
	}

	public void ButtonClicked ()
	{
		if (SaveFileExists())
		{
			SaveManager.Load(transform.GetSiblingIndex());
		}

		else
		{
			MainMenuManager.instance.ShowNewGameScreen();
		}
	}

	public void DeleteSaveFile ()
	{
		MainMenuManager.instance.PromptDelete(this);
	}

	public void ConfirmDelete ()
	{
		SaveManager.Delete (transform.GetSiblingIndex());
		nameText.text = "New Game";
		statsText.text = "HP: ---/---\nLVL: ---";
		deleteButton.SetActive (false);
	}
}

