using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PhotonView pv;

    void Start()
    {   
        pv = GetComponent<PhotonView>();

        //�ڽ��� ĳ������ ���
        if(pv.IsMine)
        {
            Debug.Log("�ڽ��� ĳ���� �Դϴ�");
        }
    }

   
    void Update()
    {
        
    }
}
