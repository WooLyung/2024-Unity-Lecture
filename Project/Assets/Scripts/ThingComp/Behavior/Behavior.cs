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

    public bool Tick() // false : end
    {
        if (steps.Count == 0)
            return false;

        if (CurStep == -1)
        {
            CurStep = 0;
            Step step = steps[CurStep];
            step.OnStart();

            if (step.IsCanceled())
            {
                step.OnCancel();
                return false;
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
                return true;
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
            return false;

        steps[CurStep].Tick();
        return true;
    }
}