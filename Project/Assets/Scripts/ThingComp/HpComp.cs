using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpComp : ThingComp
{
    public HpComp(Thing thing) : base(thing) { }

    public override string SavableName => "HpComp";
}