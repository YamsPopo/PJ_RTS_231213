using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnit : MonoBehaviour
{
    public LineRenderer[] lineRenderers;
    public Transform[] targets;
    private void Start()
    {
        for(int i = 0; i < targets.Length; i++)
        {
            lineRenderers[i].positionCount = 2;
            lineRenderers[i].SetPosition(0, new Vector3(transform.position.x, 2.5f, transform.position.z)); // �θ��� �����Ͽ� ������ �޴´�..
            lineRenderers[i].SetPosition(1, targets[i].transform.position);
        }
    }

}
