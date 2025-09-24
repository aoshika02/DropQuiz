using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class CallTitleFlowGimmick : MonoBehaviour
{
    // Title画面のフローを呼び出す
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ParamConsts.PLAYER_TAG))
        {
            TitleManager.Instance.CallTitleFlow(true);
        }
    }
    //フロー終了
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ParamConsts.PLAYER_TAG))
        {
            TitleManager.Instance.CallTitleFlow(false);
        }
    }
}
