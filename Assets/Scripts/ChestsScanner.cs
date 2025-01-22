using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestsScanner : MonoBehaviour
{
    [SerializeField] private float _searchDelay;
    [SerializeField] private float _searchRadius;

    private List<Chest> _finded;

    private WaitForSeconds _searchWait;
    private Coroutine _searchCoroutine;

    public bool HaveSubscribers =>
        ChestFinded != null;

    public event Action<GameObject> ChestFinded;

    private void Awake()
    {
        _searchWait = new WaitForSeconds(_searchDelay);
        _finded = new List<Chest>();
    }

    private void OnEnable()
    {
        _searchCoroutine = StartCoroutine(SearchCoroutine());
    }

    private void OnDisable()
    {
        if (_searchCoroutine != null)
            StopCoroutine(_searchCoroutine);
    }

    private IEnumerator SearchCoroutine()
    {
        while (enabled)
        {
            FindChests();

            Chest chest = _finded.Find(chest => chest.IsBusy == false);

            if (chest != null)
                ChestFinded?.Invoke(chest.gameObject);

            yield return _searchWait;
        }
    }

    private void FindChests()
    {
        _finded.Clear();

        Collider[] finded = Physics.OverlapSphere(transform.position, _searchRadius);

        for (int i = 0; i < finded.Length; i++)
        {
            if (finded[i].TryGetComponent(out Chest chest))
            {
                _finded.Add(chest);
            }
        }
    }
}