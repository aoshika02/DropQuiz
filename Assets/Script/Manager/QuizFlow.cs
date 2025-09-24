using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class QuizFlow : SingletonMonoBehaviour<QuizFlow>
{
    [SerializeField] private TextEventDatas _textEventDatas;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _choiceAText;
    [SerializeField] private TextMeshProUGUI _choiceBText;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private CanvasGroup _countCanvasGroup;
    [SerializeField] private int _count = 5;
    [SerializeField] private GameObject _limitObj;
    [SerializeField] private GameObject _resultLimitObj;
    private bool _failedFlag = false;
    private CancelToken _cancelToken;
    private CancellationTokenSource _cancelTokenSource;
    void Start()
    {
        _cancelToken = new CancelToken();
        _cancelTokenSource= CancellationTokenSource.CreateLinkedTokenSource( _cancelToken.GetToken(), destroyCancellationToken );
        _countCanvasGroup.alpha = 0;
        _text.text = "";
        _choiceAText.text = "";
        _choiceBText.text = "";
        QuizFlowAsync().Forget();
        QuizFailedFlowAsync().Forget();
    }
    /// <summary>
    /// クイズ不正解フロー
    /// </summary>
    /// <returns></returns>
    private async UniTask QuizFailedFlowAsync()
    {
        _resultLimitObj.SetActive(false);
        _failedFlag = false;
        await UniTask.WaitUntil(() => _failedFlag);
        _cancelToken.Cancel();
        _cancelToken.ReCreate();
        _cancelTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_cancelToken.GetToken(), destroyCancellationToken);
        _text.text = "";
        _choiceAText.text = "";
        _choiceBText.text = "";
        await UniTask.WaitForSeconds(1);
        var data = LocalizeText.GetText(TextEventType.Failed.ToString());
        await ViewQuizAsync(data.text, data.selectA, data.selectB, 0.05f);
        await CountAsync();
        _resultLimitObj.SetActive(true);
        await UniTask.WhenAny(PanelOpenAsync(PanelType.QuizA, _cancelTokenSource.Token), PanelOpenAsync(PanelType.QuizB, _cancelTokenSource.Token));
    }
    /// <summary>
    /// クイズの表示フロー
    /// </summary>
    /// <returns></returns>
    private async UniTask QuizFlowAsync()
    {
        _limitObj.SetActive(false);
        await UniTask.WaitForSeconds(2);
        List<TextEventData> datas = _textEventDatas.TextEvents.Where(x => x.TextEventType != TextEventType.Failed && x.TextEventType != TextEventType.Title).ToList();
        while (datas.Count > 0)
        {
            if(_failedFlag)
            {
                return;
            }
            var textEventData= datas[Random.Range(0, datas.Count)];
            var data = LocalizeText.GetText(textEventData.TextEventType.ToString());
            await ViewQuizAsync(data.text, data.selectA, data.selectB, textEventData.Duration);
            await CountAsync();
            await PanelOpenAsync(textEventData.NotAnswer, _cancelTokenSource.Token);
            _text.text = "";
            _choiceAText.text = "";
            _choiceBText.text = "";
            datas.Remove(textEventData);
        }
        _text.text = "全問正解";
        await UniTask.WaitForSeconds(5);
        await UniTask.WhenAny(PanelOpenAsync(PanelType.MainQuizA, _cancelTokenSource.Token), PanelOpenAsync(PanelType.MainQuizB, _cancelTokenSource.Token));
    }
    /// <summary>
    /// テキストを一文字ずつ表示
    /// </summary>
    /// <param name="viewText"></param>
    /// <param name="textMeshProUGUI"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private async UniTask ViewTextAsync(string viewText, TextMeshProUGUI textMeshProUGUI, float duration)
    {
        var length = viewText.Length;
        textMeshProUGUI.maxVisibleCharacters = 0;
        textMeshProUGUI.text = viewText;
        for (int i = 0; i <= length; i++)
        {
            textMeshProUGUI.maxVisibleCharacters = i;
            await UniTask.WaitForSeconds(duration);
        }
    }
    /// <summary>
    /// クイズの表示
    /// </summary>
    /// <param name="text"></param>
    /// <param name="selectA"></param>
    /// <param name="selectB"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private async UniTask ViewQuizAsync(string text, string selectA, string selectB, float duration)
    {
        await ViewTextAsync(text, _text, duration);
        await UniTask.WaitForSeconds(0.5f);
        await ViewTextAsync($"A\n{selectA}", _choiceAText, duration);
        await UniTask.WaitForSeconds(0.5f);
        await ViewTextAsync($"B\n{selectB}", _choiceBText, duration);
        await UniTask.WaitForSeconds(1f);
    }
    /// <summary>
    /// カウントダウン
    /// </summary>
    /// <returns></returns>
    private async UniTask CountAsync()
    {
        _countText.text = $"{_count}";
        await DOVirtual.Float(0, 1, 0.5f, f =>
        {
            _countCanvasGroup.alpha = f;
        }).ToUniTask();
        for (int i = 0; i <= _count; i++)
        {
            _countText.text = $"{_count - i}";
            await UniTask.WaitForSeconds(1);
        }
        await DOVirtual.Float(1, 0, 0.5f, f =>
        {
            _countCanvasGroup.alpha = f;
        }).ToUniTask();
    }
    /// <summary>
    /// パネルの会開閉
    /// </summary>
    /// <param name="targetType"></param>
    /// <param name="tokenSource"></param>
    /// <returns></returns>
    private async UniTask PanelOpenAsync(PanelType targetType,CancellationToken tokenSource) 
    {
        _limitObj.SetActive(true);
        QuizPanelManager.Instance.OpenQuizPanel(true, targetType);
        await UniTask.WaitForSeconds(3,cancellationToken: tokenSource);
        QuizPanelManager.Instance.OpenQuizPanel(false, targetType);
        await UniTask.WaitForSeconds(3, cancellationToken: tokenSource);
        _limitObj.SetActive(false);
    }
    public void FailedFlag()
    {
        _failedFlag = true;
    }
}
