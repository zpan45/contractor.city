using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelController : MonoBehaviour
{

    public SoundManagerScript soundManager;

    public Sprite musicOn;
    public Sprite musicOff;

    public Sprite soundOn;
    public Sprite soundOff;

    public Sprite credits;

    public WorkSiteAnimationController workSiteAnimationController;

    // Start is called before the first frame update
    void Start()
    {

    }

    // displays the shop panel
    public void DisplayPanel()
    {
        GameObject settingsPanel = GameObject.Find("SettingsPanel");
        CanvasGroup settingsPanelCanvas = settingsPanel.GetComponent<CanvasGroup>();

        settingsPanelCanvas.alpha = 1f;
        settingsPanelCanvas.blocksRaycasts = true;
        settingsPanelCanvas.interactable = true;

        workSiteAnimationController.UpdateWorkSiteAnimation();
        workSiteAnimationController.HideWorkSiteAnimation();

    }

    public void toggleBGM() {
        Debug.Log(soundManager);
        soundManager.bgmOn = !soundManager.bgmOn;
        soundManager.ToogleBGM();
        GameObject.Find("BGMButtonImage").GetComponent<Image>().sprite = soundManager.bgmOn ? musicOn : musicOff;
        GameObject.Find("BGMButton").GetComponentInChildren<Text>().text = soundManager.bgmOn ? "  BGM: ON" : "  BGM: OFF";
    }

    public void toggleSFX()
    {
        //if (soundManager.sfxOn) soundManager.sfxOn = false;
        //else soundManager.sfxOn = true;
        soundManager.sfxOn = !soundManager.sfxOn;
        GameObject.Find("SFXButtonImage").GetComponent<Image>().sprite = soundManager.sfxOn ? soundOn : soundOff;
        GameObject.Find("SFXButton").GetComponentInChildren<Text>().text = soundManager.sfxOn ? "  SFX: ON" : "  SFX: OFF";
    }

    // hides the shop panel
    public void HidePanel()
    {
        GameObject settingsPanel = GameObject.Find("SettingsPanel");
        CanvasGroup settingsPanelCanvas = settingsPanel.GetComponent<CanvasGroup>();

        settingsPanelCanvas.alpha = 0f;
        settingsPanelCanvas.blocksRaycasts = false;
        settingsPanelCanvas.interactable = false;

        workSiteAnimationController.UpdateWorkSiteAnimation();

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
