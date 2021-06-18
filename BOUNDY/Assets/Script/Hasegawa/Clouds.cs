using UnityEngine;

public class Clouds : MonoBehaviour
{
    /// <summary>
    /// カメラを追従させる対象
    /// </summary>
    [SerializeField] Transform player = null;

    //生成する雲のスプライト
    [SerializeField] Sprite[] CloudsSprite = null;

    /// <summary>
    /// 生成した雲
    /// </summary>
    private GameObject[] clouds = null;

    /// <summary>
    /// 生成した雲のスピード
    /// </summary>
    private float[] cloudspeed = null;

    /// <summary>
    /// 生成した雲の進む方向
    /// </summary>
    private int cloudvecx = 0;

    /// <summary>
    /// 雲を生成する
    /// </summary>
    private GameObject CreateCloud(int spritenumber)
    {
        GameObject cloud = new GameObject("cloud");
        cloud.AddComponent<SpriteRenderer>().sprite = CloudsSprite[spritenumber];
        cloud.GetComponent<SpriteRenderer>().sortingOrder = -5;
        return cloud;
    }

    /// <summary>
    /// 雲の位置をリセットする
    /// </summary>
    public void ResetCloudsPos()
    {
        for(int i = 0; i < CloudsSprite.Length; ++i)
        {
            clouds[i].transform.position = new Vector3(Camera.main.transform.position.x + Random.Range(-3.8125f, 3.8125f), Camera.main.transform.position.y + Random.Range(-5.5f, 5.5f), 0);
        }
    }

    private void Awake()
    {
        clouds = new GameObject[CloudsSprite.Length];
        cloudspeed = new float[CloudsSprite.Length];
        cloudvecx = Random.Range(0, 2) == 0 ? -1 : 1;
        for (int i = 0; i < CloudsSprite.Length; ++i)
        {
            clouds[i] = CreateCloud(i);
            cloudspeed[i] = Random.Range(0.5f, 1f);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;

        if (player == null) return;

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        for(int i = 0; i < CloudsSprite.Length; ++i)
        {
            clouds[i].transform.position += new Vector3(cloudspeed[i] * cloudvecx * Time.deltaTime, 0, 0);

            if ((cloudvecx==-1 && clouds[i].transform.position.x < -4f) || (cloudvecx == 1 && clouds[i].transform.position.x > 4f))
            {
                clouds[i].transform.position = new Vector3(4f * cloudvecx * -1, Camera.main.transform.position.y + Random.Range(-5.5f, 5.5f), 0);
            }
            if(clouds[i].transform.position.y > Camera.main.transform.position.y + 5.5f)
            {
                clouds[i].transform.position = new Vector3(clouds[i].transform.position.x, Camera.main.transform.position.y - 5.5f, 0);
            }
            if(clouds[i].transform.position.y < Camera.main.transform.position.y - 5.5f)
            {
                clouds[i].transform.position = new Vector3(clouds[i].transform.position.x, Camera.main.transform.position.y + 5.5f, 0);
            }
        }
    }
}
