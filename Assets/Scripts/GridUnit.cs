using UnityEngine;
using System.Collections;

public class GridUnit : MonoBehaviour
{
    public int MovementPerTurn = 3;
    public int Range = 1;
    public TypeMap MyType;
    public float Damage = 1;
    public float MaxHealth = 5;
    private float Health;

    private int Location = 0;

    void Awake()
    {
        Health = MaxHealth;
    }

    public void Attack(GridUnit target)
    {
        float damage = Damage;
        if( MyType.IsStrongAgainst( target.MyType ) )
        {
            damage *= 1.5f; // TODO: Allow these modifiers to be editable
        }
        if (MyType.IsWeakAgainst(target.MyType))
        {
            damage *= .5f; // TODO: Allow these modifiers to be editable
        }
        target.TakeDamage(damage);
    }

    public void TakeDamage(float damageToTake)
    {
        Health -= damageToTake;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
