using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCon : MonoBehaviour
{
    #region 素材

    /// <summary>
    /// プレイヤーのSprite
    /// </summary>
    [SerializeField] Sprite[] player_spt = new Sprite[4];

    /// <summary>
    /// ばねのSprite
    /// </summary>
    [SerializeField] Sprite[] spring_sprite = new Sprite[4];

    #endregion

    #region プレイヤー関連

    /// <summary>
    /// プレイヤーのSpriteRenderer
    /// </summary>
    private SpriteRenderer player_sptr = null;

    /// <summary>
    /// プレイヤーのRigidbody2D
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// プレイヤーの基礎ジャンプ力
    /// </summary>
    private Vector2 JumpForce = new Vector2(3.0f, 5.0f);

    /// <summary>
    /// FlickCon
    /// </summary>
    FlickCon flickcon;

    #endregion

    #region アニメーション関連
    /// <summary>
    /// どのアニメーションを使っているか
    /// </summary>
    private int spt_ct = 0;

    /// <summary>
    /// アニメーション用のカウント
    /// </summary>
    private float anim_ct = 0;

    /// <summary>
    /// アニメーション間隔
    /// </summary>
    float[] spt_anim_ct = { 0.2f, 0.2f, 0.2f, 0.2f };

    /// <summary>
    /// プレイヤーのアニメーションの順番
    /// </summary>
    int[] player_spt_ct = { 1, 1, 2, 3 };
    #endregion

    SpringObject[] spring_obj = null;

    /// <summary>
    /// ばねの情報を取得するための参照先
    /// </summary>
    SpringCon springcon;

    /// <summary>
    /// 着地したばねの番号
    /// </summary>
    int spring_num = 0;

    /// <summary>
    /// スクリプト内の処理の切り替え用
    /// </summary>
    enum Playermode
    {
        Ground = 0,//プレイヤーが地面にいるとき
        Fly = 1,//プレイヤーが空中にいるとき
        Down = 2,//プレイヤーがばねにふれてばねが下がっていくとき
        Up = 3//プレイヤーがばねにふれてばねが上がっていくとき
    }

    /// <summary>
    /// 現在の処理
    /// </summary>
    Playermode now_playermode { get; set; } = Playermode.Ground;

    [SerializeField] GameObject Flick_Aicon = null;
    [SerializeField] Text Flick_Text = null;
    // Start is called before the first frame update
    void Start()
    {
        //ばねの情報を参照する
        springcon =GameObject.Find("GameMaster").GetComponent<SpringCon>();
        spring_obj = springcon.GetSpring();
        //  Rigidbody&SpriteRenderer取得
        rb = GetComponent<Rigidbody2D>();
        player_sptr = GetComponent<SpriteRenderer>();
        flickcon = GetComponent<FlickCon>();
        //プレイヤーのtransformを渡す
        GameObject.Find("GameMaster").GetComponent<RectangleCollisionDetection>().SetTransformList(transform);
    }

    // Update is called once per frame
    void Update()
    {
        switch(now_playermode)
        {
            case Playermode.Ground:
                Flick_Aicon.SetActive(true);
                Flick_Text.color = new Color(1, 1, 1, 1);
                //フリックの処理
                int Flick= flickcon.Flisk_task();
                if(Flick!=0)
                {
                    rb.isKinematic = false;
                    rb.AddForce(new Vector2(Flick * 2.0f, 8.0f), ForceMode2D.Impulse);
                    transform.localScale = new Vector3(Flick, 1, 1);
                    now_playermode = Playermode.Fly;
                    Flick_Aicon.SetActive(false);
                    Flick_Text.color = new Color(1, 1, 1, 0);
                }
                break;
            case Playermode.Fly:
                springcon.Create_task();
                break;
            case Playermode.Down:
                springcon.Create_task();
                anim_ct += Time.deltaTime;
                if (anim_ct >= spt_anim_ct[spt_ct])
                {
                    //Sprite差し替え
                    player_sptr.sprite = player_spt[player_spt_ct[spt_ct]];
                    spring_obj[spring_num].AnimeSpring(spt_ct);
                    anim_ct = 0;

                    transform.position = new Vector3(transform.position.x,spring_obj[spring_num].GetPos().y + (1f / 10) * 7 - (1f / 10) * 2 * spt_ct, 0);

                    spt_ct++;

                    if (spt_ct == 2)
                    {
                        now_playermode = Playermode.Up;
                    }
                }
                break;
            case Playermode.Up:
                springcon.Create_task();
                anim_ct += Time.deltaTime;
                if (anim_ct >= spt_anim_ct[spt_ct])
                {
                    //Sprite差し替え
                    player_sptr.sprite = player_spt[player_spt_ct[spt_ct]];
                    spring_obj[spring_num].AnimeSpring(spt_ct);
                    spt_ct++;

                    anim_ct = 0;

                    if (spt_ct == 3)
                    {
                        now_playermode = Playermode.Fly;
                        rb.isKinematic = false;
                        rb.AddForce(new Vector2(JumpForce.x*transform.localScale.x, JumpForce.y * (spring_obj[spring_num].springPower + 1)), ForceMode2D.Impulse);
                        GameDirector.Spring_ct = 0;
                        springcon.DeleteSpring(spring_num);
                    }
                }
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == ("Ground"))
        {
            now_playermode = Playermode.Ground;
        }
        if (col.gameObject.tag==("Wall"))
        {
            //反転
            transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, 1);
        }

        if(col.gameObject.tag==("Spring") && now_playermode == Playermode.Fly&&transform.position.y>col.transform.position.y)
        {
            now_playermode = Playermode.Down;

            //着地したばねの番号を名前から取得
            spring_num = col.gameObject.GetComponent<SpringObject>().springNum;
            springcon.SetUseSpringnum(spring_num);
            //アニメーション遷移用のカウントをリセット
            anim_ct = 0;
            //アニメーションの再生をリセット
            spt_ct = 0;
            //処理を切り替え
            now_playermode = Playermode.Down;
            //プレイヤーのスプライトを棒立ちに切り替え
            player_sptr.sprite =player_spt[3];
            //RigidBodyでの動きを停止
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }
    }
}
