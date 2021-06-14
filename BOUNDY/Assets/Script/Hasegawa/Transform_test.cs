using UnityEngine;

public class Transform_test : MonoBehaviour
{
    [SerializeField] GameObject obj = null;

    GameObject[] obj2 = new GameObject[5];

    RectangleCollisionDetection test = null;

    GameObject cube = null;

    // Start is called before the first frame update
    void Start()
    {
        test = GetComponent<RectangleCollisionDetection>();
        for(int i = 0; i < obj2.Length; ++i)
        {
            obj2[i] = Instantiate(obj);
            obj2[i].transform.position = new Vector3(Random.Range(-2f, 2f), Random.Range(0, 7f), 0);
            test.SetTransformList(obj2[i].transform);
        }

        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition-Camera.main.transform.position);
            cube.transform.position = pos;
            test.CollisionDetection(pos, false);
        }
    }
}
