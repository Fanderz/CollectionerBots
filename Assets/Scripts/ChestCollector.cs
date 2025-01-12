using TMPro;
using UnityEngine;

public class ChestCollector : MonoBehaviour
{
    [SerializeField] private Transform _spawner;
    [SerializeField] private TextMeshProUGUI _text;

    private int _chestsCollectedScore = 0;
    private Base _scanner;

    private void Awake()
    {
        _scanner = GetComponent<Base>();

        foreach (Person person in _scanner.Persons)
            if (person.TryGetComponent(out ChestsPicker picker))
                picker.ChestDropped += CollectChest;
    }

    private void CollectChest(Chest chest)
    {
        if (chest != null)
        {
            chest.transform.SetParent(_spawner);
            chest.gameObject.SetActive(false);

            _chestsCollectedScore++;
            _text.text = _chestsCollectedScore.ToString();
        }    
    }
}
