using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;

public class FirebaseManager : MonoBehaviour
{
    // Json 파일로 만들기 위해 유저 정보를 담을 클래스 정의
    public class User
    {
        public string userName;     // 유저의 이름
        public int userScore;       // 유저의 점수
        public int timeStamp;       // 해당 스코어를 기록한 타임 스탬프

        public User(string userName, int userScore, int timeStamp)
        {
            this.userName = userName;
            this.userScore = userScore;
            this.timeStamp = timeStamp;
        }
    }

    private static FirebaseManager instance;

    private DatabaseReference reference;

    // 다른 스크립트에서 FirebaseManager 인스턴스 참조하도록.
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
        // 데이터를 사용하려면 DatabaseReference의 인스턴스가 필요.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.KeepSynced(true);
    }

    /// <summary>
    /// 새로운 유저 닉네임 등록 메서드.
    /// </summary>
    /// <param name="_userName">등록할 닉네임.</param>
    public void RegisterUserName(string _userName)
    {
        StartCoroutine(UpdateUserNameDB(_userName));
        StartCoroutine(UpdateUserScoreDB(GameManager.Instance.Score));
        StartCoroutine(UpdateTimeStampDB(GameManager.Instance.Timestamp));
        StartCoroutine(LoadRankingData());
    }

    /// <summary>
    /// 현재 파이어베이스 DB에 유저의 정보가 있는지 체크하는 코루틴.
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

            // Firebase DB가 아예 비어있는 상태인지 체크.
            if (snapshot.Value != null)
            {
                foreach (DataSnapshot data in snapshot.Children)
                {
                    string userID = data.Key;
                    userIdList.Add(userID);
                }
                for (int i = 0; i < userIdList.Count; i++)
                {
                    // 해당 유저의 데이터가 존재하면 그대로 데이터 갱신 진행.
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
                        // 기존 점수보다 새롭게 기록한 점수가 더 높으면 수정.
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
    /// 파이어베이스 DB에 이름 업데이트 코루틴.
    /// </summary>
    /// <param name="_userName">업데이트할 유저 이름.</param>
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
    /// 파이어베이스 DB에 스코어 업데이트 코루틴.
    /// </summary>
    /// <param name="_userScore">업데이트할 유저 스코어.</param>
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
    /// 파이어베이스 DB에 타임 스탬프 업데이트 코루틴.
    /// </summary>
    /// <param name="_timeStamp">업데이트할 타임 스탬프.</param>
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
    /// 파이어베이스 DB를 불러와서 랭킹 리스트를 정렬하는 코루틴.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadRankingData()
    {
        // OrderByChild : 점수가 낮은 순으로 정렬됨.
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

            // 랭킹은 3위까지만 출력하기 때문에 3위 이하의 기록이 존재 시 제거.
            if (userDBList.Count > 3)
            {
                userDBList.RemoveRange(3, (userDBList.Count - 3));
            }

            // 랭킹 UI 출력.
            UIManager.Instance.ShowRankingList(userDBList);
        }
    }
}

