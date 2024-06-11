using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThingComp : ISavable
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

    public virtual string SavableName { get; }

    public string GetJSON()
    {
        return "";
    }

    public IEnumerable<ISavable> GetChilds()
    {
        yield break;
    }
}