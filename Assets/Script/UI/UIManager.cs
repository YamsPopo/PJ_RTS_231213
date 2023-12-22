using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    [Header("��� UI")]
    public GameObject result;
    public GameObject win;
    public GameObject defeat;
    [Header("��� UI ������Ʈ")]
    public GameObject aosUI;
    public GameObject rtsUI;
    [Header("��� ��ȯ ������Ʈ")]
    public ModTransitionSystem transitionSystem;
    [Header("�ڿ� UI ������Ʈ")]
    public TextMeshProUGUI mineText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI PopulationText;
    [Header("���� ���� UI")]
    public GameObject shopUI;
    public GameObject shopMessage;
    public TextMeshProUGUI shopMessageText;
    public TextMeshProUGUI shopClosingTime;
    public bool shopAvailability;
    [Header("ĳ���� ���� UI")]
    public CharacterSlot characterSlot;
    public Image[] characterSlotHp;
    [Header("�Ϲ� ���� ���� UI")]
    public GameObject unitStatUI;
    public Image unitHp;
    public TextMeshProUGUI unitHpText;
    public TextMeshProUGUI unitNameText;
    public TextMeshProUGUI unitAtkText;
    public TextMeshProUGUI unitShieldText;
    [Header("���� ���� ���� UI")]
    public GameObject heroStatUI;
    public Image heroHp; // �ּ� HP/�ִ� HP
    public TextMeshProUGUI heroHpText;
    public Image heroMp; // �ּ� MP/�ִ�MP
    public TextMeshProUGUI heroMpText;
    public Image heroExp;
    public TextMeshProUGUI heroLvText;
    public TextMeshProUGUI heroExpText; //���� Exp / �ִ� Exp
    public TextMeshProUGUI heroNameText;
    public TextMeshProUGUI heroAtkText;
    public TextMeshProUGUI heroShieldText;
    [Header("AOS ���� ���� ����UI")]
    public Image aosHeroHp;
    public TextMeshProUGUI aosHeroHpText;
    public Image aosHeroMp;
    public TextMeshProUGUI aosHeroMpText;


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
    void Update()
    {
        ChangeMod();
        if (GameManager.Instance.playMode == PLAY_MODE.AOS_MODE)
            heroImg.sprite = GameManager.Instance.PlayerHero.HeroImage;
        AosHeroInfo();
    }
    public void AosHeroInfo()
    {
        if(GameManager.Instance.playMode == PLAY_MODE.AOS_MODE)
        {
            aosHeroHp.fillAmount = (float)GameManager.Instance.PlayerHero.info.CurentHp / (float)GameManager.Instance.PlayerHero.info.MaxHp;
        }
    }

    public void ResultExit()
    {
        //��� â ��ư
    }

    public void CloseShop()
    {
        if(shopAvailability == false)
        {
            shopMessage.SetActive(false);
            shopUI.SetActive(false);
        }
        else
        {
            shopMessage.SetActive(false);
        }
    }
    public void ExitShop()
    {
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
        if (controller.selectedUnitList.Count == 1)
        {
            if (controller.selectedUnitList[0].gameObject == GameManager.Instance.PlayerHero.gameObject)
            {
                heroStatUI.SetActive(true);
                unitStatUI.SetActive(false);
                Hero hero = controller.selectedUnitList[0].GetComponent<Hero>();
                heroNameText.text = hero.name;                            
            }
            else
            {
                heroStatUI.SetActive(false);
                unitStatUI.SetActive(true); 
                unitNameText.text = controller.selectedUnitList[0].name;
                unitAtkText.text = controller.selectedUnitList[0].unit.info.Atk.ToString();
                unitShieldText.text = controller.selectedUnitList[0].unit.info.Def.ToString();
            }
        }
        else
        {
            heroStatUI.SetActive(false);
            unitStatUI.SetActive(false);
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
                if (controller.selectedUnitList[0].gameObject == GameManager.Instance.PlayerHero.gameObject)
                {
                    Hero hero = controller.selectedUnitList[0].GetComponent<Hero>();
                    characterSlot.faceImage[0].sprite = hero.HeroImage;
                }
                else
                {
                    characterSlot.faceImage[i].sprite = controller.selectedUnitList[i].unit.faceSprite;
                }
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