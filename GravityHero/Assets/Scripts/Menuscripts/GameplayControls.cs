using UnityEngine;
using System.Collections;

public class GameplayControls : MonoBehaviour {
    /// <summary>
    /// Singleton
    /// </summary>
    public static GameplayControls Instance;

    //[SerializeField] private GameObject btPause;
    //[SerializeField] private GameObject menuPause;
    [SerializeField]
    private GameObject menuVictory;
    [SerializeField]
    private GameObject PlayerObject;
    //[SerializeField] private GameObject tutorial;
    
    //private SoundControls soundControls;

    private bool isFirstTime;

    private bool onTutorial;

    void Awake()
    {
        // Register the singleton
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of GameplayControls!");
        }

        Instance = this;
    }


    public void Start()
    {
        //soundControls = GameObject.FindGameObjectWithTag("SoundsController").GetComponent<SoundControls>();
        //menuPause.SetActive(false);
        menuVictory.SetActive(false);
        //tutorial.SetActive(false);
        /*
        if (!PlayerPrefs.HasKey ("firstTime")) {
			PlayerPrefs.SetString ("firstTime", "true");
		}
        isFirstTime = bool.Parse(PlayerPrefs.GetString("firstTime"));
        if (isFirstTime) {
			DoTutorial ();
		}*/
    }

    public void Update()
    {
        if(onTutorial && (Input.GetKeyDown("space") || TouchedScreen()))
        {
            onTutorial = false;
            //tutorial.SetActive(false);
            Time.timeScale = 1;
            isFirstTime = false;
            PlayerPrefs.SetString("firstTime", isFirstTime.ToString());
        }
    }

    /*private void DoTutorial()
    {
        onTutorial = true;
        Time.timeScale = 0;
        //tutorial.SetActive(true);
    }*/

    public void ShowVictory()
    {
        menuVictory.SetActive(true);
        //btPause.SetActive(false);
    }
    public void HideGameover() { menuVictory.SetActive(false); }
    /*
    public void ShowPauseMenu()
    {
        menuPause.SetActive(true);
        btPause.SetActive(false);
    }
    public void HidePauseMenu()
    {
        btPause.SetActive(true);
        menuPause.SetActive(false);
    }*/

    public void HidePlayer() { PlayerObject.SetActive(false); }
    public void ShowPlayer() { PlayerObject.SetActive(true); }

    public void ResetGame()
    {
        //soundControls.PlayButton();
        PlayerObject.GetComponent<PlayerController>().resetGravity();
        Application.LoadLevel(Application.loadedLevel);
    }

    public void MainMenu()
    {
        //soundControls.PlayButton();
        PlayerObject.GetComponent<PlayerController>().resetGravity();
        Application.LoadLevel("Main Menu");
        Time.timeScale = 1;
    }

    public void toStage(string stageName)
    {
        //soundControls.PlayButton();
        PlayerObject.GetComponent<PlayerController>().resetGravity();
        Application.LoadLevel(stageName);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        //Set time.timescale to 0, this will cause animations and physics to stop updating
        Time.timeScale = 0;
        //Show pause menu
        //ShowPauseMenu();
        //soundControls.StopClimbingSound();
    }

    public void Resume()
    {
        //HidePauseMenu();
        Time.timeScale = 1;
        //soundControls.PlayClimbingSound();
    }

    private static bool TouchedScreen()
    {
        if (Input.touchCount > 0)
            return Input.GetTouch(0).phase == TouchPhase.Began;
        else
            return false;
    }

}
