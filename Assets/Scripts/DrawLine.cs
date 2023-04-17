using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    public float radius = 1f;
    public Material material;
    public float linewidth = 1f;
    private List<Vector3> vPath = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        int count = 60;
        for(int i = 0; i <= count+1; i++)
        {
            if(i == (count + 1))
            {
                float x = Mathf.Cos(2 * Mathf.PI / count) * radius;
                float y = transform.localPosition.y;
                float z = Mathf.Sin(2 * Mathf.PI / count) * radius;
                vPath.Add(new Vector3(x, y, z));
            }
            else
            {
                float x = Mathf.Cos(2 * Mathf.PI / count * i) * radius;
                float y = transform.localPosition.y;
                float z = Mathf.Sin(2 * Mathf.PI / count * i) * radius;
                vPath.Add(new Vector3(x, y, z));
            }
        }

        GameObject lineGroup = new GameObject("LineGroup");
        GameObject lineObject = new GameObject("RadarLine");
        LineRenderer line = lineGroup.AddComponent<LineRenderer>();
        line.material = material;
        line.useWorldSpace = false;
        line.positionCount = vPath.Count;
        line.startWidth = linewidth; line.endWidth = linewidth;
        line.SetPositions(vPath.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
