using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum SLOTTYPE
{
    None,
    ProductBuilding,
    NexusBuilding,
    Supply_Build,
    SupplyUnit,
    ArmySelect,
}

public struct SlotArg//���Կ� ������ �̹����� ���
{
    public Sprite[] spriteArr;
    public Action[] actionButtonArr;
    public int slotsCount;
    public SlotArg(Sprite[] spriteArr, Action[] actionArr)
    {
        this.spriteArr = spriteArr;
        this.actionButtonArr = actionArr;
        slotsCount = spriteArr.Length;//���� ���� �� ����
    }
}
public class SlotManager : SingleTon<SlotManager>
{
    private Dictionary<SLOTTYPE, SlotArg> slotsDic = new Dictionary<SLOTTYPE, SlotArg>();
    private ButtonSlot[] slotArr = new ButtonSlot[slotsCount];
    //9ĭ���� ������ �迭�̿�����
    //��ư ������ ��������Ʈ��
    public Sprite[] supply_BuildIconArr;
    public Sprite[] supplyUnitIconArr;
    public Sprite[] productBuildingIconArr;
    public Sprite[] nexusIconArr;
    public Sprite[] armyMoveModeIconArr;

    public GameObject buildingProgressprefab;
    public GameObject buildingEFTprefab;

    //���� �̵��Ǽ����� �޾ƿ�
    public SupplyUnit selectedSupplyUnit;

    public const int slotsCount = 9;

    //���� ���� ��� ���� ����
    public List<GameObject> unitProductProgressFaceSlots;

    
    //�� ���Կ� �´� ��ɰ� �̹��� ����
    [SerializeField]
    private SLOTTYPE slotType;

    public SLOTTYPE SlotType
    {
        get { return slotType; }
        set 
        {
            //�ٸ��������� ��ȯ�ÿ� �ʱ�ȭ����
            for (int i = 0; i < slotsCount; i++)//��������Ʈ ����
            {
                slotArr[i].imageCompo.sprite = null;//+1�� �θ������Ʈ �̹��� ������Ʈ �����ϱ�����
                slotArr[i].slotAction = null;
            }
            
            slotType = value;
            for (int i = 0; i < slotsCount; i++)//��������Ʈ ����
            {
                if(slotsDic[slotType].spriteArr[i] != null)
                slotArr[i].imageCompo.sprite = slotsDic[slotType].spriteArr[i];//+1�� �θ������Ʈ �̹��� ������Ʈ �����ϱ�����
                if (slotsDic[slotType].actionButtonArr[i] != null)
                slotArr[i].slotAction = slotsDic[slotType].actionButtonArr[i];
            }
        }
    }
    List<IEnumerator> unitCoolTimeCos;
    private void Start()
    {
        unitCoolTimeCos = new List<IEnumerator>();
        unitProductProgressFaceSlots = new List<GameObject>();
        Init();
        StartCoroutine(unitProductManagerCo());
    }
    public void Init()//��ųʸ� �ʱ�ȭ�� �̹���,��� �����
    {
        slotArr = GetComponentsInChildren<ButtonSlot>();
        slotsDic.Add(SLOTTYPE.None, new SlotArg(new Sprite[9], new Action[slotsCount]));//�� ������
        slotsDic.Add(SLOTTYPE.ProductBuilding, new SlotArg(productBuildingIconArr, new Action[slotsCount]));
        slotsDic.Add(SLOTTYPE.NexusBuilding, new SlotArg(nexusIconArr, new Action[slotsCount]));
        slotsDic.Add(SLOTTYPE.SupplyUnit, new SlotArg(supplyUnitIconArr, new Action[slotsCount]));
        slotsDic.Add(SLOTTYPE.Supply_Build, new SlotArg(supply_BuildIconArr, new Action[slotsCount]));
        slotsDic.Add(SLOTTYPE.ArmySelect, new SlotArg(armyMoveModeIconArr, new Action[slotsCount]));
        FaceListButtonInit();
        ActionInit();
    }

    //private void Update()
    //{
    //    for(int i=0;i<5;i++)
    //    {
    //        unitProductProgressFaceSlots[i].GetComponent<Image>().sprite = null;
    //        unitProductProgressFaceSlots[i].SetActive(false);
    //    }
    //    for (int i = 0; i < unitCoolTimeCos.Count; i++)
    //    {
    //        unitProductProgressFaceSlots[i].GetComponent<Image>().sprite = ;
    //        unitProductProgressFaceSlots[i].SetActive(false);
    //    }

    //}

    //���� ���߻��� �ʱ�ȭ �κ�
    public void FaceListButtonInit()
    {
        GameObject[] facesObj = UIManager.Instance.unitFaceUI.faceObjArr;
        foreach (var go in facesObj)
        {
            unitProductProgressFaceSlots.Add(go);
        }
        for (int i = 0; i < unitProductProgressFaceSlots.Count; i++)
        {
            //���ֻ����� �����ܵ� ������ ����Ʈ���� ���� �̹��� �����ϱ�
            int index = i;
            unitProductProgressFaceSlots[index].GetComponentInParent<Button>().onClick.AddListener(() => 
            { 
                //StopCoroutine(unitCoolTimeCos[index]);//������ �������
                unitCoolTimeCos.RemoveAt(index);
                
                //������ UI �ּ�
              //  unitProductProgressFaceSlots[index].GetComponent<Image>().sprite = null;
              //  unitProductProgressFaceSlots[index].SetActive(false);
            });
        }
    }


    public void ActionInit()
    {
        //�������� �ൿ �׼�
        slotsDic[SLOTTYPE.SupplyUnit].actionButtonArr[0] += () => { SlotType = SLOTTYPE.Supply_Build; };
        //������ �ൿ �׼�
        for (int i = 0; i < 4; i++)//���� ����
        {
            int index = i;
            slotsDic[SLOTTYPE.Supply_Build].actionButtonArr[index] += () =>
            {
                slotArr[index].targetObj = GameManager.Instance.buildingObjectPool.Pop(index);
                slotArr[index].isBuildClicked = true;
                slotArr[index].meshRenderer = slotArr[index].targetObj.GetComponent<MeshRenderer>();
            };
        }
        //�ڷΰ��� ó�� �޴���
        slotsDic[SLOTTYPE.Supply_Build].actionButtonArr[8] += () => { SlotType = SLOTTYPE.SupplyUnit; };
        //�ؼ������� �ൿ �׼�
        UnitProductAction(SLOTTYPE.NexusBuilding,0,5);
        UnitProductAction(SLOTTYPE.NexusBuilding,1,6);

        //������� �ൿ �׼�
        for (int i = 0; i < 5; i++)//���� ���� ������ƮǮ���� ���� 5����
        {
            int index = i;
            UnitProductAction(SLOTTYPE.ProductBuilding, index, index);
        }
        //�δ����� �ൿ �׼�
        slotsDic[SLOTTYPE.ArmySelect].actionButtonArr[0] += () => { RTSController.armyMode = ARMYMOVEMODE.Horizontal; };
        slotsDic[SLOTTYPE.ArmySelect].actionButtonArr[1] += () => { RTSController.armyMode = ARMYMOVEMODE.Vertical; };
        slotsDic[SLOTTYPE.ArmySelect].actionButtonArr[2] += () => { RTSController.armyMode = ARMYMOVEMODE.Square; };

        //������ �κ�
        //���ӽ��ۿ��� �ൿ�������� ����
        SlotType = SLOTTYPE.None;
    }


    void UnitProductAction(SLOTTYPE slotType, int buttonIndex, int popIndex)
    {
        slotsDic[slotType].actionButtonArr[buttonIndex] += () =>
        {
            //5���� ��⿭ ����
            if (unitCoolTimeCos.Count == 5)
                return;

            Transform selectBuildingTf = GameManager.Instance.rtsController.SelectBuilding.gameObject.transform;
            unitCoolTimeCos.Add(UnitCoolTimeCo(popIndex, selectBuildingTf));


            //������ UI �ּ�
            //�̹��� ��⿭ ǥ��
            //     unitProductProgressFaceSlots[unitCoolTimeCos.Count - 1].SetActive(true);
            //     unitProductProgressFaceSlots[unitCoolTimeCos.Count - 1].GetComponent<Image>().sprite =
            //     GameManager.Instance.unitObjectPool.Peek(popIndex).GetComponent<Unit>().faceSprite;
        };
    }

    //���� ��⿭ �˻����ִ� �ڷ�ƾ
    IEnumerator unitProductManagerCo()
    {
        while(true)
        {
            if(unitCoolTimeCos.Count > 0)
            {
                IEnumerator currentCo = unitCoolTimeCos[0];

                yield return StartCoroutine(currentCo);
                
                unitCoolTimeCos.RemoveAt(0);
            }
            yield return null;
        }
    }


    IEnumerator UnitCoolTimeCo(int popIndex, Transform selectBuildingTf)
    {
        float cool = GameManager.Instance.unitObjectPool.Peek(popIndex).GetComponent<Unit>().coolTime;
        //��Ÿ�ӵ��� �ٸ��� �����Ҽ� �־� �̸� ��ġ����
        TextMeshProUGUI[] buildInfoTexts  = UIManager.Instance.unitProductModeUI.GetComponentsInChildren<TextMeshProUGUI>();
        //���൵
        buildInfoTexts[1].text = "������";
        //���ֻ��� ��Ÿ��
        while (cool > 0f)
        {
            cool -= Time.fixedDeltaTime;
            UIManager.Instance.buildProgressCountText.text = cool.ToString();
            UIManager.Instance.buildProgressFill.fillAmount = (1 / cool) - cool/10;
            yield return new WaitForFixedUpdate();
        }
        buildInfoTexts[1].text = null;
        UIManager.Instance.buildProgressCountText.text = null;
        UIManager.Instance.buildProgressFill.fillAmount = 0;

        GameObject unit = GameManager.Instance.unitObjectPool.Pop(popIndex).gameObject;
        GameManager.Instance.rtsController.fieldUnitList.Add(unit.GetComponent<UnitController>());
        unit.transform.position = selectBuildingTf.position;
        unit.transform.Translate(new Vector3(0, 0, -6), Space.Self);
        unit.GetComponent<NavMeshAgent>().enabled = true;

        //���ֻ̰� ��ġ�� �ʱ� ���� �̵�,
        unit.GetComponent<NavMeshAgent>().SetDestination(unit.transform.position + new Vector3(0, 0, -3));
        yield return null;
        
        unit.GetComponent<NavMeshAgent>().ResetPath();

        //������ UI �ּ�
        // unitProductProgressFaceSlots[unitCoolTimeCos.Count].GetComponent<Image>().sprite = null;
        //  unitProductProgressFaceSlots[unitCoolTimeCos.Count].SetActive(false);
    }
}
