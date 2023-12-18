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
    [Header("���� ���� ���� UI")]
    public GameObject heroStatUI;

    //�������� ����
    [Header("���� ���� ����")]
    public Image buildProgressImg;
    public GameObject bottomInfoRTSUI;
    public GameObject unitProductModeUI;

    [Header("���� AOS��� UI")]
    public SkillSlot[] skillSlots;
    public Image heroImg;

    [Header("����// ���� �������� ���� ������/�迭")]
    public SlotManager slotManager;
    public GameObject unitFaceGO;

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


    
   
    public void SelectUnitUIChange(RTSController controller)
    {
        //for (int i = 0; i < controller.selectedUnitList.Count; i++)
        //{
        //    if(controller.selectedUnitList[i].gameObject == GameManager.Instance.PlayerHero.gameObject)
        //    {
        //        Swap(controller.selectedUnitList, controller.selectedUnitList);
        //    }
        //}
        for (int i = 0; i < controller.selectedUnitList.Count; i++)
        {
            characterSlot.faceSlot[i].SetActive(true);
            characterSlot.hpImage[i].fillAmount = (float)controller.selectedUnitList[i].unit.Hp / (float)100; // �ӽ÷� 100���� ������ ü�� ǥ��
            characterSlot.faceImage[i].sprite = controller.selectedUnitList[i].unit.faceSprite;
        }        
    }
    public void Swap(RTSController controller, int aValue, int bValue)
    {

    }
    
}