using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelController : MonoBehaviour
{

    public enum Tab { Contractor = 0, Equipment = 1, RP = 2 }

    public GameController gameController;
    public ContractorController contractorController;
    public MainScreenController mainScreenController;
    public WorkSiteAnimationController workSiteAnimationController;
    public SoundManagerScript soundManager;

    public Text moneyText;
    public Text rpText;
    public RectTransform scrollableContent;
    public GameObject moneyBar;
    public Scrollbar scrollbar;

    public GameObject listEntry;

    private List<GameObject> entries;
    private Text[] labels;

    // Start is called before the first frame update
    void Start()
    {
        entries = new List<GameObject>();
        labels = moneyBar.GetComponentsInChildren<Text>();
    }
    /*
    public void SetTab(Tab t) {
        switch(t) {
            case Tab.Contractor:
                DisplayContractorContent();
                break;
            case Tab.Equipment:
                DisplayEquipmentContent();
                break;
            case Tab.RP:
                DisplayRPContent();
                break;
        }
    }*/

    // displays information for contractors to be bought
    public void DisplayContractorContent() {
        UpdateMoneyBar();
        scrollableContent.sizeDelta = new Vector2(1000 ,240 * gameController.game.Contractors.FindAll((Contractor obj) => obj.ContractorStatus == Contractor.Status.Unlocked).Count + 0);

        foreach (GameObject o in entries) {
            Destroy(o);
        }
        foreach (Contractor c in gameController.game.Contractors) if (c.ContractorStatus != Contractor.Status.Hired) {
            GameObject newEntry = Instantiate(listEntry, listEntry.transform.position - new Vector3(0, 240 * (c.contractorID - 1), 0), listEntry.transform.rotation, listEntry.transform.parent);
            Text[] texts = newEntry.GetComponentsInChildren<Text>();
            texts[0].text = gameController.ToShortString(c.contractorPrice);
            texts[1].text = c.contractorName;
            texts[2].text = c.contractorCategory.ToString();
            texts[3].text = c.Skills[0].ToString() + "\n";
            if (c.Skills.Count > 1) texts[3].text += ("<b>" + c.Skills[1].ToString() + "</b>");

            Image[] images = newEntry.GetComponentsInChildren<Image>();
            images[1].sprite = c.contractorLevel >= gameController.scifiRequiredLevel
                ? gameController.scifiContractorSprites[c.contractorID - 1]
                : gameController.contractorSprites[c.contractorID - 1];

            Button button = newEntry.GetComponentInChildren<Button>();
            switch(c.ContractorStatus) {
                case Contractor.Status.Locked:
                    button.GetComponentInChildren<Text>().text = "locked";
                    button.interactable = false;
                    break;
                case Contractor.Status.Hired:
                    button.GetComponentInChildren<Text>().text = "hired";
                    button.interactable = false;
                    break;
                case Contractor.Status.Unlocked:
                    button.onClick.AddListener(() => contractorController.BuyContractor(c.contractorID));
                    button.onClick.AddListener(() => soundManager.PlaySound("hire"));
                    button.interactable = true;
                    break;
            }
            newEntry.SetActive(true);
            entries.Add(newEntry);
        }
        //scrollbar.value = 1.0f;
        StartCoroutine(ResetScrollBar());
    }

    // displays information for equipments to be bought
    public void DisplayEquipmentContent() {
        UpdateMoneyBar();
        scrollableContent.sizeDelta = new Vector2(1000, 240 * gameController.game.Equipments.FindAll((Equipment obj) => obj.EquipmentStatus == Equipment.Status.Available).Count + 0);

        foreach (GameObject o in entries)
        {
            Destroy(o);
        }
        foreach (Equipment e in gameController.game.Equipments) if (e.EquipmentStatus == Equipment.Status.Available)
        {
            int price = gameController.CalculateCost(e.NumberOwned, e.basecost);
            GameObject newEntry = Instantiate(listEntry, listEntry.transform.position - new Vector3(0, 240 * (e.equipmentID - 1), 0), listEntry.transform.rotation, listEntry.transform.parent);
            Text[] texts = newEntry.GetComponentsInChildren<Text>();
            texts[0].text = gameController.ToShortString(price);
            texts[1].text = e.name;
            texts[2].text = e.size;
            texts[3].text = "<b>Owned:</b> " + e.NumberOwned + " / <b>Not in use:</b> " + e.NumberAvailable;

            Image[] images = newEntry.GetComponentsInChildren<Image>();
            images[1].sprite = gameController.equipmentSprites[e.equipmentID - 1];

            Button button = newEntry.GetComponentInChildren<Button>();
            switch (e.EquipmentStatus)
            {
                case Equipment.Status.Locked:
                    button.GetComponentInChildren<Text>().text = "locked";
                    button.interactable = false;
                    break;
                case Equipment.Status.Available:
                    button.onClick.AddListener(() => gameController.BuyEquipment(e.equipmentID, price));
                    button.onClick.AddListener(() => soundManager.PlaySound("button"));
                    button.interactable = true;
                    break;
            }
            newEntry.SetActive(true);
            entries.Add(newEntry);
        }
        StartCoroutine(ResetScrollBar());
    }

    public void DisplayRPContent() {

    }

    IEnumerator ResetScrollBar() {
        yield return new WaitForEndOfFrame();
        scrollbar.value = 1.0f;

    }

    // displays the shop panel
    public void DisplayPanel() {
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        workSiteAnimationController.UpdateWorkSiteAnimation();
        workSiteAnimationController.HideWorkSiteAnimation();

        DisplayContractorContent();

    }

    // hides the shop panel
    public void HidePanel()
    {
        GetComponent<CanvasGroup>().alpha = 0f;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        workSiteAnimationController.UpdateWorkSiteAnimation();


    }

    // updates the money and respect point display bar
    void UpdateMoneyBar() {
        labels[0].text = gameController.ToShortString(gameController.game.Money);
        labels[1].text = gameController.ToShortString(gameController.game.Rp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

