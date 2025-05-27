using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArcRenderer : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject segmentPrefab;
    [SerializeField] int segmentPoolSize = 50;
    private List<GameObject> segmentPool = new List<GameObject>();
    private GameObject arrow;

    [SerializeField] float segmentSpacing = 50;
    [SerializeField] float arrowAngle = 0;
    private Vector3 arrowDirection;

    // Start is called before the first frame update
    void Start()
    {
        arrow = Instantiate(arrowPrefab, transform);
        arrow.transform.localPosition = Vector3.zero;
        InitialiseSegments();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;

        Vector3 startPos = transform.position;
        Vector3 arcMidpoint = CalculateMidpoint(startPos, mousePos);

        UpdateArc(startPos, arcMidpoint, mousePos);
        // Position the arrow
        arrow.transform.position = mousePos;
        Vector3 direction = arrowDirection - mousePos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += arrowAngle;
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void UpdateArc(Vector3 start, Vector3 midpoint, Vector3 end)
    {
        int segmentNum = Mathf.CeilToInt(Vector3.Distance(start, end) / segmentSpacing);
        if (segmentNum < 2) return;
        if (segmentNum > segmentPoolSize) segmentNum = segmentPoolSize;
        for(int i = 0; i < segmentNum; i++)
        {
            float t = Mathf.Clamp(i / (float)segmentNum, 0f, 1f);
            Vector3 position = QuadraticBezierPoint(start, midpoint, end, t);
            // Skip one dot to make room for arrowhead
            if( i < segmentNum - 1)
            {
                segmentPool[i].transform.position = position;
                segmentPool[i].SetActive(true);
            }
            // end of arc, finalise arrow direction
            else
            {
                arrowDirection = segmentPool[i-1].transform.position;
            }
        }
        // Deactivate inactive segments
        for(int x = segmentNum - 1; x < segmentPoolSize; x++)
        {
            if(x > 0)
            {
                segmentPool[x].SetActive(false);
            }
        }
    }

    private Vector3 QuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = (1 - t);
        return (u * u) * p0 + (2 * u * t) * p1 + (t * t) * p2;
    }

    private Vector3 CalculateMidpoint(Vector3 startPos, Vector3 mousePos)
    {
        Vector3 midpoint = (startPos + mousePos) / 2;
        float arcHeight = Vector3.Distance(startPos, mousePos) / 3f;
        midpoint.y += arcHeight;
        return midpoint;
    }

    private void InitialiseSegments()
    {
        for(int i = 0; i < segmentPoolSize; i++)
        {
            GameObject segment = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity, transform);
            segment.SetActive(false);
            segmentPool.Add(segment);
        }
    }
}
