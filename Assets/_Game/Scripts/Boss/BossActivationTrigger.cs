using UnityEngine;
using Zenject;

public class BossActivationTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [Inject] private BossController _boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerControllerFacade _))
        {
            _boss.OnPlayerEnteredTrigger();
            gameObject.SetActive(false);
            _door.SetActive(true);
        }
    }
}