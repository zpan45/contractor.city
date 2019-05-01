using System.Collections.Generic;

public class Game
{

    //game properties
    //money
    public int Money                    { get; set; }
    public int Rp                       { get; set; }
    public int Experience               { get; set; }

    //equipments
    public int ContractorsAvailable     { get; set; }
    public int ContractorsTotal         { get; set; }
    public int ContractorsMax           { get; set; }

    public int ShovelsAvailable         { get; set; }
    public int ShovelsTotal             { get; set; }
    public int ShovelsMax               { get; set; }

    public int BackhoesAvailable        { get; set; }
    public int BackhoesTotal            { get; set; }
    public int BackhoesMax              { get; set; }

    public int SmExcavatorsAvailable    { get; set; }
    public int SmExcavatorsTotal        { get; set; }
    public int SmExcavatorsMax          { get; set; }

    public int LgExcavatorsAvailable    { get; set; }
    public int LgExcavatorsTotal        { get; set; }
    public int LgExcavatorsMax          { get; set; }

    //stats
    public int JobsCompleted            { get; set; }
    public int TotalMoneyEarned         { get; set; }
    public int SwipesMissed             { get; set; }
    public int TotalTaps                { get; set; }

    //contractors
    public List<Contractor> Contractors { get; set; }

    //jobs
    public List<Job> Jobs               { get; set; }
    public int UnlockedJobsNumber       { get; set; }

    //equipments
    public List<Equipment> Equipments   { get; set; }

    public int CurrentViewingJobID      { get; set; }
}
