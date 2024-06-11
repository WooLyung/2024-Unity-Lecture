using System;
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
            int result = CurBehavior.Tick();
            if (result == 1)
            {
                CurBehavior.OnFinish();
                CurBehavior = null;
            }
            else if (result == 2)
            {
                CurBehavior.OnCancel();
                CurBehavior = null;
            }
        }
    }

    public override string SavableName => "BehaviorComp";
}