using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModTransitionSystem : MonoBehaviour
{
    public GameObject rtsControlSystem;
    UnitController unitController;
    ClickMoveController aosHeroController;
    bool modSwitch;

    public bool ModSwitch
    {
        get { return modSwitch; }
        set { modSwitch = value; }  
    }

    private void Start()
    {
        unitController = GameManager.Instance.PlayerHero.GetComponent<UnitController>();
        aosHeroController = GameManager.Instance.PlayerHero.GetComponent<ClickMoveController>();

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)) 
        {
            ChangeMod();
        }
    }

    public void ChangeMod()
    {
        rtsControlSystem.SetActive(ModSwitch);
        unitController.enabled = ModSwitch;
        aosHeroController.enabled = !ModSwitch;
        ModSwitch = !ModSwitch;


        // ChangeMod �޼��� ȣ�� �� Play ��� ���� (ȸ��)
        // �� �÷��� ��忡 ���� ���� ������ �޶����Ƿ� ������ ��
        // � ��ȣ�ۿ��� �� �� ���� ��忡 ���� �������� �Ұ������� �����Ǳ⿡ ������ ����
        if (GameManager.Instance.playMode == PLAY_MODE.AOS_MODE)
        {
            GameManager.Instance.playMode = PLAY_MODE.RTS_MODE;
            aosHeroController.lineRenderer.enabled = false; //�ʿ� �� ����� �κ� ����ó��
        }
        else
        {
            GameManager.Instance.playMode = PLAY_MODE.AOS_MODE;
            GameManager.Instance.rtsController.DeselectAll();
            GameManager.Instance.rtsController.DeselctBuliding();
        }

    }
}
