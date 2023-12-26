using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDetection : MonoBehaviour
{
    [SerializeField]
    private float detectiveRadius; // ����� Ž�� ����
    [SerializeField]
    private LayerMask masterHeroLayer; // ���� ���̾�
    [SerializeField]
    private LayerMask userHeroLayer;
    [SerializeField]
    private bool shopAvailability; // ������ �̿밡������ ����
    public bool ShopAvailability
    {
        get => shopAvailability;
        set 
        { 
            shopAvailability = value;
            UIManager.Instance.shopAvailability = ShopAvailability;
            if (ShopUse && ShopAvailability == false)
            {
                UIManager.Instance.shopMessage.SetActive(true);
                UIManager.Instance.shopMessageText.text = "������ �������ϴ�.";
            }
        }
    }
    private bool shopUse;
    public bool ShopUse
    {
        get { return shopUse; }
        set 
        { 
            shopUse = value;
            if (ShopUse)
            {
                UIManager.Instance.shopUI.SetActive(true);
            }
        }
    }

    public ShopController shopController;
    private void Start()
    {
        shopController =  GetComponent<ShopController>();
    }
    private void Update()
    {
        ShopAvailability = (IsDetection() && shopController.ShopStop && GameManager.Instance.playMode == PLAY_MODE.AOS_MODE);
        Debug.Log("���Ӹ��"+GameManager.Instance.playMode);
        Debug.Log("������ ����"+shopController.ShopStop);
        Debug.Log("�����̿�" + ShopAvailability);
    }
    public bool IsDetection()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, detectiveRadius, masterHeroLayer | userHeroLayer);
        if (cols.Length > 0)
        {
            Debug.Log("���� �߰�");
            return true;
        }

        else
        {
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectiveRadius);
    }
}
