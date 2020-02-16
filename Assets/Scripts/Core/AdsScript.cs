using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class AdsScript : MonoBehaviour {

    InterstitialAd interstitial;
    BannerView bannerView;

    // Use this for initialization
    void Start () {
#if UNITY_IPHONE
        Advertisement.Initialize("1600783", false);
#else
        Advertisement.Initialize("1600784", false);        
        #endif
        //RequestInterstitial();
       RequestBanner();      
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UnityVideoAds() {
        Advertisement.Show();
    }

    public void ShowBanner() {
        this.bannerView.Show();
    }

    public void ShowInterstitial() {        
        interstitial.Show();
    }


    private void RequestBanner()
    {
        #if UNITY_EDITOR
                        string adUnitId = "ca-app-pub-5011152033559148/1082319173";
        #elif UNITY_ANDROID
                        string adUnitId = "ca-app-pub-5011152033559148/1082319173";
        #elif UNITY_IPHONE
                        string adUnitId = "";
        #else
                        string adUnitId = "";
        #endif
        AdSize adSize = new AdSize(320, 50);
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, adSize, AdPosition.Bottom);
        // Create an empty ad request.
          AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

}
