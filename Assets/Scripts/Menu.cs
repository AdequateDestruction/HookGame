using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    // Button Gameobjects for making buttons active / inactive
    public GameObject PlayBtn;
    public GameObject SettingsBtn;
    public GameObject CreditsBtn;
    public GameObject QuitBtn;
    public GameObject HowToBtn;
    public GameObject BackBtn;

    // Panels (objects) for showing and hiding menu elements
    public GameObject CreditsPanel;
    public GameObject SettingsPanel;
    public GameObject HelpPanel;
    public GameObject DifficultyPanel;
    public GameObject HowToPlayPanel;

    // loading screen objects
    public GameObject LoadingScreen;
    public Animator loreTextAnimator;
    public Text loadingText;

    void Start()
    {
        //Active when menu opens
        PlayBtn.SetActive(true);
        SettingsBtn.SetActive(true);
        CreditsBtn.SetActive(true);
        HowToBtn.SetActive(true);
        QuitBtn.SetActive(true);

        //Not active when menu opens
        BackBtn.SetActive(false);
        CreditsPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        HelpPanel.SetActive(false);
        LoadingScreen.SetActive(false);
        DifficultyPanel.SetActive(false);
        HowToPlayPanel.SetActive(false);

        FindObjectOfType<MusicManagerScript>().StartMainMenuMusic();
    }

    public void Play()
    {
        PlayBtn.SetActive(false);
        SettingsBtn.SetActive(false);
        CreditsBtn.SetActive(false);
        HowToBtn.SetActive(false);
        QuitBtn.SetActive(false);

        BackBtn.SetActive(true);
        DifficultyPanel.SetActive(true);
    }

    //For Quit button
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    //For Back button
    public void GoBack()
    {
        PlayBtn.SetActive(true);
        SettingsBtn.SetActive(true);
        CreditsBtn.SetActive(true);
        QuitBtn.SetActive(true);
        HowToBtn.SetActive(true);
        BackBtn.SetActive(false);

        CreditsPanel.SetActive(false);
        HelpPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        DifficultyPanel.SetActive(false);
        HowToPlayPanel.SetActive(false);
    }

    //For Credits Button
    public void Credits()
    {
        PlayBtn.SetActive(false);
        SettingsBtn.SetActive(false);
        CreditsBtn.SetActive(false);
        QuitBtn.SetActive(false);
        HowToBtn.SetActive(false);

        BackBtn.SetActive(true);
        CreditsPanel.SetActive(true);
    }

    //For Help/? button
    public void Help()
    {
        PlayBtn.SetActive(false);
        SettingsBtn.SetActive(false);
        CreditsBtn.SetActive(false);
        QuitBtn.SetActive(false);
        HowToBtn.SetActive(false);

        BackBtn.SetActive(true);
        HelpPanel.SetActive(true);
    }

    public void HowToPlay()
    {
        PlayBtn.SetActive(false);
        SettingsBtn.SetActive(false);
        CreditsBtn.SetActive(false);
        QuitBtn.SetActive(false);
        HowToBtn.SetActive(false);

        BackBtn.SetActive(true);
        HowToPlayPanel.SetActive(true);
    }

    //For Settings button
    public void Settings()
    {
        PlayBtn.SetActive(false);
        SettingsBtn.SetActive(false);
        CreditsBtn.SetActive(false);
        QuitBtn.SetActive(false);
        HowToBtn.SetActive(false);

        BackBtn.SetActive(true);
        SettingsPanel.SetActive(true);
    }

    public void ChooseDifficulty(int difficulty)
    {
        FindObjectOfType<GameSettingsScript>().SetDifficulty(difficulty);
        LoadLevel(1);
    }

    //Funtion to load level (assigned for play button)
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
        SettingsBtn.SetActive(false);
        CreditsBtn.SetActive(false);
        QuitBtn.SetActive(false);
    }

    // Coroutine for loading passed level, check scene index in Build menu
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        LoadingScreen.SetActive(true);
        loreTextAnimator.Play("LoadingLoreTextAnimation");  // This is mainly a PH for boss 1, replace with another animation / effect / whatever you want to show before the loaded level.
        FindObjectOfType<MusicManagerScript>().FadeOutMusic();
        yield return new WaitForSeconds(1.5f);  // Fake "loading time" to allow for the menu music to fade and loading screen to show. Adjust if the actual level load is longer (currently boss1 scene loads almost instantly).

        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneIndex);
        ao.allowSceneActivation = false;

        // Wait in current scene (menu) until the loading progress is done, then give the option to switch to loaded scene with E (or other button / key, switch Input here).
        while (!ao.isDone)
        {
            if (ao.progress == 0.9f)
            {
                loadingText.text = "PRESS E TO BEGIN";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ao.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
