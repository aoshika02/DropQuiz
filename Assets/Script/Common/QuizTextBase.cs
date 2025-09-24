using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuizTextBase
{
    public string TextEventType;
    public string Text;
    public string SelectA;
    public string SelectB;
    public string Answer;
}
/// <summary>
/// Jsonからstringへ変換するクラス
/// </summary>
public static class JsonConverter
{
    public static List<T>FromJson<T>(string json)
    {
        KeyWrapper<T> wrapper = JsonUtility.FromJson<KeyWrapper<T>>(json);
        return wrapper.KeyTexts;
    }

    [Serializable]
    private class KeyWrapper<T>
    {
        public List<T> KeyTexts;
    }
}
