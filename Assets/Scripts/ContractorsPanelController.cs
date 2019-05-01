using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractorsPanelController : MonoBehaviour
{
    public GameController gameController;
    public ContractorController contractorController;
    public WorkSiteAnimationController workSiteAnimationController;
    public SoundManagerScript soundManager;

    public GameObject listEntry;
    public GameObject moneyBar;
    public Scrollbar scrollbar;


    private List<GameObject> entries;
    private Text[] labels;

    // Start is called before the first frame update
    void Start()
    {
        entries = new List<GameObject>();
        StartCoroutine(UpdateMoneyBar());
    }

    IEnumerator UpdateMoneyBar() {

        while (true) {
            yield return new WaitForSeconds(2.0f);
            if(this.GetComponentInParent<CanvasGroup>().alpha > 0f) UpdateMoney();

        }
    }

    void UpdateMoney() {
        //Debug.Log("Updating");
        moneyBar.GetComponentsInChildren<Text>()[0].text = gameController.ToShortString(gameController.game.Money);
        moneyBar.GetComponentsInChildren<Text>()[1].text = gameController.ToShortString(gameController.game.Rp);
    }

    void UpdateContractorList() {
        foreach (GameObject o in entries)
        {
            Destroy(o);
        }

        int count = gameController.game.Contractors.FindAll((Contractor obj) => obj.ContractorStatus == Contractor.Status.Hired).Count;
        if (count < 8) count = 8;
        listEntry.GetComponentInParent<RectTransform>().sizeDelta = new Vector2(1000, 240 * count + 0);

        foreach (Contractor c in gameController.game.Contractors) if (c.ContractorStatus == Contractor.Status.Hired)
        {
            int price = contractorController.CalculateCost(c.contractorLevel - 1, 1000, 1.2f);  //change this later
            GameObject newEntry = Instantiate(listEntry, listEntry.transform.position - new Vector3(0, 240 * (c.contractorID - 1), 0), listEntry.transform.rotation, listEntry.transform.parent);
            Text[] texts = newEntry.GetComponentsInChildren<Text>();
            texts[0].text = gameController.ToShortString(price);
            texts[1].text = c.contractorName;
            texts[2].text = c.contractorCategory.ToString() + " | Lvl " + c.contractorLevel;
            texts[3].text = c.Skills[0].ToString();
            if (c.Skills.Count > 1) texts[3].text = ("<b>" + c.Skills[1].ToString() + "</b>");

            Image[] images = newEntry.GetComponentsInChildren<Image>();
            images[1].sprite = c.contractorLevel >= gameController.scifiRequiredLevel
                ? gameController.scifiContractorSprites[c.contractorID - 1]
                : gameController.contractorSprites[c.contractorID - 1];

            Button button = newEntry.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => contractorController.UpgradeContractor(c));
            button.onClick.AddListener(() => UpdateContractorList());
            button.onClick.AddListener(() => UpdateMoney());

            Slider slider = newEntry.GetComponentInChildren<Slider>();
            slider.value = (float)(c.contractorLevel % gameController.scifiRequiredLevel) / gameController.scifiRequiredLevel;



            newEntry.SetActive(true);
            entries.Add(newEntry);
        }

        StartCoroutine(ResetScrollBar());
    }

    IEnumerator ResetScrollBar()
    {
        yield return new WaitForEndOfFrame();
        scrollbar.value = 1.0f;

    }




    // displays the shop panel
    public void DisplayPanel()
    {
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        workSiteAnimationController.UpdateWorkSiteAnimation();
        workSiteAnimationController.HideWorkSiteAnimation();

        UpdateContractorList();
        UpdateMoney();
    }

    // hides the shop panel
    public void HidePanel()
    {
        GetComponent<CanvasGroup>().alpha = 0f;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        workSiteAnimationController.UpdateWorkSiteAnimation();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
