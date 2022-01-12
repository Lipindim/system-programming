using System.Collections;
using UnityEngine;


public class Task1 : MonoBehaviour
{
    [SerializeField] private int _initialHealth = 0;
    [SerializeField] private int _healValue = 5;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private float _totalHealingTime = 3.0f;
    [SerializeField] private float _healingInterval = 0.5f;

    private Unit _unit;

    private void Start()
    {
        _unit = new Unit(_initialHealth);
        ReceiveHealing();
    }

    private void ReceiveHealing()
    {
        StartCoroutine(Healing());
    }

    private IEnumerator Healing()
    {
        float totalTimeSpent = 0;
        while (totalTimeSpent < _totalHealingTime && _unit.Health < _maxHealth)
        {
            yield return new WaitForSeconds(_healingInterval);
            totalTimeSpent += _healingInterval;
            _unit.Heal(_healValue);
            Debug.Log($"Current health: {_unit.Health}");
        }

        Debug.Log($"Healing finish");
    }
}
