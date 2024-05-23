using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Thing : MonoBehaviour
{
    protected List<ThingComp> comps = new List<ThingComp>();

    // Variables
    public abstract string Name { get; }

    // Methods
    public void AddComp(ThingComp comp)
    {
        comps.Add(comp);
        comp.OnAdded();
    }

    public bool HasComp(Type clazz)
    {
        return comps.Any(comp => comp.GetType() == clazz);
    }

    // LifeCycle
    public virtual void OnInstantiate() { }

    public virtual void PreTick()
    {
        foreach (ThingComp comp in comps)
            comp.PreTick();
    }

    public virtual void Tick()
    {
        foreach (ThingComp comp in comps)
            comp.Tick();
    }

    public virtual void PostTick()
    {
        foreach (ThingComp comp in comps)
            comp.PostTick();
    }

    public virtual void OnStart()
    {
        foreach (ThingComp comp in comps)
            comp.OnStart();
    }

    public virtual void OnFinish()
    {
        foreach (ThingComp comp in comps)
            comp.OnFinish();
    }
}