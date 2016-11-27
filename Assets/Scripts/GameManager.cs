using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;
    public static GameManager Instance()
    {
        return _instance;
    }

    public GameGrid TheGrid;
    public int NumberOfPlayers = 2;

    private List<GridUnit> _gridUnits = new List<GridUnit>();
    private List<Player> _players = new List<Player>();
    private int _activePlayer = -1;
    private GridUnit _activeGridUnit = null;
    private int _gridLayerMask = 0;

    public void SetActiveGridUnit( GridUnit toBeActive )
    {
        _activeGridUnit = toBeActive;
    }

    void Awake()
    {
        _instance = this;

        // Alloc player
        for( int i = 0; i < NumberOfPlayers; ++i )
        {
            _players.Add(new Player());
            _players[i].PlayerIndex = i;
        }

        // Set up units with the appropriate player
        // TODO(daviD); This will need to be re-worked for the "spawning phase" described in the design doc
        var units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (var unit in units)
        {
            GridUnit gu = unit.GetComponent<GridUnit>();
            if( gu )
            {
                gu.Grid = TheGrid;
                gu.SetLocation(TheGrid.GetGridCoordFromWorldPos(gu.transform.position.ToVec2XZ()));
                _gridUnits.Add(gu);
                _players[gu.PlayerAffinity].AddUnit(gu);
            }
            else
            {
                Debug.Log("Gameobject marked as Grid Unit but missing component!");
            }
        }

        // Some commonly used variable setup
        _gridLayerMask = (1 << LayerMask.GetMask("Grid"));
        _gridLayerMask = ~_gridLayerMask;

        // Finally, start this thing
        NextTurn();
    }

    void Update()
    {
        DrawGrid();

        if(Input.GetMouseButtonUp(0))
        {
            int hitIdx = -1;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000.0f, _gridLayerMask))
            {
                hitIdx = TheGrid.GetGridCoordFromWorldPos(hit.point.ToVec2XZ());
                if( hitIdx >= 0 )
                {
                   if( _activeGridUnit && _activeGridUnit.CanMoveTo( hitIdx ) )
                   {
                        // User selected a valid location for this unit to move to
                        _activeGridUnit.SetLocation( hitIdx );

                        NextTurn();
                   }
                }
            }
        }
    }

    void NextTurn()
    {
        if (FindNextActivePlayer())
        {
            _players[_activePlayer].TakeTurn();
        }
    }

    // Returns false when there is only one player reamaining
    bool FindNextActivePlayer()
    {
        for( int i = 0; i < _players.Count; ++i )
        {
            // Increment
            ++_activePlayer;
            // Handle wrap around
            _activePlayer %= _players.Count;
            if( _players[i].IsStillPlaying() )
            {
                return true;
            }
        }
        return false;
    }

    void DrawGrid()
    {
        // TODO( daviD ) : Need to talk to Billy about how we want this thing to look and how we want to edit it, maybe this function doesn't need to exist.
    }
}
