using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIComp : ThingComp
{
    private BehaviorComp behaviorComp;

    public AIComp(Thing thing) : base(thing) {}

    public override void OnStart()
    {
        behaviorComp = (BehaviorComp)Thing.GetComp(typeof(BehaviorComp));
    }

    public override void Tick()
    {
        base.Tick();
        if (behaviorComp.CurBehavior == null)
        {
            if (Random.Range(0f, 1f) < 0.3f)
                behaviorComp.SetBehavior(new IdleBehavior(Random.Range(60, 181)));
            else
            {
                Vector2Int? tile = ThingSystem.Instance.GetRandomEmptyTile(Thing.Pos, 20);
            if (tile == null)
                behaviorComp.SetBehavior(new IdleBehavior(Random.Range(60, 180)));
            else
                behaviorComp.SetBehavior(new MoveBehavior(Thing, Thing.Pos, tile.Value));
            }
        }
    }
}
