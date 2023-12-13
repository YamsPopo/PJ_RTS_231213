using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    [SerializeField] private GameObject unitMaker;
    private NavMeshAgent navMeshAgent;
    private Animator anim;

    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }

    private void Start()
    {
        //�ν����� â�� ������Ʈ Ȱ��ȭ ��ư�� ������� ���װ� �־ �� Start�� �־�����ϴ�.
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    public void SelectUnit()
    {
        unitMaker.SetActive(true);
    }

    public void DeselectUnit()
    {
        unitMaker.SetActive(false);
    }

    public void MoveTo(Vector3 end) // ���� �̵�
    {
        NavMeshAgent.SetDestination(end);
    }

    //����������, ������ ����� �ڵ����� ����ʱ�ȭ
    //���� �̻��� �����ؾ���


    private void FixedUpdate() 
    {
        // velocity ���� ���ϸ� run, �ƴϸ� idle - ����
        if (NavMeshAgent.velocity != Vector3.zero)
            anim.SetBool("IsMove", true);
        else
            anim.SetBool("IsMove", false);
    }
}
