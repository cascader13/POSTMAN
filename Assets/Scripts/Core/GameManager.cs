using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Наш синглтон
    private static GameManager _instance;

    [System.Obsolete]
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject gmObject = new GameObject("GameManager");
                    _instance = gmObject.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    [Header("Game Stats")]
    [SerializeField] private float _morale = 50f;
    [SerializeField] private float _trust = 50f;
    [SerializeField] private int _quota = 0;
    [SerializeField] private int _day = 1;
    [Header("Game Settings")]
    [SerializeField] private float _nightDuration = 300f;
    private GameState _currentState;

    public float Morale => _morale;
    public float Trust => _trust;
    public int Quota => _quota;
    public int Day => _day;
    public float NightDuration => _nightDuration;
    public GameState CurrentState => _currentState;

    public System.Action OnDayStarted;
    public System.Action OnNightStarted;
    public System.Action OnReportStarted;
    public System.Action<float> OnMoraleChanged;
    public System.Action<float> OnTrustChanged;
    public System.Action<int> OnQuotaChanged;

    public void ChangeMorale(float amount)
    {
        _morale = Mathf.Clamp(_morale + amount, 0f, 100f);
        OnMoraleChanged?.Invoke(_morale);
        Debug.Log($"Morale changed to: {_morale}");
    }

    public void ChangeTrust(float amount)
    {
        _trust = Mathf.Clamp(_trust + amount, 0f, 100f);
        OnTrustChanged?.Invoke(_trust);
        Debug.Log($"Trust changed to: {_trust}");
    }

    public void ChangeQuota(int amount)
    {
        _quota = Mathf.Max(_quota + amount, 0);
        OnQuotaChanged?.Invoke(_quota);
        Debug.Log($"Quota changed to: {_quota}");
    }

    public void NextDay()
    {
        _day++;
        Debug.Log($"Day {_day} started");
    }

    public void SetState(GameState newState)
    {
        if (_currentState != null)
        {
            _currentState.ExitState();
        }

        _currentState = newState;

        if (_currentState != null)
        {
            _currentState.EnterState();
        }
    }

    public void StartDay()
    {
        SetState(new DayState(this));
        OnDayStarted?.Invoke();
    }

    public void StartNight()
    {
        SetState(new NightState(this));
        OnNightStarted?.Invoke();
    }

    public void StartReport()
    {
        SetState(new ReportState(this));
        OnReportStarted?.Invoke();
    }

    // Жизненный цикл
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); // сохранение объекта при переходе между сценами.....вроде

        Debug.Log("GameManager initialized");
    }

    private void Start()
    {
        // пока костыль костылём, после будет сделаны сейвы
        StartDay();
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState();
        }
    }

    // а вот это я нагенерил)))))
    [ContextMenu("Debug: Add 10 Morale")]
    private void DebugAddMorale() => ChangeMorale(10);

    [ContextMenu("Debug: Remove 10 Morale")]
    private void DebugRemoveMorale() => ChangeMorale(-10);

    [ContextMenu("Debug: Go to Night")]
    private void DebugGoToNight() => StartNight();

    [ContextMenu("Debug: Go to Report")]
    private void DebugGoToReport() => StartReport();

    [ContextMenu("Debug: Next Day")]
    private void DebugNextDay()
    {
        NextDay();
        StartDay();
    }
}