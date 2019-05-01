public class Equipment
{
    //define quipment status code
    public enum Status { Locked = 0, Available = 1 }

    //equipment properties
    public int equipmentID;
    public string name;
    public string size;
    public int basecost;
    public Status EquipmentStatus   { set; get; }
    public int NumberOwned          { get; set; }
    public int NumberAvailable      { get; set; }

}
