using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdMobManager : MonoBehaviour
{
    private static GoogleAdMobManager instance;

    private InterstitialAd interstitial;

    // �ٸ� ��ũ��Ʈ���� GoogleAdMobManager �ν��Ͻ� �����ϵ���.
    public static GoogleAdMobManager Instance
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

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        RequestInterstitial();
    }

    /// <summary>
    /// ���鱤�� ��û �޼���.
    /// </summary>
    private void RequestInterstitial()
    {
        // adUnitId: InterstitialAd�� ���� �ε��ϴ� AdMob ���� ���� ID.
        // ���鱤�� �׽�ƮID : ca-app-pub-3940256099942544/1033173712
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3659776300668186/7564157101";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    /// <summary>
    /// ���鱤�� ��� �޼���.
    /// </summary>
    public void StartInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }
}
