using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitStep : Step
{
    private int waitTick;

    public WaitStep(int waitTick)
    {
        this.waitTick = waitTick;
    }

    public override bool IsCanceled()
    {
        return false;
    }

    public override bool IsFinished()
    {
        return waitTick <= 0;
    }

    public override void Tick()
    {
        base.Tick();
        waitTick--;
    }
}