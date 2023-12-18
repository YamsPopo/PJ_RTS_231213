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

}