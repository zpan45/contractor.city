using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenController : MonoBehaviour
{
    public GameController gameController;
    public JobDetailPanelController jobDetailPanelController;
    public WorkSiteAnimationController workSiteAnimationController;
    public ShopPanelController shopPanelController;
    public ContractorsPanelController contractorsPanelController;
    public SettingsPanelController settingsPanelController;
    public SoundManagerScript soundManager;

    public GameObject jobListEntry;
    public RectTransform scrollableContent;
    public GameObject topBar;


    Text[] labels;
    private List<GameObject> entries;

    //swipe detection stuff
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    private List<Vector3> touchPositions = new List<Vector3>(); //store all the touch positions in list

    private float lastSwipeTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        entries = new List<GameObject>();
        StartCoroutine(DisplayJobList());
        labels = topBar.GetComponentsInChildren<Text>();
        //soundManager.PlaySound("background");
        dragDistance = Screen.width * 20 / 100;
    }




    // displays the current jobs on the main screen
    IEnumerator DisplayJobList() {

        scrollableContent.sizeDelta = new Vector2(300 * gameController.game.Jobs.Count, 900);

        while (true) {
            yield return new WaitForSeconds(1.0f);

            foreach (GameObject o in entries)
            {
                Destroy(o);
            }

            labels[0].text = gameController.ToShortString(gameController.game.Money);
            labels[1].text = gameController.ToShortString(gameController.game.Rp);


            foreach (Job j in gameController.game.Jobs)
            {
                GameObject newEntry = Instantiate(jobListEntry, jobListEntry.transform.position + new Vector3(300 * (j.jobID - 1), 0, 0), jobListEntry.transform.rotation, jobListEntry.transform.parent);
                //Text[] texts = newEntry.GetComponentsInChildren<Text>();
                //texts[0].text = j.jobTitle;

                Image[] images = newEntry.GetComponentsInChildren<Image>();


                switch (j.JobStatus)
                {
                    case Job.Status.Locked:
                        images[0].sprite = gameController.jobSprites[j.jobID - 1];
                        images[0].color = Color.black;
                        break;
                    case Job.Status.Ready:
                        if (j.HasBeenBuilt)
                        {
                            images[0].sprite = gameController.jobSprites[j.jobID - 1];
                            images[0].color = Color.white;
                        }
                        else
                        {
                            images[0].sprite = gameController.jobBillboardSprites[j.jobID - 1];
                            images[0].color = Color.white;
                        }

                        break;
                    case Job.Status.Ongoing:
                        images[0].sprite = j.HasBeenBuilt ? gameController.jobSprites[j.jobID - 1] : gameController.blankSprite;
                        images[0].color = Color.white;
                        //set work in progress part transpancy
                        newEntry.GetComponentsInChildren<CanvasGroup>()[0].alpha = 1f;
                        newEntry.GetComponentsInChildren<Text>()[0].text = gameController.FormatTime((int)(j.TimeLastFinished - Time.time));
                        break;
                }


                images[4].sprite = gameController.game.CurrentViewingJobID == j.jobID ? gameController.arrowSprite : gameController.blankSprite;

                //images[0].sprite = gameController.jobSprites[j.jobID - 1];

                Button button = newEntry.GetComponentInChildren<Button>();
                button.onClick.RemoveAllListeners();
                if (j.JobStatus != Job.Status.Locked) {
                    button.onClick.AddListener(() => gameController.game.CurrentViewingJobID = j.jobID);
                    button.onClick.AddListener(() => workSiteAnimationController.UpdateWorkSiteAnimation());
                    button.onClick.AddListener(() => workSiteAnimationController.HideWorkSiteAnimation());
                    button.onClick.AddListener(() => jobDetailPanelController.DisplayContent());
                    button.onClick.AddListener(() => soundManager.PlaySound("button"));

                }

                button.interactable = j.JobStatus != Job.Status.Locked;

                newEntry.SetActive(true);

                entries.Add(newEntry);
            }

        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
        bool panelsOpen = shopPanelController.GetComponent<CanvasGroup>().alpha > 0f ||
                         jobDetailPanelController.GetComponent<CanvasGroup>().alpha > 0f ||
                         jobDetailPanelController.oopsPanel.GetComponent<CanvasGroup>().alpha > 0f ||
                         contractorsPanelController.GetComponent<CanvasGroup>().alpha > 0f ||
                         settingsPanelController.GetComponent<CanvasGroup>().alpha > 0f;

        if (!panelsOpen)
        {

            //keyboard: press  to mimic "swipe"
            if (Input.GetKey("left"))
            {



                if (gameController.game.CurrentViewingJobID > 1 && Time.time - lastSwipeTime > 0.2f) 
                {
                    gameController.game.CurrentViewingJobID--;
                    lastSwipeTime = Time.time;
                    workSiteAnimationController.UpdateWorkSiteAnimation();
                    Debug.Log("Last Job");
                }

                touchPositions.Clear();

            }

            if (Input.GetKey("right"))
            {
            

                if (gameController.game.CurrentViewingJobID < 10 && Time.time - lastSwipeTime > 0.2f)
                {
                    if (gameController.game.Jobs[gameController.game.CurrentViewingJobID].JobStatus != Job.Status.Locked)
                    {
                        gameController.game.CurrentViewingJobID++;
                        lastSwipeTime = Time.time;
                        workSiteAnimationController.UpdateWorkSiteAnimation();
                        Debug.Log("Next Job");
                    }
                }
                 
                touchPositions.Clear();

            }



            foreach (Touch touch in Input.touches)  //use loop to detect more than one swipe
            {
                if (touch.phase == TouchPhase.Moved) //add the touches to list as the swipe is being made
                {
                    touchPositions.Add(touch.position);
                }

                if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    //lp = touch.position;  //last touch position. Ommitted if you use list
                    fp = touchPositions[0]; //get first touch position from the list of touches
                    lp = touchPositions[touchPositions.Count - 1]; //last touch position 

                    //Check if drag distance is greater than 20% of the screen width
                    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                    {//It's a drag
                     //check if the drag is vertical or horizontal 
                        if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y) && fp.y < 1200) //make sure swiping the bottom part of screen
                        {   //If the horizontal movement is greater than the vertical movement...
                            if ((lp.x > fp.x))  //If the movement was to the right)
                            {   //Right swipe

                                if (gameController.game.CurrentViewingJobID > 1 && Time.time - lastSwipeTime > 0.2f)
                                {
                                    gameController.game.CurrentViewingJobID--;
                                    lastSwipeTime = Time.time;
                                    workSiteAnimationController.UpdateWorkSiteAnimation();
                                    Debug.Log("Last Job");
                                }

                                touchPositions.Clear();

                            }
                            else
                            {   //Left swipe

                                if (gameController.game.CurrentViewingJobID < 10 && Time.time - lastSwipeTime > 0.2f)
                                {
                                    if (gameController.game.Jobs[gameController.game.CurrentViewingJobID].JobStatus != Job.Status.Locked)
                                    {
                                        gameController.game.CurrentViewingJobID++;
                                        lastSwipeTime = Time.time;
                                        workSiteAnimationController.UpdateWorkSiteAnimation();
                                        Debug.Log("Next Job");
                                    }
                                }

                                touchPositions.Clear();

                            }
                        }
                        else
                        {   //the vertical movement is greater than the horizontal movement
                            if (lp.y > fp.y)  //If the movement was up
                            {   //Up swipe
                                Debug.Log("Up Swipe");
                                touchPositions.Clear();
                            }
                            else
                            {   //Down swipe
                                Debug.Log("Down Swipe");
                                touchPositions.Clear();
                            }
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height

                }
            }
        }
        //Debug.Log(gameController.game.CurrentViewingJobID);
    }
    */
}
