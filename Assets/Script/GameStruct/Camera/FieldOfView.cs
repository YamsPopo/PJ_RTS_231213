using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//https://www.youtube.com/watch?v=5NTmxDSKj-Q
public struct ViewCastInfo
{
    public Vector3 point;
    public float dst;
    public float angle;

    public ViewCastInfo(Vector3 _point, float _dst, float _angle)
    {
        point = _point;
        dst = _dst;
        angle = _angle;
    }
}


public class FieldOfView : MonoBehaviour
{
    public GameObject fov;
    // �þ� ������ �������� �þ� ����
    public float viewRadius;
    [Range(0, 360)] public float viewAngle;
    public float meshResolution;
    [Range(0, 100)] public float realMeshSize;

    //Mesh viewMesh;
    //public MeshFilter viewMeshFilter;

    //void Start()
    //{
    //    viewMesh = new Mesh();
    //    viewMesh.name = "View Mesh";
    //    viewMeshFilter.mesh = viewMesh;
    //}
    // y�� ���Ϸ� ���� 3���� ���� ���ͷ� ��ȯ�Ѵ�.
    private void Awake()
    {
        if (this.gameObject.GetComponent<PhotonView>().IsMine == false)
            realMeshSize = 0;
    }


    private void Start()
    {
        fov.transform.localScale = Vector3.one * realMeshSize;
    }
    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }

    void DrawFieldOfView()
    {
        // ���ø��� ���� ���ø����� ���������� ���� ũ�⸦ ���Ѵ�.
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        // ���ø��� ������ ���ϴ� ��ǥ�� ����Ͽ� stepCount���� ������ ����ؼ� ���� ������ �����.
        // ���� ������ �ִ� �Ÿ���ŭ�� ������ point�� �������ش�.
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;

            Vector3 dir = DirFromAngle(angle, true);
            ViewCastInfo newViewCast = new ViewCastInfo(transform.position + dir * viewRadius, viewRadius, angle);
            viewPoints.Add(newViewCast.point);
        }

        ////�޽��� �����Ѵ�.
        //int vertexCount = viewPoints.Count + 1;
        //Vector3[] vertices = new Vector3[vertexCount];
        //int[] triangles = new int[(vertexCount-2)*3];
        //vertices[0] = Vector3.zero;

        ////���ؽ� �ε��� �������ִ� �κ�
        //for (int i = 0; i < vertexCount - 1; i++)
        //{
        //    vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
        //    if (i < vertexCount - 2)
        //    {
        //        triangles[i * 3] = 0;
        //        triangles[i * 3 + 1] = i + 1;
        //        triangles[i * 3 + 2] = i + 2;
        //    }
        //}

        ////�޽� �ʱ�ȭ
        //viewMesh.Clear();
        //viewMesh.vertices = vertices;
        //viewMesh.triangles = triangles;
        //viewMesh.RecalculateNormals(); //�̺κ��� �⺻ ��� �ִ� �κ��� ��.
    }

    //void LateUpdate()
    //{
    //    //DrawFieldOfView(); // �� ������ �޽� ����
    //}
}
