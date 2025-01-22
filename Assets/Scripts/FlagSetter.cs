using System;
using UnityEngine;

public class FlagSetter : MonoBehaviour
{
    [SerializeField] private FlagSpawner _flagSpawner;
    [SerializeField] private string _groundLayerName;

    private bool _spawningFlag;
    private readonly float _yPosition = 3f;
    private Flag _flag;
    private Supermarket _supermarket;

    public bool HaveFlag { get; private set; }

    public GameObject FlagGameObject => _flag.gameObject;
    public FlagSpawner FlagSpawner => _flagSpawner;

    public event Action PayingForBuilding;
    public event Action<Flag> FlagSetted;

    private void Awake()
    {
        _supermarket = GetComponent<Supermarket>();
        _flag = null;
    }

    private void FixedUpdate()
    {
        if (_spawningFlag)
            SetFlag();
    }

    public void BaseClicked() =>
        _spawningFlag = true;

    public void SetSpawner(FlagSpawner spawner) =>
            _flagSpawner = spawner;

    private void SetFlag()
    {
        if (_flag != null && _flag.gameObject.activeSelf)
            RemoveFlag();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit) && Input.GetMouseButtonDown(0))
        {
            if (hit.collider.TryGetComponent(out MeshCollider ground))
            {
                if (ground.gameObject.layer == LayerMask.NameToLayer(_groundLayerName))
                {
                    _flag = _flagSpawner.SpawnFlag(new Vector3(hit.point.x, _yPosition, hit.point.z), transform);
                    _flag.SupermarketBuilded += FlagRemovedByBuilding;
                    _spawningFlag = false;
                    HaveFlag = true;
                }
            }
        }
        else
        {
            HaveFlag = false;
        }
    }

    private void FlagRemovedByBuilding(Person person)
    {
        if (person != null)
        {
            _supermarket.RemovePerson(person);
            PayingForBuilding?.Invoke();
            RemoveFlag();
        }
    }

    private void RemoveFlag()
    {
        _flag.gameObject.SetActive(false);
        _flag.SupermarketBuilded -= FlagRemovedByBuilding;
        HaveFlag = false;
        _flag = null;
    }
}
