using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    [Header("��� UI ������Ʈ")]
    public GameObject aosUI;
    public GameObject rtsUI;
    [Header("��� ��ȯ ������Ʈ")]
    public ModTransitionSystem transitionSystem;
    [Header("�ڿ� UI ������Ʈ")]
    public TextMeshProUGUI treeText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI PopulationText;
    [Header("���� ���� UI")]
    public GameObject shopUI;
    public GameObject leavingShop;
    public TextMeshProUGUI shopClosingTime;
    [Header("ĳ���� ���� UI")]
    public CharacterSlot characterSlot;
    [Header("�Ϲ� ���� ���� UI")]
    public GameObject unitStatUI;
    public Image unitHp;
    public TextMeshProUGUI unitNameText;
    public TextMeshProUGUI unitAtkText;
    public TextMeshProUGUI unitShieldText;
    [Header("���� ���� ���� UI")]
    public GameObject heroStatUI;
    public Image heroHp;
    public Image heroMp;
    public Image heroExp;
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI heroLvText;
    public TextMeshProUGUI heroAtkText;
    public TextMeshProUGUI heroShieldText;

    //�������� ����
    [Header("���� ���� ����")]
    public Image buildProgressFill;
    public TextMeshProUGUI buildProgressCountText;
    public GameObject bottomInfoRTSUI;
    public GameObject unitProductModeUI;

    [Header("���� AOS��� UI")]
    public SkillSlot[] skillSlots;
    public Image heroImg;

    [Header("����// ���� �������� ���� ������/�迭")]
    public SlotManager slotManager;
    public UnitFaceListUI unitFaceUI;

    public void ChangeMod()
    {
        if (transitionSystem != null)
        {
            if (transitionSystem.ModSwitch)
            {
                aosUI.SetActive(true);
                rtsUI.SetActive(false);
            }
            else
            {
                aosUI.SetActive(false);
                rtsUI.SetActive(true);
            }
        }
    }

    public void ResourcesUpdate()
    {
        treeText.text = GameManager.Instance.Tree.ToString();
        goldText.text = GameManager.Instance.Gold.ToString();
        PopulationText.text = GameManager.Instance.Population.ToString();
    }
    void Update()
    {
        ResourcesUpdate();
        ChangeMod();
        if (GameManager.Instance.playMode == PLAY_MODE.AOS_MODE)
            heroImg.sprite = GameManager.Instance.PlayerHero.HeroImage;
    }
    public void CloseStore()
    {
        leavingShop.SetActive(false);
        shopUI.SetActive(false);
    }


    public void BottomRTSUISetActive(GameObject setTarget)
    {
        for (int i = 2; i < 6; i++)//4���� ��� ������ ���ְ� ���ϴ°͸� ų����
        {
            bottomInfoRTSUI.transform.GetChild(i).gameObject.SetActive(false);
        }
        setTarget.SetActive(true);
    }

    public void OnClickSkillSlot(int index)
    {
        skillSlots[index].TrySkillActive();
    }


    
   // 1���� ���ý� ���� �߰� �ؾߵ�. 
    public void SelectUnitUIChange(RTSController controller)
    {
        //if(controller.selectedUnitList.Count == 1)
        //{
        //    if (controller.selectedUnitList[0].gameObject == GameManager.Instance.PlayerHero.gameObject)
        //    {
        //        heroStatUI.SetActive(true);
        //        unitStatUI.SetActive(false);
        //        Hero hero =controller.selectedUnitList[0].GetComponent<Hero>();
        //        heroHp.fillAmount = hero.Hp / hero.MaxMp;
        //        heroMp.fillAmount = hero.CurMp / hero.MaxMp;
        //        heroExp.fillAmount = hero.CurExp / hero.AimExp;   // �ƽ� ����ġ �޴� �κ�
        //        heroLvText.text = hero.Level.ToString();
        //        heroNameText.text = hero.name;
        //        heroAtkText.text = hero.Atk.ToString();
        //        heroShieldText.text = "999";

        //    }
        //    else
        //    {
        //        heroStatUI.SetActive(false);
        //        unitStatUI.SetActive(true);
        //    }
        //}
        //else
        {
            for (int i = 0; i < controller.selectedUnitList.Count; i++)
            {
                if(controller.selectedUnitList[i].gameObject == GameManager.Instance.PlayerHero.gameObject)
                {
                    Swap(controller, 0, i);
                }
            }
            for (int i = 0; i < controller.selectedUnitList.Count; i++)
            {
                characterSlot.faceSlot[i].SetActive(true);
                characterSlot.hpImage[i].fillAmount = (float)controller.selectedUnitList[i].unit.Hp / (float)100; // �ӽ÷� 100���� ������ ü�� ǥ��
                characterSlot.faceImage[i].sprite = controller.selectedUnitList[i].unit.faceSprite;
            }        
        }
    }
    public void Swap(RTSController controller, int aValue, int bValue)
    {
        UnitController temp = controller.selectedUnitList[aValue];
        controller.selectedUnitList[aValue] = controller.selectedUnitList[bValue];
        controller.selectedUnitList[bValue] = temp;
    }
    
}