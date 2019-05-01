using UnityEngine;

public class GridElement
{
    public enum State { Untouched = 0, Flagged = 1, HandDigged = 2, Excavated = 3, HDLineStrike = 4, EXLineStrike = 5 }

    public State state              { set; get; }
    public bool pipeUnder           { set; get; }
    public Color pipeColor          { set; get; }
    public GameObject gameObject    { set; get; }
}
