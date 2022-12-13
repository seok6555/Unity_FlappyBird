using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // 다른 스크립트에서 GameManager 인스턴스 참조하도록.
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    public eGameState GameState { get; private set; } = eGameState.Ready;
    public int Score { get; private set; } = 0;
    public int Timestamp { get; private set; } = 0;

    public AudioClip getScoreSound;
    public AudioClip deathSound;

    private float poolingDelay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        poolingDelay = 1.5f;
        UIManager.Instance.ShowBoardUI(eUIState.None);
    }

    // Update is called once per frame
    void Update()
    {
        GameExit();
    }

    /// <summary>
    /// 게임오버 담당 메서드.
    /// </summary>
    public void GameOver()
    {
        //게임이 종료되었는지 체크
        if (GameManager.Instance.GameState == eGameState.End)
        {
            return;
        }

        GameManager.Instance.CurrentGameState(eGameState.End);
        SoundManager.Instance.SFXPlay("DeathSound", deathSound);
        GoogleAdMobManager.Instance.StartInterstitial();
        UIManager.Instance.SortingCanvasOrder();
        
        // 타임스탬프를 구하기.
        var now = System.DateTime.Now.ToLocalTime();
        var span = (now - new System.DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
        Timestamp = (int)span.TotalSeconds;

        // 우선 파이어베이스에 플레이어 정보가 있는지 체크
        StartCoroutine(FirebaseManager.Instance.CheckUserDB());
    }

    /// <summary>
    /// 게임 종료 메서드.
    /// </summary>
    private void GameExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// 장애물 스폰 담당 코루틴.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ObstacleSpawn()
    {
        yield return new WaitForSeconds(poolingDelay);

        Obstacle obstacle = PoolingManager.GetObject();
        obstacle.transform.position = new Vector2(6f, Random.Range(0, 1.1f));

        if (GameManager.Instance.GameState == eGameState.Play)
        {
            StartCoroutine(ObstacleSpawn());
        }
    }

    /// <summary>
    /// 스코어 획득 담당 메서드.
    /// </summary>
    /// <param name="plusScore">획득한 스코어.</param>
    public void GetScore(int plusScore)
    {
        Score += plusScore;
        UIManager.Instance.ShowCurrentScoreText(Score);
        SoundManager.Instance.SFXPlay("GetScoreSound", getScoreSound);
    }

    /// <summary>
    /// 현재 게임 상태 enum 변경 담당 메서드.
    /// </summary>
    /// <param name="_gameState">변경할 상태 enum.</param>
    public void CurrentGameState(eGameState _gameState)
    {
        GameManager.Instance.GameState = _gameState;
    }
}
