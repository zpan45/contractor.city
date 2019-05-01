using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class WorkSiteAnimationController : MonoBehaviour
{
    public GameController gameController;
    public ShopPanelController shopPanelController;
    public JobDetailPanelController jobDetailPanelController;
    public ContractorsPanelController contractorsPanelController;
    public SettingsPanelController settingsPanelController;
    public SoundManagerScript soundManager;


    public CanvasGroup moleCG;

    public GameObject[] workSites = new GameObject[10];

    //GameObject currentWorkSite;

    public void UpdateWorkSiteAnimation()
    {
        int cvjid = gameController.game.CurrentViewingJobID;

        foreach (GameObject obj in workSites)
        {
            obj.SetActive(false);
            obj.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
            obj.GetComponentInChildren<Button>().onClick.AddListener(() => gameController.SpeedUpJob());
        }

        workSites[cvjid - 1].SetActive(true);

        bool panelsOpen = shopPanelController.GetComponent<CanvasGroup>().alpha > 0f ||
                         jobDetailPanelController.GetComponent<CanvasGroup>().alpha > 0f ||
                         jobDetailPanelController.oopsPanel.GetComponent<CanvasGroup>().alpha > 0f ||
                         contractorsPanelController.GetComponent<CanvasGroup>().alpha > 0f ||
                         settingsPanelController.GetComponent<CanvasGroup>().alpha > 0f; 

        if (gameController.game.Jobs[gameController.game.CurrentViewingJobID - 1].JobStatus == Job.Status.Ongoing && !panelsOpen)
        {
            ShowWorkSiteAnimation();
            if (soundManager.sfxOn)
            workSites[gameController.game.CurrentViewingJobID - 1].GetComponent<AudioSource>().Play();
            else workSites[gameController.game.CurrentViewingJobID - 1].GetComponent<AudioSource>().Stop();
        }
        else
        {
            HideWorkSiteAnimation();
            workSites[gameController.game.CurrentViewingJobID - 1].GetComponent<AudioSource>().Stop();
        }
    }

    public void ShowWorkSiteAnimation()
    {
        Vector3 pos = workSites[gameController.game.CurrentViewingJobID - 1].GetComponentsInChildren<RectTransform>()[2].anchoredPosition3D;
        pos.z = -0.1f;
        workSites[gameController.game.CurrentViewingJobID - 1].GetComponentsInChildren<RectTransform>()[2].anchoredPosition3D = pos;

        WorkSiteAnimator animator = workSites[gameController.game.CurrentViewingJobID - 1].GetComponent<WorkSiteAnimator>();
        if(animator != null)
            animator.PlayAnimation();

        workSites[gameController.game.CurrentViewingJobID - 1].GetComponentInChildren<CanvasGroup>().alpha = 1f;


    }

    public void HideWorkSiteAnimation()
    {
        Vector3 pos = workSites[gameController.game.CurrentViewingJobID - 1].GetComponentsInChildren<RectTransform>()[2].anchoredPosition3D;
        pos.z = 0f;
        workSites[gameController.game.CurrentViewingJobID - 1].GetComponentsInChildren<RectTransform>()[2].anchoredPosition3D = pos;

        WorkSiteAnimator animator = workSites[gameController.game.CurrentViewingJobID - 1].GetComponent<WorkSiteAnimator>();
        if (animator != null)
            animator.StopAnimation();

        workSites[gameController.game.CurrentViewingJobID - 1].GetComponentInChildren<CanvasGroup>().alpha = 0f;


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
