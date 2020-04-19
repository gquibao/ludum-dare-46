using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    private static readonly int Clicked = Animator.StringToHash("Clicked");

    public void StartButton()
    {
        animator.SetTrigger(Clicked);
    }

    public void SelectDifficulty(int id)
    {
        var difficulty = "";
        switch (id)
        {
            case 0:
                difficulty = "Easy";
                break;
            
            case 1:
                difficulty = "Medium";
                break;
            
            case 2:
                difficulty = "Hard";
                break;
        }
        animator.SetTrigger(Clicked);
        StartCoroutine(LoadLevel(difficulty));
    }

    IEnumerator LoadLevel(string difficulty)
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(difficulty);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
