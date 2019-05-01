
using System;
using System.Collections.Generic;

public class Contractor
{
    // define contractor level/status
    public enum Category { Common = 0, Rare = 1, Special = 2 }
    public enum Status { Locked = 0, Unlocked = 1, Hired = 2 }

    // contractor properties
    public int contractorID;
    public string contractorName;
    public int contractorPrice;
    public int contractorLevel = 1;
    public Category contractorCategory;
    public List<Skill> Skills { get; set; }
    public Status ContractorStatus { get; set; }

}


//contractor skill class, should contain skillID, text description, skill category (speed up/income boost/etc. needs to be defined)
public class Skill
{
    public enum Category { Quirk = 0, IncomeBoost = 1, SpeedBoost = 2, StrikeReduction = 3}

    public Category skillCategory;
    public float SkillValue = 0f;
    public string skillText;

    public override string ToString() {
        if (this.skillCategory != Skill.Category.Quirk) return this.skillText + String.Format("{0:0.00%}", this.SkillValue);
        else return this.skillText;
    }

    public Skill NewSkill() {
        Skill res = new Skill();
        res.skillCategory = this.skillCategory;
        res.skillText = this.skillText;
        res.SkillValue = this.SkillValue;
        return res;
    }

}