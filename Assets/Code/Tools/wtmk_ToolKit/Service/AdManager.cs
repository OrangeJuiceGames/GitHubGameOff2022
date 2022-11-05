using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleMobileAds.Api;

/*
public class AdManager : MonoBehaviour
{
    [SerializeField]
    private Transform _GameView;

    private BannerView _BannerAd;
    private EventManager _EventManager = EventManager.Instance;
    private List<GameObject> _Ads = new List<GameObject>();

    private string _AdName = "ADAPTIVE(Clone)";
    private int _UILayer = 200;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(InitializationStatus => { });

        RequestBanner();
        Register();
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
            string adUnitId = "unexpected_platform";
#endif
        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        // Create a 320x50 banner at the top of the screen.
        _BannerAd = new BannerView(adUnitId, adaptiveSize, AdPosition.Top);
        _BannerAd.OnAdLoaded += HandleAdLoaded;
        _BannerAd.OnAdFailedToLoad += HandleAdFailedToLoad;
        _BannerAd.OnAdOpening += HandleAdOpened;
        _BannerAd.OnAdClosed += HandleAdClosed;
    }

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        Canvas[] canvas = FindObjectsOfType<Canvas>();

        for (int i = 0; i < canvas.Length; i++)
        {
            if(canvas[i].name == _AdName)
            {
                GameObject go = canvas[i].GetComponent<GameObject>();
                if(!_Ads.Contains(go))
                {
                    _Ads.Add(go);
                    canvas[i].sortingOrder = _UILayer;
                }
            }
        }
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
                "HandleFailedToReceiveAd event received with message: " + args);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    
    private void ShowBannerAd() 
    {
        AdRequest request = new AdRequest.Builder().Build();
        _BannerAd.LoadAd(request);
        //GameObject ad = FindOb
    }

    private void OnShowBannerAd(string name, object data)
    {
        ShowBannerAd();
        
    }

    private void Register()
    {
        _EventManager.RegisterEventCallback("GameStart", OnShowBannerAd);
    }
}
*/