using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ctrlMenu : MonoBehaviour {
    // Use this for initialization
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject creditsScreen;
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    private GameObject soundToggle;
    //[SerializeField]
    //private GameObject bigX;
    private bool soundOn;

    public static ctrlMenu Instance;
    void Awake()
    {
        // Register the singleton
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of GameplayControls!");
        }
<<<<<<< HEAD
        
        soundOn = (PlayerPrefs.GetString("music") != "False");
=======

        soundOn = PlayerPrefs.GetString("music") == "true";
>>>>>>> origin/master
        soundToggle.GetComponent<Toggle>().isOn = soundOn;
        updateSoundToggle();
        Instance = this;
    }

    public void Play(string stageName)
    {
		Application.LoadLevel(stageName);
        //Debug.Log ("Nova Cena");
    }

    public void OpenOptions()
    {
        settingsMenu.SetActive(true);
        Panel.SetActive(false);
    }
    public void CloseOptions()
    {
        settingsMenu.SetActive(false);
        Panel.SetActive(true);
    }

    public void OpenCredits()
    {
        creditsScreen.SetActive(true);
        Panel.SetActive(false);
    }
    public void CloseCredits()
    {
        creditsScreen.SetActive(false);
        Panel.SetActive(true);
    }
    public void updateSoundToggle()
    {
        if (soundOn)
        {
            AudioListener.volume = 1f;
        }
        else
        {
            AudioListener.volume = 0f;
        }
    }
    public void ToogleMusic()
    {
<<<<<<< HEAD
        soundOn = soundToggle.GetComponent<Toggle>().isOn;//!soundOn;
=======
        soundOn = !soundOn;
>>>>>>> origin/master
        PlayerPrefs.SetString("music", soundOn.ToString());
        updateSoundToggle();
        //SetSoundsVolume(musicToggleOn, toogleSoundFx.isOn);
    }
    
    private void SetSoundsVolume(bool musicToggleOn, bool soundFxToggleOn)
    {
        float fadeTime = 0.1f;/*
        if (musicToggleOn && soundFxToggleOn) allSoundsOn.TransitionTo(fadeTime);
        if (!musicToggleOn && !soundFxToggleOn) muteAllSounds.TransitionTo(fadeTime);
        if (!musicToggleOn && soundFxToggleOn) muteMusic.TransitionTo(fadeTime);
        if (musicToggleOn && !soundFxToggleOn) muteSoundFx.TransitionTo(fadeTime);*/
    }
    public void Exit(){

	}
}
