using UnityEngine;
using System.Collections;

public class GridUnit : MonoBehaviour
{
    public int MovementPerTurn = 3;
    public int Range = 1;
    public TypeMap MyType;
    public float Damage = 1;
    public float MaxHealth = 5;
    public GameGrid Grid;

    public int PlayerAffinity = 0;
    private int _gridLocation = 0;
    private float Health;
    private int _movementRemainingThisTurn = -1;

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

    public void SetActiveUnit()
    {
        _movementRemainingThisTurn = MovementPerTurn;
    }

    public void SetLocation(int location)
    {
        if (location < 0)
            return;
        Grid.SetGridObject(_gridLocation, null);
        _gridLocation = location;
        Grid.SetGridObject(_gridLocation, this.gameObject);
        Vector2 xzPos = Grid.GetWorldPos(_gridLocation);
        transform.position = new Vector3( xzPos.x, Grid.transform.position.y, xzPos.y );
    }

    public bool CanMoveTo(int location)
    {
        return Grid.GetManhattenDistance(_gridLocation, location) <= _movementRemainingThisTurn && Grid.CanMoveTo(location);
    }

    public void MoveNoCheck(int location)
    {
        _movementRemainingThisTurn -= Grid.GetManhattenDistance(_gridLocation, location);
        SetLocation(location);
    }

    public bool HasMovementRemaining()
    {
        return _movementRemainingThisTurn > 0;
    }
}
