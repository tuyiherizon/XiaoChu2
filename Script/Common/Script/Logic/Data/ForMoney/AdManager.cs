using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Advertisements;

public class AdManager:MonoBehaviour, IUnityAdsListener
{
    #region 

#if UNITY_ANDROID
    static string _GameId = "3464211";
#else
    static string _GameId = "3464210";
#endif

    public static void InitAdManager()
    {
        GameObject go = new GameObject();
        go.AddComponent<AdManager>();

        Advertisement.Initialize(_GameId, false, true);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _Instance = this;
    }

    private static AdManager _Instance;

    public static AdManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    #endregion

    #region ad video

    private Action _AdVideoCallBack;

    private string _VideoPlacementID = "DiamondVideoAD";

    public void PrepareVideo()
    {
        //PlatformHelper.Instance.LoadVideoAD();
        //LoadRewardAd();

        Advertisement.Load(_VideoPlacementID);
    }

    public void WatchAdVideo(Action finishCallBack)
    {
        finishCallBack.Invoke();
        return;

        _AdVideoCallBack = finishCallBack;

        //if (PlatformHelper.Instance.GetLocationPermission())
        //{
        //    //PlatformHelper.Instance.ShowVideoAD();
        //    ShowRewardAd();
        //}

        _PreparingAD = true;
        StartCoroutine(WaitForAd());
    }

    private bool _PreparingAD = false;
    IEnumerator WaitForAd()
    {
        while (!Advertisement.IsReady(_VideoPlacementID))
        {
            if (_PreparingAD)
            {
                if (!UILoadingTips.IsShowing())
                {
                    UILoadingTips.ShowAsyn(5);
                }
            }
            yield return null;
        }

        _PreparingAD = false;
        UILoadingTips.HideAsyn();

        Advertisement.AddListener(this);
        Advertisement.Show(_VideoPlacementID);

    }


    public void WatchAdVideoFinish()
    {
        if (_AdVideoCallBack != null)
        {
            _AdVideoCallBack.Invoke();
        }
    }

    #endregion

    #region ad inter

    public float _ShowInterADTime = 30.0f;
    public float _StartInterADTime;
    public bool _IsInterADPrepared = false;

    private bool _InterADExposure = false;
    private int _LoadSceneTimes = 0;
    private int _ShowAdLoadTimes = 3;
    private bool _IsShowInterAD = false;

    private string _InterADPlacementID = "InterAD";

    public bool IsShowInterAD
    {
        get
        {
            //return false;
            return _IsShowInterAD;
        }
    }

    public void PrepareInterAD()
    {
        //PlatformHelper.Instance.LoadInterAD();
        //LoadNativeNannerAd();
        Advertisement.Load(_InterADPlacementID);
    }

    public void LoadedInterAD()
    {
        _IsInterADPrepared = true;
        _StartInterADTime = 0;
    }

    public void ShowInterAD()
    {
        _IsInterADPrepared = false;
        _StartInterADTime = Time.time;
        //_InterADExposure = true;
        //PlatformHelper.Instance.ShowInterAD();
        //ShowNativeIntersititialAd();
        Advertisement.Show(_InterADPlacementID);
    }

    public bool IsShowInterADFinish()
    {
        if (!_InterADExposure)
            return true;

        //if (Time.time - _StartInterADTime > _ShowInterADTime)
        //{
        //    CloseInterAD();
        //    return true;
        //}
        return false;
    }

    public void CloseInterAD()
    {
        
    }

    public void OnInterADExposure()
    {
        Debug.Log("OnInterADExposure:" + _InterADExposure);
        _InterADExposure = true;
    }

    public void OnInterADClosed()
    {
        Debug.Log("OnInterADExposure:" + _InterADExposure);
        _InterADExposure = false;
    }

    public void AddLoadSceneTimes()
    {
        CloseInterAD();
        ++_LoadSceneTimes;
        if (_LoadSceneTimes % _ShowAdLoadTimes == 0)
        {
            LoadedInterAD();
            _IsShowInterAD = true;
        }
        else
        {
            _IsShowInterAD = false;
        }
    }

    #endregion

    #region unity ad

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == _InterADPlacementID)
        {
            LoadedInterAD();
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == _VideoPlacementID && showResult == ShowResult.Finished)
        {
            WatchAdVideoFinish();
        }
        else if(placementId == _InterADPlacementID)
        {
            OnInterADExposure();
        }
    }

    #endregion
}
