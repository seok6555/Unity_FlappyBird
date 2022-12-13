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
    /// GPGS �α��� �õ� �޼���.
    /// </summary>
    private void GPGSLogin()
    {
        // �α����� üũ ���� Ȯ��. �α����� �Ǿ����� �ʴٸ� �ش� if�� ����.
        if (!Social.localUser.authenticated)
        {
            // �α��� �õ�.
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
    /// ���̾�̽� �α��� �õ� �ڷ�ƾ.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
        {
            yield return null;
        }
        // ������ ��ū�� �޾ƿ�.
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

            // ���̾�̽� auth�� ��ϵ� �����UID
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
