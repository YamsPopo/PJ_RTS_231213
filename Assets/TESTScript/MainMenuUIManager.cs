using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("���θ޴� ������Ʈ")]
    public GameObject matchingMenu;
    public GameObject setting;

    [Header("��Ī�޴� ������Ʈ")]
    public GameObject pl1Ready;


    public void MatchingMenuEnter()
    {
        matchingMenu.SetActive(true);  //���� �޴��� ��Ī ��ư
    }
    public void MatchingMenuExit()
    {
        matchingMenu.SetActive(false);
    }

    public void SettingEnter() // ���� 
    {
        setting.SetActive(true);
    }
    public void SettingExit()
    {
        setting.SetActive(false);
    }
    
  

    public void Exit()
    {
        Application.Quit();
    }
    //�÷��̾� ���� ������ ���� ������ Dropdown.value�� �����Ͽ� �˾Ƴ��ߵ˴ϴ�.
}
