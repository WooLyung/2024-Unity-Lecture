using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Player : Entity
{
    public override string Name => "Player";

    public override void OnInstantiate()
    {
        base.OnInstantiate();

        AddComp(new HpComp(this));
        AddComp(new MoveComp(this, 30));
    }

    public override void OnStart()
    {
        base.OnStart();

        BehaviorComp behaviorComp = (BehaviorComp)GetComp(typeof(BehaviorComp));
        behaviorComp.SetBehavior(new MoveBehavior(this, Pos, new Vector2Int(7, 0)));
    }
}