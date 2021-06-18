using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringObject : MonoBehaviour
{
    /// <summary>
    /// ばねのSpriteRenderer
    /// </summary>
    [SerializeField] SpriteRenderer springSpriteRenderer;

    /// <summary>
    /// ばねの番号
    /// </summary>
    public int springNum { get; set; } = -1;

    /// <summary>
    /// ばねの強さ
    /// </summary>
    public float springPower { get; set; } = 1;

    /// <summary>
    /// ばねのSprite
    /// </summary>
    [SerializeField] Sprite[] spring_sprite = new Sprite[4];

    /// <summary>
    /// Playerの情報を取得するための参照先
    /// </summary>
    private PlayerCon player;

    /// <summary>
    /// 着地したばね
    /// </summary>
    private GameObject spring = null;

    /// <summary>
    /// プレイヤーオブジェクトをセットする
    /// </summary>
    /// <param name="pl"></param>
    void SetPlayer(PlayerCon pl)
    {
        player = pl;
    }

   public Vector3 GetPos()
    {
        return transform.position;
    }

    public void AnimeSpring(int spt_ct)
    {
        //ばねのSprite差し替え
        springSpriteRenderer.sprite = spring_sprite[spt_ct];
    }

    public void ResetSpring()
    {
        springSpriteRenderer.sprite = spring_sprite[3];
        springPower = 1;
    }
}
