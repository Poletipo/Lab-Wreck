using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoogleAd : MonoBehaviour {

    private static BannerView _bannerView;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });

        GameManager.Instance.Player.GetComponent<Health>().OnDeath += OnPlayerDeath;
    }

    public static void DestroyBanner()
    {
        _bannerView.Destroy();

    }

    private void OnPlayerDeath()
    {
        RequestBanner();
        GameManager.Instance.Player.GetComponent<Health>().OnDeath -= OnPlayerDeath;
    }

    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-2074740839040877/6301997894";
        //string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        AdRequest request = new AdRequest.Builder().Build();

        _bannerView.LoadAd(request);
    }
}
