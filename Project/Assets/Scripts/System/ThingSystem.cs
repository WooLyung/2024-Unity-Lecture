using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingSystem : MonoBehaviour
{
    public static ThingSystem Instance { get; private set; }

    private List<Thing> newThings = new List<Thing>();
    private List<Thing> delThings = new List<Thing>();
    private List<Thing> things = new List<Thing>();
    private float tickTime = 0;

    public GameObject player;

    void Start()
    {
        Instance = this;
        InstantiateThing(player, Vector2Int.zero);
    }

    void Update()
    {
        tickTime += Time.deltaTime;
        if (tickTime > 1f / 60)
        {
            tickTime -= 1f / 10;

            InstantiateThings();
            PreTick();
            Tick();
            PostTick();
            DestroyThings();
        }
    }

    // Find
    public List<Thing> FindThingsWithComp(Type clazz)
    {
        List<Thing> result = new();
        foreach (Thing thing in things)
            if (thing.HasComp(clazz))
                result.Add(thing);

        return result;
    }

    // Thing
    public void InstantiateThing(GameObject prefab, Vector2Int pos)
    {
        GameObject obj = Instantiate(prefab);
        Thing thing = obj.GetComponent<Thing>();

        obj.transform.position = new Vector3(pos.x, pos.y);
        thing.OnInstantiate();

        newThings.Add(thing);
    }

    private void InstantiateThings()
    {
        foreach (Thing thing in newThings)
            things.Add(thing);
        foreach (Thing thing in newThings)
            thing.OnStart();
        newThings.Clear();
    }

    public void DestroyThing(Thing thing)
    {
        delThings.Add(thing);
    }

    private void DestroyThings()
    {
        foreach (Thing thing in delThings)
            things.Remove(thing);
        foreach (Thing thing in delThings)
            thing.OnFinish();
        delThings.Clear();
    }

    // LifeCycle
    private void PreTick()
    {
        foreach (Thing thing in things)
            thing.PreTick();
    }

    private void Tick()
    {
        foreach (Thing thing in things)
            thing.Tick();
    }

    private void PostTick()
    {
        foreach (Thing thing in things)
            thing.PostTick();
    }
}