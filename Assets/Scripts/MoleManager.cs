using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;

public class MoleManager : MonoBehaviour
{

    public SoundManagerScript soundManager;
    public GameController gameController;

    public ShopPanelController shopPanelController;
    public JobDetailPanelController jobDetailPanelController;
    public ContractorsPanelController contractorsPanelController;
    public SettingsPanelController settingsPanelController;

    public MinesweeperController minesweeperController;

    public CanvasGroup moleCG;

    public SkeletonGraphic moleSG;
    public Image moleBubble;
    public Sprite gameIcon;
    public Sprite moneyIcon;
    public Sprite rpIcon;
    public Sprite blankSprite;

    public enum Type { MiniGame = 0, Money = 1, Rp = 2 }

    private System.Random rnd;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MolePopping());
        rnd = new System.Random();

        moleBubble.sprite = blankSprite;
        //HideMole();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator MolePopping()
    {
        while(true)
        {
            yield return new WaitForSeconds(30);

            bool panelsOpen = shopPanelController.GetComponent<CanvasGroup>().alpha > 0f ||
                         jobDetailPanelController.GetComponent<CanvasGroup>().alpha > 0f ||
                         jobDetailPanelController.oopsPanel.GetComponent<CanvasGroup>().alpha > 0f ||
                         contractorsPanelController.GetComponent<CanvasGroup>().alpha > 0f||
                         settingsPanelController.GetComponent<CanvasGroup>().alpha > 0f;

            if (!panelsOpen) SpawnMole(10, (MoleManager.Type)(int)(rnd.Next(5, 23) / 10));
        }
    }


    public void SpawnMole(float duration, Type type)
    {
        //mole appears


        moleSG.AnimationState.SetAnimation(0, "appear", false);
        soundManager.PlaySound("mole");

        //ShowMole();

        StartCoroutine(ShowMoleAtEndOfFrame());

        Coroutine moleControlling = StartCoroutine(MoleControl(duration));
        //bubble appears

        moleBubble.GetComponent<Button>().onClick.RemoveAllListeners();

        switch (type)
        {
            case Type.MiniGame:
                moleBubble.sprite = gameIcon;
                moleBubble.GetComponent<Button>().onClick.AddListener(() => Debug.Log("Start mini game here!"));
                //moleBubble.GetComponent<Button>().onClick.AddListener(() => MoleDisappears());
                moleBubble.GetComponent<Button>().onClick.AddListener(() => HideMole());
                moleBubble.GetComponent<Button>().onClick.AddListener(() => soundManager.PlaySound("button"));
                moleBubble.GetComponent<Button>().onClick.AddListener(() => StopCoroutine(moleControlling));
                moleBubble.GetComponent<Button>().onClick.AddListener(() => launchMiniGame());
                break;
            case Type.Money:
                moleBubble.sprite = moneyIcon;
                moleBubble.GetComponent<Button>().onClick.AddListener(() => gameController.EarnMoney((int)(gameController.game.Money * 0.1f)));
                moleBubble.GetComponent<Button>().onClick.AddListener(() => MoleDisappears());
                moleBubble.GetComponent<Button>().onClick.AddListener(() => soundManager.PlaySound("button"));
                moleBubble.GetComponent<Button>().onClick.AddListener(() => StopCoroutine(moleControlling));
                break;
            case Type.Rp:
                moleBubble.sprite = rpIcon;
                moleBubble.GetComponent<Button>().onClick.AddListener(() => gameController.EarnRP(1));
                moleBubble.GetComponent<Button>().onClick.AddListener(() => MoleDisappears());
                moleBubble.GetComponent<Button>().onClick.AddListener(() => soundManager.PlaySound("button"));
                moleBubble.GetComponent<Button>().onClick.AddListener(() => soundManager.PlaySound("earning money"));
                moleBubble.GetComponent<Button>().onClick.AddListener(() => StopCoroutine(moleControlling));
                break;

        }
        //wait for x sec, mole disappears
    }

    public void launchMiniGame() {
        var minigameCanvasGroup = GameObject.Find("FlagLayer").GetComponent<CanvasGroup>();
        minigameCanvasGroup.alpha = 1f;
        minigameCanvasGroup.blocksRaycasts = true;
        minigameCanvasGroup.interactable = true;
        minesweeperController.StartMiniGame();
    }

    public void MoleDisappears()
    {
        StartCoroutine(HideMoleAfterAnimation());

        moleSG.AnimationState.AddAnimation(0, "disappear", false, 0f);
        //soundManager.PlaySound("mole");
        moleBubble.sprite = blankSprite;
        moleBubble.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void HideMole()
    {
        moleCG.alpha = 0f;
        moleCG.blocksRaycasts = false;
        moleCG.interactable = false;
    }

    public void ShowMole()
    {
        moleCG.alpha = 1f;
        moleCG.blocksRaycasts = true;
        moleCG.interactable = true;
    }


    public IEnumerator MoleControl(float duration)
    {
        yield return new WaitForSeconds(duration);
        MoleDisappears();
    }

    public IEnumerator HideMoleAfterAnimation()
    {
        yield return new WaitForSeconds(2f);
        HideMole();
    }

    public IEnumerator ShowMoleAtEndOfFrame() 
    {
        yield return new WaitForEndOfFrame();
        ShowMole();
    }



}
