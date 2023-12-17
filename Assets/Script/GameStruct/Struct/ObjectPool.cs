using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectPool : MonoBehaviourPunCallbacks
{ 
    Dictionary<int, Stack<GameObject>> poolDict;//�����ո���Ʈ �ε����� �´� ���ص�ųʸ� 
    //��Ʈ�κ��� �ٱ����� �̳ѿ��� ��Ʈ�� ĳ�����Ͽ� �ָ� �̳������� �Ѱܹޱ� ����
    public List<GameObject> prefabList;//�ν����Ϳ��� �������� �����ðŶ� ���⼭ �ʱ�ȭ�ϸ� �ȉ�
    public int initSize;//��������
    private void Start()
    {
         Init();
    }

    void Init()
    {
        poolDict = new Dictionary<int, Stack<GameObject>>();

        for (int i = 0; i < prefabList.Count; i++)
        {
            poolDict.Add(i, new Stack<GameObject>());
        }
        //�����հ���������ŭ �� InitSize ��� ä����
        for (int j = 0; j < prefabList.Count; j++)
        {
            for (int i = 0; i < initSize; i++)
            {
                if (prefabList[0] == null)//����ó��
                break;

                GameObject temp = PhotonNetwork.Instantiate(prefabList[j].name, transform.position, prefabList[j].transform.rotation);
                temp.transform.SetParent(transform);//������ƮǮ�� �θ��
                temp.GetComponent<PhotonView>().RPC("RPCSetActive", RpcTarget.AllBuffered, false);
                GameManager.Instance.onRoundEnd += () => { ReturnPool(temp); };// ���������ϸ� �ʵ� ������Ʈ ���� Ǯ�� ��ȯ�� �̺�Ʈ�� �˾Ƽ�
                poolDict[j].Push(temp);
            }
        }
    }
    
    public GameObject Pop(int prefabListIndex = 0)
    {
        if (poolDict[prefabListIndex].Count <= 0)
        {
            for (int i = 0; i < 5; i++)//����ó��
                //Ǯ ��������� 5���� �߰������ؼ� �ֱ�
            {
                GameObject temp = PhotonNetwork.Instantiate(prefabList[prefabListIndex].name, transform.position, Quaternion.identity);
                temp.GetComponent<PhotonView>().RPC("RPCSetActive", RpcTarget.AllBuffered, false);
                poolDict[prefabListIndex].Push(temp);
            }
        }
        GameObject outObj = poolDict[prefabListIndex].Pop();
        outObj.GetComponent<PhotonView>().RPC("RPCSetActive", RpcTarget.All, true);
        return outObj;
    }
    public void ReturnPool(GameObject targetObj,int prefabListIndex = 0)
    {
        targetObj.GetComponent<PhotonView>().RPC("RPCSetActive", RpcTarget.AllBuffered, false);
        poolDict[prefabListIndex].Push(targetObj);
    }

    public GameObject Peek(int prefabListIndex = 0)
    {
        GameObject outObj = poolDict[prefabListIndex].Peek();
        return outObj;
    }
}
