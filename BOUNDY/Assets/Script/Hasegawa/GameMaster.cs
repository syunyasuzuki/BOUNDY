using UnityEngine;
using UnityEngine.UI;
using System;

public class GameMaster : MonoBehaviour
{
    /// <summary>
    /// 広告表示するためのクラス
    /// </summary>
    UnityAds unityads = null;

    /// <summary>
    /// ゲームの操作を無効化する
    /// </summary>
    private bool IsPause = false;

    /// <summary>
    /// ポーズを解除する
    /// </summary>
    public void UnPause()
    {
        IsPause = false;
    }

    /// <summary>
    /// 渡された値を小数点第2以下四捨五入しコンマ区切り文字列に変換する
    /// また最後に単位（m）もつける
    /// </summary>
    private string ScoreSep(float n)
    {
        return string.Format("{0:#,0.00}m", Math.Round(n >= 9999999.99 ? 9999999.99 : n, 2));
    }

    #region Title

    /// <summary>
    /// タイトルを表示しているテキストと
    /// その下に表示しているテキスト
    /// </summary>
    [SerializeField] Text[] TitleText = new Text[3];

    /// <summary>
    /// タイトルのときのカメラの位置
    /// </summary>
    private Vector3 TitlePos = new Vector3(-6, 3, -10);

    /// <summary>
    /// ゲーム開始時のカメラの位置
    /// </summary>
    private Vector3 GameStartPos = new Vector3(0, 3, -10);

    /// <summary>
    /// フェードにかける時間
    /// </summary>
    const float FadeTime = 1.5f;

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
                if (!IsPause && Input.GetMouseButtonDown(0))
                {
                    nowfade = Fade.FadeIn;
                    time = 0;
                }
                break;
            //ゲームシーンへ
            case Fade.FadeIn:
                Vector3 incamerapos = Camera.main.transform.position;
                Camera.main.transform.position += new Vector3((GameStartPos.x - incamerapos.x) / (FadeTime - time), (GameStartPos.y - incamerapos.y) / (FadeTime - time), 0);
                TitleText[0].color = TitleText[1].color = TitleText[2].color = new Color(1, 1, 1, 1 / FadeTime * (FadeTime - time));
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
    /// ヒントを表示するテキスト
    /// </summary>
    [SerializeField] Text HintText = null;

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
            ScoreText.text = ScoreSep(Score);
        }
    }

    ScoreFile scorefile = null;

    #endregion

    #region Result

    /// <summary>
    /// リザルトを表示するテキスト
    /// </summary>
    [SerializeField] Text ResultText = null;

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
    /// ベストスコア更新用に使う
    /// </summary>
    private string newbestscore_str = "";

    /// <summary>
    /// リザルトでの表示に使う時間
    /// </summary>
    private float resulttime = 0;

    /// <summary>
    /// リザルトの文字を表示する速度
    /// </summary>
    const float ResultTransitionSpeed = 0.1f;

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
        ResultText.color = new Color(1, 1, 1, 1);
        ResultText.text = "";
        TitleText[2].color = new Color(1, 1, 1, 1);
        TitleText[2].text = "";
        resulttime = 0;
        score_str = ScoreSep(Score);
        bestscore_str = ScoreSep(BestScore);
        wordcount = score_str.Length + bestscore_str.Length;
        count = 0;
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

                //現在のスコアとベストスコアを表示する
                resulttime += Time.deltaTime;
                if (resulttime >= ResultTransitionSpeed)
                {
                    resulttime = 0;
                    if (++count <= score_str.Length)
                    {
                        ResultText.text = score_str.Substring(0, count);
                    }
                    else
                    {
                        TitleText[2].text = bestscore_str.Substring(0, count - score_str.Length);
                    }
                }

                //全ての文字を表示するか表示中に画面をタップされた場合
                //処理をスキップして全ての文字を表示させる
                if (count == wordcount || Input.GetMouseButtonDown(0))
                {
                    resulttime = 0;
                    count = 0;
                    ResultText.text = score_str;
                    TitleText[2].text = bestscore_str;

                    //もし最高到達地点を超えていたハイライトの処理に移る
                    if (Score > BestScore)
                    {
                        BestScore = Score;
                        newbestscore_str = "NewBest:" + ScoreSep(BestScore);
                        wordcount = score_str.Length + newbestscore_str.Length;
                        string eir = "";
                        for(int i=0;i< newbestscore_str.Length - bestscore_str.Length; ++i)
                        {
                            eir += " ";
                        }
                        bestscore_str = eir + bestscore_str;
                        nowresultmode = ResultMode.HighAnimation;
                    }
                    //そうでない場合待機へ移動
                    else
                    {
                        nowresultmode = ResultMode.Weit;
                    }
                }
                break;
            case ResultMode.HighAnimation:

                //現在のスコアをハイライトした後とベストスコアをハイライト表示する
                resulttime += Time.deltaTime;
                if (resulttime >= ResultTransitionSpeed / 2f)
                {
                    resulttime = 0;
                    if (++count <= score_str.Length)
                    {
                        ResultText.text = "<color=#FFFF00>" + score_str.Substring(0, count) + "</color>" + score_str.Substring(count, score_str.Length - count);
                    }
                    else
                    {
                        TitleText[2].text = "<color=#FFFF00>" + newbestscore_str.Substring(0, count - score_str.Length) + "</color>" + bestscore_str.Substring(count - score_str.Length, bestscore_str.Length - count + score_str.Length);
                    }
                }

                //全ての文字をハイライト表示するか表示中に画面をタップされた場合
                //処理をスキップして全ての文字を表示させ、待機状態に移動する
                if (count == wordcount || Input.GetMouseButtonDown(0))
                {
                    resulttime = 0;
                    count = 0;
                    ResultText.color = new Color(1, 1, 0, 1);
                    TitleText[2].color = new Color(1, 1, 0, 1);
                    ResultText.text = score_str;
                    TitleText[2].text = newbestscore_str;
                    nowresultmode = ResultMode.Weit;
                }
                break;
            case ResultMode.Weit:
                if (Input.GetMouseButtonDown(0))
                {
                    ChangeMode(Mode.Title);
                }
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
                TitleText[0].color = TitleText[1].color = new Color(1, 1, 1, 0);
                break;
            case Mode.Game:
                ScoreText.color = new Color(1, 1, 1, 0);
                ScoreText.text = "0.00m";
                HintText.color = new Color(1, 1, 1, 0);
                break;
            case Mode.Result:
                TitleText[2].color = new Color(1, 1, 1, 1);
                ResultText.color = new Color(1, 1, 1, 0);
                TitleText[2].text = "Best:" + ScoreSep(BestScore);
                IsPause = true;
                scorefile.WriteScore(BestScore);
                unityads.AdVideoStart();
                break;
        }

        //次必要なものをアクティブ化
        switch (nextmode)
        {
            case Mode.Title:
                TitleText[0].color = TitleText[1].color = TitleText[2].color = new Color(1, 1, 1, 1);
                nowtask = Title_task;
                break;
            case Mode.Game:
                PlayerTransform.position = Vector3.zero;
                ScoreText.color = new Color(1, 1, 1, 1);
                ScoreText.text = "0.00m";
                nowtask = Score_task;
                HintText.color = new Color(1, 1, 1, 1);
                break;
            case Mode.Result:
                Result_set();
                nowtask = Result_task;
                break;
        }

        nowmode = nextmode;
    }

    /// <summary>
    /// 外部からリザルト画面に移す
    /// </summary>
    public void ChangeResult()
    {
        ChangeMode(Mode.Result);
    }


    void Start()
    {
        //広告を取得する
        unityads = GetComponent<UnityAds>();

        //スコアを書き出し読み込みするところを取得する
        scorefile = GetComponent<ScoreFile>();

        //最高スコアを取得する
        BestScore = scorefile.ReadScore();
        TitleText[2].text = "Best:" + ScoreSep(BestScore);


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
        DebugMode();
    }

    void DebugMode()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeResult();
        }
    }
}
