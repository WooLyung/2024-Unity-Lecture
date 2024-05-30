using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public abstract class NonPlayer : Entity
{
    protected int curState = 0;
    protected BehaviorComp BehaviorComp { get; private set; }

    public override void OnInstantiate()
    {
        base.OnInstantiate();
        BehaviorComp = (BehaviorComp)GetComp(typeof(BehaviorComp));
    }

    public override void OnStart()
    {
        base.OnStart();
        InitState();
    }

    public override void Tick()
    {
        base.Tick();
        AbortState();
        if (BehaviorComp.CurBehavior == null)
            NextState();
    }

    protected virtual void InitState() { }
    protected virtual void NextState() { }
    protected virtual void AbortState() { }
}