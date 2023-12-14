using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDetection : MonoBehaviour
{
    [SerializeField]
    private float detectiveRadius; // ����� Ž�� ����
    [SerializeField]
    private LayerMask unitLayer; // ���� ���̾�
    [SerializeField]
    private bool storeAvailability; // ������ �̿밡������ ����
    public bool StoreAvailability
    {
        get => storeAvailability;
        set { storeAvailability = value; }
    }
    private bool storeUse;
    public bool StoreUse
    {
        get { return storeUse; }
        set { storeUse = value; }
    }

    public ShopController shopControlle;
    private void Start()
    {
        shopControlle =  GetComponent<ShopController>();
    }
    private void Update()
    {
        Debug.Log("���� �̿밡��" + StoreAvailability);
        StoreAvailability = (IsDetection() && shopControlle.ShopStop && GameManager.Instance.playMode == PLAY_MODE.AOS_MODE);
    }
    public bool IsDetection()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, detectiveRadius, unitLayer);
        if (cols.Length > 0)
        {
            if (cols[0].gameObject.tag == ("PlayerHero"))
                return true;
            else
                return false;
        }
        else
            return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectiveRadius);
    }
}
