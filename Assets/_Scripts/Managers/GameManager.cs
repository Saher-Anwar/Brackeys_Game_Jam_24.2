using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Vector2 playerSpawnPos = Vector2.zero;
    public GameObject player;
 
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public GameState State {get; private set;}

    protected override void Awake() {
        base.Awake();
        player = Instantiate(player, playerSpawnPos, Quaternion.identity);
    }

    void Start() => ChangeState(GameState.Starting);

    public void ChangeState(GameState newState){
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch(newState){
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Win:
                HandleWin();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterStateChanged?.Invoke(newState);
        Debug.Log($"New State: {newState}");
    }

    public void HandleStarting(){ 
        // Do something
    }
    public void HandleWin(){}
    public void HandleLose(){
        // TODO: destroy all enemies or simply display "Loser" UI

    }
}

[Serializable]
public enum GameState{
    Starting,
    Win,
    Lose,
}
