using Unity.AI.Navigation;
using UnityEngine;

public class GroundRebaker : MonoBehaviour
{
    private NavMeshSurface _surface;

    private void Awake()
    {
        _surface = GetComponent<NavMeshSurface>();    
    }

    public void Rebake()
    {
        _surface.BuildNavMesh();
    }
}
