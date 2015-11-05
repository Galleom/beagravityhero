using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    private GameObject ButtonSound;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private GameObject loadingScreen;


    [SerializeField] private AudioMixerSnapshot muteMusic;
    [SerializeField] private AudioMixerSnapshot muteSoundFx;
    [SerializeField] private AudioMixerSnapshot muteAllSounds;
    [SerializeField] private AudioMixerSnapshot allSoundsOn;

    [SerializeField] private Toggle toogleMusic;
    [SerializeField] private Toggle toogleSoundFx;

    private bool onCreditScreen;

    void Start()
    {
        onCreditScreen = false;
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        creditsScreen.SetActive(false);

        // load user sounds preferences
        if (!PlayerPrefs.HasKey("music")) PlayerPrefs.SetString("music", "true");
        if (!PlayerPrefs.HasKey("soundFx")) PlayerPrefs.SetString("soundFx", "true");

        toogleMusic.isOn = bool.Parse(PlayerPrefs.GetString("music"));
        toogleSoundFx.isOn = bool.Parse(PlayerPrefs.GetString("soundFx"));
    }

    void Update()
    {
        if((Input.GetKeyDown("space") || TouchedScreen()) && onCreditScreen)
        {
            ReturnToMainMenu();
        }
    }

    /// <summary>
    /// User Interface Functions
    /// </summary>
    public void Play()
    {
        //Application.LoadLevel("Gameplay");
        StartCoroutine(GoPlay());
    }

    public void OpenSettings()
    {
        HideMainMenu();
        ShowSettingsMenu();
    }

    public void OpenCredits()
    {
        HideMainMenu();
        ShowCreditsScreen();
        onCreditScreen = true;
    }

    public void ReturnToMainMenu()
    {
        onCreditScreen = false;
        HideCreditsScreen();
        HideSettingsMenu();
        ShowMainMenu();
    }

    public void PlayButtonSound()
    {
        ButtonSound.GetComponent<AudioSource>().Play();
    }

    public void ToogleMusicOn(bool musicToggleOn)
    {
        PlayerPrefs.SetString("music", musicToggleOn.ToString());
        SetSoundsVolume(musicToggleOn, toogleSoundFx.isOn);
    }

    public void ToogleSoundFxOn(bool soundFxToggleOn)
    {
        PlayerPrefs.SetString("soundFx", soundFxToggleOn.ToString());
        SetSoundsVolume(toogleMusic.isOn, soundFxToggleOn);
    }

    /// <summary>
    /// Menus, Screens, etc. Hide/Show functions
    /// </summary>
    private void ShowSettingsMenu() { settingsMenu.SetActive(true); }
    private void HideSettingsMenu() { settingsMenu.SetActive(false); }
    private void ShowCreditsScreen() { creditsScreen.SetActive(true); }
    private void HideCreditsScreen() { creditsScreen.SetActive(false); }
    private void ShowMainMenu() { mainMenu.SetActive(true); }
    private void HideMainMenu() { mainMenu.SetActive(false); }

    /// <summary>
    /// Helper functions
    /// </summary>
    /// <returns></returns>
	IEnumerator GoPlay()
	{
        //GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ().SetBool ("Play", true);
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds (1.5f);
		Application.LoadLevel ("Gameplay");
	}


    private static bool TouchedScreen()
    {
        if (Input.touchCount > 0)
            return Input.GetTouch(0).phase == TouchPhase.Began;
        else
            return false;
    }

    private void SetSoundsVolume(bool musicToggleOn, bool soundFxToggleOn)
    {
        float fadeTime = 0.1f;
        if ( musicToggleOn  &&  soundFxToggleOn) allSoundsOn.   TransitionTo(fadeTime);
        if (!musicToggleOn  && !soundFxToggleOn) muteAllSounds. TransitionTo(fadeTime);
        if (!musicToggleOn  &&  soundFxToggleOn) muteMusic.     TransitionTo(fadeTime);
        if ( musicToggleOn  && !soundFxToggleOn) muteSoundFx.   TransitionTo(fadeTime);
    }
}
