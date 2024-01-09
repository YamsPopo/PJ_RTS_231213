using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
    private Dictionary<SLOTTYPE, SlotArg> slotsDic;
    private ButtonSlot[] slotArr;
    //9ĭ���� ������ �迭�̿�����
    //��ư ������ ��������Ʈ��
    public Sprite[] supply_BuildIconArr;
    public Sprite[] supplyUnitIconArr;
    public Sprite[] humanProductBuildingIconArr;
    public Sprite[] orcProductBuildingIconArr;
    public Sprite[] humanNexusIconArr;
    public Sprite[] orcNexusIconArr;
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
            for (int i = 0; i < slotsCount; i++)
            {
                slotArr[i].imageCompo.sprite = null;
                slotArr[i].slotAction = null;
            }
            
            slotType = value;
            for (int i = 0; i < slotsCount; i++)//�׼ǰ� ���� ����
            {
                if(slotsDic[slotType].spriteArr[i] != null)
                slotArr[i].imageCompo.sprite = slotsDic[slotType].spriteArr[i];
                if (slotsDic[slotType].actionButtonArr[i] != null)
                slotArr[i].slotAction = slotsDic[slotType].actionButtonArr[i];
            }
        }
    }
    private void Start()
    {
        Init();
    }
    public void Init()//��ųʸ� �ʱ�ȭ�� �̹���,��� �����
    {
        unitProductProgressFaceSlots = new List<GameObject>();
        slotArr = new ButtonSlot[slotsCount];
        slotArr = GetComponentsInChildren<ButtonSlot>();

        slotsDic = new Dictionary<SLOTTYPE, SlotArg>();

        //���Ե�ųʸ���
        slotsDic.Add(SLOTTYPE.None, new SlotArg(new Sprite[9], new Action[slotsCount]));//�� ������
        slotsDic.Add(SLOTTYPE.SupplyUnit, new SlotArg(supplyUnitIconArr, new Action[slotsCount]));
        slotsDic.Add(SLOTTYPE.Supply_Build, new SlotArg(supply_BuildIconArr, new Action[slotsCount]));
        slotsDic.Add(SLOTTYPE.ArmySelect, new SlotArg(armyMoveModeIconArr, new Action[slotsCount]));

        //������ ���� ���� ���� ��� ����
        //�� ���ξ� �ͼ�������߸���
        //Init �� �Լ��� �ΰ��� ���;� �ʱ�ȭ�ؾ���
        //��ư �̹��� �迭�� �ٸ�
        switch (GameManager.Instance.tribe)
        {
            case Tribe.HUMAN:
                slotsDic.Add(SLOTTYPE.NexusBuilding, new SlotArg(humanNexusIconArr, new Action[slotsCount]));
                slotsDic.Add(SLOTTYPE.ProductBuilding, new SlotArg(humanProductBuildingIconArr, new Action[slotsCount]));
                break;
            case Tribe.ORC:
                slotsDic.Add(SLOTTYPE.NexusBuilding, new SlotArg(orcNexusIconArr, new Action[slotsCount]));
                slotsDic.Add(SLOTTYPE.ProductBuilding, new SlotArg(orcProductBuildingIconArr, new Action[slotsCount]));
                break;
            default:
                break;
        }
        FaceListButtonInit();
        ActionInit();

    }

    //���� ���߻��� �ʱ�ȭ �κ�
    public void FaceListButtonInit()
    {
        GameObject[] facesObj = UIManager.Instance.unitFaceUI.faceObjArr;
        foreach (var go in facesObj)
        {
            unitProductProgressFaceSlots.Add(go);
        }
        UIManager.Instance.unitProductModeUI.SetActive(true);
        for (int i = 0; i < unitProductProgressFaceSlots.Count; i++)
        {
            //���ֻ����� �����ܵ� ������ ����Ʈ���� ���� �̹��� �����ϱ�
            int index = i;
            unitProductProgressFaceSlots[index].GetComponentInParent<Button>().onClick.AddListener(() => 
            {
                ProductBuilding ownerBuilding = GameManager.Instance.rtsController.SelectBuilding.GetComponent<ProductBuilding>();

                ownerBuilding.unitCoolTimeCos.RemoveAt(index);
                //���� �������� ���õ� �� ����Ʈ�� ����������
                ownerBuilding.spawnList.RemoveAt(index);
                GameManager.Instance.Population--;
            });
        }
        UIManager.Instance.unitProductModeUI.SetActive(false);
    }

    public void ActionInit()
    {
        //�������� �ൿ �׼�
        slotsDic[SLOTTYPE.SupplyUnit].actionButtonArr[0] += () => { SlotType = SLOTTYPE.Supply_Build; };
        //������ �ൿ �׼�
        for (int i = 0; i < 4; i++)//���� ����
        {
            int index = i;
            BuildingProductAction(SLOTTYPE.Supply_Build, index, index);
        }
        //�ڷΰ��� ó�� �޴���
        slotsDic[SLOTTYPE.Supply_Build].actionButtonArr[8] += () => { SlotType = SLOTTYPE.SupplyUnit; };

        //(�ؼ���, ������� �׼��� ������ ���� �ٸ����� �����Դ޶���)
        ///////////////////////////////////////////////////////
        switch (GameManager.Instance.tribe)
        {
            case Tribe.HUMAN:
                //�ؼ������� �ൿ �׼�
                UnitProductAction(SLOTTYPE.NexusBuilding, 0, 5);
                UnitProductAction(SLOTTYPE.NexusBuilding, 1, 6);

                //������� �ൿ �׼�
                for (int i = 0; i < 5; i++)//���� ���� ������ƮǮ���� ���� 5����
                {
                    int index = i;
                    int slotIndex = index;
                    UnitProductAction(SLOTTYPE.ProductBuilding, slotIndex, index);
                }
                break;
            case Tribe.ORC:
                //�ؼ������� �ൿ �׼�
                UnitProductAction(SLOTTYPE.NexusBuilding, 0, 12);
                UnitProductAction(SLOTTYPE.NexusBuilding, 1, 13);

                //������� �ൿ �׼�
                for (int i = 7; i < 12; i++)//���� ���� ������ƮǮ���� ���� 5����
                {
                    int index = i;
                    int slotIndex = index - 7;
                    UnitProductAction(SLOTTYPE.ProductBuilding, slotIndex, index);
                }
                break;
            default:
                break;
        }

        //�δ����� �ൿ �׼�
        slotsDic[SLOTTYPE.ArmySelect].actionButtonArr[0] += () => { RTSController.armyMode = ARMYMOVEMODE.Horizontal; };
        slotsDic[SLOTTYPE.ArmySelect].actionButtonArr[1] += () => { RTSController.armyMode = ARMYMOVEMODE.Vertical; };
        slotsDic[SLOTTYPE.ArmySelect].actionButtonArr[2] += () => { RTSController.armyMode = ARMYMOVEMODE.Square; };

        //���ӽ��ۿ��� �ൿ�������� ����
        SlotType = SLOTTYPE.None;
    }

    void BuildingProductAction(SLOTTYPE slotType, int buttonIndex, int popIndex)
    {
        slotsDic[slotType].actionButtonArr[buttonIndex] += () =>
        {
            Building targetBuilding = (GameManager.Instance.buildingObjectPool.Peek(popIndex).GetComponent<Building>());

            if (targetBuilding.cost > GameManager.Instance.Mine)
                return;

            slotArr[buttonIndex].targetObj = GameManager.Instance.buildingObjectPool.Pop(popIndex);
            slotArr[buttonIndex].isBuildClicked = true;
            slotArr[buttonIndex].meshRenderer = slotArr[buttonIndex].targetObj.GetComponent<MeshRenderer>();

            GameManager.Instance.Mine -= targetBuilding.cost;
        };

    }
    void UnitProductAction(SLOTTYPE slotType, int buttonIndex, int popIndex)
    {
        slotsDic[slotType].actionButtonArr[buttonIndex] += () =>
        {
            IProductAble ownerBuilding = GameManager.Instance.rtsController.SelectBuilding.GetComponent<IProductAble>();
            Unit targetUnit = (GameManager.Instance.unitObjectPool.Peek(popIndex).GetComponent<Unit>());
            Transform selectBuildingTf = GameManager.Instance.rtsController.SelectBuilding.gameObject.transform;

            ownerBuilding.Production(popIndex, selectBuildingTf,targetUnit);
        };
    }
}
