using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Player : Entity
{
    public override string Name => "Player";

    private BehaviorComp behaviorComp;

    public override void OnInstantiate()
    {
        base.OnInstantiate();

        AddComp(new HpComp(this));
        AddComp(new MoveComp(this, 10));
        AddComp(new InvComp(this));
    }

    public override void OnStart()
    {
        base.OnStart();
        behaviorComp = (BehaviorComp)GetComp(typeof(BehaviorComp));
    }

    public override void Tick()
    {
        base.Tick();
        if (InputSystem.Instance.GetKeyState(KeyCode.Mouse0) == InputSystem.KeyState.KeyDown && behaviorComp.CurBehavior == null)
        {
            var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int target = new Vector2Int((int)(mouse.x + 0.5f), (int)(mouse.y + 0.5f));

            if (ThingSystem.Instance.FindThing(target) == null)
                behaviorComp.SetBehavior(new MoveBehavior(this, Pos, target));
            else
                behaviorComp.SetBehavior(new BreakBehavior(this, Pos, target));
        }
    }
}