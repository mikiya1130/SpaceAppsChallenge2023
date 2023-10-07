using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveOnLine : MonoBehaviour
{
    private LineRenderer lineComponent = null;
    [SerializeField, Tooltip("速度[m/sec]")] float speed = 1.5f;
    int lineComponentPtr;
    float[] costs;
    float remain;

    // Start is called before the first frame update
    void Start()
    {
        int[] values = Enumerable.Range(0, 100).ToList().Select(_ => Random.Range(0, 255)).ToArray();
        lineComponent = generateLineRenderer(values);
        init();
    }

    // Update is called once per frame
    void Update()
    {
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
        transform.position = basePos;
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

    private LineRenderer generateLineRenderer(int[] values)
    {
        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        var lineRenderer = GetComponent<LineRenderer>();
        List<int> indexes = Enumerable.Range(0, values.Length).ToList();
        Vector3[] positions = indexes.Select(i => new Vector3(i * 2.5f, ((values[i] / 255.0f) * 5.0f) - 2.5f, 0.0f)).ToArray();
        lineRenderer.positionCount = positions.Length;
        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);
        return lineRenderer;
    }
}