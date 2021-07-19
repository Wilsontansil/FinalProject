using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassScoreGame
{
    public int UserID;
    public int ScoreGame;
}

[System.Serializable]
public class ScoreLeaderboard
{
    public string UserName;
    public int ScoreGame;
    public string ClassName;
}
[System.Serializable]
public class listClassUserLeaderboard
{
    public List<ScoreLeaderboard> listUser = new List<ScoreLeaderboard>();
}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}