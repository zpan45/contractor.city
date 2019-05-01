using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JobDetailPanelController : MonoBehaviour
{

    public GameController gameController;
    public JobController jobController;
    public MainScreenController mainScreenController;
    public WorkSiteAnimationController workSiteAnimationController;
    public SoundManagerScript soundManager;

    public GameObject equipEntry;
    //public GameObject readyButton;
    public GameObject swipeBar;
    public GameObject oopsPanel;
    public Image jobImage;

    public float swipeAvailableTime = 5.0f;

    private List<GameObject> entries;




    //swipe detection stuff
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    private List<Vector3> touchPositions = new List<Vector3>(); //store all the touch positions in list

    public bool waitingForSwipe = false;

    private Color32 darkGreen = new Color32(0x3a, 0x67, 0x1a, 0xff);
    private Color32 Blue = new Color32(0x1b, 0x90, 0xd0, 0xff);

    private Tweener colorTweener;

    public Coroutine swipeDetectionCoroutine;

    public void DisplayPanel()
    {
        GetComponent<CanvasGroup>().alpha = 1f;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        workSiteAnimationController.UpdateWorkSiteAnimation();
        workSiteAnimationController.HideWorkSiteAnimation();

    }

    public void HidePanel()
    {
        GetComponent<CanvasGroup>().alpha = 0f;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        //workSiteAnimationController.ShowWorkSite();
        workSiteAnimationController.UpdateWorkSiteAnimation();


        if (waitingForSwipe) {
            waitingForSwipe = false;
            DisplayOops();
            swipeBar.GetComponent<CanvasGroup>().alpha = 0f;
        }

    }

    public IEnumerator RefreshContent()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            DisplayContent();
        }

    }

    public void DisplayOops() {
        oopsPanel.GetComponent<CanvasGroup>().alpha = 1f;
        oopsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        gameController.game.SwipesMissed += 1;
        workSiteAnimationController.HideWorkSiteAnimation();
        soundManager.PlaySound("lose");
    }

    public void DisplayContent(){
        //display the job details for the currently viewing job
        int id = gameController.game.CurrentViewingJobID;
        Job currentJob = gameController.game.Jobs.Find(x => x.jobID == id);
        jobImage.sprite = gameController.jobSprites[id - 1];


        Text[] labels = this.GetComponentsInChildren<Text>();
        labels[0].text = currentJob.jobTitle;
        if (currentJob.JobStatus == Job.Status.Ongoing) {
            labels[1].text = gameController.FormatTime((int)(currentJob.TimeLastFinished - Time.time));
            labels[5].text = currentJob.contractorsReq.ToString() + " / " + currentJob.contractorsReq.ToString();
            labels[5].color = darkGreen;

        } else {
            labels[1].text = gameController.FormatTime(currentJob.timeRequired);
            labels[5].text = gameController.game.ContractorsAvailable.ToString() + " / " + currentJob.contractorsReq.ToString();

            if (gameController.game.ContractorsAvailable < currentJob.contractorsReq) labels[5].color = Color.grey;
            else labels[5].color = Blue;
        }

        labels[2].text = "";
        //labels[2].text = gameController.ToShortString(currentJob.payout_exp);
        labels[3].text = gameController.ToShortString(currentJob.payout_money);
        
        Button[] buttons = this.GetComponentsInChildren<Button>();
        buttons[0].onClick.RemoveAllListeners();
        buttons[0].onClick.AddListener(() => HidePanel());
        buttons[1].onClick.RemoveAllListeners();
        buttons[1].onClick.AddListener(() => jobController.StartJob(id));
        buttons[1].onClick.AddListener(() => DisplaySwipeBar());
        buttons[2].onClick.RemoveAllListeners();
        buttons[2].onClick.AddListener(() => jobController.InstantFinish(id));

        buttons[2].GetComponentsInChildren<Text>()[1].text = "- " + jobController.InstantFinishPrice(id).ToString();

        ShowEquipmentsRequired(currentJob);


        buttons[1].interactable = currentJob.JobStatus == Job.Status.Ready && jobController.HasRequiredEquipments(currentJob) ? true : false;
        buttons[2].interactable = gameController.game.Rp >= jobController.InstantFinishPrice(id) ? true : false;

        buttons[1].GetComponent<CanvasGroup>().alpha = currentJob.JobStatus == Job.Status.Ongoing ? 0.0f : 1.0f;
        buttons[2].GetComponent<CanvasGroup>().alpha = currentJob.JobStatus == Job.Status.Ongoing ? 1.0f : 0.0f;
        buttons[2].GetComponent<CanvasGroup>().blocksRaycasts = currentJob.JobStatus == Job.Status.Ongoing ? true : false;

        buttons[0].onClick.AddListener(() => soundManager.PlaySound("button"));
        buttons[1].onClick.AddListener(() => soundManager.PlaySound("ready button"));
        buttons[2].onClick.AddListener(() => soundManager.PlaySound("button"));


    }

    public void DisplaySwipeBar(){
        //display swipe bar
        swipeBar.GetComponent<CanvasGroup>().alpha = 1f;
        //set color transition
        swipeBar.GetComponent<Image>().color = new Color32(0x79, 0xd3, 0x39, 0xff);
        if(colorTweener != null) colorTweener.Kill();
        colorTweener = swipeBar.GetComponent<Image>().DOColor(new Color32(0xeb, 0x21, 0x2d, 0xff), swipeAvailableTime);
        //start detecting swipe
        waitingForSwipe = true;
        touchPositions.Clear();
        swipeDetectionCoroutine = StartCoroutine(DetectingSwipe());
    }

    public IEnumerator DetectingSwipe() {

        yield return new WaitForSeconds(swipeAvailableTime);
        if (waitingForSwipe) {
            waitingForSwipe = false;
            swipeBar.GetComponent<CanvasGroup>().alpha = 0f;
            DisplayOops();
        }


    }

    /*
    void StartJob(int id) {
        Job currentJob = gameController.game.Jobs.Find(x => x.jobID == id);
        jobController.StartJob(currentJob);
    }*/

    void ShowEquipmentsRequired(Job j) {

        int[,] equips = j.EquipmentsRequired;

        foreach (GameObject o in entries)
        {
            Destroy(o);
        }

        for (int x = 0; x < equips.GetLength(0); x++) {
            int id = equips[x, 0];
            int num = equips[x, 1];

            Equipment equip = gameController.game.Equipments.Find(e => e.equipmentID == id);

            GameObject newEntry = Instantiate(equipEntry, equipEntry.transform.position - new Vector3(100 * x , 0, 0), equipEntry.transform.rotation, equipEntry.transform.parent);
            Text[] texts = newEntry.GetComponentsInChildren<Text>();
            if(j.JobStatus == Job.Status.Ongoing) {
                texts[0].text = num.ToString() + " / " + num.ToString();
                texts[0].color = darkGreen;
            }
            else {
                texts[0].text = equip.NumberAvailable.ToString() + " / " + num.ToString();
                if (equip.NumberAvailable < num) texts[0].color = Color.grey;
                else texts[0].color = Blue;

            }
            //texts[1].text = equip.name;
            newEntry.GetComponentsInChildren<Image>()[0].sprite = gameController.equipmentSprites[id - 1];
            newEntry.SetActive(true);
            entries.Add(newEntry);
        }



    }
    

    // Start is called before the first frame update
    void Start()
    {
        entries = new List<GameObject>();


        dragDistance = Screen.width * 20 / 100; //dragDistance is 20% width of the screen

    }

    // Update is called once per frame
    void Update()
    {
        //detecing swipe
        if(waitingForSwipe) {

            //keyboard: press the space bar to mimic "swipe"
            if (Input.GetKey("space")) {
                swipeBar.GetComponent<CanvasGroup>().alpha = 0f;
                Debug.Log("Swiped");
                waitingForSwipe = false;
                gameController.EarnRP(1);
                touchPositions.Clear();
                HidePanel();
                StopCoroutine(swipeDetectionCoroutine);
                soundManager.PlaySound("swipe");
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
                        if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                        {   //If the horizontal movement is greater than the vertical movement...
                            if ((lp.x > fp.x))  //If the movement was to the right)
                            {   //Right swipe
                                swipeBar.GetComponent<CanvasGroup>().alpha = 0f;
                                Debug.Log("Right Swipe");
                                waitingForSwipe = false;
                                gameController.EarnRP(1);
                                touchPositions.Clear();
                                HidePanel();
                                StopCoroutine(swipeDetectionCoroutine);
                                soundManager.PlaySound("swipe");
                            }
                            else
                            {   //Left swipe
                                swipeBar.GetComponent<CanvasGroup>().alpha = 0f;
                                Debug.Log("Left Swipe");
                                waitingForSwipe = false;
                                gameController.EarnRP(1);
                                touchPositions.Clear();
                                HidePanel();
                                StopCoroutine(swipeDetectionCoroutine);
                                soundManager.PlaySound("swipe");
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
    }
}
