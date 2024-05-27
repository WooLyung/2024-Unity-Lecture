using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Step
{
    public virtual void OnStart() { }
    public virtual void Tick() { }
    public virtual void OnCancel() { }
    public virtual void OnFinish() { }

    public abstract bool IsCanceled();
    public abstract bool IsFinished();
}