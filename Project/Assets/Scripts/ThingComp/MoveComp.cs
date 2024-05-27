using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComp : ThingComp
{
    public int Speed { get; private set; }

    public bool IsMoving { get; private set; }
    private Vector2Int from, to;
    int movingTick = 0;
    int endTick = 0;
    float movingTime = 0;

    public MoveComp(Thing thing, int speed) : base(thing)
    {
        Speed = speed;
    }

    public override void Update()
    {
        if (!IsMoving)
            return;

        movingTime += Time.deltaTime;
        Thing.transform.position = new Vector3(
            Mathf.Lerp(from.x, to.x, movingTime * 60f / endTick),
            Mathf.Lerp(from.y, to.y, movingTime * 60f / endTick));
    }

    public override void Tick()
    {
        if (!IsMoving)
            return;

        movingTick++;
        if (movingTick >= endTick)
        {
            IsMoving = false;
            Thing.transform.position = new Vector3(to.x, to.y);
        }
    }

    public void Move(Vector2Int from, Vector2Int to)
    {
        IsMoving = true;
        this.from = from;
        this.to = to;
        movingTick = 0;
        movingTime = 0;
        endTick = (int)(Vector2Int.Distance(from, to) * Speed);

        Thing.Pos = to;
        Thing.transform.position = new Vector3(from.x, from.y);
    }
}