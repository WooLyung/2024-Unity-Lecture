using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    public string GetJSON();
    public IEnumerable<ISavable> GetChilds();
    public string SavableName { get; }
}