using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPanelController : MonoBehaviour {
    // Start is called before the first frame update

    public void ShowPanel() {
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<CanvasGroup>().interactable = true;
    }

    public void HidePanel() {
        GetComponent<CanvasGroup>().alpha = 0f;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<CanvasGroup>().interactable = false;
    }

    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
