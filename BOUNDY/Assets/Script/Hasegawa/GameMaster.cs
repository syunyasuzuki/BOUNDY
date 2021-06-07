using UnityEngine;
using UnityEngine.UI;
using System;

public class GameMaster : MonoBehaviour
{
    #region Title

    /// <summary>
    /// タイトルを表示しているテキストと
    /// その下に表示しているテキスト
    /// </summary>
    [SerializeField] Text[] TitleText = new Text[2];

    /// <summary>
    /// タイトルのときのカメラの位置
    /// </summary>
    private Vector3 TitlePos = new Vector3(-3, 3, -10);

    /// <summary>
    /// ゲーム開始時のカメラの位置
    /// </summary>
    private Vector3 GameStartPos = new Vector3(0, 3, -10);

    /// <summary>
    /// フェードにかける時間
    /// </summary>
    const float FadeTime = 3f;

    /// <summary>
    /// フェードした時間
    /// </summary>
    private float time = 0;

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
        switch (nowfade)
        {
            //タイトルでの入力待ち
            case Fade.NotFade:
                if (Input.GetMouseButtonDown(0))
                {
                    nowfade = Fade.FadeIn;
                    time = 0;
                }
                break;
            //ゲームシーンへ
            case Fade.FadeIn:
                Vector3 incamerapos = Camera.main.transform.position;
                Camera.main.transform.position += new Vector3((GameStartPos.x - incamerapos.x) / (FadeTime - time), (GameStartPos.y - incamerapos.y) / (FadeTime - time), 0);
                TitleText[0].color = TitleText[1].color = new Color(1, 1, 1, 1 / FadeTime * (FadeTime - time));
                time += Time.deltaTime;
                if (time >= FadeTime)
                {
                    Camera.main.transform.position = GameStartPos;
                    nowfade = Fade.FadeOut;
                    time = 0;
                    ChangeMode(Mode.Game);
                }
                break;
            //タイトルシーンへ
            case Fade.FadeOut:
                time += Time.deltaTime;
                Vector3 outcamerapos = Camera.main.transform.position;
                Camera.main.transform.position += new Vector3((TitlePos.x - outcamerapos.x) / (FadeTime - time), (TitlePos.y - outcamerapos.y) / (FadeTime - time), 0);
                TitleText[0].color = TitleText[1].color = new Color(1, 1, 1, 1 / FadeTime * time);
                time += Time.deltaTime;
                if (time >= FadeTime)
                {
                    Camera.main.transform.position = TitlePos;
                    TitleText[0].color = TitleText[1].color = new Color(1, 1, 1, 1);
                    nowfade = Fade.NotFade;
                    time = 0;
                }
                break;
        }
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
    private float BestScore { get; set; } = 0;

    /// <summary>
    /// 現在の到達地点
    /// </summary>
    private float Score { get; set; } = 0;

    /// <summary>
    /// スコアの処理
    /// </summary>
    void Score_task()
    {
        //スコア用のテキストUIがない場合処理をはじく
        if (ScoreText == null) return;

        //到達地点を超えた場合に到達地点を書き換える
        if (Score < PlayerTransform.position.y)
        {
            Score = PlayerTransform.position.y;
            ScoreText.text = Math.Round(Score, 2) + "m";
        }
    }

    #endregion

    #region Result

    /// <summary>
    /// リザルトを表示するテキスト
    /// </summary>
    [SerializeField] Text[] ResultText = new Text[2];

    /// <summary>
    /// リザルト画面での処理の状態
    /// </summary>
    private enum ResultMode
    {
        Animation = 0,//現在リザルトアニメーション中
        HighAnimation = 1,//現在リザルトアニメーション中
        Weit = 2,//待機状態
    }

    /// <summary>
    /// 現在のリザルト画面での処理の状態
    /// </summary>
    private ResultMode nowresultmode = ResultMode.Animation;

    /// <summary>
    /// スコア表示に使う
    /// </summary>
    private string score_str = "";

    /// <summary>
    /// ベストスコア表示に使う
    /// </summary>
    private string bestscore_str = "";

    /// <summary>
    /// リザルトでの表示に使う時間
    /// </summary>
    private float resulttime = 0;

    /// <summary>
    /// リザルトの文字を表示する速度
    /// </summary>
    const float ResultTransitionSpeed = 0.2f;

    /// <summary>
    /// リザルトで表示する文字数
    /// </summary>
    private int wordcount = 0;

    /// <summary>
    /// リザルト表示する文字を現在どこまで表示したか
    /// </summary>
    private int count = 0;

    /// <summary>
    /// リザルトを初期化
    /// </summary>
    void Result_set()
    {
        resulttime = 0;
        score_str = Math.Round(Score, 2).ToString();
        bestscore_str = Math.Round(BestScore, 2).ToString();
        wordcount = score_str.Length + bestscore_str.Length;
        nowresultmode = ResultMode.Animation;
    }

    /// <summary>
    /// リザルトでの処理
    /// </summary>
    void Result_task()
    {
        switch (nowresultmode)
        {
            case ResultMode.Animation:
                //
                resulttime += Time.deltaTime;
                if (resulttime >= ResultTransitionSpeed)
                {
                    resulttime = 0;
                    if (++count < score_str.Length)
                    {

                    }
                    else
                    {

                    }
                }

                //全ての文字を表示出来たら次へ移動する
                if (count == wordcount)
                {

                }
                break;
            case ResultMode.HighAnimation:
                break;
            case ResultMode.Weit:
                break;
        }
    }

    #endregion

    /// <summary>
    /// 現在実行中の処理
    /// </summary>
    Action nowtask = null;

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
        //念のためTimeScaleを元に戻す
        Time.timeScale = 1;

        //現在使われているもので使わないものを見えなくする
        switch (nowmode)
        {
            case Mode.Title:
                for(int i = 0; i < 2; ++i)
                {
                    TitleText[i].color = new Color(1, 1, 1, 0);
                }
                break;
            case Mode.Game:
                ScoreText.color = new Color(1, 1, 1, 0);
                ScoreText.text = "0m";
                break;
            case Mode.Result:
                for(int i = 0; i < 2; ++i)
                {
                    ResultText[i].color = new Color(1, 1, 1, 0);
                }
                break;
        }

        //次必要なものをアクティブ化
        switch (nextmode)
        {
            case Mode.Title:
                nowtask = Title_task;
                break;
            case Mode.Game:
                nowtask = Score_task;
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

        ChangeMode(Mode.Title);
    }

    void Update()
    {
        nowtask();
    }
}
