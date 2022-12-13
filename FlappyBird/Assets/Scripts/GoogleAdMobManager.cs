using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdMobManager : MonoBehaviour
{
    private static GoogleAdMobManager instance;

    private InterstitialAd interstitial;

    // 다른 스크립트에서 GoogleAdMobManager 인스턴스 참조하도록.
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
    /// 전면광고 요청 메서드.
    /// </summary>
    private void RequestInterstitial()
    {
        // adUnitId: InterstitialAd가 광고를 로드하는 AdMob 광고 단위 ID.
        // 전면광고 테스트ID : ca-app-pub-3940256099942544/1033173712
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
    /// 전면광고 출력 메서드.
    /// </summary>
    public void StartInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }
}
