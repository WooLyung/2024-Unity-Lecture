using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeDataWrapper
{
    public RecipeDataJson[] datas;
}

[System.Serializable]
public class RecipeDataJson
{
    public string[] input_items;
    public int[] input_amounts;
    public string[] output_items;
    public int[] output_amounts;
}

public class RecipeData : Data
{
    public (string, int)[] Inputs { get; private set; }
    public (string, int)[] Outputs { get; private set; }

    public RecipeData((string, int)[] inputs, (string, int)[] outputs)
    {
        Inputs = inputs;
        Outputs = outputs;
    }
}