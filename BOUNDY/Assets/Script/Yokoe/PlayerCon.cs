using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Vector2 JumpForce = new Vector2(1, 1);

    #endregion

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
    float[] spt_anim_ct = { 2f, 2f, 2f, 2f };

    /// <summary>
    /// プレイヤーのアニメーションの順番
    /// </summary>
    int[] player_spt_ct = { 1, 1, 2, 3 };

    #region ばね関連

    /// <summary>
    /// ばねの情報を取得するための参照先
    /// </summary>
    SpringCon springcon;

    /// <summary>
    /// 着地したばね
    /// </summary>
    private GameObject spring = null;

    /// <summary>
    /// 着地したばねのSpriteRenderer
    /// </summary>
    private SpriteRenderer spring_sptr = null;

    /// <summary>
    /// 着地したばねの強さ
    /// </summary>
    private float spring_value = 0;

    /// <summary>
    /// 着地したばねの番号
    /// </summary>
    int spring_num = 0;

    #endregion

    /// <summary>
    /// スクリプト内の処理の切り替え用
    /// </summary>
    enum Playermode
    {
        Ground = 0,//プレイヤーが地面にいるとき
        Fly = 1,//プレイヤーが空中にいるとき
        Down = 3,//プレイヤーがばねにふれてばねが下がっていくとき
        Up = 1//プレイヤーがばねにふれてばねが上がっていくとき
    }

    /// <summary>
    /// 現在の処理
    /// </summary>
    Playermode now_playermode = Playermode.Fly;

    // Start is called before the first frame update
    void Start()
    {
        //ばねの情報を参照する
        springcon =GameObject.Find("GameDirector").GetComponent<SpringCon>();
        //  Rigidbody&SpriteRenderer取得
        rb = GetComponent<Rigidbody2D>();
        player_sptr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(now_playermode)
        {
            case Playermode.Ground:
                //フリックの処理

                break;
            case Playermode.Down:
                anim_ct += Time.deltaTime;
                if (anim_ct >= spt_anim_ct[spt_ct])
                {
                    //プレイヤーのSprite差し替え
                    player_sptr.sprite = player_spt[player_spt_ct[spt_ct]];
                    //ばねのSprite差し替え
                    spring_sptr.sprite = spring_sprite[spt_ct];
                    anim_ct = 0;

                    transform.position = new Vector3(transform.position.x, spring.transform.position.y + (1f / 10) * 7 - (1f / 10) * 2 * spt_ct, 0);

                    spt_ct++;

                    if (spt_ct == 2)
                    {
                        now_playermode = Playermode.Up;
                    }
                }
                break;
            case Playermode.Up:
                anim_ct += Time.deltaTime;
                if (anim_ct >= spt_anim_ct[spt_ct])
                {
                    //プレイヤーのSprite差し替え
                    player_sptr.sprite = player_spt[player_spt_ct[spt_ct]];
                    //ばねのSprite差し替え
                    spring_sptr.sprite = spring_sprite[spt_ct];
                    spt_ct++;
                    anim_ct = 0;

                    if (spt_ct == 3)
                    {
                        now_playermode = Playermode.Fly;
                        rb.isKinematic = false;
                        rb.AddForce(new Vector2(JumpForce.x, JumpForce.y + spring_value), ForceMode2D.Impulse);
                        GameDirector.Spring_ct = 0;
                        springcon.DeleteSpring(spring_num);
                    }
                }
                break;
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag==("Wall"))
        {
            transform.localScale = new Vector2(transform.localScale.x * -1.0f, transform.localScale.y);
        }

        if(col.gameObject.tag==("Spring") && now_playermode == Playermode.Fly&&transform.position.y>col.transform.position.y)
        {
            //着地したばねのスプライトレンダラーを取得
            spring_sptr = col.gameObject.GetComponent<SpriteRenderer>();
            //着地したばねを取得
            spring = col.gameObject;
            //着地したばねのColliderを削除
            Destroy(col.gameObject.GetComponent<BoxCollider2D>());
            //着地したばねの番号を名前から取得
            spring_num = int.Parse(col.gameObject.name);
            //着地したばねの強さを取得
            spring_value = springcon.GetSpringpower(spring_num);
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
