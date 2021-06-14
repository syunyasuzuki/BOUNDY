using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringCon : MonoBehaviour
{
    /// <summary>
    /// ばねのプレハブ
    /// </summary>
    [SerializeField] SpringObject Spring = null;

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
    /// 生成したばね
    /// </summary>
    SpringObject[] Springs = new SpringObject[Max_springs];

    /// <summary>
    /// 生成したばねの履歴
    /// </summary>
    int[] history_spring =new int[Max_springs];

    /// <summary>
    /// プレイヤーが使っているばねの番号(使っていない場合：-1)
    /// </summary>
    private int use_spring_num = -1;

    Transform []alltrans = null;

    /// <summary>
    /// 当たり判定のScript
    /// </summary>
    RectangleCollisionDetection rcd;

     /// <summary>
     /// 履歴を指定された箇所から左詰めにする
     /// </summary>
     /// <param name="n">ばねの番号</param>
    void ShiftLeftSHistory(int n)
    {
        for (int i = n; i < Max_springs - 1; ++i) 
        {
            history_spring[i] = history_spring[i + 1];
        }
    }

    /// <summary>
    /// 履歴の中の指定された箇所を削除
    /// </summary>
    /// <param name="n">ばねの番号</param>
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
        ShiftLeftSHistory(breakpoint);
    }

    /// <summary>
    /// ばねを生成する
    /// </summary>
    void Create_Spring()
    {   
        int next = 0;
        //次に生成する場所を探す
        if(now_create_num==Max_springs)
        {
            if(use_spring_num!=-1&&history_spring[0]==use_spring_num)
            {
                next = history_spring[1];
                ShiftLeftSHistory(1);
            }
            else
            {
                next = history_spring[0];
            }     
            --now_create_num;
        }
        else
        {
            for(int i=0;i<Max_springs;++i)
            {
                if(!Springs[i].gameObject.activeSelf)
                {
                    next = i;
                    break;
                }
            }
        }

        //ばねの生成
        ////Debug.Log("AAAAAAA:" + next.ToString());
        Springs[next].springNum = next;

        //生成したばねの位置変更
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.position);
        Springs[next].gameObject.transform.position = pos;

        Springs[next].ResetSpring();


        Springs[next].gameObject.name = next.ToString() + "_" + next.ToString();
        Springs[next].gameObject.name = next.ToString() + "_" + next.ToString();
        past_spring_sprr = Springs[next].GetComponent<SpriteRenderer>();
        past_spring_num = next;

        Springs[next].gameObject.SetActive(true);


        //生成した回数を加算
        create_spring_count++;
        now_create_num++;

        //履歴を更新
        ShiftLeftSHistory(0);
        history_spring[Max_springs - 1] = next;

        Debug.Log(history_spring[0] + "_" + history_spring[1] + "_" + history_spring[2]);
    }

    /// <summary>
    /// ばねを削除
    /// </summary>
    /// <param name="n">ばねの番号</param>
    public void DeleteSpring(int n)
    {
        //対象を削除
        Springs[n].gameObject.SetActive(false);

        //生成されている回数減算
        now_create_num--;

        //履歴から対象の番号を削除
        DeleteHistory(n);

        //ばねを使用していない状態
        use_spring_num = -1;
    }

    public SpringObject[] GetSpring()
    {
        return Springs;
    }

    void Awake()
    {
        rcd = GameObject.Find("GameMaster").GetComponent<RectangleCollisionDetection>();

        //マルチタッチ無効化
        Input.multiTouchEnabled = false;

        for (int i = 0; i < Springs.Length; i++)
        {
            Springs[i] = Instantiate(Spring);
            Springs[i].gameObject.SetActive(false);
            rcd.SetTransformList(Springs[i].transform);
        }
    }

    /// <summary>
    /// ばねの強さ
    /// </summary>
    float springpower = 0;
    // Update is called once per frame
    public void Creare_task()
    {
      if(Input.GetMouseButtonDown(0))
        {
            //置いたときにオブジェクトに当たっているかどうか確認
            if(rcd.CollisionDetection(Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.position), true)==0)
            {
                Create_Spring();
                Time.timeScale = 0.3f;
            }
        }
      if(Input.GetMouseButton(0))
        {
            springpower = Mathf.Clamp(springpower += Time.deltaTime, 0, 0.6f);
            Springs[past_spring_num].springPower = springpower;
            //強さによる色変え
            float col = 1 / 0.3f * springpower;
            past_spring_sprr.color = new Color(1, 1 - col, 1 - col, 1);
        }
      if(Input.GetMouseButtonUp(0))
        {
            Time.timeScale = 1;
            Springs[past_spring_num].springPower = springpower;
            springpower = 0;
        }
    }
}
