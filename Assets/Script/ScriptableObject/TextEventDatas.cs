using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextEventDatas", menuName = "ScriptableObjects/TextEventDatas", order = 3)]
public class TextEventDatas : ScriptableObject
{
    [SerializeField] public List<TextEventData> TextEvents;
}
[Serializable]
public class TextEventData
{
    public TextEventType TextEventType;
    public PanelType NotAnswer;
    public float Duration;
}
