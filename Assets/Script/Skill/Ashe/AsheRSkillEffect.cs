using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheRSkillEffect : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 direction;
    [SerializeField] float radius;
    [SerializeField] GameObject hitEffect;
    Rigidbody rb;
    Ashe owner;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        owner = GetComponentInParent<Ashe>();
        radius = gameObject.GetComponent<SphereCollider>().radius;
    }

    private void OnEnable()
    {
        rb.MovePosition(rb.transform.position + direction * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ��ų�� ����ϴ� ������ ���̾ Unit �̱⿡ ���� ����ϴ� ���� �����
        if (other.gameObject.layer == LayerMask.NameToLayer("Unit")) 
        {
            other.gameObject.GetComponent<IHitAble>().Hit(owner);

            if (Physics.OverlapSphere(transform.position, radius, LayerMask.NameToLayer("Unit")).Length >= 0)
            {
                Destroy(this.gameObject);
                GameObject hit = Instantiate(hitEffect,transform.position,transform.rotation);
            }
        }
            
    }
}
