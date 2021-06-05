using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    #region Title

    /// <summary>
    /// タイトルを表示しているテキストと
    /// その下に表示しているテキスト
    /// </summary>
    [SerializeField] Text[] TitleText = new Text[2];

    //フェードの状態
    enum Fade
    {
        FadeOut = -1,
        NotFade = 0,
        FadeIn = 1
    }

    //現在のフェードの状態
    private Fade nowfade = Fade.NotFade;

    void Title_task()
    {

    }


    #endregion

    #region Score

    /// <summary>
    /// 位置を監視する対象
    /// </summary>
    [SerializeField]Transform PlayerTransform = null;

    /// <summary>
    /// スコアを表示するテキスト
    /// </summary>
    [SerializeField] Text ScoreText = null;

    /// <summary>
    /// 最高到達地点
    /// </summary>
    private float highestpoint = 0;

    /// <summary>
    /// 最高到達地点を書き換える
    /// </summary>
    public void SetHighestPoint(float n)
    {
        highestpoint = n;
    }

    /// <summary>
    /// 最高到達地点を取得する
    /// </summary>
    public float GetHighestPoint()
    {
        return highestpoint;
    }

    /// <summary>
    /// スコアの処理
    /// </summary>
    void Score_task()
    {
        //スコア用のテキストUIがない場合処理をはじく
        if (ScoreText == null) return;

        //最高到達地点を超えた場合に最高到達地点を書き換える
        if (highestpoint < PlayerTransform.position.y)
        {
            highestpoint = PlayerTransform.position.y;
            ScoreText.text = highestpoint + "m";
        }
    }

    #endregion

    /// <summary>
    /// ゲームの状態
    /// </summary>
    private enum Mode
    {
        Title = 0,
        Game = 1,
        Result = 2
    }

    /// <summary>
    /// 現在のゲームの状態
    /// </summary>
    private Mode nowmode = Mode.Title;

    //ゲームの状態を切り替える
    private void ChangeMode(Mode nextmode)
    {
        //現在使われているものを非アクティブ化
        switch (nowmode)
        {
            case Mode.Title:
                break;
            case Mode.Game:
                break;
            case Mode.Result:
                break;
        }

        //次必要なものをアクティブ化
        switch (nextmode)
        {
            case Mode.Title:
                break;
            case Mode.Game:
                break;
            case Mode.Result:
                break;
        }

        nowmode = nextmode;
    }


    void Start()
    {
        //プレイヤーが存在しない場合生成する（統合前のエラー回避用）
        if (PlayerTransform == null) 
        {
            new GameObject("Player");
        }

        //プレイヤーのトランスフォームを参照
        PlayerTransform = GameObject.Find("Player").transform;
    }

    void Update()
    {
        switch (nowmode)
        {
            case Mode.Title:

                break;
            case Mode.Game:
                Score_task();
                break;
            case Mode.Result:

                break;
        }
    }
}
