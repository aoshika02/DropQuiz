using System.Collections.Generic;
using UnityEngine;

public class QuizPanelManager : SingletonMonoBehaviour<QuizPanelManager>
{
    [SerializeField] private List<Animator> _mainQuizAAnimators;
    [SerializeField] private List<Animator> _mainQuizBAnimators;

    [SerializeField] private List<Animator> _quizAAnimators;
    [SerializeField] private List<Animator> _quizBAnimators;

    /// <summary>
    /// クイズパネルの開閉
    /// </summary>
    /// <param name="isOpen"></param>
    /// <param name="panelType"></param>
    public void OpenQuizPanel(bool isOpen, PanelType panelType)
    {
        
        switch (panelType)
        {
            case PanelType.QuizA:
                _quizAAnimators.ForEach(x=>x.SetBool(ParamConsts.IS_OPEN, isOpen));
                break;
            case PanelType.QuizB:
                _quizBAnimators.ForEach(x => x.SetBool(ParamConsts.IS_OPEN, isOpen));
                break;
            case PanelType.MainQuizA:
                _mainQuizAAnimators.ForEach(x => x.SetBool(ParamConsts.IS_OPEN, isOpen));
                break;
            case PanelType.MainQuizB:
                _mainQuizBAnimators.ForEach(x => x.SetBool(ParamConsts.IS_OPEN, isOpen));
                break;
        }
    }
}
