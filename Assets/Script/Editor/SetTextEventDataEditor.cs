#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class SetTextEventDataEditor : EditorWindow
{
    private TextAsset _jsonTextAsset;
    private TextEventDatas _textEventDatas;
    private List<QuizTextBase> _keyToTextBases;
    private float _defaultDuration = 0.05f;
    [MenuItem("Tools/SetTextEventDataEditor", false, 1)]
    private static void ShowEditerWindow()
    {
        SetTextEventDataEditor window = GetWindow<SetTextEventDataEditor>();
        window.titleContent = new GUIContent("Editor Window");
    }
    private void OnGUI()
    {
        //GUI(フィールド)でオブジェクトの参照埋め込みを行う
        EditorGUILayout.Space(10);
        GUILayout.Label($"▼{nameof(TextAsset)}をセット▼", EditorStyles.boldLabel);
        _jsonTextAsset = (TextAsset)EditorGUILayout.ObjectField("Json", _jsonTextAsset, typeof(TextAsset), true);
        EditorGUILayout.Space(10);
        GUILayout.Label($"▼{nameof(TextEventDatas)}をセット▼", EditorStyles.boldLabel);
        _textEventDatas = (TextEventDatas)EditorGUILayout.ObjectField("TextEventDatas", _textEventDatas, typeof(TextEventDatas), true);
        GUILayout.Label($"▼{nameof(_defaultDuration)}を入力▼", EditorStyles.boldLabel);
        _defaultDuration = EditorGUILayout.FloatField("Duration", _defaultDuration);
        //jsonのみ読み込み
        string path = AssetDatabase.GetAssetPath(_jsonTextAsset);
        if (_jsonTextAsset != null && !path.EndsWith(".json"))
        {
            Debug.LogWarning("このフィールドには .json ファイルのみを割り当てられます");
            _jsonTextAsset = null;
        }

        //ボタンを押した際の処理
        bool isNull = (_jsonTextAsset == null || _textEventDatas == null);
        if (isNull == false && GUILayout.Button("データを初期化してセット"))
        {
            SetData();
        }
    }
    private void SetData()
    {
        try
        {
            _textEventDatas.TextEvents.Clear();
            _keyToTextBases = JsonConverter.FromJson<QuizTextBase>(_jsonTextAsset.text);
            //Keyのデータのリストの作成(重複回避)
            foreach (var keyToTextBase in _keyToTextBases)
            {
                //合致するデータがなければcontinue
                if (keyToTextBase == null)
                {
                    continue;
                }
                TextEventType textEventType;
                if (Enum.TryParse(keyToTextBase.TextEventType, out textEventType) == false) continue;
                //データの追加
                _textEventDatas.TextEvents.Add(new TextEventData
                {
                    TextEventType = textEventType,
                    NotAnswer = (keyToTextBase.SelectA == keyToTextBase.Answer ? PanelType.MainQuizB : PanelType.MainQuizA),
                    Duration = _defaultDuration 
                });
            }
            Debug.Log("データの追加に成功しました");
            //変更を保存
            EditorUtility.SetDirty(_textEventDatas);
            AssetDatabase.SaveAssets();
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"データの追加に失敗しました: {ex.Message}");
            //元に戻す
            Undo.PerformUndo();
        }
    }
}
#endif