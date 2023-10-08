using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveOnLine : MonoBehaviour
{
    private LineRenderer lineComponent = null;
    [SerializeField, Tooltip("最高速度[m/sec]")] float maxSpeed = 6.0f;
    [SerializeField, Tooltip("加速度[m/s2]")] float acceleration = 0.005f;
    float speed = 1.5f;
    float maxRate = 1.0f;
    int lineComponentPtr;
    float[] costs;
    float remain;
    Vector3 vibrationPosition = new Vector3(0f, 0f, 0f);
    public TextAsset[] dataList;
    public int curentDataset = 0;

    // Start is called before the first frame update
    void Start()
    {
        lineComponent = generateLineRenderer(curentDataset);
        init();
    }

    // Update is called once per frame
    void Update()
    {
        speed += acceleration;
        float z = transform.rotation.z;

        if (z > 0f)
        {
            float rate = z / 0.5f;
            if (rate > 1.0f) rate = 1.0f;
            maxRate = 1.0f - (rate * 0.35f);
        }
        else
        {
            float rate = -z / 0.5f;
            if (rate > 1.0f) rate = 1.0f;
            maxRate = 1.0f + (rate * 0.35f);
        }

        if (maxSpeed * maxRate < speed)
        {
            speed = maxSpeed * maxRate;
        }

        // LineRenderer上を動く点については、以下のURLを参照
        // @see https://qiita.com/ELIXIR/items/2661a2ed72eb0ae2a0fc
        float delta = speed * Time.deltaTime;
        while (delta > 0f)
        {
            if (remain > delta)
            {
                remain -= delta;
                delta = 0f;
                break;
            }
            else
            {
                delta -= remain;
                lineComponentPtr = (lineComponentPtr + 1) % lineComponent.positionCount;
                if (lineComponentPtr == 0)
                {
                    curentDataset = (curentDataset + 1) % dataList.Count();
                    lineComponent = generateLineRenderer(curentDataset);
                }
                remain = costs[lineComponentPtr];
            }
        }
        Vector3 basePos = lineComponent.GetPosition(lineComponentPtr);
        if (remain > 0f)
        {
            float rate = 1f - remain / costs[lineComponentPtr];
            basePos += (lineComponent.GetPosition((lineComponentPtr + 1) % lineComponent.positionCount) - basePos) * rate;
        }
        if (!lineComponent.useWorldSpace)
        {
            basePos = lineComponent.transform.position + Vector3.Scale(lineComponent.transform.rotation * basePos, lineComponent.transform.lossyScale);
        }

        // 角度を決める
        Vector2 dt = lineComponent.GetPosition(lineComponentPtr + 1) - basePos;
        float rad = Mathf.Atan2(dt.y, dt.x);
        float degree = rad * Mathf.Rad2Deg;
        // 線形補間しながら角度をつける
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, degree), 10.0f * Time.deltaTime);


        float ry = Random.Range(-0.05f, 0.05f);
        vibrationPosition = new Vector3(0, ry, 0);

        transform.position = basePos + vibrationPosition;
        Camera.main.transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }

    private void init()
    {
        costs = new float[lineComponent.positionCount];
        Vector3 pos = lineComponent.GetPosition(0);
        for (int i = 0; i < lineComponent.positionCount; ++i)
        {
            Vector3 nextPos = lineComponent.GetPosition((i + 1) % lineComponent.positionCount);
            costs[i] = (nextPos - pos).magnitude;
            pos = nextPos;
        }
        if (!lineComponent.loop)
        {
            costs[lineComponent.positionCount - 1] = 0f;
        }
        lineComponentPtr = 0;
        remain = costs[0];
    }

    private LineRenderer generateLineRenderer(int num)
    {
        int[] values = dataList[num].text.Split(',').Select(int.Parse).ToArray();
        float[] rescaleValues = RescaleValues.rescale(values);
        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        var lineRenderer = GetComponent<LineRenderer>();
        List<int> indexes = Enumerable.Range(0, rescaleValues.Length).ToList();
        Vector3[] positions = indexes.Select(i => new Vector3(i * 1.5f, rescaleValues[i] - 2.5f, 0.0f)).ToArray();
        lineRenderer.positionCount = positions.Length;
        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);
        return lineRenderer;
    }
}