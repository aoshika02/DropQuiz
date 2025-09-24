using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalizeText : SingletonMonoBehaviour<LocalizeText>
{
    private static List<QuizTextBase> _keyToTextBases;
    [SerializeField] private TextAsset _jsonText;
    protected override void Awake()
    {
        if (CheckInstance())
        {
            DontDestroyOnLoad(gameObject);

            try
            {
                _keyToTextBases = JsonConverter.FromJson<QuizTextBase>(_jsonText.text);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
    /// <summary>
    /// keyからテキストを取得
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static (string text,string selectA,string selectB) GetText(string key)
    {
        var text = _keyToTextBases.FirstOrDefault(x => x.TextEventType == key);
        if (text == null)
        {
            Debug.LogError($"Keyが存在しません log : {key} is null");
            if (key.ToString() == "")
            {
                return (null,null,null);
            }
            else
            {
                return ($"{key} is null", $"{key} is null", $"{key} is null");
            }
        }
        return (text.Text, text.SelectA, text.SelectB);
    }
}