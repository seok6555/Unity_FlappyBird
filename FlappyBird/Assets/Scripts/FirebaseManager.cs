using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;

public class FirebaseManager : MonoBehaviour
{
    // Json ���Ϸ� ����� ���� ���� ������ ���� Ŭ���� ����
    public class User
    {
        public string userName;     // ������ �̸�
        public int userScore;       // ������ ����
        public int timeStamp;       // �ش� ���ھ ����� Ÿ�� ������

        public User(string userName, int userScore, int timeStamp)
        {
            this.userName = userName;
            this.userScore = userScore;
            this.timeStamp = timeStamp;
        }
    }

    private static FirebaseManager instance;

    private DatabaseReference reference;

    // �ٸ� ��ũ��Ʈ���� FirebaseManager �ν��Ͻ� �����ϵ���.
    public static FirebaseManager Instance
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // �����͸� ����Ϸ��� DatabaseReference�� �ν��Ͻ��� �ʿ�.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.KeepSynced(true);
    }

    /// <summary>
    /// ���ο� ���� �г��� ��� �޼���.
    /// </summary>
    /// <param name="_userName">����� �г���.</param>
    public void RegisterUserName(string _userName)
    {
        StartCoroutine(UpdateUserNameDB(_userName));
        StartCoroutine(UpdateUserScoreDB(GameManager.Instance.Score));
        StartCoroutine(UpdateTimeStampDB(GameManager.Instance.Timestamp));
        StartCoroutine(LoadRankingData());
    }

    /// <summary>
    /// ���� ���̾�̽� DB�� ������ ������ �ִ��� üũ�ϴ� �ڷ�ƾ.
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckUserDB()
    {
        var DBTask = reference.Child("Users").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Fail {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            List<string> userIdList = new List<string>();
            List<User> userDBList = new List<User>();

            // Firebase DB�� �ƿ� ����ִ� �������� üũ.
            if (snapshot.Value != null)
            {
                foreach (DataSnapshot data in snapshot.Children)
                {
                    string userID = data.Key;
                    userIdList.Add(userID);
                }
                for (int i = 0; i < userIdList.Count; i++)
                {
                    // �ش� ������ �����Ͱ� �����ϸ� �״�� ������ ���� ����.
                    if (userIdList[i] == FirebaseAuth.DefaultInstance.CurrentUser.UserId)
                    {
                        foreach (DataSnapshot data in snapshot.Children)
                        {
                            IDictionary userInfo = (IDictionary)data.Value;
                            userDBList.Add(new User(
                                userInfo["userName"].ToString(),
                                int.Parse(userInfo["userScore"].ToString()),
                                int.Parse(userInfo["timeStamp"].ToString())
                                ));
                        }
                        // ���� �������� ���Ӱ� ����� ������ �� ������ ����.
                        if (userDBList[i].userScore < GameManager.Instance.Score)
                        {
                            StartCoroutine(UpdateUserScoreDB(GameManager.Instance.Score));
                            StartCoroutine(UpdateTimeStampDB(GameManager.Instance.Timestamp));
                        }
                        UIManager.Instance.ShowBoardUI(eUIState.LeaderBoard);
                        break;
                    }
                    UIManager.Instance.ShowBoardUI(eUIState.CreateName);
                }
            }
            else
            {
                UIManager.Instance.ShowBoardUI(eUIState.CreateName);
            }
            StartCoroutine(LoadRankingData());
        }
    }

    /// <summary>
    /// ���̾�̽� DB�� �̸� ������Ʈ �ڷ�ƾ.
    /// </summary>
    /// <param name="_userName">������Ʈ�� ���� �̸�.</param>
    /// <returns></returns>
    private IEnumerator UpdateUserNameDB(string _userName)
    {
        var DBTesk = reference.Child("Users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Child("userName").SetValueAsync(_userName);

        yield return new WaitUntil(predicate: () => DBTesk.IsCompleted);

        if (DBTesk.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTesk.Exception}");
        }
        else
        {
            // userName is now update
        }
    }

    /// <summary>
    /// ���̾�̽� DB�� ���ھ� ������Ʈ �ڷ�ƾ.
    /// </summary>
    /// <param name="_userScore">������Ʈ�� ���� ���ھ�.</param>
    /// <returns></returns>
    private IEnumerator UpdateUserScoreDB(int _userScore)
    {
        var DBTesk = reference.Child("Users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Child("userScore").SetValueAsync(_userScore);

        yield return new WaitUntil(predicate: () => DBTesk.IsCompleted);

        if (DBTesk.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTesk.Exception}");
        }
        else
        {
            // userScore is now update
        }
    }

    /// <summary>
    /// ���̾�̽� DB�� Ÿ�� ������ ������Ʈ �ڷ�ƾ.
    /// </summary>
    /// <param name="_timeStamp">������Ʈ�� Ÿ�� ������.</param>
    /// <returns></returns>
    private IEnumerator UpdateTimeStampDB(int _timeStamp)
    {
        var DBTesk = reference.Child("Users").Child(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Child("timeStamp").SetValueAsync(_timeStamp);

        yield return new WaitUntil(predicate: () => DBTesk.IsCompleted);

        if (DBTesk.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTesk.Exception}");
        }
        else
        {
            // timeStamp is now update
        }
    }

    /// <summary>
    /// ���̾�̽� DB�� �ҷ��ͼ� ��ŷ ����Ʈ�� �����ϴ� �ڷ�ƾ.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadRankingData()
    {
        // OrderByChild : ������ ���� ������ ���ĵ�.
        var DBTask = reference.Child("Users").OrderByChild("userScore").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Fail {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            List<User> userDBList = new List<User>();

            foreach (DataSnapshot data in snapshot.Children)
            {
                IDictionary userInfo = (IDictionary)data.Value;
                userDBList.Add(new User(
                    userInfo["userName"].ToString(),
                    int.Parse(userInfo["userScore"].ToString()),
                    int.Parse(userInfo["timeStamp"].ToString())
                    ));
            }

            userDBList.Reverse();

            // ��ŷ�� 3�������� ����ϱ� ������ 3�� ������ ����� ���� �� ����.
            if (userDBList.Count > 3)
            {
                userDBList.RemoveRange(3, (userDBList.Count - 3));
            }

            // ��ŷ UI ���.
            UIManager.Instance.ShowRankingList(userDBList);
        }
    }
}

