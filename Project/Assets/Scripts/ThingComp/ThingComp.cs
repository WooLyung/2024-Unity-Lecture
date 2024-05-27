using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThingComp
{
    public Thing Thing { get; private set; }

    public ThingComp(Thing thing) => Thing = thing;

    // Lifecycle
    public virtual void Update() { }
    public virtual void PreTick() { }
    public virtual void Tick() { }
    public virtual void PostTick() { }
    public virtual void OnAdded() { }
    public virtual void OnStart() { }
    public virtual void OnFinish() { }
}