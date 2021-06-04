using UnityEngine;

public class Sample_H : MonoBehaviour
{
    //生成するばねのプレハブ
    [SerializeField] GameObject Spring = null;

    //ばねの最大個数
    const int MaxSpring = 3;

    //今までに生成したばねの合計
    int create_spring_count = 0;

    //今生成されているばねの合計
    int now_create_sum = 0;

    //ひとつ前に生成したばねの配列番号
    int past_spring_num = 0;

    //ひとつ前に生成したばねのスプライトレンダラー
    SpriteRenderer past_spring_sprr = null;

    //次にばねを生成するときの配列番号
    int next_spring_num = 0;

    //生成したばね
    GameObject[] springs = new GameObject[MaxSpring];

    //生成したばねの履歴
    int[] history_spring = new int[MaxSpring];

    //生成したばねの強さ
    float[] springs_value = new float[MaxSpring];

    //プレイヤーが使っているばねの番号（使っていない場合-1）
    private int use_spring_num = -1;

    //指定されたばねの強さを取得
    public float GetSpringValue(int n)
    {
        use_spring_num = n;
        return 1 / 0.3f * springvalue;
    }

    //履歴を指定された箇所から左詰めする
    void LeftShiftHistory(int n)
    {
        for(int i = n; i < MaxSpring - 1; ++i)
        {
            history_spring[i] = history_spring[i + 1];
        }
    }

    //履歴の中の指定されてた箇所を削除
    void DeleteHistory(int n)
    {
        int breakpoint = 0;
        for(int i = 0; i < MaxSpring; ++i)
        {
            if (history_spring[i] == n)
            {
                breakpoint = i;
                break;
            }
        }

        LeftShiftHistory(breakpoint);
    }

    //ばねを生成するとこ
    void CreateSpring()
    {
        //最大まで生成されている場合一番古いものを消す
        if(now_create_sum >= MaxSpring)
        {
            if (use_spring_num != -1 && history_spring[0] == use_spring_num)
            {
                Destroy(springs[history_spring[1]].gameObject);
                springs_value[history_spring[1]] = 0;
                next_spring_num = history_spring[1];
                LeftShiftHistory(history_spring[2]);
            }
            else
            {
                Destroy(springs[history_spring[0]].gameObject);
                springs_value[history_spring[0]] = 0;
            }
            --now_create_sum;
        }

        //ばね生成
        springs[next_spring_num] = Instantiate(Spring);
        springs[next_spring_num].name = next_spring_num.ToString();
        springs_value[history_spring[0]] = 0;
        past_spring_sprr = springs[next_spring_num].GetComponent<SpriteRenderer>();
        past_spring_num = next_spring_num;

        //生成したばねの位置変え
        Vector3 tuchpos = Camera.main.ScreenToWorldPoint(Input.mousePosition-Camera.main.transform.position);
        springs[past_spring_num].transform.position = tuchpos;

        //生成した回数加算
        ++create_spring_count;
        ++now_create_sum;

        //履歴を更新
        //履歴をもとに次生成する際の配列の番号を決定
        if(create_spring_count <= MaxSpring)
        {
            history_spring[next_spring_num] = next_spring_num;
            ++next_spring_num;
            if (next_spring_num == MaxSpring)
            {
                next_spring_num = 0;
            }
        }
        else
        {
            LeftShiftHistory(0);
            history_spring[MaxSpring - 1] = next_spring_num;
            next_spring_num = history_spring[0];
        }

        Debug.Log(history_spring[0] + "_" + history_spring[1] + "_" + history_spring[2]);
    }

    //指定された番号のばねを削除する
    public void DeleteSpring(int n)
    {
        //対象を削除
        Destroy(springs[n].gameObject);

        //生成されている回数減算
        --now_create_sum;

        //履歴から対象の番号を削除
        DeleteHistory(n);

        //次生成する際の配列番号を更新
        next_spring_num = n;

        //使用してない状態に戻す
        use_spring_num = -1;
    }

    private void Awake()
    {
        Input.multiTouchEnabled = false;
    }

    //ばねの強さ
    float springvalue = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateSpring();
            Time.timeScale = 0.3f;
        }
        if (Input.GetMouseButton(0))
        {
            springvalue = Mathf.Clamp(springvalue += Time.deltaTime, 0, 0.9f);
            springs_value[past_spring_num] = springvalue;
            //強さによる色変え
            float col = 1 / 0.3f * springvalue;
            past_spring_sprr.color = new Color(1, 1 - col, 1 - col, 1);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Time.timeScale = 1;
            //強さの確定
            springs_value[past_spring_num] = springvalue;
            springvalue = 0;
        }
    }
}
