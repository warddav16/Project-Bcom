using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player 
{
    enum PlayerState
    {
        TakingTurn,
        NotTakingTurn,
        Lost
    }

    PlayerState _myState = PlayerState.NotTakingTurn;
    List<GridUnit> _myUnits = new List<GridUnit>();
    int _actveUnit = -0;
    public int PlayerIndex { get; set; }

    void Update()
    {
        if( _myState == PlayerState.TakingTurn )
        {
            // Should we be handling input here or in game manager?
        }
    }

    public void TakeTurn()
    {
        ++_actveUnit;
        _actveUnit %= _myUnits.Count;
        GameManager.Instance().SetActiveGridUnit(_myUnits[_actveUnit]);
        _myState = PlayerState.TakingTurn;
    }

    public void AddUnit(GridUnit unit)
    {
        if(unit.PlayerAffinity != PlayerIndex)
        {
            Debug.Log("Cannot add unit to this player");
            return;
        }
        _myUnits.Add(unit);
    }

    public bool IsStillPlaying()
    {
        return _myUnits.Count > 0;
    }
}
