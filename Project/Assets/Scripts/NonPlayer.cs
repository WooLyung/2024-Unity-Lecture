using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public abstract class NonPlayer : Entity
{
    public override void OnInstantiate()
    {
        base.OnInstantiate();
        AddComp(new AIComp(this));
    }
}