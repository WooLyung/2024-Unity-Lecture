using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Database<T> where T : Data
{
    private static Database<T> instance = null;
    private static Database<T> Instance => instance ??= new();
    private List<T> datas = new();

    public static void Load(T data) => Instance.datas.Add(data);

    public static IEnumerable AllDatas() 
    {
        foreach (T data in Instance.datas)
            yield return data;
    }

    public static IEnumerable ConditionDatas(Func<T, bool> lambda)
    {
        foreach (T data in Instance.datas)
            if (lambda(data))
                yield return data;
    }

    public static T ConditionData(Func<T, bool> lambda)
    {
        foreach (T data in Instance.datas)
            if (lambda(data))
                return data;
        return null;
    }
}