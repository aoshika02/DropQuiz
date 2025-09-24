using Cysharp.Threading.Tasks;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class SceneLoadObj : MonoBehaviour
{
    [SerializeField] private SceneType _sceneType;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ParamConsts.PLAYER_TAG))
        {
            SceneLoadManager.Instance.LoadSceneAsync(_sceneType).Forget();
        }
    }
}
