using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// 広告を表示するためのクラス
/// </summary>
public class UnityAds : MonoBehaviour,IUnityAdsListener
{
    /// <summary>
    /// グーグル用のゲームID
    /// </summary>
    private readonly int GoogleID = 4157423;

    //const int IOSID = 4157422;

    /// <summary>
    /// 動画広告を流すためのID
    /// </summary>
    private readonly string AdVideoID = "video";

    /// <summary>
    /// リワード広告を流すためのID
    /// </summary>
    private readonly string AdRewardedVideoID = "rewardedVideo";

    /// <summary>
    /// ゲームを統括している部分を参照する
    /// </summary>
    GameMaster gamemaster = null;

    /// <summary>
    /// 動画広告を流す
    /// </summary>
    public bool AdVideoStart()
    {
        //広告の取得ができていない場合はじく
        if (!Advertisement.IsReady())
        {
            return false;
        }

        //実行したい広告の準備ができているか取得する
        PlacementState videoready = Advertisement.GetPlacementState(AdVideoID);
        //準備できていない場合はじく
        if (videoready != PlacementState.Ready)
        {
            return false;
        }

        //動画の再生をする
        Advertisement.Show(AdVideoID);
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {

        gamemaster = GetComponent<GameMaster>();

        //広告を初期化する
        //第二引数にtrueを入れるとダミー広告
        Advertisement.Initialize(GoogleID.ToString(), true);

        Advertisement.AddListener(this);
    }

    //広告準備完了
    public void OnUnityAdsReady(string id)
    {

    }

    //広告でエラー
    public void OnUnityAdsDidError(string id)
    {
        gamemaster.UnPause();
    }

    //広告開始
    public void OnUnityAdsDidStart(string id)
    {

    }

    //広告が終了
    public void OnUnityAdsDidFinish(string id,ShowResult result)
    {
        gamemaster.UnPause();
    }

}
