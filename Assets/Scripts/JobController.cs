using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JobController : MonoBehaviour
{

    // reference to the game controller
    public GameController gameController;
    public JobDetailPanelController jobDetailPanelController;
    public WorkSiteAnimationController workSiteAnimationController;
    public SoundManagerScript soundManager;
    public bool timeForMiniGame;

    public ParticleSystem confettiPS;


    /*
    // public method to update job status
    public void UpdateJobStatus(int jobID, Job.Status status)
    {
        gameController.game.Jobs[jobID - 1].JobStatus = status;
    }

    public void UpdateJobStartTime(int jobID, int time)
    {
        gameController.game.Jobs[jobID - 1].TimeLastStarted = time;
    }

    public void UpdateJobFinishTime(int jobID, int time)
    {
        gameController.game.Jobs[jobID - 1].TimeLastFinished = time;
    }

    public Job GetJob(int jobID) {
        return gameController.game.Jobs[jobID - 1];
    }
    */

    //start the job if it's ready and there is enough equippments
    //change job status, mark time stamps
    public bool StartJob (int jobID) {

        Job j = gameController.game.Jobs.Find(x => x.jobID == jobID);
        if (j.JobStatus != Job.Status.Ready) return false;
        if (!HasRequiredEquipments(j)) return false;

        // change job status
        j.JobStatus = Job.Status.Ongoing;
        j.TimeLastStarted = Time.time;
        j.TimeLastFinished = Time.time + j.timeRequired;

        gameController.game.ContractorsAvailable -= j.contractorsReq;

        // check out the required equipments
        for (int x = 0; x < j.EquipmentsRequired.GetLength(0); x++)
        {
            int id = j.EquipmentsRequired[x, 0];
            int num = j.EquipmentsRequired[x, 1];

            Equipment equip = gameController.game.Equipments.Find(e => e.equipmentID == id);

            equip.NumberAvailable -= num;
        }
        // update job detail panel
        jobDetailPanelController.DisplayContent();
        //jobDetailPanelController.DisplaySwipeBar();
        //PlayJobSounds();
        Debug.Log("Started Job: " + j.jobTitle);
        return true;
    }

    // iterates all jobs and checks their status
    IEnumerator CheckJobs()
    {
        while(true) {
            yield return new WaitForSeconds(0.5f);
            CheckJobStatus();
        }

    }

    public int InstantFinishPrice(int jobID) {
        float t = gameController.game.Jobs[jobID - 1].TimeLastFinished - Time.time;
        if (t < 0f) return 0;
        return (int)((t-20)/30+2); //HACK: I just made these up.
    }


    public void InstantFinish(int jobID) {
        //Debug.Log("finishing...");
        if(gameController.SpendRp(InstantFinishPrice(jobID))) {
            gameController.game.Jobs[jobID - 1].TimeLastFinished = Time.time;

            if (jobDetailPanelController.waitingForSwipe)
            {
                jobDetailPanelController.waitingForSwipe = false;
                jobDetailPanelController.swipeBar.GetComponent<CanvasGroup>().alpha = 0f;
                if (jobDetailPanelController.swipeDetectionCoroutine != null) jobDetailPanelController.StopCoroutine(jobDetailPanelController.swipeDetectionCoroutine);
            }
        }

    }


    // return true if given Job has the required number and type of equipment
    public bool HasRequiredEquipments(Job j) {
        bool res = true;
        res = res && j.contractorsReq <= gameController.game.ContractorsAvailable;

        for (int x = 0; x < j.EquipmentsRequired.GetLength(0); x++)
        {
            int id = j.EquipmentsRequired[x, 0];
            int num = j.EquipmentsRequired[x, 1];

            Equipment equip = gameController.game.Equipments.Find(e => e.equipmentID == id);

            res = res && num <= equip.NumberAvailable;
        }
        //Debug.Log(res.ToString());
        return res;
    }


        //check to see if any job has just finished 
        //if so, free the occupied equipments and earn money
        private void CheckJobStatus () {
        foreach (Job j in gameController.game.Jobs) {
            if (j.JobStatus == Job.Status.Ongoing && j.TimeLastFinished <= Time.time) {
                j.JobStatus = Job.Status.Ready;

                if (!j.HasBeenBuilt) j.HasBeenBuilt = true;

                gameController.game.ContractorsAvailable += j.contractorsReq;

                // return equipments
                for (int x = 0; x < j.EquipmentsRequired.GetLength(0); x++)
                {
                    int id = j.EquipmentsRequired[x, 0];
                    int num = j.EquipmentsRequired[x, 1];

                    Equipment equip = gameController.game.Equipments.Find(e => e.equipmentID == id);

                    equip.NumberAvailable += num;
                }
                Debug.Log("Finished job: " + j.jobTitle);

                // payout calculation
                gameController.EarnMoney(j.payout_money);
                gameController.EarnExperience(j.payout_exp);
                gameController.game.JobsCompleted += 1;

                // log
                gameController.LogToFile(
                    gameController.FormatTime((int)Time.time) + 
                    " Job " + j.jobID.ToString() + " finished." +
                    " Jobs done: " + gameController.game.JobsCompleted +
                    " Money earned: " + gameController.game.TotalMoneyEarned +
                    " Money left: " + gameController.game.Money +
                    " Taps: " + gameController.game.TotalTaps +
                    " Swiped: " + gameController.game.Rp + //change this later
                    " Missed: " + gameController.game.SwipesMissed +
                    " Workers hired: " + gameController.game.ContractorsTotal +
                    " Equipments owned: " +
                    gameController.game.Equipments[0].NumberOwned + " / " +
                    gameController.game.Equipments[1].NumberOwned + " / " +
                    gameController.game.Equipments[2].NumberOwned + " / " +
                    gameController.game.Equipments[3].NumberOwned + " / " +
                    gameController.game.Equipments[4].NumberOwned);

                // update job detail panel
                jobDetailPanelController.DisplayContent();

                //update animation

                workSiteAnimationController.UpdateWorkSiteAnimation();

                confettiPS.Play(true);

                if (jobDetailPanelController.waitingForSwipe)
                {
                    jobDetailPanelController.waitingForSwipe = false;
                    jobDetailPanelController.swipeBar.GetComponent<CanvasGroup>().alpha = 0f;
                    if (jobDetailPanelController.swipeDetectionCoroutine != null) jobDetailPanelController.StopCoroutine(jobDetailPanelController.swipeDetectionCoroutine);
                }

                // unlock new jobs
                if (gameController.game.JobsCompleted % 2 == 0 && gameController.game.UnlockedJobsNumber < gameController.game.Jobs.Count) {
                    gameController.game.Jobs[gameController.game.UnlockedJobsNumber].JobStatus = Job.Status.Ready;
                    gameController.game.UnlockedJobsNumber += 1;
                }

                soundManager.PlaySound("job completed");

                /*todo: change mini game trigger to mole
                if (!timeForMiniGame) {
                    timeForMiniGame = true;
                } else {
                    SceneManager.LoadSceneAsync("MSMiniGame", LoadSceneMode.Additive); //todo: decide on the chance of showing this
                    timeForMiniGame = false;
                }  */
            }
        }
    }

    // plays sounds for jobs (deprecated)
    /*
    public void PlayJobSounds()
    {
        foreach (Job j in gameController.game.Jobs)
        {
            if (j.JobStatus == Job.Status.Ongoing)
            {
                soundManager.PlaySound("working");
                return;
            }
        }
        soundManager.StopSound("working");
    }


    // sets the status of a given job to "has called BC 1 Call"
    public void makeCall(Job j)
    {
        j.HasCalled = true;
    }
    */


    public void Start()
    {
        StartCoroutine(CheckJobs());
    }

}
