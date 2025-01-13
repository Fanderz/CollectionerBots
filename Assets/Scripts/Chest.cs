using UnityEngine;
using UnityEngine.AI;

public class Chest : MonoBehaviour
{
    public bool busy { get; private set; }

    private void OnEnable()
    {
        busy = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if ((collider.TryGetComponent(out ChestCollector collector) && busy == false) || collider.TryGetComponent(out NavMeshObstacle obstacle))
            gameObject.SetActive(false);
    }

    public void SetBusy() =>
        busy = true;
}
