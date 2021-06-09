using UnityEngine;
using UnityEngine.UI;

public class Flick_test : MonoBehaviour
{
    [SerializeField] Text ui = null;


    int count = 0;

    const int proccesing = 1;

    const int AccuracyLebel = 5;

    GameObject[] cube = new GameObject[AccuracyLebel];

    private Vector3[] pos = new Vector3[AccuracyLebel];

    private int poscount = 0;

    private float averagespeed = 0;

    void PosClear()
    {
        for(int i = 0; i < AccuracyLebel; ++i)
        {
            pos[i] = Vector3.zero;
        }
        poscount = 0;
        averagespeed = 0;
    }

    void CheckFlickSpeed()
    {
        float sum = 0;
        int max = AccuracyLebel > poscount ? poscount : AccuracyLebel;
        for(int i = 0; i < max - 1; ++i)
        {
            sum += (pos[i] - pos[i + 1]).sqrMagnitude;
        }
        averagespeed = sum / max * 10;
        ui.text = averagespeed.ToString();
    }

    void SavePos()
    {
        if (++count % proccesing != 0) return;

        pos[poscount % AccuracyLebel] = Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.position);
        cube[poscount % AccuracyLebel].transform.position = pos[poscount % AccuracyLebel];
        ++poscount;
    }


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < AccuracyLebel; ++i)
        {
            cube[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube[i].transform.localScale = new Vector3(0.2f, 0.2f, 1);
        }
    }

    public int Flick_task()
    {
        if (ui == null) return 0;

        if (Input.GetMouseButton(0))
        {
            SavePos();
            CheckFlickSpeed();
        }
        if (Input.GetMouseButtonUp(0))
        {

            if (averagespeed > 2f)
            {
                Debug.Log("フリック");
                return 1;
            }

            PosClear();
        }

        if (count == int.MaxValue) count = 0;

        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (ui == null) return;

        if (Input.GetMouseButton(0))
        {
            SavePos();
            CheckFlickSpeed();
        }
        if (Input.GetMouseButtonUp(0))
        {

            if (averagespeed > 2f) Debug.Log("フリック");

            PosClear();
        }

        if (count == int.MaxValue) count = 0;
    }
}
