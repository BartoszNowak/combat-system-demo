using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIAnimationManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup background;
    [SerializeField]
    private Transform pauseMenu;
    [SerializeField]
    private Transform endMenu;
    [SerializeField]
    private float transitionTime;

    private bool menuOpen = false;
    private bool endMenuOpen = false;
    private ActionManager am;

    void Start()
    {
        pauseMenu.localPosition = new Vector2(0, -Screen.height);
        endMenu.localPosition = new Vector2(0, -Screen.height);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
		{
			if (menuOpen)
			{
				CloseMenu();
			}
			else
			{
				OpenMenu();
			}
			//OpenEndMenu();
		}
    }

    public void FadeScreen()
	{
        background.alpha = 0f;
        background.LeanAlpha(1f, 3f);
    }

    public void OpenMenu()
	{
        am = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionManager>();
        am.StartAction(new PausedState());
        background.alpha = 0f;
        background.LeanAlpha(0.8f, transitionTime);

        pauseMenu.localPosition = new Vector2(0, -Screen.height);
        pauseMenu.LeanMoveLocalY(0, transitionTime).setEaseInExpo().setDelay(0.1f).setOnComplete(Pause);
        menuOpen = true;
    }

    public void OpenEndMenu()
    {
        am = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionManager>();
        am.StartAction(new PausedState());

        endMenu.localPosition = new Vector2(0, -Screen.height);
        endMenu.LeanMoveLocalY(-300, 1f).setEaseInExpo().setDelay(0.1f).setOnComplete(Pause);
        endMenuOpen = true;
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f;
        background.LeanAlpha(0f, transitionTime);
        pauseMenu.LeanMoveLocalY(-Screen.height, transitionTime).setEaseInExpo().setOnComplete(UnPause);
        menuOpen = false;
    }

    public void CloseEndMenu()
    {
        Time.timeScale = 1f;
        background.LeanAlpha(0f, transitionTime);
        endMenu.LeanMoveLocalY(-Screen.height, transitionTime).setEaseInExpo().setOnComplete(UnPause);
        endMenuOpen = false;
    }

    public void PlayAgain()
	{
        StartCoroutine(LoadNewGame());
    }

    private IEnumerator LoadNewGame()
	{
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
	{
        StartCoroutine(Quit());
	}

    private IEnumerator Quit()
	{
        if (menuOpen) CloseMenu();
        if (endMenuOpen) CloseEndMenu();

        background.LeanAlpha(1f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Game Quit");
        Application.Quit();
	}



    private void Pause()
	{
        Time.timeScale = 0f;
    }

    private void UnPause()
    {
        am.CancelCurrentAction();
    }
}

public class PausedState : IAction
{

}
