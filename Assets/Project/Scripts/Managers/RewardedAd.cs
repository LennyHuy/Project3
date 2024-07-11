using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
 
public class RewardedAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";

    [Header("BUTTONS")]
    [SerializeField] Button _showAdButton;
    [SerializeField] Button _showAdButtonForCoins;

    [Header("SCRIPTS")]
    [SerializeField] private LeanAnimation leanAnimation;
    [SerializeField] private Player player;

    [Header("Values")]
    [SerializeField] private int maxFreeCoinsAds;
     [SerializeField] private int maxFreeReviveAds;

    string _adUnitId = null; // This will remain null for unsupported platforms

    private int adsWatchedForRevive;
    private int adsWatchedForCoins;
 
    void Awake()
    {   
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Disable the button until the ad is ready to show:
        //_showAdButton.interactable = false;
    }
    void Start() 
    {
        adsWatchedForRevive = 0;
        adsWatchedForCoins = 0;
        LoadAd();
    }
 
    // Call this public method when you want to get an ad ready to show.
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }
 
    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
    }
 
    // Implement a method to execute when the user clicks the button:
    public void ShowAdForRevive()
    {

        if(adsWatchedForRevive  == maxFreeReviveAds)
        {
            _showAdButton.interactable = false;
        } 
        else Advertisement.Show(_adUnitId, this);
    }
    public void ShowAdForCoins()
    {
        if(adsWatchedForCoins  == maxFreeCoinsAds)
        {
            _showAdButtonForCoins.interactable = false;
        } 
        else Advertisement.Show(_adUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    // This function will automatically activated once the ads complete
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            //Debug.Log("Unity Ads Rewarded Ad Completed");
            Time.timeScale = 1f;

            if (GameManager.gameState == GameState.Stop && GameManager.playerState == PlayerState.Idle ) 
            {
                CurrencyManager.Instance.AddCurrency(10);
                adsWatchedForCoins +=1;
            }
            else if (GameManager.gameState == GameState.Stop && GameManager.playerState == PlayerState.Die ) 
            {
                CurrencyManager.Instance.AddCurrency(player.reviveCost);
                player.Revive();
                adsWatchedForRevive += 1;
                leanAnimation.ClosePanel();
            }
            
        }
    }
 
    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        _showAdButton.interactable = false;
    }
 
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        _showAdButton.interactable = false;
    }
 
    public void OnUnityAdsShowStart(string adUnitId) 
    {
        // This function will automatically activated once the ads started
        Time.timeScale = 0f;
    }
    public void OnUnityAdsShowClick(string adUnitId) { }

}
