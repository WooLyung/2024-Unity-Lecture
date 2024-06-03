using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : Data
{
    public string Name { get; private set; }
    public string Id { get; private set; }
    public string[] Tags { get; private set; }

    public ItemData(string name, string id, string[] tags)
    {
        Name = name;
        Id = id;
        Tags = tags;
    }
}