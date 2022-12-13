using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    // 다른 스크립트에서 UIManager 인스턴스 참조하도록.
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
    /// 현재 스코어 출력 메서드.
    /// </summary>
    /// <param name="_score">현재 스코어.</param>
    public void ShowCurrentScoreText(int _score)
    {
        scoreText.text = _score.ToString();
    }

    /// <summary>
    /// 리더보드 UI enum 관리 메서드.
    /// </summary>
    /// <param name="_uiState">변경할 UI enum.</param>
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
    /// 닉네임 생성 시 팝업 텍스트 출력 메서드.
    /// </summary>
    /// <param name="text">출력할 텍스트.</param>
    public void PopUpText(string text)
    {
        popUpText.text = text;
    }

    /// <summary>
    /// 랭킹 리스트 출력 메서드.
    /// </summary>
    /// <param name="users">출력할 랭킹 데이터가 담긴 리스트.</param>
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
    /// 광고 출력시 캔버스의 순서를 뒤로 보내는 메서드.
    /// </summary>
    public void SortingCanvasOrder()
    {
        gameCanvas.sortingOrder = -1;
    }

    /// <summary>
    /// 유저 닉네임 등록 버튼 이벤트.
    /// </summary>
    public void RegisterUserNameButton()
    {
        string userName = userNameInput.text;

        if (userName.Length < 1)
        {
            PopUpText("이름은 1글자 ~ 6글자 까지만 가능합니다.");
            return;
        }
        // 입력한 닉네임을 등록.
        FirebaseManager.Instance.RegisterUserName(userName);
        ShowBoardUI(eUIState.LeaderBoard);
    }

    /// <summary>
    /// 게임 시작 버튼 이벤트.
    /// </summary>
    public void StartGameButton()
    {
        GameManager.Instance.CurrentGameState(eGameState.Play);
        StartCoroutine(GameManager.Instance.ObstacleSpawn());
        startGameButton.SetActive(false);
    }

    /// <summary>
    /// 게임 재시작 버튼 이벤트.
    /// </summary>
    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }
}
