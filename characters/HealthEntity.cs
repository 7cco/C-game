namespace C__game;

public abstract class HealthEntity : AnimatedEntity
{
    protected float _maxHealth;
    protected float _currentHealth;
    protected bool _isDead;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;
    public bool IsDead => _isDead;
    public float HealthPercentage => _currentHealth / _maxHealth;

    protected HealthEntity(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        _isDead = false;
    }

    public virtual void TakeDamage(float damage)
    {
        if (_isDead) return;
        
        _currentHealth = Math.Max(0, _currentHealth - damage);
        if (_currentHealth <= 0)
        {
            _isDead = true;
            OnDeath();
        }
    }

    public virtual void Heal(float amount)
    {
        if (_isDead) return;
        
        _currentHealth = Math.Min(_maxHealth, _currentHealth + amount);
    }

    protected virtual void OnDeath()
    {
        // Переопределяется в дочерних классах
    }
} 