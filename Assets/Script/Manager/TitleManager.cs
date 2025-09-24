using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
    private CancelToken _cancelToken;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _choiceAText;
    [SerializeField] private TextMeshProUGUI _choiceBText;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private CanvasGroup _countCanvasGroup;
    [SerializeField] private GameObject _limitObj;
    [SerializeField] private int _count = 5;
    void Start()
    {
        _cancelToken = new CancelToken();
        _countCanvasGroup.alpha = 0;
        _text.text = "";
        _choiceAText.text = "";
        _choiceBText.text = "";
        TitleFlow().Forget();
    }
    private async UniTask TitleFlow() 
    {
        _limitObj.SetActive(false);
        var data = LocalizeText.GetText(TextEventType.Title.ToString());
        await ViewQuizAsync(data.text, data.selectA, data.selectB, 0.05f);
    }
    private async UniTask ToInGameFlow() 
    {
        await CountAsync();
        await PanelOpenAsync(PanelType.MainQuizA);
    }
    private async UniTask ViewQuizAsync(string text, string selectA, string selectB, float duration)
    {
        await ViewTextAsync(text, _text, duration);
        await UniTask.WaitForSeconds(0.5f,cancellationToken:destroyCancellationToken);
        await ViewTextAsync($"A\n{selectA}", _choiceAText, duration);
        await UniTask.WaitForSeconds(0.5f, cancellationToken: destroyCancellationToken);
        await ViewTextAsync($"B\n{selectB}", _choiceBText, duration);
        await UniTask.WaitForSeconds(1f, cancellationToken: destroyCancellationToken);
    }
    private async UniTask ViewTextAsync(string viewText, TextMeshProUGUI textMeshProUGUI, float duration)
    {
        var length = viewText.Length;
        textMeshProUGUI.maxVisibleCharacters = 0;
        textMeshProUGUI.text = viewText;
        for (int i = 0; i <= length; i++)
        {
            textMeshProUGUI.maxVisibleCharacters = i;
            await UniTask.WaitForSeconds(duration, cancellationToken: destroyCancellationToken);
        }
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
        }).ToUniTask(cancellationToken: _cancelToken.GetToken());
        for (int i = 0; i <= _count; i++)
        {
            _countText.text = $"{_count - i}";
            await UniTask.WaitForSeconds(1, cancellationToken: _cancelToken.GetToken());
        }
        await DOVirtual.Float(1, 0, 0.5f, f =>
        {
            _countCanvasGroup.alpha = f;
        }).ToUniTask(cancellationToken: _cancelToken.GetToken());
    }
    private async UniTask PanelOpenAsync(PanelType targetType)
    {
        _limitObj.SetActive(true);
        QuizPanelManager.Instance.OpenQuizPanel(true, targetType);
        await UniTask.WaitForSeconds(3, cancellationToken: _cancelToken.GetToken());
        QuizPanelManager.Instance.OpenQuizPanel(false, targetType);
        await UniTask.WaitForSeconds(3, cancellationToken: _cancelToken.GetToken());
        _limitObj.SetActive(false);
    }
    public void CallTitleFlow(bool toInGame) 
    {
        if (toInGame) 
        {
            ToInGameFlow().Forget();
        }
        else 
        {
            _countCanvasGroup.alpha = 0;
            _countText.text = null;
            _cancelToken.Cancel();
            _cancelToken.ReCreate();
        }
    }
}
