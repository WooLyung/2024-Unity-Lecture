using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBehavior : Behavior
{
    private MoveComp moveComp;
    private Vector2Int from, to;

    public MoveBehavior(Thing thing, Vector2Int from, Vector2Int to)
    {
        moveComp = (MoveComp)thing.GetComp(typeof(MoveComp));
        this.from = from;
        this.to = to;
    }

    public override void InitSteps()
    {
        //List<Vector2Int> path;
        //bool result = ThingSystem.Instance.PathFind(from, to, out path);
        //if (result)
        //{
        //    foreach (Vector2Int tile in path)
        //        Debug.Log(tile);
        //}
    }
}