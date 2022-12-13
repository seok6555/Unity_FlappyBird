using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;

public class LoginManager : MonoBehaviour
{
    private FirebaseAuth auth;
    //private FirebaseUser newUser;

    // Start is called before the first frame update
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().RequestIdToken().Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        GPGSLogin();
        //CreateEmail();
        //EmailLogin();
    }

    /// <summary>
    /// GPGS 로그인 시도 메서드.
    /// </summary>
    private void GPGSLogin()
    {
        // 로그인의 체크 유무 확인. 로그인이 되어있지 않다면 해당 if문 실행.
        if (!Social.localUser.authenticated)
        {
            // 로그인 시도.
            Social.localUser.Authenticate(success =>
            {
                if (success)
                {
                    Debug.Log("GPGS service success");
                    StartCoroutine(TryFirebaseLogin());
                }
                else
                {
                    Debug.LogError("GPGS service fail");
                }
            });
        }
    }

    /// <summary>
    /// 파이어베이스 로그인 시도 코루틴.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
        {
            yield return null;
        }
        // 유저의 토큰을 받아옴.
        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error. " + task.Exception);
                return;
            }

            // 파이어베이스 auth에 등록된 사용자UID
            //newUser = task.Result;
        });
    }

    private void CreateEmail()
    {
        auth.CreateUserWithEmailAndPasswordAsync("seok6555@naver.com", "sk14515828").ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                return;
            }
            if (task.IsFaulted)
            {
                return;
            }

            //newUser = task.Result;
        });
    }

    private void EmailLogin()
    {
        auth.SignInWithEmailAndPasswordAsync("seok6555@naver.com", "sk14515828").ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                return;
            }
            if (task.IsFaulted)
            {
                return;
            }

            //newUser = task.Result;
        });
    }
}
