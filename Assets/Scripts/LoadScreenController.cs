using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreenController : MonoBehaviour {
    public Slider slider;
    public CanvasGroup loadingScreen;
    public SoundManagerScript soundManager;

    public  int loadBarTimerMax;
    private int loadBarTimerCurrent;
    private bool awakePanel;
    private GameObject startButton;


    private void Start() {
        startButton = GameObject.Find("StartGameButton");
        startButton.SetActive(false);
        Button startButtonComponent = startButton.GetComponent<Button>();
        startButtonComponent.onClick.AddListener(() => soundManager.ButtonSound());
        startButtonComponent.onClick.AddListener(() => startGame());
    }

    private void startGame() {
        loadingScreen.alpha = 0f;
        loadingScreen.interactable = false;
        loadingScreen.blocksRaycasts = false;
    }

    public void ShowPanel() {
        Debug.Log("load bar timer max is: " + loadBarTimerMax);
        loadingScreen.alpha = 1f;
        loadingScreen.interactable = true;
        loadingScreen.blocksRaycasts = true;
        loadBarTimerCurrent = 0;
        awakePanel = true;
        Debug.Log("awake panel is: " + awakePanel);
    }

    private void Update() {
        Debug.Log("in update");
        Debug.Log("awake panel is in update: " + awakePanel);
        if (awakePanel) {
            Debug.Log("panel is awoken");
            if (loadBarTimerCurrent == loadBarTimerMax) {
                startButton.SetActive(true);
            }
            else {
                Debug.Log("incrementing slider val");
                slider.value = loadBarTimerCurrent;
                loadBarTimerCurrent++;
            }
        }
    }
}
