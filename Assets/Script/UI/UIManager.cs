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
    public GameObject shop;
    public GameObject leavingStore;
    public TextMeshProUGUI shopClosingTime;
    //�������� ����
    [Header("���� ���� ����")]
    public Image buildProgressImg;
    public GameObject bottomInfoRTSUI;
    public GameObject buildingModeUI;

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
        SetStoreUI();
    }

    public void SetStoreUI() // ���� UI�� �Ѱ� ���� UI����
    {
        if (shop.TryGetComponent(out ShopDetection shopDetection))
        {
            shopUI.SetActive(shopDetection.StoreUse);
            leavingStore.SetActive(!shopDetection.StoreUse);
            if (shop.TryGetComponent(out ShopController shopController))
                shopClosingTime.text = "�����ð� : " + shopController.CurCoolTime;
        }
    }
    public void CloseStore()
    {
        if (shop.TryGetComponent(out ShopDetection shopDetection))
        {
            shopDetection.StoreUse = false;
            leavingStore.SetActive(false);
            shopUI.SetActive(false);
        }
    }


    public void BottomRTSUISetActive(GameObject setTarget)
    {
        for (int i = 2; i < 6; i++)//4���� ��� ������ ���ְ� ���ϴ°͸� ų����
        {
            bottomInfoRTSUI.transform.GetChild(i).gameObject.SetActive(false);
        }
        setTarget.SetActive(true);
    }

}
