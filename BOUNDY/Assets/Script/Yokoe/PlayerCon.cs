using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのSprite
    /// </summary>
    [SerializeField] Sprite[] player_spt = new Sprite[4];

    /// <summary>
    /// ばねのSprite
    /// </summary>
    public Sprite[] spring_sprite = new Sprite[4];

    /// <summary>
    /// プレイヤーのSpriteRenderer
    /// </summary>
    SpriteRenderer player_sptr = null;

    /// <summary>
    /// プレイヤーのRigidbody2D
    /// </summary>
    private Rigidbody2D rb;

    /// <summary>
    /// どのアニメーションを使っているか
    /// </summary>
    int spt_ct = 0;

    /// <summary>
    /// アニメーション用のカウント
    /// </summary>
    float anim_ct = 0;

    /// <summary>
    /// 速度
    /// </summary>
    private Vector3 speed;

    /// <summary>
    /// ジャンプ力
    /// </summary>
    float jumpforce = 1;

    /// <summary>
    /// ばねのSpriteRenderer
    /// </summary>
    SpriteRenderer spring_sptr = null;

    /// <summary>
    /// ばね
    /// </summary>
    GameObject spring = null;

    /// <summary>
    /// アニメーション間隔
    /// </summary>
    float[] spt_anim_ct = { 2f, 2f, 2f, 2f };

    /// <summary>
    /// プレイヤーのアニメーションの順番
    /// </summary>
    int[] player_spt_ct = { 1, 1, 2, 3 };

    /// <summary>
    /// ばねの強さ
    /// </summary>
    float spring_value = 0;

    /// <summary>
    /// ばねの番号
    /// </summary>
    int spring_num = 0;

    enum Playermode
    {
        Ground = 0,
        Fly = 1,
        Onspring = 2,
    }

    Playermode now_playermode = Playermode.Fly;

    enum Anim_mode
    {
        Down = 0,
        Up = 1,
    }

    Anim_mode now_anim_mode = 0;

    SpringCon springcon;

    // Start is called before the first frame update
    void Start()
    {
        springcon =GameObject.Find("GameDirector").GetComponent<SpringCon>();
        //  Rigidbody&SpriteRenderer取得
        rb = GetComponent<Rigidbody2D>();
        player_sptr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ////移動
        //if(Input.GetKey(KeyCode.RightArrow))
        //{
        //    transform.position += new Vector3(0.1f, 0.0f, 0.0f);
        //}
        //if(Input.GetKey(KeyCode.LeftArrow))
        //{
        //    transform.position += new Vector3(-0.1f, 0.0f, 0.0f);
        //}

        switch(now_playermode)
        {
            case Playermode.Ground:
                //フリックの処理
                break;
            case Playermode.Onspring:
                anim_ct += Time.deltaTime;
                switch (now_anim_mode)
                {
                    case Anim_mode.Down:
                        if(anim_ct>=spt_anim_ct[spt_ct])
                        {
                            //プレイヤーのSprite差し替え
                            player_sptr.sprite = player_spt[player_spt_ct[spt_ct]];
                            //ばねのSprite差し替え
                            spring_sptr.sprite = spring_sprite[spt_ct];
                            anim_ct = 0;

                            transform.position = new Vector3(transform.position.x, spring.transform.position.y + (1f / 10) * 7 - (1f / 10) * 2 * spt_ct, 0);

                            spt_ct++;

                            if (spt_ct==2)
                            {
                                now_anim_mode = Anim_mode.Up;
                            }
                        }

                        break;
                         
                    case Anim_mode.Up:
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
                                now_anim_mode = Anim_mode.Up;
                                now_playermode = Playermode.Fly;
                                rb.isKinematic = false;
                                rb.AddForce(new Vector2(0.0f,jumpforce+spring_value), ForceMode2D.Impulse);
                                jumpforce = 5.0f;
                                GameDirector.Spring_ct = 0;
                                springcon.DeleteSpring(spring_num);
                            }
                        }
                        break;
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
            //取得
            spring_sptr = col.gameObject.GetComponent<SpriteRenderer>();
            spring = col.gameObject;
            anim_ct = 0;
            spt_ct = 0;
            now_playermode = Playermode.Onspring;
            now_anim_mode = Anim_mode.Down;
            player_sptr.sprite =player_spt[3];
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            Destroy(col.gameObject.GetComponent<BoxCollider2D>());
            spring_num = int.Parse(col.gameObject.name);
            spring_value = springcon.GetSpringpower(spring_num);
        }
    }
}
