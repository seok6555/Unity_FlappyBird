using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // �ٸ� ��ũ��Ʈ���� GameManager �ν��Ͻ� �����ϵ���.
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
    /// ���ӿ��� ��� �޼���.
    /// </summary>
    public void GameOver()
    {
        //������ ����Ǿ����� üũ
        if (GameManager.Instance.GameState == eGameState.End)
        {
            return;
        }

        GameManager.Instance.CurrentGameState(eGameState.End);
        SoundManager.Instance.SFXPlay("DeathSound", deathSound);
        GoogleAdMobManager.Instance.StartInterstitial();
        UIManager.Instance.SortingCanvasOrder();
        
        // Ÿ�ӽ������� ���ϱ�.
        var now = System.DateTime.Now.ToLocalTime();
        var span = (now - new System.DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
        Timestamp = (int)span.TotalSeconds;

        // �켱 ���̾�̽��� �÷��̾� ������ �ִ��� üũ
        StartCoroutine(FirebaseManager.Instance.CheckUserDB());
    }

    /// <summary>
    /// ���� ���� �޼���.
    /// </summary>
    private void GameExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// ��ֹ� ���� ��� �ڷ�ƾ.
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
    /// ���ھ� ȹ�� ��� �޼���.
    /// </summary>
    /// <param name="plusScore">ȹ���� ���ھ�.</param>
    public void GetScore(int plusScore)
    {
        Score += plusScore;
        UIManager.Instance.ShowCurrentScoreText(Score);
        SoundManager.Instance.SFXPlay("GetScoreSound", getScoreSound);
    }

    /// <summary>
    /// ���� ���� ���� enum ���� ��� �޼���.
    /// </summary>
    /// <param name="_gameState">������ ���� enum.</param>
    public void CurrentGameState(eGameState _gameState)
    {
        GameManager.Instance.GameState = _gameState;
    }
}
