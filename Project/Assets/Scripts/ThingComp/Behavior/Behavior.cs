using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Behavior
{
    protected List<Step> steps = new();
    protected int CurStep { get; private set; }

    public abstract void InitSteps();
    public virtual void newSteps() => InitSteps();

    public void OnStart()
    {
        CurStep = -1;
        InitSteps();
    }

    public virtual void OnFinish() { }

    public virtual void OnCancel() { }

    public int Tick() // 1:finish, 2:cancel
    {
        if (steps.Count == 0)
            return 1;

        if (CurStep == -1)
        {
            CurStep = 0;
            Step step = steps[CurStep];
            step.OnStart();

            if (step.IsCanceled())
            {
                step.OnCancel();
                return 2;
            }
            else if (step.IsFinished())
            {
                step.OnFinish();
                CurStep++;
                if (CurStep < steps.Count)
                    steps[CurStep].OnStart();
            }
        }
        else
        {
            Step step = steps[CurStep];

            if (step.IsCanceled())
            {
                step.OnCancel();
                CurStep = -1;
                newSteps();
                return 0;
            }
            else if (step.IsFinished())
            {
                step.OnFinish();
                CurStep++;
                if (CurStep < steps.Count)
                    steps[CurStep].OnStart();
            }
        }

        if (CurStep >= steps.Count)
            return 1;

        steps[CurStep].Tick();
        return 0;
    }
}