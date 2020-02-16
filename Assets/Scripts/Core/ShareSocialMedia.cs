using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitterKit.Unity;
using Facebook.Unity;
using System;

public class ShareSocialMedia : MonoBehaviour {

    string Description = "I got {0} points in BrickPuzzle. Check out how far you can go: Android: https://play.google.com/store/apps/details?id=com.test.tetris";
    int Score = 0;
    ScoreManager score_manager;
    List<string> perms = new List<string>() { "email", "user_friends", "public_profile" };


    // Use this for initialization
    void Start () {
        Twitter.Init();
        this.score_manager = GameObject.FindObjectOfType<ScoreManager>();

    }

    void Awake() {
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);

        }
        else
        {
            FB.ActivateApp();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    #region Twitter
    void TwitterLoginComplete(TwitterSession session)
    {
        ScreenCapture.CaptureScreenshot("Screenshot.png");
        string imageUri = "file://" + Application.persistentDataPath + "/Screenshot.png";
        StartComponseTweet(session, imageUri);
    }

    void TwitterLoginFailure(ApiError error)
    {
        print("error: " + error);
    }

    void StartComponseTweet(TwitterSession session, string imageUri)
    {
      //  this.Score = score_manager.GetCurrentScore();
        Twitter.Compose(session, imageUri, string.Format(Description, ScoreZeros(Score, 5)),
                          new string[] { "#tetris #puzzle #brick" },
                         (string tweetId) => { UnityEngine.Debug.Log("Tweet Success, tweetId = " + tweetId); },
                          (ApiError error) => { UnityEngine.Debug.Log("Tweet Failed " + error.message); },
                          () =>
                          {
                              Debug.Log("Compose cancelled");
                          }
            );
    }

    void TwitterSession()
    {
        TwitterSession twitterSession = Twitter.Session;
        if (twitterSession == null)
        {
            Twitter.LogIn(TwitterLoginComplete, TwitterLoginFailure);
        }
        else
        {
            TwitterLoginComplete(twitterSession);
        }
    }

    public void ShareOnTwitter()
    {
        TwitterSession();
        this.Score = this.score_manager.GetCurrentScore();
    }

    public void ShareOnTwitterMainMenuScreen() {
        this.Score = this.score_manager.GetBestScore();
        TwitterSession();
    }


    #endregion

    #region FaceBook

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("Succesfuly Initialized the Facebook SDK");         
        }
        else
        {            
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ShareOnFaceBook()
    {

         this.Score = score_manager.GetCurrentScore();
        if (!FB.IsLoggedIn)
        {
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
        else
        {
            ShareLink();
            Debug.Log("User Currently Logged");
        }
    }


    public void ShareOnFaceBookMainMenuScreen()
    {

        this.Score = score_manager.GetBestScore();
        if (!FB.IsLoggedIn)
        {
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
        else
        {
            ShareLink();
            Debug.Log("User Currently Logged");
        }
    }


    public void AuthCallback(IResult result)
    {
        if (!result.Cancelled)
        {
            Debug.Log("Login Succesfully");
            ShareLink();
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    public void ShareLink() {        
        /*
        #if UNITY_IPHONE
                  FB.ShareLink(new Uri(""), "Tetris", String.Format(this.Description,  ScoreZeros(Score, 5)), null, ShareCallBack);
        #endif */

        #if UNITY_ANDROID
                FB.ShareLink(new Uri("https://play.google.com/store/apps/details?id=com.test.tetris"), "Brick Puzzle", String.Format(this.Description, ScoreZeros(Score, 5)), null, ShareCallBack);
        #endif


    }

    public void ShareCallBack(IShareResult result) {
        if (result.Cancelled) {
            Debug.Log("User Cancelled");
        }else{
            Debug.Log("Link Shared");
        }
    }

#endregion

    string ScoreZeros(int n, int zeros)
    {
        string value = n.ToString();
        while (value.Length < zeros)
        {
            value = "0" + value;
        }
        return value;
    }


}
