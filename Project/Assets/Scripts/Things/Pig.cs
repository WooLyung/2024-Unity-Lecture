using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : NonPlayer
{
    public override string Name => "Pig";

    public override void OnInstantiate()
    {
        base.OnInstantiate();
        AddComp(new MoveComp(this, 30));
    }

    protected override void InitState()
    {
        Vector2Int? dest = ThingSystem.Instance.GetRandomEmptyTile(Pos, 30);
        if (dest == null || Random.Range(0, 1f) < 0.3f)
        {
            curState = 0; // idle
            BehaviorComp.SetBehavior(new IdleBehavior(Random.Range(30, 180)));
        }
        else
        {
            List<Vector2Int> path;
            if (ThingSystem.Instance.PathFind(Pos, dest.Value, out path))
            {
                curState = 1; // move
                BehaviorComp.SetBehavior(new MoveBehavior(this, Pos, dest.Value));
            }
            else
            {
                curState = 0; // idle
                BehaviorComp.SetBehavior(new IdleBehavior(Random.Range(30, 180)));
            }
        }
    }

    protected override void NextState()
    {
        if (curState == 1)
        {
            curState = 0; // idle
            BehaviorComp.SetBehavior(new IdleBehavior(Random.Range(30, 180)));
        }
        else
            InitState();
    }
}