using System.Collections;
using UnityEngine;

//это нагенерино(c# трудно даётся)
public class NightState : GameState
{
    private float _nightTimer;
    private bool _nightFinished = false;
    private bool _officerIsComing = false;
    private float _officerTimer;

    // Для хранения ссылок на корутины
    private Coroutine _officerCoroutine;
    private MonoBehaviour _coroutineRunner;

    public NightState(GameManager gameManager) : base(gameManager)
    {
        Log("NightState created");

        // Находим объект для запуска корутин
        _coroutineRunner = gameManager.GetComponent<MonoBehaviour>();
        if (_coroutineRunner == null)
        {
            // Если GameManager не MonoBehaviour, создаем временный объект
            GameObject temp = new GameObject("CoroutineRunner");
            _coroutineRunner = temp.AddComponent<DummyMonoBehaviour>();
        }
    }

    public override void EnterState()
    {
        base.EnterState();

        // Инициализация ночи
        _nightFinished = false;
        _nightTimer = _gameManager.NightDuration;
        _officerIsComing = false;

        // Запускаем таймер офицера через корутину
        _officerCoroutine = _coroutineRunner.StartCoroutine(OfficerPatrolTimer());

        EventManager.Instance.TriggerEvent("NightStarted", _nightTimer);
    }

    private IEnumerator OfficerPatrolTimer()
    {
        // Ожидаем случайное время (30-150 секунд)
        float waitTime = Random.Range(30f, 150f);
        Log($"Officer will come in: {waitTime} seconds");

        yield return new WaitForSeconds(waitTime);

        StartOfficerPatrol();
    }

    private void StartOfficerPatrol()
    {
        if (_officerIsComing || _nightFinished) return;

        _officerIsComing = true;
        Log("Officer is coming! Hide the letters!");

        EventManager.Instance.TriggerEvent("OfficerPatrolStart");

        // Запускаем таймер ухода офицера
        _coroutineRunner.StartCoroutine(EndOfficerPatrolAfterDelay(10f));
    }

    private IEnumerator EndOfficerPatrolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndOfficerPatrol();
    }

    private void EndOfficerPatrol()
    {
        if (!_officerIsComing) return;

        _officerIsComing = false;
        Log("Officer left.");

        EventManager.Instance.TriggerEvent("OfficerPatrolEnd");
    }

    public override void ExitState()
    {
        base.ExitState();

        // Останавливаем все корутины
        if (_officerCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_officerCoroutine);
        }

        EventManager.Instance.TriggerEvent("NightEnded");
    }
}

// Вспомогательный класс для запуска корутин
public class DummyMonoBehaviour : MonoBehaviour { }