using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float _maxHealth = 10.0f;
    public float MaxHealth => _maxHealth;

    [SerializeField]
    private UnityEvent<float> _damageTaken;
    public UnityEvent<float> DamageTaken => _damageTaken;

    [SerializeField]
    private UnityEvent<float> _healthRestored;
    public UnityEvent<float> HealthRestored => _healthRestored;

    [SerializeField]
    private UnityEvent _healthDepleted;
    public UnityEvent HealthDepleted => _healthDepleted;

    private float _currentHealth;
    public float CurrentHealth => _currentHealth;

    public void TakeDamage(float attackPower)
    {
        _currentHealth -= attackPower;
        _damageTaken.Invoke(attackPower);
        if (_currentHealth <= 0.0f)
        {
            _healthDepleted.Invoke();
        }
    }

    public void Heal(float healAmount)
    {
        _currentHealth += healAmount;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        _healthRestored.Invoke(healAmount);
    }

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
    }

    public void Profit(int x)
    {
        Global.Money += x;
    }
}
