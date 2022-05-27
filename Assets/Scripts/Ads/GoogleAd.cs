using System.Collections;
using GoogleMobileAds.Api;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoogleAd : MonoBehaviour {

    private BannerView _bannerView;


    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => { });

        GameManager.Instance.Player.GetComponent<Health>().OnDeath += OnPlayerDeath;
        //RequestBanner();



    }

    private void OnPlayerDeath()
    {
    }

    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-2074740839040877/6301997894";
        //string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        //AdSize adSize = new AdSize(500, 500);

        // Create a 320x50 banner at the top of the screen.
        _bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        AdRequest request = new AdRequest.Builder().Build();

        _bannerView.LoadAd(request);

    }
}
