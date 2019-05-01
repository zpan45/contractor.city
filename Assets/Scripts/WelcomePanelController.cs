using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomePanelController : MonoBehaviour
{
    public SoundManagerScript soundManager;

    public void ShowPanel() {
        CanvasGroup welcomeScreenCG = GameObject.Find("WelcomePanel").GetComponent<CanvasGroup>();
        welcomeScreenCG.alpha = 1f;
        welcomeScreenCG.interactable = true;
        welcomeScreenCG.blocksRaycasts = true;
        Button gotItButton = GameObject.Find("GotItButton").GetComponent<Button>();
        gotItButton.onClick.AddListener(() => soundManager.ButtonSound());
        gotItButton.onClick.AddListener(() => HidePanel());
    }

    public void HidePanel()
    {
        CanvasGroup welcomeScreenCG = GameObject.Find("WelcomePanel").GetComponent<CanvasGroup>();
        welcomeScreenCG.alpha = 0f;
        welcomeScreenCG.interactable = false;
        welcomeScreenCG.blocksRaycasts = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
