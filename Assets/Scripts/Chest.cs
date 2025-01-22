using UnityEngine;
using UnityEngine.AI;

public class Chest : MonoBehaviour
{
    public bool IsBusy { get; private set; }

    private void OnEnable()
    {
        IsBusy = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if ((collider.TryGetComponent(out ChestCollector collector) && IsBusy == false) || (collider.TryGetComponent(out NavMeshObstacle obstacle) && IsBusy == false))
            gameObject.SetActive(false);
    }

    public void SetBusy() =>
        IsBusy = true;
}
