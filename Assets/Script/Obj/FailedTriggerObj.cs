using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class FailedTriggerObj : MonoBehaviour
{
    //不正解トリガー
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ParamConsts.PLAYER_TAG)) 
        {
            QuizFlow.Instance.FailedFlag();
        }
    }
}
