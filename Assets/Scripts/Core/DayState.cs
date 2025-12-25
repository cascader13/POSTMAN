using UnityEngine;

public class DayState : GameState
{
    private float _dayTimer = -1f; // бесконечно
    private bool _dayFinished = false;

    public DayState(GameManager gameManager) : base(gameManager)
    {
        Log("DayState created");
    }

    public override void EnterState()
    {
        base.EnterState();

        _dayFinished = false;
        _dayTimer = 60f;

        // тут будет генерация очереди, диалогов и писем

        Log($"Day {_gameManager.Day} started");

        EventManager.Instance.TriggerEvent("DayStarted", _gameManager.Day);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        //здесь нужно будет что-то написать.....
    }

    private void FinishDay()
    {
        if (_dayFinished) return;

        _dayFinished = true;
        Log("Day finished. Transitioning to night...");
        _gameManager.StartNight();
    }

    public override void ExitState()
    {
        base.ExitState();

        // тут будет очистка памяти

        EventManager.Instance.TriggerEvent("DayEnded");
    }
}