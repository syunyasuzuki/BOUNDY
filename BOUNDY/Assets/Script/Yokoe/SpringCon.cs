using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringCon : MonoBehaviour
{
    /// <summary>
    /// ジャンプ力
    /// </summary>
    public static Vector2 jumpforce = new Vector2(0.0f,5.0f);

    /// <summary>
    /// プレイヤーに触れた回数
    /// </summary>
    int pl_touchct = 0;

    private void OnCollisionEnter2D(Collision2D col)
    {
        //当たったオブジェクトのタグがプレイヤーの場合
        if(col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpforce, ForceMode2D.Impulse);
            jumpforce.y = 5.0f;
            pl_touchct += 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pl_touchct==1)
        {
            Destroy(gameObject);
            pl_touchct = 0;
            GameDirector.Spring_ct = 0;
        }
    }
}
