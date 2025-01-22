using TMPro;
using UnityEngine;

public class ChestCollector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Canvas _canvas;
    private Supermarket _base;

    public int Score { get; private set; }

    private void Awake()
    {
        _base = GetComponent<Supermarket>();
        _base.PayingPersonSpawn += SpendMoney;
        _base.PayingSupermarketSpawn += SpendMoney;
        _canvas.worldCamera = Camera.main;

        Score = 0;
    }

    private void OnEnable()
    {
        foreach (Person person in _base.Persons)
            if (person.TryGetComponent(out ChestsPicker picker) && picker.HaveChestDroppedSubscribers == false)
                picker.ChestDropped += CollectChest;
    }

    public void CollectChest(Chest chest)
    {
        if (chest != null)
        {
            chest.gameObject.SetActive(false);

            Score++;
            _text.text = Score.ToString();
        }    
    }

    private void SpendMoney(int cost)
    {
        Score -= cost;
        _text.text = Score.ToString();
    }
}
