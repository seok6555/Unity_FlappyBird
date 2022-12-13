using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    // �ٸ� ��ũ��Ʈ���� UIManager �ν��Ͻ� �����ϵ���.
    public static UIManager Instance
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

    public GameObject startGameButton;
    public GameObject boardUI;
    public GameObject leaderBoardUI;
    public GameObject createUserNameUI;
    public GameObject[] rankingUI;

    public Text scoreText;
    public Text popUpText;
    public Text[] rankingUserNames;
    public Text[] rankingUserScores;

    public Canvas gameCanvas;

    public InputField userNameInput;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// ���� ���ھ� ��� �޼���.
    /// </summary>
    /// <param name="_score">���� ���ھ�.</param>
    public void ShowCurrentScoreText(int _score)
    {
        scoreText.text = _score.ToString();
    }

    /// <summary>
    /// �������� UI enum ���� �޼���.
    /// </summary>
    /// <param name="_uiState">������ UI enum.</param>
    public void ShowBoardUI(eUIState _uiState)
    {
        switch (_uiState)
        {
            case eUIState.None:
                boardUI.SetActive(false);
                leaderBoardUI.SetActive(false);
                createUserNameUI.SetActive(false);
                break;
            case eUIState.LeaderBoard:
                boardUI.SetActive(true);
                leaderBoardUI.SetActive(true);
                createUserNameUI.SetActive(false);
                break;
            case eUIState.CreateName:
                boardUI.SetActive(true);
                leaderBoardUI.SetActive(false);
                createUserNameUI.SetActive(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// �г��� ���� �� �˾� �ؽ�Ʈ ��� �޼���.
    /// </summary>
    /// <param name="text">����� �ؽ�Ʈ.</param>
    public void PopUpText(string text)
    {
        popUpText.text = text;
    }

    /// <summary>
    /// ��ŷ ����Ʈ ��� �޼���.
    /// </summary>
    /// <param name="users">����� ��ŷ �����Ͱ� ��� ����Ʈ.</param>
    public void ShowRankingList(List<FirebaseManager.User> users)
    {
        for(int i = 0; i < users.Count; i++)
        {
            rankingUserNames[i].text = users[i].userName;
            rankingUserScores[i].text = users[i].userScore.ToString();
            rankingUI[i].SetActive(true);
        }
    }

    /// <summary>
    /// ���� ��½� ĵ������ ������ �ڷ� ������ �޼���.
    /// </summary>
    public void SortingCanvasOrder()
    {
        gameCanvas.sortingOrder = -1;
    }

    /// <summary>
    /// ���� �г��� ��� ��ư �̺�Ʈ.
    /// </summary>
    public void RegisterUserNameButton()
    {
        string userName = userNameInput.text;

        if (userName.Length < 1)
        {
            PopUpText("�̸��� 1���� ~ 6���� ������ �����մϴ�.");
            return;
        }
        // �Է��� �г����� ���.
        FirebaseManager.Instance.RegisterUserName(userName);
        ShowBoardUI(eUIState.LeaderBoard);
    }

    /// <summary>
    /// ���� ���� ��ư �̺�Ʈ.
    /// </summary>
    public void StartGameButton()
    {
        GameManager.Instance.CurrentGameState(eGameState.Play);
        StartCoroutine(GameManager.Instance.ObstacleSpawn());
        startGameButton.SetActive(false);
    }

    /// <summary>
    /// ���� ����� ��ư �̺�Ʈ.
    /// </summary>
    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }
}
