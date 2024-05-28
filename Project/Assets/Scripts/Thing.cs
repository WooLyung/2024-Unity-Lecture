using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public abstract class Thing : MonoBehaviour
{
    protected List<ThingComp> comps = new List<ThingComp>();

    // Variables
    public abstract string Name { get; }
    private Vector2Int pos;
    public Vector2Int Pos {
        get => pos;
        set
        {
            ThingSystem.Instance.Move(pos, value);
            pos = value;
        }
    }

    // Methods
    public ThingComp AddComp(ThingComp comp)
    {
        comps.Add(comp);
        comp.OnAdded();
        return comp;
    }

    public void InitPos(Vector2Int pos)
    {
        this.pos = pos;
        transform.position = new Vector3(pos.x, pos.y);
    }

    public bool HasComp(Type clazz)
    {
        return comps.Any(comp => comp.GetType() == clazz);
    }

    public ThingComp GetComp(Type clazz)
    {
        return comps.Find(comp => comp.GetType() == clazz);
    }

    // LifeCycle
    public virtual void OnInstantiate() { }

    public virtual void OnUpdate()
    {
        foreach (ThingComp comp in comps)
            comp.Update();
    }

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