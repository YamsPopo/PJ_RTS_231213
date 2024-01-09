using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NexusBuilding : Building, IProductAble
{
    //Ŀ���� �ڷᱸ��
    public UnitList spawnList;
    //��⿭ ����Ʈ
    public List<IEnumerator> unitCoolTimeCos;
    //��⿭ ���� �ڷ�ƾ
    public IEnumerator unitProductManagerCo;
    public override void Die()
    {
        if (photonView.IsMine)
        {
            UIManager.Instance.result.SetActive(true);
            UIManager.Instance.defeat.SetActive(true);
        }
        else
        {
            UIManager.Instance.result.SetActive(true);
            UIManager.Instance.win.SetActive(true);
        }
    }

    public override void Hit()
    {
    }

    public new void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(unitProductManagerCo);
        GameManager.Instance.NexusCount++;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopCoroutine(unitProductManagerCo);
        GameManager.Instance.NexusCount--;
    }
    public override void Awake()
    {
        base.Awake();
        Init();
    }
    private void Init()
    {
        spawnList = new UnitList();
        spawnList.OnAddUnit += (Unit unit) => ProductUIMatch();
        spawnList.OnRemoveUnit += (int index) => ProductUIMatch();
        unitCoolTimeCos = new List<IEnumerator>();
        unitProductManagerCo = UnitProductManagerCo();
    }
    //ȣ���� �ɶ� �������� ������ �ϴ� �������̽� �Լ�
    public void Production(int popIndex, Transform selectBuildingTf, Unit targetUnit)
    {
        //5���� ��⿭�϶�
        if (unitCoolTimeCos.Count >= 5)
            return;
        //��� ���ڶ���
        if (targetUnit.cost > GameManager.Instance.Mine)
            return;
        //�α��� �ʰ� ������
        if (GameManager.Instance.MaxPopulation <= GameManager.Instance.Population)
            return;

        spawnList.Add(targetUnit);
        unitCoolTimeCos.Add(UnitCoolTimeCo(popIndex, selectBuildingTf, this));
        GameManager.Instance.Mine -= targetUnit.cost;
        GameManager.Instance.Population++;
    }

    public IEnumerator UnitProductManagerCo()
    {
        while (true)
        {
            if (unitCoolTimeCos.Count > 0)
            {
                IEnumerator currentCo = unitCoolTimeCos[0];
                yield return StartCoroutine(currentCo);
                unitCoolTimeCos.RemoveAt(0);
                spawnList.RemoveAt(0);
            }
            yield return null;
        }
    }
    public IEnumerator UnitCoolTimeCo(int popIndex, Transform selectBuildingTf, Building building)
    {
        float cool = GameManager.Instance.unitObjectPool.Peek(popIndex).GetComponent<Unit>().coolTime;

        //��Ÿ�ӵ��� �ٸ��� �����Ҽ� �־� �̸� ��ġ����
        TextMeshProUGUI[] buildInfoTexts = UIManager.Instance.unitProductModeUI.GetComponentsInChildren<TextMeshProUGUI>();
        //���ֻ��� ��Ÿ��
        while (cool > 0f)
        {
            cool -= Time.fixedDeltaTime;

            if (GameManager.Instance.rtsController.SelectBuilding != null)
            {
                if (building == GameManager.Instance.rtsController.SelectBuilding.GetComponent<Building>())
                {
                    buildInfoTexts[1].text = "������";
                    UIManager.Instance.buildProgressCountText.text = cool.ToString();
                    UIManager.Instance.buildProgressFill.fillAmount = (1 / cool) - cool / 10;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        buildInfoTexts[1].text = null;
        UIManager.Instance.buildProgressCountText.text = null;
        UIManager.Instance.buildProgressFill.fillAmount = 0;

        //����� ����
        GameObject unit = GameManager.Instance.unitObjectPool.Pop(popIndex).gameObject;
        GameManager.Instance.rtsController.fieldUnitList.Add(unit.GetComponent<UnitController>());
        unit.transform.position = selectBuildingTf.position;
        unit.transform.Translate(new Vector3(0, 0, -6), Space.Self);
        unit.GetComponent<NavMeshAgent>().enabled = true;


        //���ֻ̰� ��ġ�� �ʱ� ���� �̵�,
        unit.GetComponent<NavMeshAgent>().SetDestination(unit.transform.position + new Vector3(0, 0, -3));
        yield return null;

        unit.GetComponent<NavMeshAgent>().ResetPath();

    }
    public void ProductUIMatch()
    {
        for (int i = 0; i < SlotManager.Instance.unitProductProgressFaceSlots.Count; i++)
        {
            if (GameManager.Instance.rtsController.SelectBuilding != null)
            {
                if (this == GameManager.Instance.rtsController.SelectBuilding.GetComponent<Building>())
                {
                    if (i < spawnList.Count)
                    {
                        SlotManager.Instance.unitProductProgressFaceSlots[i].SetActive(true);
                        SlotManager.Instance.unitProductProgressFaceSlots[i].GetComponent<Image>().sprite = spawnList[i].faceSprite;
                    }
                    else
                    {
                        SlotManager.Instance.unitProductProgressFaceSlots[i].SetActive(false);
                        SlotManager.Instance.unitProductProgressFaceSlots[i].GetComponent<Image>().sprite = null;
                    }
                }
            }
        }
    }
}
