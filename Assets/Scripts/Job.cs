

public class Job 
{
    //define job status code
    public enum Status { Locked = 0, Ready = 1, Ongoing = 2 }

    //job properties
    public int jobID;
    public string jobTitle;
    public int contractorsReq;

    //equip stat (depreciated)
    public int shovelsReq;
    public int backhoesReq;
    public int smExcavatorsReq;
    public int lgExcavatorsReq;

    //payouts
    public int payout_money;
    public int payout_exp;

    public int timeRequired;

    public Status JobStatus             { set; get; }
    public float TimeLastStarted        { get; set; }
    public float TimeLastFinished       { get; set; }

    public bool HasCalled               { get; set; }

    public bool HasBeenBuilt            { get; set; }

    public int[,] EquipmentsRequired    { get; set; }
}
