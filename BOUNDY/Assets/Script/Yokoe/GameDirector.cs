using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    /// <summary>
    /// 配置したいオブジェクト
    /// </summary>
    [SerializeField] GameObject Spring;

    /// <summary>
    /// ばねの数
    /// </summary>
    public static int Spring_ct = 0;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            //長押ししている間ジャンプ力を上げる
            SpringCon.jumpforce.y += 0.1f;
            Time.timeScale = 0.3f;
        }

        if(Input.GetMouseButtonDown(0))
        {
            //マウスの座標取得
            Vector3 mouse_pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z));
            //ワールド座標に変換
            Vector3 world_pos = Camera.main.ScreenToWorldPoint(mouse_pos);

            
            //ばねが１つ以上置かれているときばねを消す
            if (Spring_ct != 0)
            {
                Spring_ct = 0;
                Destroy(GameObject.FindGameObjectWithTag("Spring"));
            }

            Spring_ct += 1;
            //マウスの座標にオブジェクト生成
            Instantiate(Spring, world_pos, Quaternion.identity);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Time.timeScale = 1.0f;
        }
    }

}
