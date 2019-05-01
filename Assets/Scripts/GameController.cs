using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public ContractorController contractorController;
    public ShopPanelController shopPanelController;
    public JobDetailPanelController jobDetailPanelController;
    public JobController jobController;
    public WorkSiteAnimationController workSiteAnimationController;
    public SoundManagerScript soundManager;
    public LoadScreenController loadScreenController;
    public WelcomePanelController welcomePanelController;

    public Text earnedMoney;

    public static float constant = 1.5f;

    public int baseCostContractor = 500;
    public int baseCostEquip1 = 50;       // shovel
    public int baseCostEquip2 = 500;      // sm cement mixer
    public int baseCostEquip3 = 20000;    // skid steer
    public int baseCostEquip4 = 30000;    // mini excavator
    public int baseCostEquip5 = 60000;    // back hoe
    public int baseCostEquip6 = 150000;   // lg excavator
    public int baseCostEquip7 = 200000;   // lg cement mixer
    public int baseCostEquip8 = 300000;   // dump truck
    public int baseCostEquip9 = 500000;   // flat bed truck
    public int baseCostEquip10 = 1000000; // crane

    public int scifiRequiredLevel = 10;

    public Sprite[] contractorSprites = new Sprite[30];

    public Sprite[] scifiContractorSprites = new Sprite[30];

    public Sprite[] jobSprites = new Sprite[10];

    public Sprite[] jobBillboardSprites = new Sprite[10];

    public Sprite[] equipmentSprites = new Sprite[10];

    public Sprite blankSprite;

    public Sprite arrowSprite;

    public ParticleSystem moneyPS;

    public ParticleSystem rpPS;


    private Tweener moneyEarnedTweener;


    public Game game;


    public void EarnMoney(int money)
    {
        game.Money += Mathf.Abs(money);
        game.TotalMoneyEarned += Mathf.Abs(money);
        StartCoroutine(DisplayEarnedMoney(money));
        soundManager.PlaySound("earning money");
        moneyPS.Play();
    }

    IEnumerator DisplayEarnedMoney(int i)
    {
        earnedMoney.color = new Color32(0x28, 0x46, 0x13, 0xff);
        earnedMoney.text = "+ " + ToShortString(i);
        if (moneyEarnedTweener != null) moneyEarnedTweener.Kill();
        moneyEarnedTweener = earnedMoney.transform.DOLocalMoveY(-1000f, 3).From(true);
        //earnedMoney.transform.DOScale(3.0f, 3);
        yield return new WaitForSeconds(5.0f);
        earnedMoney.text = "";
    }

    IEnumerator DisplayEarnedRP(int i)
    {
        earnedMoney.color = new Color32(0x66, 0x4f, 0x10, 0xff);
        earnedMoney.text = "RP + " + ToShortString(i);
        if (moneyEarnedTweener != null) moneyEarnedTweener.Kill();
        moneyEarnedTweener = earnedMoney.transform.DOLocalMoveY(-1000f, 3).From(true);
        //earnedMoney.transform.DOScale(3.0f, 3);
        yield return new WaitForSeconds(5.0f);
        earnedMoney.text = "";
    }



    public void LogToFile(string line) {
        //Debug.Log(line);
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/testlog.txt", true);
        writer.WriteLine(line + "\n");
        writer.Flush();
        writer.Dispose();
    }


    public void EarnExperience(int exp)
    {
        game.Experience += Mathf.Abs(exp);
    }


    public bool SpendMoney(int money)
    {
        if (game.Money < money) return false;
        game.Money -= Mathf.Abs(money);
        return true;
    }

    public void EarnRP(int i)
    {
        game.Rp += Mathf.Abs(i);
        StartCoroutine(DisplayEarnedRP(i));
        rpPS.Play();
    }

    public bool SpendRp(int rp)
    {
        if (game.Rp < rp) return false;
        game.Rp -= Mathf.Abs(rp);
        return true;
    }

    /*
    //calculate cost and check if user has money and room to hire one more worker
    //if not, return false; if ture, deduct money and add one more worker
    public bool HireWorker()
    {
        int cost = CalculateCost(baseWorkerCost, constant, game.ContractorsTotal - 1);
        if (game.Money < cost || game.ContractorsTotal == game.ContractorsMax) return false;
        SpendMoney(cost);
        game.ContractorsAvailable += 1;
        game.ContractorsTotal += 1;
        return true;
    }

    //calculate cost and check if user has money and room to buy a shovel
    //if not, return false; if ture, deduct money and add one more shovel
    public bool BuyShovel()
    {
        int cost = CalculateCost(baseShovelCost, constant, game.ShovelsTotal - 1);
        if (game.Money < cost || game.ShovelsTotal == game.ShovelsMax) return false;
        SpendMoney(cost);
        game.ShovelsAvailable += 1;
        game.ShovelsTotal += 1;
        return true;
    }

    //calculate cost and check if user has money and room to buy a backhoe
    //if not, return false; if ture, deduct money and add one more backhoe
    public bool BuyBackhoe()
    {
        int cost = CalculateCost(baseBackhoeCost, constant, game.BackhoesTotal - 1);
        if (game.Money < cost || game.BackhoesTotal == game.BackhoesMax) return false;
        SpendMoney(cost);
        game.BackhoesAvailable += 1;
        game.BackhoesTotal += 1;
        return true;
    }

    //calculate cost and check if user has money and room to buy a small excavator
    //if not, return false; if ture, deduct money and add one more small excavator
    public bool BuySmExcavator()
    {
        int cost = CalculateCost(baseSmExcavatorCost, constant, game.SmExcavatorsTotal - 1);
        if (game.Money < cost || game.SmExcavatorsTotal == game.SmExcavatorsMax) return false;
        SpendMoney(cost);
        game.SmExcavatorsAvailable += 1;
        game.SmExcavatorsTotal += 1;
        return true;
    }

    //calculate cost and check if user has money and room to buy a large excavator
    //if not, return false; if ture, deduct money and add one more large excavator
    public bool BuyLgExcavator()
    {
        int cost = CalculateCost(baseLgExcavatorCost, constant, game.LgExcavatorsTotal - 1);
        if (game.Money < cost || game.LgExcavatorsTotal == game.LgExcavatorsMax) return false;
        SpendMoney(cost);
        game.LgExcavatorsAvailable += 1;
        game.LgExcavatorsTotal += 1;
        return true;
    }*/
    public void BuyEquipment(int id, int price)
    {
        Equipment currentEquipment = game.Equipments.Find(x => x.equipmentID == id);
        if (SpendMoney(price))
        {
            currentEquipment.NumberOwned += 1;
            currentEquipment.NumberAvailable += 1;
        }
        shopPanelController.DisplayEquipmentContent();
    }

    //helper method to calculate equipment or worker cost
    //cost = baseCost * constant ^ (numOwned)
    public int CalculateCost(int power, int baseCost = 2000, float cons = 1.2f)
    {
        float cost = baseCost * Mathf.Pow(cons, power);
        return Mathf.CeilToInt(cost);
    }

    // converts a given int to "short string" with length <= 7
    // ex. 1000 -> 1K, 1001 -> 1K, 999990 -> 999.99K, 101499 -> 1.01M
    public string ToShortString(int n)
    {
        if (n < 1000) return System.Convert.ToString(n);
        if (n < 1000000) return System.Convert.ToString(System.Math.Round((double)n / 1000, 2)) + "K";
        if (n < 1000000000) return System.Convert.ToString(System.Math.Round((double)n / 1000000, 2)) + "M";
        return System.Convert.ToString(System.Math.Round((double)n / 1000000000, 2)) + "B";
    }

    // converts a given int into a string of format hh:mm:ss
    public string FormatTime(int n)
    {
        if (n <= 0) return "00:00:00";
        int numHours = (int)System.Math.Floor((double)(n / 3600));
        int extraSeconds = n % 3600;
        int numMins = (int)System.Math.Floor((double)(extraSeconds / 60));
        int numSeconds = extraSeconds % 60;
        return ToTwoDigitString(numHours) + ":" + ToTwoDigitString(numMins) + ":" + ToTwoDigitString(numSeconds);
    }

    // converts a given int (num digits <= 2) to a two digit string
    // ASSUMES: n is < 100
    public static string ToTwoDigitString(int n)
    {
        if (n < 10) return "0" + System.Convert.ToString(n);
        return System.Convert.ToString(n);
    }

    //helper method to calculate equipment or worker cost
    //cost = baseCost * constant ^ (numOwned - 1)
    public int CalculateCost(int baseCost, float cons, int power)
    {
        float cost = baseCost * Mathf.Pow(cons, power);
        return Mathf.CeilToInt(cost);
    }


    public void ForceResolution()
    {
        if (Screen.width != 675)
        {
            Screen.SetResolution(675, 1200, false);
            // set to true if you want a fullscreen game
        }
    }

    public void SpeedUpJob() {
        game.TotalTaps += 1;
        if (game.Jobs[game.CurrentViewingJobID - 1].JobStatus == Job.Status.Ongoing) game.Jobs[game.CurrentViewingJobID - 1].TimeLastFinished -= 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        //initialize log
        LogToFile("////////////SESSION STARTED////////////\t\t" + DateTime.Now.ToString());

        //ForceResolution();

        // show loading screen
        loadScreenController.ShowPanel();
        welcomePanelController.ShowPanel();

        //initialize game
        game = new Game
        {
            Money = 100000,
            Rp = 10,
            ContractorsAvailable = 1,
            ContractorsTotal = 1,
            ContractorsMax = 10,
            JobsCompleted = 0,
            TotalMoneyEarned = 0,
            SwipesMissed = 0,
            TotalTaps = 0,
            Contractors = new List<Contractor>(),
            Jobs = new List<Job>(),
            UnlockedJobsNumber = 1,
            CurrentViewingJobID = 1,
            Equipments = new List<Equipment>(),

        };

        //initialize contractors
        InitializeContractorList();
        game.Contractors[0].ContractorStatus = Contractor.Status.Hired;


        //initialize job list
        InitializeJobList();

        //foreach (Job j in game.Jobs) Debug.Log(j.jobID + j.jobTitle);

        //initialize equipment list
        InitializeEquipmentList();
        //foreach (Equipment e in game.Equipments) Debug.Log(e.equipmentID + e.name);

        workSiteAnimationController.UpdateWorkSiteAnimation();

        StartCoroutine(jobDetailPanelController.RefreshContent());

        jobController.timeForMiniGame = false;
    }

    void InitializeSkillset() {
        contractorController.skillset = new List<Skill>{
            new Skill
            {
                skillCategory = Skill.Category.IncomeBoost,
                SkillValue = 0.05f,
                skillText = "Boost income by "
            },

            new Skill
            {
                skillCategory = Skill.Category.SpeedBoost,
                SkillValue = 0.02f,
                skillText = "Speed up jobs by "
            },

            new Skill
            {
                skillCategory = Skill.Category.StrikeReduction,
                SkillValue = 0.01f,
                skillText = "Reduce strike rate by "
            },
        };
        //Debug.Log(contractorController.skillset.Count);
    }

    void InitializeQuirks()
    {
        contractorController.quirks = new List<Skill>{
            new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Roller derbiest."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Really loves their kids."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Still lives at nan’s house."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Wants to climb Mt. Everest."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Always talking about dogs."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Always wanted to go to Rome."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "REALLY religious. Really."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Craft beer lover."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Wine snob."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Talks in their sleep."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Member of the 501st."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Benches 240lbs."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Reigning dodgeball champion."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Still has their babyteeth."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Convinced they’re in a game."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Annoyingly cheerful."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Always shares their snacks."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Has a cold that never goes away."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Always brings coffee and donuts."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Rides a Harley to work."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Saving up for a house."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Hotdog eating champion."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Loves hamburgers."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Thinks Trump's smart."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Hates Justin Bieber."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Uses BC One Call."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Cheats at poker."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Lover not a fighter."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Arachnophobe."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Afraid of heights."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Believes in aliens."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Flatearther."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Autistic and awesome!"             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "Fighter not a lover."             },              new Skill             {                 skillCategory = Skill.Category.Quirk,                 skillText = "E-Sports champion."             }, 
        };
    }



    // initializes contractor list
    void InitializeContractorList() {

        contractorController.rnd = new System.Random();
        contractorController.names = new List<string>(new string[] { "Carl", "Joseph", "Joe", "Daniel", "Jude", "Jater", "Betty", "Bill", "Yangos", "Peter", "Sophie", "Mikayla", "Sam", "Dafne", "Jason", "Larry", "Richard", "Dave", "Patrick", "Jon", "Rachel", "Laura", "Wayland", "Steve", "Jack", "Dickie", "Jackie", "Emily", "Lucy", "Katie", "Daisy", "Harry", "Ethan", "Max", "Luke", "Matthew" });


        InitializeSkillset();
        InitializeQuirks();


        for (int i = 0; i < 18; i++)
        {
            game.Contractors.Add(contractorController.GenerateContractor(Contractor.Category.Common, i + 1));
        }

        for (int i = 18; i < 30; i++)
        {
            game.Contractors.Add(contractorController.GenerateContractor(Contractor.Category.Rare, i + 1));
            //rare, twice expensive (not discussed)
            game.Contractors[i].contractorPrice += game.Contractors[i].contractorPrice;
        }

        /*
        for (int i = 20; i < 25; i++)
        {
            game.Contractors.Add(contractorController.GenerateContractor(Contractor.Category.Special, i + 1));
            //special, four times expensive (not discussed)
            game.Contractors[i].contractorPrice *= 4;
        }
        */

    }

    // initializes job list
    void InitializeJobList()
    {
        game.Jobs = new List<Job>
        {
            // Landscaping job - 
            new Job
            {
                jobID = 1,
                jobTitle = "Do Landscaping",
                contractorsReq = 1,
                timeRequired = 10,
                HasCalled = false,
                payout_money = 1025,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Ready,
                HasBeenBuilt = false,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 2,
                jobTitle = "Build Hot Tub",
                contractorsReq = 2,
                timeRequired = 30,
                HasCalled = false,
                payout_money = 8000,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 2 }, { 2, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 3,
                jobTitle = "Build Skate Park",
                contractorsReq = 4,
                timeRequired = 120,
                HasCalled = false,
                payout_money = 23350, 
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 4 }, { 2, 1 }, { 4, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 4,
                jobTitle = "Build House",
                contractorsReq = 6,
                timeRequired = 150,
                HasCalled = false,
                payout_money = 66400, 
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 6 }, { 2, 3 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 5,
                jobTitle = "Build Restaurant",
                contractorsReq = 8,
                timeRequired = 180,
                HasCalled = false,
                payout_money = 84500,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 10 }, { 2, 5 }, { 3, 1 }, { 4, 2 }, { 5, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 6,
                jobTitle = "Build Small Apartment Building",
                contractorsReq = 10,
                timeRequired = 210,
                HasCalled = false,
                payout_money = 270750,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 10 }, { 3, 2 }, { 5, 2 }, { 6, 1 }, { 7, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 7,
                jobTitle = "Build Grocery Store",
                contractorsReq = 13,
                timeRequired = 240,
                HasCalled = false,
                payout_money = 558500,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 3, 1 }, { 5, 1 }, { 6, 2 }, { 7, 2 }, { 8, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 8,
                jobTitle = "Build Housing Complex",
                contractorsReq = 16,
                timeRequired = 270,
                HasCalled = false,
                payout_money = 1116500,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,]  { { 3, 3 }, { 5, 3 }, { 6, 3 }, { 7, 3 }, { 8, 3 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 9,
                jobTitle = "Build Shopping Mall",
                contractorsReq = 20,
                timeRequired = 300, 
                HasCalled = false,
                payout_money = 2075000,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,]  { { 6, 4 }, { 7, 4 }, { 8, 4 }, { 9, 1 }, { 10, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 10,
                jobTitle = "Build Drone Factory",
                contractorsReq = 30,
                timeRequired = 330,
                HasCalled = false,
                payout_money = 7031500,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 6, 10 }, { 7, 10 }, { 8, 10 }, { 9, 5 }, { 10, 5 }, }, // {EquipmentID, NumRequired}
            },

        };
    }

    // initializes equipment list
    void InitializeEquipmentList()
    {
        game.Equipments = new List<Equipment>
        {
            new Equipment
            {
                equipmentID = 1,
                name = "Shovel",
                basecost = baseCostEquip1,
                EquipmentStatus = Equipment.Status.Available,
                NumberOwned = 1,
                NumberAvailable = 1,
            },

            new Equipment
            {
                equipmentID = 2,
                name = "Cement Mixer",
                size = "Small",
                basecost = baseCostEquip2,
                EquipmentStatus = Equipment.Status.Available,
                NumberOwned = 0,
                NumberAvailable = 0,
            },

            new Equipment
            {
                equipmentID = 3,
                name = "Skid Steer",
                basecost = baseCostEquip3,
                EquipmentStatus = Equipment.Status.Available,
                NumberOwned = 0,
                NumberAvailable = 0,
            },

            new Equipment
            {
                equipmentID = 4,
                name = "Excavator",
                size = "Small",
                basecost = baseCostEquip4,
                EquipmentStatus = Equipment.Status.Available,
                NumberOwned = 0,
                NumberAvailable = 0,
            },

            new Equipment
            {
                equipmentID = 5,
                name = "Back Hoe",
                basecost = baseCostEquip5,
                EquipmentStatus = Equipment.Status.Available,
                NumberOwned = 0,
                NumberAvailable = 0,
            },

            new Equipment
            {
                equipmentID = 6,
                name = "Excavator",
                size = "Large",
                basecost = baseCostEquip6,
                EquipmentStatus = Equipment.Status.Available,
                NumberOwned = 0,
                NumberAvailable = 0,
            },

            new Equipment
            {
                equipmentID = 7,
                name = "Cement Mixer",
                size = "Large",
                basecost = baseCostEquip7,
                EquipmentStatus = Equipment.Status.Available,
                NumberOwned = 0,
                NumberAvailable = 0,
            },

            new Equipment
            {
                equipmentID = 8,
                name = "Dump Truck",
                basecost = baseCostEquip8,
                EquipmentStatus = Equipment.Status.Available,
                NumberOwned = 0,
                NumberAvailable = 0,
            },

            new Equipment
            {
                equipmentID = 9,
                name = "Flat Bed Truck",
                basecost = baseCostEquip9,
                EquipmentStatus = Equipment.Status.Available,
                NumberOwned = 0,
                NumberAvailable = 0,
            },

            new Equipment
            {
                equipmentID = 10,
                name = "Crane",
                basecost = baseCostEquip10,
                EquipmentStatus = Equipment.Status.Available,
                NumberOwned = 0,
                NumberAvailable = 0,
            },
        };
    }


    // Update is called once per frame
    void Update()
    {
        //ForceResolution();
    }



}