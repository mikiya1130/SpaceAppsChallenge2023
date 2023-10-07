using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateField : MonoBehaviour
{
    public GameObject fieldPrefab;

    // Start is called before the first frame update
    void Start()
    {
        int[] values = { 1, 4, 3, 5, 2, 6 };  // TODO: 画素値の配列に置き換え
        float scale = 0.5f;  // 1ブロックのベースサイズ

        for(int i=0; i<values.Length; i++)
        {
            Vector3 pos = new Vector3(i * scale, values[i] * scale / 2.0f, 0.0f);
            GameObject field = Instantiate(fieldPrefab, pos, Quaternion.identity);
            field.transform.localScale = new Vector3(scale, values[i] * scale, scale);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
