using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MinesweeperController : MonoBehaviour
{
    public GridElement[,] elements;
    public Color[,,] maps = {
          // Map 0
        { { Color.grey, Color.blue, Color.grey, Color.grey },
          { Color.grey, Color.blue, Color.blue, Color.blue },
          { Color.yellow, Color.yellow, Color.grey, Color.grey },
          { Color.grey, Color.yellow, Color.grey, Color.grey } },

          // Map 1
        { { Color.grey, Color.grey, Color.magenta, Color.magenta },
          { Color.grey, Color.grey, Color.magenta, Color.grey },
          { Color.magenta, Color.magenta, Color.magenta, Color.grey },
          { Color.grey, Color.grey, Color.grey, Color.grey } },

          // Map 2
        { { Color.grey, Color.grey, Color.yellow, Color.grey },
          { Color.yellow, Color.yellow, Color.yellow, Color.grey },
          { Color.grey, Color.grey, Color.green, Color.grey },
          { Color.grey, Color.grey, Color.green, Color.grey } }
    };

    public GameController gameController;
    public GameObject infoPanel;
    public CanvasGroup gameScene;
    public SoundManagerScript soundManager;
    public Text timer;

    public Button startButton;
    public Button mapButton;
    public Button closeButton;

    public Sprite yellowStar;
    public Sprite greyStar;

    public Sprite untouchedTile;
    public Sprite hdTile;
    public Sprite hdPipeHitTile;
    public Sprite exTile;
    public Sprite exPipeHitTile;

    public Image[] tiles;
    public Sprite[] mapImages;

    private int starRating;
    private bool gameIsOver;
    private bool tutorialCompleted;
    private int currentMap;
    private int incorrectFlagDelay = 5;

    private float timeLeft;
    private bool startTimer;
    private int numLineHits;
    private int numCorrectFlags;

    private IEnumerator handDiggingCoroutine;
    private IEnumerator excavatingCoroutine;
    private IEnumerator exclamationCoroutine;
    private IEnumerator starRatingCoroutine;

    public Vector2 workerASize;
    public Vector2 workerBSize;
    public Vector3 workerAPos;
    public Vector3 workerBPos;

    public Vector2 panelContractor1Size;
    public Vector2 panelContractor2Size;
    public Vector3 panelContractor1Pos;
    public Vector3 panelContractor2Pos;

    public Vector3 excavatorPos;

    // takes in the index of the map and updates "elements" with appropriate fields
    public void GenerateGrid()
    {
        for (int i = 0; i < elements.GetLength(0); i++)
        {
            for (int j = 0; j < elements.GetLength(1); j++)
            {
                if (maps[currentMap, i, j] != Color.grey)
                {
                    elements[i, j] = new GridElement();
                    elements[i, j].pipeUnder = true;
                    elements[i, j].pipeColor = maps[currentMap, i, j];
                    elements[i, j].state = GridElement.State.Untouched;
                }
                else
                {
                    elements[i, j] = new GridElement();
                    elements[i, j].pipeUnder = false;
                    elements[i, j].state = GridElement.State.Untouched;
                }
            }
        }
    }

    // if given GridElement is untouched, flags it; increments numCorrectFlags if flag correctly laid
    public void Flag(GridElement element)
    {
        if (element.state == GridElement.State.Untouched || element.state == GridElement.State.HDLineStrike)
        {
            element.state = GridElement.State.Flagged;
            if (element.pipeUnder) { numCorrectFlags++; }
        }
    }

    // will move workers to grid element location, and update state of GridElements
    public IEnumerator StartHandDigging()
    {

        yield return new WaitForSeconds(2.0F);

        for (int i = 0; i < tiles.Length; i++)
        {
            moveContractors(i);
            soundManager.PlaySound("dig");
            GridElement currentElement = elements[i / 4, i % 4];
            if (currentElement.pipeUnder && currentElement.state != GridElement.State.Flagged)
            {
                currentElement.state = GridElement.State.HDLineStrike;
                exclamationCoroutine = ShowExclamation(i);
                StartCoroutine(exclamationCoroutine);
            }
            else if (!currentElement.pipeUnder && currentElement.state == GridElement.State.Untouched)
            {
                currentElement.state = GridElement.State.HandDigged;
            }
            if (i == 2)
            {
                StartCoroutine(excavatingCoroutine);
            }
            yield return new WaitForSeconds(2.0F);
        }
    }

    private void moveContractors(int tileIndex)
    {
        Image contractor1 = GameObject.Find("WorkerA").GetComponent<Image>();
        Image contractor2 = GameObject.Find("WorkerB").GetComponent<Image>();
        contractor1.rectTransform.position = tiles[tileIndex].rectTransform.position;
        contractor1.transform.Translate(-0.4f, 0.5f, 0, contractor1.transform);
        contractor2.rectTransform.position = tiles[tileIndex].rectTransform.position;
        contractor2.transform.Translate(0.4f, 0.5f, 0, contractor2.transform);
    }

    private IEnumerator ShowExclamation(int tileIndex)
    {
        if (tileIndex == -1) yield return null;
        soundManager.PlaySound("lose");
        Image exclamation = GameObject.Find("Exclamation").GetComponent<Image>();
        exclamation.rectTransform.position = tiles[tileIndex].rectTransform.position;
        exclamation.transform.Translate(-0.6f, 1.3f, 0, exclamation.transform);

        MakeAppear("Exclamation");
        yield return new WaitForSeconds(0.5F);
        MakeDisappear("Exclamation");

        Image bubble = GameObject.Find("Speech Bubble").GetComponent<Image>();
        bubble.rectTransform.position = tiles[tileIndex].rectTransform.position;
        bubble.transform.Translate(-0.3f, 1.6f, 0, bubble.transform);

        MakeAppear("Speech Bubble");
        yield return new WaitForSeconds(1.5F);
        MakeDisappear("Speech Bubble");
    }

    private void MakeAppear(string imgName) {
        Image img = GameObject.Find(imgName).GetComponent<Image>();
        var tempColor = img.color;
        tempColor.a = 1f;
        img.color = tempColor;
    }

    private void MakeDisappear(string imgName)
    {
        Image img = GameObject.Find(imgName).GetComponent<Image>();
        var tempColor = img.color;
        tempColor.a = 0f;
        img.color = tempColor;
    }

    // will move excavator to grid element location, and update state of GridElements
    public IEnumerator StartExcavating()
    {

        // yield return new WaitForSeconds(2.0F);

        for (int i = 0; i < tiles.Length; i++)
        {
            moveExcavator(i);
            soundManager.PlaySound("excavate");
            GridElement currentElement = elements[i / 4, i % 4];
            if (currentElement.state == GridElement.State.HDLineStrike)
            {
                currentElement.state = GridElement.State.EXLineStrike;
                numLineHits++;
            }
            else
            {
                currentElement.state = GridElement.State.Excavated;
            }
            yield return new WaitForSeconds(2.0F);
        }
    }

    private void moveExcavator(int tileIndex)
    {
        Image excavator = GameObject.Find("Excavator").GetComponent<Image>();
        excavator.rectTransform.position = tiles[tileIndex].rectTransform.position;
        excavator.transform.Translate(0, 0.5f, 0, excavator.transform);
    }

    // displays the results panel with the appropriate star rating and rewards player
    public void ShowResultsPanel()
    {
        calculateStarRating();
        UpdateInfoPanelContent();
        infoPanel.GetComponent<CanvasGroup>().alpha = 1f;
        infoPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        closeButton.onClick.AddListener(() => soundManager.ButtonSound());
        closeButton.onClick.AddListener(() => switchToHomeScreen());

    }

    private void switchToHomeScreen()
    {
        soundManager.ButtonSound();
        gameScene.alpha = 0f;
        gameScene.blocksRaycasts = false;
        gameScene.interactable = false;
        infoPanel.GetComponent<CanvasGroup>().alpha = 0f;
        infoPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        infoPanel.GetComponent<CanvasGroup>().interactable = false;
        MakeDisappear("Excavator");
        MakeDisappear("Speech Bubble");
        MakeDisappear("Exclamation");
        HideButton(mapButton);
        ResetContractors();
        ResetExcavator();
        ResetInfoPanel();
        closeButton.onClick.RemoveAllListeners();
        if (starRating > 0) gameController.EarnRP(starRating);
    }

    private void ResetContractors() {
        Image workerA = GameObject.Find("WorkerA").GetComponent<Image>();
        Image workerB = GameObject.Find("WorkerB").GetComponent<Image>();
        workerA.rectTransform.sizeDelta = workerASize;
        workerA.transform.position = workerAPos;
        workerB.rectTransform.sizeDelta = workerBSize;
        workerB.transform.position = workerBPos;
    }

    private void ResetExcavator() {
        Image excavator = GameObject.Find("Excavator").GetComponent<Image>();
        excavator.transform.position = excavatorPos;
    }

    private void ResetInfoPanel() {
        Text text1 = GameObject.Find("Text1").GetComponent<Text>();
        text1.text = "Place the flags on the ground to mark the pipes position so the workers won't hit them.";

        Text text2 = GameObject.Find("Text2").GetComponent<Text>();
        text2.text = "Do it fast! The excavator is right behind the workers.";

        MakeAppear("Map");
        MakeDisappear("Star1");
        MakeDisappear("Star2");
        MakeDisappear("Star3");

        Image contractor1 = GameObject.Find("Contractor1").GetComponent<Image>();
        contractor1.rectTransform.sizeDelta = panelContractor1Size;
        contractor1.transform.position = panelContractor1Pos;

        Image contractor2 = GameObject.Find("Contractor2").GetComponent<Image>();
        contractor2.rectTransform.sizeDelta = panelContractor2Size;
        contractor2.transform.position = panelContractor2Pos;

    }

    // updates the info panel's contents to result information 
    public void UpdateInfoPanelContent()
    {
        Text text1 = GameObject.Find("Text1").GetComponent<Text>();
        text1.text = starRating > 0 ? "Congratulations! You won " + starRating + " respect points!" : "Better luck next time! You didn't win any respect points.";

        Text text2 = GameObject.Find("Text2").GetComponent<Text>();
        text2.text = "";

        MakeDisappear("Map");

        Image contractor1 = GameObject.Find("Contractor1").GetComponent<Image>();
        contractor1.rectTransform.sizeDelta = new Vector2(250, 450);
        contractor1.transform.Translate(0.6f, 0.6f, 0, contractor1.transform);

        Image contractor2 = GameObject.Find("Contractor2").GetComponent<Image>();
        contractor2.rectTransform.sizeDelta = new Vector2(250, 450);
        contractor2.transform.Translate(1.4f, 0.6f, 0, contractor2.transform);

        StartCoroutine(starRatingCoroutine);
    }

    // updates the star images on result panel to correct sprites
    private IEnumerator DisplayStarRating()
    {

        Image leftStar = GameObject.Find("Star1").GetComponent<Image>();
        Image middleStar = GameObject.Find("Star2").GetComponent<Image>();
        Image righttStar = GameObject.Find("Star3").GetComponent<Image>();

        if (starRating == 3)
        {
            leftStar.sprite = yellowStar;
            middleStar.sprite = yellowStar;
            righttStar.sprite = yellowStar;
        }
        else if (starRating == 2)
        {
            leftStar.sprite = yellowStar;
            middleStar.sprite = yellowStar;
            righttStar.sprite = greyStar;
        }
        else if (starRating == 1)
        {
            leftStar.sprite = yellowStar;
            middleStar.sprite = greyStar;
            righttStar.sprite = greyStar;
        }
        else if (starRating < 1)
        {
            leftStar.sprite = greyStar;
            middleStar.sprite = greyStar;
            righttStar.sprite = greyStar;
        }
        yield return new WaitForSeconds(0.25F);
        MakeAppear("Star1");
        if (starRating > 0) soundManager.PlaySound("star");
        yield return new WaitForSeconds(0.25F);
        MakeAppear("Star2");
        if (starRating > 1) soundManager.PlaySound("star");
        yield return new WaitForSeconds(0.25F);
        MakeAppear("Star3");
        if (starRating > 2) soundManager.PlaySound("star");
    }

    // calculates the number of flags to be laid for the currentMap, sets value to numFlagsToLay
    private int calculateNumFlagsToLay()
    {
        int numFlagTiles = 0;
        for (int i = 0; i < elements.GetLength(0); i++)
        {
            for (int j = 0; j < elements.GetLength(1); j++)
            {
                if (maps[currentMap, i, j] != Color.grey)
                {
                    numFlagTiles++;
                }
            }
        }
        return numFlagTiles;
    }

    // calculates the star rating for the player 
    private void calculateStarRating()
    {
        int totalFlags = calculateNumFlagsToLay();
        int starsEarned = 3 * numCorrectFlags / totalFlags;
        starRating = starsEarned - numLineHits;
    }

    // displays the panel with BC 1 Call map for reference
    public void ShowMapPanel()
    {
        if (!tutorialCompleted)
        {
            this.HideButton(startButton);
            MakeDisappear("MiniGameBanner");
            tutorialCompleted = true;
        }
        infoPanel.GetComponent<CanvasGroup>().alpha = 1f;
        infoPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        infoPanel.GetComponent<CanvasGroup>().interactable = true;

        // add listener for map button
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() => soundManager.ButtonSound());
        closeButton.onClick.AddListener(() => HideMapPanel());
    }

    public void HideMapPanel()
    {
        infoPanel.GetComponent<CanvasGroup>().alpha = 0f;
        infoPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

        this.ShowButton(mapButton);
        mapButton.onClick.AddListener(() => soundManager.ButtonSound());
        mapButton.onClick.AddListener(() => ShowMapPanel());

        if (!startTimer)
        {
            startTimer = true;
            StartCoroutine(handDiggingCoroutine);
        }
    }

    private void shrinkContractors()
    {
        Image contractor1 = GameObject.Find("WorkerA").GetComponent<Image>();
        contractor1.rectTransform.sizeDelta = new Vector2(100, 150);
        contractor1.transform.Translate(0.2f, -0.8f, 0, contractor1.transform);


        Image contractor2 = GameObject.Find("WorkerB").GetComponent<Image>();
        contractor2.rectTransform.sizeDelta = new Vector2(100, 150);
        contractor2.transform.Translate(-0.2f, -0.8f, 0, contractor2.transform);
    }

    // increments position one to the right, or to [0, nextRow]
    private int[] IncrementMapPosition(int[] currPosition)
    {
        if (currPosition[0] < 3) return new int[] { currPosition[0]++, currPosition[1] };
        return new int[] { 0, currPosition[1]++ };
    }

    private void HideButton(Button button)
    {
        button.gameObject.SetActive(false);
    }

    private void ShowButton(Button button)
    {
        button.gameObject.SetActive(true);
    }

    private string FormatSeconds(float secondsLeft)
    {
        int secondsAsInt = (int)secondsLeft;
        string twoDigitString;
        if (secondsAsInt > 9)
        {
            twoDigitString = secondsAsInt.ToString();
        }
        else
        {
            twoDigitString = "0" + secondsAsInt.ToString();
        }
        return "00:" + twoDigitString;

    }

    // takes in an x and y coordinate of a 2D array and returns a single 1D index
    private int get1DIndexFrom2DIndex(int x, int y)
    {
        return 4 * x + y;
    }

    private void EndGame()
    {
        gameIsOver = true;
        StopCoroutine(handDiggingCoroutine);
        StopCoroutine(excavatingCoroutine);
        StopCoroutine(starRatingCoroutine);
        if (exclamationCoroutine != null) StopCoroutine(exclamationCoroutine);
        ShowResultsPanel();
    }


    // Start is called before the first frame update
    void Start()
    {
        Image workerA = GameObject.Find("WorkerA").GetComponent<Image>();
        Image workerB = GameObject.Find("WorkerB").GetComponent<Image>();
        workerASize = workerA.rectTransform.sizeDelta;
        workerAPos = workerA.transform.position;
        workerBSize = workerB.rectTransform.sizeDelta;
        workerBPos = workerB.transform.position;

        Image contractor1 = GameObject.Find("Contractor1").GetComponent<Image>();
        Image contractor2 = GameObject.Find("Contractor2").GetComponent<Image>();
        panelContractor1Size = contractor1.rectTransform.sizeDelta;
        panelContractor1Pos = contractor1.transform.position;
        panelContractor2Size = contractor2.rectTransform.sizeDelta;
        panelContractor2Pos = contractor2.transform.position;

        Image excavator = GameObject.Find("Excavator").GetComponent<Image>();
        excavatorPos = excavator.transform.position;
}

    public void StartMiniGame() {
        // game has started, but player hasn't seen initial instructions yet
        timeLeft = 21.0f;
        timer.text = "";
        startTimer = false;
        gameIsOver = false;
        tutorialCompleted = false;
        starRating = 3;
        numLineHits = 0;
        numCorrectFlags = 0;
        elements = new GridElement[4, 4];
        currentMap = Random.Range(0, 3);


        UpdateMap();
        this.ShowButton(startButton);
        MakeAppear("MiniGameBanner");
        shrinkContractors();
        MakeAppear("Excavator");

        handDiggingCoroutine = StartHandDigging();
        excavatingCoroutine = StartExcavating();
        starRatingCoroutine = DisplayStarRating();

        // hide mapButton, add listener to Start button
        this.HideButton(mapButton);
        startButton.onClick.AddListener(() => soundManager.ButtonSound());
        startButton.onClick.AddListener(() => this.ShowMapPanel());

        // generate the grid of tiles with approprite states
        GenerateGrid();
        UpdateTiles();
    }

    private void UpdateMap() {
        Image map = GameObject.Find("Map").GetComponent<Image>();
        map.sprite = mapImages[currentMap];
    }

    private void UpdateTiles()
    {
        for (int x = 0; x < elements.GetLength(0); x++)
        {
            for (int y = 0; y < elements.GetLength(1); y++)
            {
                int index = get1DIndexFrom2DIndex(x, y);
                GridElement element = elements[x, y];
                Image flag = GameObject.Find("Flag" + index).GetComponent<Image>();

                if (element.state == GridElement.State.Flagged)
                {
                    if (element.pipeUnder)
                    {
                        var tempColor = element.pipeColor;
                        tempColor.a = 1f;
                        flag.color = tempColor;
                    }
                    else
                    {
                        MakeAppear("Flag" + index);
                        incorrectFlagDelay--;
                        if (incorrectFlagDelay < 0)
                        {
                            element.state = GridElement.State.Untouched;
                            incorrectFlagDelay = 5;
                        }
                    }
                }
                else if (element.state == GridElement.State.HandDigged)
                {
                    tiles[index].sprite = hdTile;
                }
                else if (element.state == GridElement.State.Excavated)
                {
                    tiles[index].sprite = exTile;
                }
                else if (element.state == GridElement.State.HDLineStrike)
                {
                    tiles[index].sprite = hdPipeHitTile;
                }
                else if (element.state == GridElement.State.EXLineStrike)
                {
                    tiles[index].sprite = exPipeHitTile;
                }
                else
                {
                    flag.color = Color.white;
                    MakeDisappear("Flag" + index);
                    tiles[index].sprite = untouchedTile;
                }
            }
        }

    }

    private void CheckForTileTouch() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hit != null)
            {
                int tileIndex = System.Convert.ToInt32(hit.transform.name.Substring(4));
                int x = System.Convert.ToInt32(System.Math.Floor((double)tileIndex / 4));
                int y = System.Convert.ToInt32(System.Math.Floor((double)tileIndex % 4));
                soundManager.ButtonSound();
                this.Flag(elements[x, y]);
            }
        }
    }


    // Update is called once per frame
    // updates sprites for all grid elements
    void Update()
    {
        if (elements != null && (numCorrectFlags == calculateNumFlagsToLay() || timeLeft < 0) && !gameIsOver)
        {
            EndGame();
        }
        else if (startTimer)
        {
            timer.text = FormatSeconds(timeLeft);
            timeLeft -= Time.deltaTime;
            CheckForTileTouch();
            UpdateTiles();
        }
    }
}


