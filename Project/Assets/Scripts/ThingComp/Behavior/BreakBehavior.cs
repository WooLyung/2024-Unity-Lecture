using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BreakBehavior : Behavior
{
    private MoveComp moveComp;
    private Vector2Int from, to;

    public BreakBehavior(Thing thing, Vector2Int from, Vector2Int to)
    {
        moveComp = (MoveComp)thing.GetComp(typeof(MoveComp));
        this.from = from;
        this.to = to;
    }

    public override void InitSteps()
    {
        steps = new();
        List<Vector2Int> path;
        if (ThingSystem.Instance.PathFindNeighbor(moveComp.Thing.Pos, to, out path, 200))
        {
            for (int i = 0; i < path.Count - 1; i++)
                steps.Add(new MoveStep(moveComp, path[i], path[i + 1]));
            steps.Add(new WaitStep(120));
        }
    }

    public override void OnFinish()
    {
        Thing thing = ThingSystem.Instance.FindThing(to);
        if (thing != null)
            ThingSystem.Instance.DestroyThing(thing);
    }
}