using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorComp : ThingComp
{
    public Behavior CurBehavior { get; private set; } = null;
    private Behavior nowBehavior;

    public BehaviorComp(Thing thing) : base(thing) { }

    public void SetBehavior(Behavior behavior)
    {
        nowBehavior = behavior;
    }


    public override void Tick()
    {
        base.Tick();

        if (nowBehavior != null) 
        {
            CurBehavior = nowBehavior;
            nowBehavior = null;
            CurBehavior.OnStart();
        }

        if (CurBehavior != null)
        {
            if (!CurBehavior.Tick())
            {
                CurBehavior.OnFinish();
                CurBehavior = null;
            }
        }
    }
}