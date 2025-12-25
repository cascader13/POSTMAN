using UnityEngine;

public class ReportState : GameState
{
    private bool _reportSubmitted = false;

    public ReportState(GameManager gameManager) : base(gameManager)
    {
        Log("ReportState created");
    }

    public override void EnterState()
    {
        base.EnterState();

        _reportSubmitted = false;

        Log("Report state started. Fill out the form.");

        // Тут тоже что-то

        EventManager.Instance.TriggerEvent("ReportStarted");


    }

    public override void UpdateState()
    {
        base.UpdateState();


        if (Input.GetKeyDown(KeyCode.S) && !_reportSubmitted)
        {
            SubmitReport();
        }
    }

    private void AutoSubmitReport()
    {
        if (!_reportSubmitted)
        {
            SubmitReport();
        }
    }

    private void SubmitReport()
    {
        _reportSubmitted = true;

        Log("Report submitted. Calculating results...");

        // Проверка квот

        CalculateDayResults();

        _gameManager.NextDay();
        _gameManager.StartDay();
    }

    private void CalculateDayResults()
    {
        // Временная логика расчета
        float quotaBonus = Random.Range(-5f, 10f);
        _gameManager.ChangeMorale(quotaBonus);
        _gameManager.ChangeTrust(Random.Range(-3f, 7f));
        _gameManager.ChangeQuota(Random.Range(5, 15));

        Log($"Day results: Morale {_gameManager.Morale}, Trust {_gameManager.Trust}, Quota {_gameManager.Quota}");
    }

    public override void ExitState()
    {
        base.ExitState();
        EventManager.Instance.TriggerEvent("ReportEnded");
    }
}