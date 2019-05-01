using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{

    public ContractorController contractorController;
    public ShopPanelController shopPanelController;

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

    public Sprite[] contractorSprites = new Sprite[30];

    public Sprite[] jobSprites = new Sprite[10];

    public Sprite[] jobBillboardSprites = new Sprite[10];

    public Sprite[] equipmentSprites = new Sprite[10];

    public Sprite blankSprite;

    public Sprite jobOngoingSprite;


    public Game game;


    public void EarnMoney(int money)
    {
        game.Money += Mathf.Abs(money);
        game.TotalMoneyEarned += Mathf.Abs(money);
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
    public int CalculateCost(int power, int baseCost = 2000, float cons = 1.5f)
    {
        float cost = baseCost * Mathf.Pow(cons, power);
        return Mathf.CeilToInt(cost);
    }

    // converts a given int to "short string" with length <= 5
    // ex. 10000 -> 10000, 100000 -> 100K, 1000000 -> 1M
    public string ToShortString(int n)
    {
        //if (n == null || n.GetType() != typeof(int)) return "";
        if (n < 100000) return System.Convert.ToString(n);
        if (n < 1000000) return System.Convert.ToString(System.Math.Floor((double)(n / 1000))) + "K";
        if (n < 1000000000) return System.Convert.ToString(System.Math.Floor((double)(n / 1000000))) + "M";
        return System.Convert.ToString(System.Math.Floor((double)(n / 1000000000))) + "B";
    }

    // converts a given int into a string of format hh:mm:ss
    public string FormatTime(int n)
    {
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

    // Start is called before the first frame update
    void Start()
    {
        //initialize DOTween

        //initialize game
        game = new Game
        {
            Money = 1000,
            Rp = 0,
            ContractorsAvailable = 1,
            ContractorsTotal = 1,
            ContractorsMax = 10,
            JobsCompleted = 0,
            TotalMoneyEarned = 0,
            Contractors = new List<Contractor>(),
            Jobs = new List<Job>(),
            Equipments = new List<Equipment>(),

        };

        //initialize contractors
        InitializeContractorList();
        game.Contractors[0].ContractorStatus = Contractor.Status.Hired;


        //initialize job list
        InitializeJobList();
        game.CurrentViewingJobID = 1;

        //foreach (Job j in game.Jobs) Debug.Log(j.jobID + j.jobTitle);

        //initialize equipment list
        InitializeEquipmentList();
        //foreach (Equipment e in game.Equipments) Debug.Log(e.equipmentID + e.name);
    }

    // initializes contractor list
    void InitializeContractorList() {

        contractorController.rnd = new System.Random();
        contractorController.names = new List<string>(new string[] { "Carl", "Joseph", "Joe", "Daniel", "Jude", "Jater", "Betty", "Bill", "Yangos", "Peter", "Sophie", "Mikayla", "Sam", "Dafne", "Jason", "Larry", "Richard", "Dave", "Patrick", "Jon", "Rachel", "Laura" });


        for (int i = 0; i < 15; i++)
        {
            game.Contractors.Add(contractorController.GenerateContractor(Contractor.Level.Common, i + 1));
        }

        for (int i = 15; i < 20; i++)
        {
            game.Contractors.Add(contractorController.GenerateContractor(Contractor.Level.Rare, i + 1));
            //rare, twice expensive (not discussed)
            game.Contractors[i].contractorPrice += game.Contractors[i].contractorPrice;
        }
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
                jobTitle = "Landscaping",
                contractorsReq = 1,
                timeRequired = 10, 
                HasCalled = false,
                payout_money = 1025, 
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Ready,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 2,
                jobTitle = "Hot Tub",
                contractorsReq = 2,
                timeRequired = 30,
                HasCalled = false,
                payout_money = 8000,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Ready,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 2 }, { 2, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 3,
                jobTitle = "Skate Park",
                contractorsReq = 4,
                timeRequired = 120,
                HasCalled = false,
                payout_money = 23350, 
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Ready,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 4 }, { 2, 1 }, { 4, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 4,
                jobTitle = "House",
                contractorsReq = 6,
                timeRequired = 150,
                HasCalled = false,
                payout_money = 66400, 
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Ready,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 6 }, { 2, 3 }, { 3, 1 }, { 4, 1 }, { 5, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 5,
                jobTitle = "Restaurant",
                contractorsReq = 10,
                timeRequired = 180,
                HasCalled = false,
                payout_money = 130500,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Ready,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 10 }, { 2, 5 }, { 3, 1 }, { 5, 1 }, { 6, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 6,
                jobTitle = "Small Apartment Building",
                contractorsReq = 16,
                timeRequired = 210,
                HasCalled = false,
                payout_money = 273750,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 1, 10 }, { 3, 2 }, { 5, 2 }, { 6, 1 }, { 7, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 7,
                jobTitle = "Grocery Store",
                contractorsReq = 20,
                timeRequired = 240,
                HasCalled = false,
                payout_money = 562000,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,] { { 3, 1 }, { 5, 1 }, { 6, 2 }, { 7, 2 }, { 8, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 8,
                jobTitle = "Housing Complex",
                contractorsReq = 26,
                timeRequired = 270,
                HasCalled = false,
                payout_money = 1121500,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,]  { { 3, 3 }, { 5, 3 }, { 6, 3 }, { 7, 3 }, { 8, 3 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 9,
                jobTitle = "Shopping Mall",
                contractorsReq = 32,
                timeRequired = 300, 
                HasCalled = false,
                payout_money = 2081000,
                payout_exp = 100, // TODO: decide on exp payout for job
                JobStatus = Job.Status.Locked,
                TimeLastStarted = 0f,
                TimeLastFinished = 0f,
                EquipmentsRequired = new int[,]  { { 6, 4 }, { 7, 4 }, { 8, 4 }, { 9, 1 }, { 10, 1 }, }, // {EquipmentID, NumRequired}
            },

            new Job
            {
                jobID = 10,
                jobTitle = "Robot Manufacturing Base",
                contractorsReq = 40,
                timeRequired = 330,
                HasCalled = false,
                payout_money = 7036500,
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
                name = "Small Cement Mixer",
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
                name = "Mini Excavator",
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
                name = "Large Excavator",
                basecost = baseCostEquip6,
                EquipmentStatus = Equipment.Status.Available,
                NumberOwned = 0,
                NumberAvailable = 0,
            },

            new Equipment
            {
                equipmentID = 7,
                name = "Large Cement Mixer",
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

    }



}