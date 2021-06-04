using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringCon : MonoBehaviour
{
    /// <summary>
    /// ばねのプレハブ
    /// </summary>
    [SerializeField] GameObject Spring = null;

    /// <summary>
    /// 生成できるばねの最大数
    /// </summary>
    const int Max_springs = 3;

    /// <summary>
    /// 今までに生成した数（合計）
    /// </summary>
    int create_spring_count = 0;

    /// <summary>
    /// 現在生成されているばねの合計数
    /// </summary>
    int now_create_num = 0;

   /// <summary>
   /// 1つ前に生成したばねの配列番号
   /// </summary>
    int past_spring_num = 0;

    /// <summary>
    /// 1つ前に生成したばねのSpriteRenderer
    /// </summary>
    SpriteRenderer past_spring_sprr = null;

    /// <summary>
    /// 次にばねを生成するときの配列番号
    /// </summary>
    int next_spring_num = 0;

    /// <summary>
    /// 生成したばね
    /// </summary>
    GameObject[] Springs = new GameObject[Max_springs];

    /// <summary>
    /// 生成したばねの履歴
    /// </summary>
    int[] history_spring =new int[Max_springs];

    /// <summary>
    /// 生成したばねの強さ
    /// </summary>
    float[] springs_value = new float[Max_springs];

    /// <summary>
    /// プレイヤーが使っているばねの番号(使っていない場合：-1)
    /// </summary>
    private int use_spring_num = -1;

    /// <summary>
    /// 指定されたばねの強さを取得
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public float GetSpringpower(int n)
    {
        use_spring_num = n;
        return 1 / 0.3f * springpower;
    }

     /// <summary>
     /// 履歴を指定された箇所から左詰めにする
     /// </summary>
     /// <param name="n"></param>
    void LeftHistory(int n)
    {
        for (int i = n; i < Max_springs - 1; ++i) 
        {
            history_spring[i] = history_spring[i + 1];
        }
    }

    /// <summary>
    /// 履歴の中の指定された箇所を削除
    /// </summary>
    /// <param name="n"></param>
    public void DeleteHistory(int n)
    {
        int breakpoint = 0;
        for(int i=0;i<Max_springs;++i)
        {
            if(history_spring[i]==n)
            {
                breakpoint = i;
                break;
            }
        }
        LeftHistory(breakpoint);
    }

    /// <summary>
    /// ばねを生成する
    /// </summary>
    void Create_Spring()
    {
       //最大まで生成されている場合1番古いものを消す
       if(now_create_num>=Max_springs)
        {
            if(use_spring_num!=-1&&history_spring[0]==use_spring_num)
            {
                Destroy(Springs[history_spring[1]].gameObject);
                next_spring_num = history_spring[1];
                LeftHistory(history_spring[2]);
            }
            else
            {
                Destroy(Springs[history_spring[0]].gameObject);
            }
            --now_create_num;
        }

        //ばねの生成
        Springs[next_spring_num] = Instantiate(Spring);
        Springs[next_spring_num].name = next_spring_num.ToString();
        past_spring_sprr = Springs[next_spring_num].GetComponent<SpriteRenderer>();
        past_spring_num = next_spring_num;

        //生成したばねの位置変更
        Vector3 touchpos = Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.position);
        Springs[past_spring_num].transform.position = touchpos;

        //生成した回数を加算
        create_spring_count++;
        now_create_num++;

        //履歴を更新
        //履歴をもとに次生成する際の配列の番号を決定
        if(create_spring_count<=Max_springs)
        {
            history_spring[next_spring_num] = next_spring_num;
            next_spring_num++;
            if(next_spring_num==Max_springs)
            {
                next_spring_num = 0;
            }
        }
        else
        {
            LeftHistory(0);
            history_spring[Max_springs - 1] = next_spring_num;
            next_spring_num = history_spring[0];
        }

        Debug.Log(history_spring[0] + "_" + history_spring[1] + "_" + history_spring[2]);
    }

    public void DeleteSpring(int n)
    {
        //対象を削除
        Destroy(Springs[n].gameObject);

        //生成されている回数減算
        now_create_num--;

        //履歴から対象の番号を削除
        DeleteHistory(n);

        //次に生成する際の配列の番号を更新
        next_spring_num = n;

        //ばねを使用していない状態
        use_spring_num = -1;
    }

    void Awake()
    {
        //マルチタッチ無効化
        Input.multiTouchEnabled = false;
    }

    /// <summary>
    /// ばねの強さ
    /// </summary>
    float springpower = 0;

    // Update is called once per frame
    private void Update()
    {
      if(Input.GetMouseButtonDown(0))
        {
            Create_Spring();
            Time.timeScale = 0.3f;
        }
      if(Input.GetMouseButton(0))
        {
            springpower = Mathf.Clamp(springpower += Time.deltaTime, 0, 0.9f);
            springs_value[past_spring_num] = springpower;
            //強さによる色変え
            float col = 1 / 0.3f * springpower;
            past_spring_sprr.color = new Color(1, 1 - col, 1 - col, 1);
        }
      if(Input.GetMouseButtonUp(0))
        {
            Time.timeScale = 1;
            springs_value[past_spring_num] = springpower;
            springpower = 0;
        }
    }
}
