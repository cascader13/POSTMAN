using UnityEngine;
public abstract class GameState
{
    protected GameManager _gameManager;

    public GameState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public virtual void EnterState()
    {
        Debug.Log($"Entering state: {GetType().Name}");
    }

    public virtual void UpdateState()
    {
    }

    public virtual void ExitState()
    {
        Debug.Log($"Exiting state: {GetType().Name}");
    }

    protected void Log(string message)
    {
        Debug.Log($"[{GetType().Name}] {message}");
    }
}