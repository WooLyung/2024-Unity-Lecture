using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : Thing
{
    public override void OnInstantiate()
    {
        base.OnInstantiate();

        AddComp(new BehaviorComp(this));
    }
}