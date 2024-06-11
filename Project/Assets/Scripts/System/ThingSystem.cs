using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThingSystem : MonoBehaviour, ISavable
{
    public static ThingSystem Instance { get; private set; }

    private List<(Vector2Int, Thing)> newThings = new();
    private List<Thing> delThings = new();
    private List<Thing> things = new();
    private Dictionary<Vector2Int, Thing> map = new();
    private float tickTime = 0;

    public ThingSystem()
    {
        Instance = this;
    }

    // Load Resources
    void Awake()
    {
        TextAsset items = Resources.Load<TextAsset>("items");
        ItemDataWrapper itemsWrapper = JsonUtility.FromJson<ItemDataWrapper>(items.text);
        foreach (ItemDataJson data in itemsWrapper.datas)
            Database<ItemData>.Load(new ItemData(data.name, data.id, data.tags));

        TextAsset recipes = Resources.Load<TextAsset>("recipes");
        RecipeDataWrapper recipesWrapper = JsonUtility.FromJson<RecipeDataWrapper>(recipes.text);
        foreach (RecipeDataJson data in recipesWrapper.datas)
        {
            (string, int)[] inputs = new (string, int)[data.input_items.Count()];
            (string, int)[] outputs = new (string, int)[data.output_items.Count()];

            for (int i = 0; i < data.input_items.Count(); i++)
                inputs[i] = (data.input_items[i], data.input_amounts[i]);
            for (int i = 0; i < data.output_items.Count(); i++)
                outputs[i] = (data.output_items[i], data.output_amounts[i]);

            Database<RecipeData>.Load(new RecipeData(inputs, outputs));
        }
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

        thing.InitPos(pos);
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
        {
            thing.OnFinish();
            Destroy(thing.gameObject);
        }
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
        InputSystem.Instance.PreTick();
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

    private static readonly Vector2Int[] Directions = new[]
    {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };

    private static readonly (Vector2Int, Vector2Int, Vector2Int)[] Directions2 = new[]
    {
        (new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1)),
        (new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(-1, 1)),
        (new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(1, -1)),
        (new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(-1, -1)),
    };

    public bool PathFind(Vector2Int from, Vector2Int to, out List<Vector2Int> path, float maxDist = 20f)
    {
        path = new();

        Dictionary<Vector2Int, float> openList = new();
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

            if (G + Vector2Int.Distance(pos, to) > maxDist)
                break;

            if (pos == to)
            {
                Vector2Int cur = to;
                while (cur != from)
                {
                    path.Add(cur);
                    cur = pre[cur];
                }
                path.Add(from);
                path.Reverse();

                return true;
            }

            closedList.TryAdd(pos, G);
            openList.Remove(pos);

            // 수평 및 수직 이동
            foreach (Vector2Int direction in Directions)
            {
                Vector2Int neighborPos = new Vector2Int(pos.x + direction.x, pos.y + direction.y);
                if (closedList.ContainsKey(neighborPos) || map.ContainsKey(neighborPos))
                    continue;

                float neighborG = G + 1;
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
            // 대각선 이동
            foreach (var direction in Directions2)
            {
                Vector2Int neighborPosA = pos + direction.Item1;
                Vector2Int neighborPosB = pos + direction.Item2;
                Vector2Int neighborPos = pos + direction.Item3;
                if (closedList.ContainsKey(neighborPos) || map.ContainsKey(neighborPos) || map.ContainsKey(neighborPosA) || map.ContainsKey(neighborPosB))
                    continue;

                float neighborG = G + 1;
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

    public bool PathFindNeighbor(Vector2Int from, Vector2Int to, out List<Vector2Int> path, float maxDist = 20f)
    {
        path = new();

        Dictionary<Vector2Int, float> openList = new();
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

            if (G + Vector2Int.Distance(pos, to) > maxDist)
                break;

            foreach (Vector2Int direction in Directions)
            {
                if (pos + direction == to)
                {
                    Vector2Int cur = pos;
                    while (cur != from)
                    {
                        path.Add(cur);
                        cur = pre[cur];
                    }
                    path.Add(from);
                    path.Reverse();

                    return true;
                }
            }

            closedList.TryAdd(pos, G);
            openList.Remove(pos);

            // 수평 및 수직 이동
            foreach (Vector2Int direction in Directions)
            {
                Vector2Int neighborPos = new Vector2Int(pos.x + direction.x, pos.y + direction.y);
                if (closedList.ContainsKey(neighborPos) || map.ContainsKey(neighborPos))
                    continue;

                float neighborG = G + 1;
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
            // 대각선 이동
            foreach (var direction in Directions2)
            {
                Vector2Int neighborPosA = pos + direction.Item1;
                Vector2Int neighborPosB = pos + direction.Item2;
                Vector2Int neighborPos = pos + direction.Item3;
                if (closedList.ContainsKey(neighborPos) || map.ContainsKey(neighborPos) || map.ContainsKey(neighborPosA) || map.ContainsKey(neighborPosB))
                    continue;

                float neighborG = G + 1;
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

    public Vector2Int? GetRandomEmptyTile(Vector2Int center, int size)
    {
        HashSet<Vector2Int> tiles = new();
        Queue<Vector2Int> q = new();
        
        foreach (var dir in Directions)
        {
            if (!map.ContainsKey(center + dir))
            {
                q.Enqueue(center + dir);
                tiles.Add(center + dir);
            }
        }

        while (tiles.Count < size && q.Count > 0)
        {
            Vector2Int tile = q.Dequeue();
            foreach (var dir in Directions)
            {
                if (!tiles.Contains(tile + dir) && !map.ContainsKey(tile + dir))
                {
                    q.Enqueue(tile + dir);
                    tiles.Add(tile + dir);
                }
            }
        }

        if (tiles.Count == 0)
            return null;
        return tiles.ToList()[UnityEngine.Random.Range(0, tiles.Count)];
    }

    public string SavableName => "things";

    public string GetJSON()
    {
        return null;
    }

    public IEnumerable<ISavable> GetChilds()
    {
        foreach (Thing thing in things)
            yield return thing;
    }
}