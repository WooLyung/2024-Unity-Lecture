using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ThingSystem : MonoBehaviour
{
    public static ThingSystem Instance { get; private set; }

    private List<(Vector2Int, Thing)> newThings = new();
    private List<Thing> delThings = new();
    private List<Thing> things = new();
    private Dictionary<Vector2Int, Thing> map = new();
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
            tickTime -= 1f / 60;

            InstantiateThings();
            PreTick();
            Tick();
            PostTick();
            DestroyThings();
        }

        UpdateThings();
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

    public Thing FindThing(Vector2Int pos)
    {
        Thing value;
        if (map.TryGetValue(pos, out value))
            return value;
        return null;
    }

    // Thing
    public void InstantiateThing(GameObject prefab, Vector2Int pos)
    {
        GameObject obj = Instantiate(prefab);
        Thing thing = obj.GetComponent<Thing>();

        obj.transform.position = new Vector3(pos.x, pos.y);
        thing.OnInstantiate();

        newThings.Add((pos, thing));
    }

    private void InstantiateThings()
    {
        foreach ((Vector2Int, Thing) pair in newThings)
        {
            things.Add(pair.Item2);
            map.Add(pair.Item1, pair.Item2);
        }
        foreach ((Vector2Int, Thing) pair in newThings)
            pair.Item2.OnStart();
        newThings.Clear();
    }

    public void DestroyThing(Thing thing)
    {
        delThings.Add(thing);
    }

    private void DestroyThings()
    {
        foreach (Thing thing in delThings)
        {
            things.Remove(thing);
            map.Remove(map.First(pair => pair.Value == thing).Key);
        }
        foreach (Thing thing in delThings)
            thing.OnFinish();
        delThings.Clear();
    }

    private void UpdateThings()
    {
        foreach (Thing thing in things)
            thing.OnUpdate();
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

    public void Move(Vector2Int from, Vector2Int to)
    {
        map.TryAdd(to, map[from]);
        map.Remove(from);
    }

    private static readonly Vector2Int[] Directions = new Vector2Int[]
{
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
};

    public bool PathFind(Vector2Int from, Vector2Int to, out List<Vector2Int> path)
    {
        path = new();

        // A*
        Dictionary<Vector2Int, float> openList = new(); // pos: G
        Dictionary<Vector2Int, float> closedList = new();
        Dictionary<Vector2Int, Vector2Int> pre = new();

        openList.Add(from, 0);
        while (openList.Count > 0)
        {
            Vector2Int pos = openList.First().Key;
            foreach (Vector2Int key in openList.Keys)
                if (openList[pos] + Vector2Int.Distance(pos, to) > openList[key] + Vector2Int.Distance(key, to))
                    pos = key;
            float G = openList[pos];

            if (pos == to)
            {
                Vector2Int cur = from;
                while (cur != to)
                {
                    path.Add(cur);
                    cur = pre[cur];
                }

                path.Add(to);
                path.Reverse();
                return true;
            }

            closedList.TryAdd(pos, G);
            foreach (Vector2Int direction in Directions)
            {
                Vector2Int neighborPos = new Vector2Int(pos.x + direction.x, pos.y + direction.y);
                if (closedList.ContainsKey(neighborPos) || map.ContainsKey(neighborPos))
                    continue;

                float neighborG = G + 1; // 거리 계산 필요
                if (openList.ContainsKey(neighborPos))
                {
                    if (openList[neighborPos] > neighborG)
                    {
                        openList[neighborPos] = neighborG;
                        pre[neighborPos] = pos;
                    }
                }
                else
                {
                    openList.Add(neighborPos, neighborG);
                    pre.Add(neighborPos, pos);
                }
            }
        }

        return false;
    }
}