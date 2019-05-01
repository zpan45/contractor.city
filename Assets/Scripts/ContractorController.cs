using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractorController : MonoBehaviour
{
    public List<string> names;
    public List<Skill> quirks;
    public List<Skill> skillset;
    public System.Random rnd;
    public GameController gameController;
    public ShopPanelController shopPanelController;
    public SoundManagerScript soundManager;

    // generates new contractor with given level and ID
    public Contractor GenerateContractor(Contractor.Category cat, int ID)
    {
        //Debug.Log(skillset.Count + " " + quirks.Count);

        string newName = names[rnd.Next(names.Count)];
        names.Remove(newName);
        Contractor newContractor = new Contractor
        {
            contractorCategory = cat,
            ContractorStatus = Contractor.Status.Unlocked,
            contractorName = newName,
            contractorPrice = CalculateCost(ID - 1),
            contractorID = ID,
            Skills = new List<Skill>(),
        };

        if (cat != Contractor.Category.Special)
        {
            int num = rnd.Next(quirks.Count);
            newContractor.Skills.Add(quirks[num]);
            quirks.Remove(quirks[num]);
        }
        else newContractor.Skills.Add(skillset[2].NewSkill());

        if (cat != Contractor.Category.Common) newContractor.Skills.Add(skillset[rnd.Next(skillset.Count - 1)].NewSkill());



        //Debug.Log(newContractor.contractorName + newContractor.contractorID + newContractor.contractorCategory.ToString());
        return newContractor;
    }

    public void UpgradeContractor(Contractor c) {
        int price = CalculateCost(c.contractorLevel - 1, 1000, 1.2f);
        if (gameController.SpendMoney(price)) {

            //if (c.contractorCategory == Contractor.Category.Common && c.Skills.Count > 1) c.Skills[1] = quirks[rnd.Next(quirks.Count)];
            //else c.Skills.Add(quirks[rnd.Next(quirks.Count)]);

            c.contractorLevel += 1;
            foreach (Skill s in c.Skills)
            s.SkillValue *= 1.1f;
            soundManager.PlaySound("upgrade");
        }
    }


    // purchases a contractor with a given ID 
    public void BuyContractor(int id)
    {
        Contractor currentContractor = gameController.game.Contractors.Find(x => x.contractorID == id);
        if (gameController.SpendMoney(currentContractor.contractorPrice))
        {
            currentContractor.ContractorStatus = Contractor.Status.Hired;
            gameController.game.ContractorsAvailable += 1;
            gameController.game.ContractorsTotal += 1;
        }
        shopPanelController.DisplayContractorContent();
    }

    //helper method to calculate equipment or worker cost
    //cost = baseCost * constant ^ (numOwned)
    public int CalculateCost(int power, int baseCost = 2000, float cons = 1.05f)
    {
        float cost = baseCost * Mathf.Pow(cons, power);
        return Mathf.CeilToInt(cost);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
