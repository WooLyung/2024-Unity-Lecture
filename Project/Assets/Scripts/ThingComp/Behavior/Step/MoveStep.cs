using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStep : Step
{
    private MoveComp moveComp;
    private Vector2Int from, to;

    public MoveStep(MoveComp moveComp, Vector2Int from, Vector2Int to)
    {
        this.moveComp = moveComp;
        this.from = from;
        this.to = to;
    }

    public override bool IsCanceled()
    {
        Thing t = ThingSystem.Instance.FindThing(to);
        return t != null && t != moveComp.Thing;
    }

    public override bool IsFinished()
    {
        return !moveComp.IsMoving;
    }

    public override void OnStart()
    {
        moveComp.Move(from, to);
    }
}