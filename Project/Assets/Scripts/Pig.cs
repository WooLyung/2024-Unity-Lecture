using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : NonPlayer
{
    public override string Name => "Pig";

    public override void OnInstantiate()
    {
        base.OnInstantiate();

        AddComp(new MoveComp(this, 60));
    }
}