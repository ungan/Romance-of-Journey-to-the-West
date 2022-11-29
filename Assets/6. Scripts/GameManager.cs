using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ClassInfo //프리펩 정보 클래스
{
    public string characterName; //캐릭터 이름
    public GameObject group;
    public RectTransform[] skillTransform; //스킬이미지
    public Text[] skillDelay; //스킬딜레이
}

public class GameManager : MonoBehaviour
{

    //scripts
    public CameraController cam;
    public PartyManager partyManager;
    public EventManager eventManager;
    public Character[] character;
    public boss_nachal boss;
    

    //canvas
    public GameObject gamePanel;
    public GameObject gameOverSet;
    public GameObject WinSet;
    public GameObject choicePanel;
    //canvas-upgrade
    public GameObject upgradePanel;
    //public Button[] upgradeButton;
    public Image[] party1upgradeImg;
    public Image[] party2upgradeImg;
    public Image[] party3upgradeImg;


    public Button retryButton;
    public Button[] choiceButton;
    //Player
    public RectTransform playerHealthBar;
    public GameObject HealthSet;
    public GameObject AmmoSet;
    public Text health;
    public Text ammo;
    public Image[] characterImg;
    //Party
    public RectTransform[] partyHealthBar;
    public Text[] partyhealth;
    public Image[] party1characterImg;
    public Image[] party2characterImg;
    public Image[] party3characterImg;
    public Image[] party4characterImg;
    //Boss
    public GameObject BossSet;
    public Text bossName;
    public Text bossHealth;
    public RectTransform bossHealthBar;
    //EXP
    public GameObject EXPSet;
    public Text expAmount;
    public RectTransform expBar;

    //bool
    public bool[] swapChoice;
    public bool isUpgrading;

    //PressKeyboard
    bool lDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;

    //Skill
    [SerializeField]
    public ClassInfo[] classInfos = null;

    void LateUpdate()
    {
        GetInput();
        if (isUpgrading) UpgradeButton();

        if (lDown) GameRetry();

        if (eventManager.Boss.activeSelf)
        {
            BossSet.SetActive(true);
        }
        else
        {
            BossSet.SetActive(false);
        }

        if(boss.isDead) //보스가 사망했을 경우 발동. 보스전 만들었으면 이거 // 제거해서 활성화하셈
        {
            WinSet.SetActive(true);
        }

        //if (eventManager.EnemyKiller.activeSelf) WinSet.SetActive(true); //보스전 덜만들었을 시 사용. 보스전 만들었음 삭제 처리하셈

        StartCoroutine(CoolTime());

        health.text = character[partyManager.list[partyManager.charactersIndex]].curHealth + " / " + character[partyManager.list[partyManager.charactersIndex]].maxHealth;
        playerHealthBar.localScale = new Vector3(character[partyManager.list[partyManager.charactersIndex]].curHealth / character[partyManager.list[partyManager.charactersIndex]].maxHealth, 1, 1);
        ammo.text = character[partyManager.list[partyManager.charactersIndex]].curAmmo + " / " + character[partyManager.list[partyManager.charactersIndex]].maxAmmo;

        //Boss
        bossHealth.text = boss.curHealth + " / 500";
        bossHealthBar.localScale = new Vector3((float)boss.curHealth / 500, 1, 1);

        //EXP
        expAmount.text = "LV " + partyManager.curLV;//partyManager.curEXP + " / " + partyManager.maxEXP;
        expBar.localScale = new Vector3((float)partyManager.curEXP / partyManager.maxEXP, 1, 1);

        //캐릭터 바꾸기
        characterImg[0].color = new Color(1, 1, 1, character[0].value == partyManager.list[partyManager.charactersIndex] ? 1 : 0);
        characterImg[1].color = new Color(1, 1, 1, character[1].value == partyManager.list[partyManager.charactersIndex] ? 1 : 0);
        characterImg[2].color = new Color(1, 1, 1, character[2].value == partyManager.list[partyManager.charactersIndex] ? 1 : 0);
        characterImg[3].color = new Color(1, 1, 1, character[3].value == partyManager.list[partyManager.charactersIndex] ? 1 : 0);

        //파티 현황판
        //파티원1
        if (partyManager.hasCharactersCount >= 1) {
            partyhealth[0].text = character[partyManager.list[0]].curHealth + " / " + character[partyManager.list[0]].maxHealth;
            partyHealthBar[0].localScale = new Vector3(character[partyManager.list[0]].curHealth / character[partyManager.list[0]].maxHealth, 1, 1);
            for(int i = 0; i < 4; i++)
            {
                party1characterImg[i].color = new Color(1, 1, 1, character[i].value == partyManager.list[0] ? 1 : 0);
                party1upgradeImg[i].color = new Color(1, 1, 1, character[i].value == partyManager.list[0] ? 1 : 0);
            }
        }
        //파티원2
        if (partyManager.hasCharactersCount >= 2)
        {
            partyhealth[1].text = character[partyManager.list[1]].curHealth + " / " + character[partyManager.list[1]].maxHealth;
            partyHealthBar[1].localScale = new Vector3(character[partyManager.list[1]].curHealth / character[partyManager.list[1]].maxHealth, 1, 1);
            for (int i = 0; i < 4; i++)
            {
                party2characterImg[i].color = new Color(1, 1, 1, character[i].value == partyManager.list[1] ? 1 : 0);
                party2upgradeImg[i].color = new Color(1, 1, 1, character[i].value == partyManager.list[1] ? 1 : 0);
            }
        }

        //파티원3
        if (partyManager.hasCharactersCount >= 3)
        {
            partyhealth[2].text = character[partyManager.list[2]].curHealth + " / " + character[partyManager.list[2]].maxHealth;
            partyHealthBar[2].localScale = new Vector3(character[partyManager.list[2]].curHealth / character[partyManager.list[2]].maxHealth, 1, 1);
            for (int i = 0; i < 4; i++)
            {
                party3characterImg[i].color = new Color(1, 1, 1, character[i].value == partyManager.list[2] ? 1 : 0);
                party3upgradeImg[i].color = new Color(1, 1, 1, character[i].value == partyManager.list[2] ? 1 : 0);
            }
        }

        //파티원4
        if (partyManager.hasCharactersCount >= 4)
        {
            partyhealth[3].text = character[partyManager.list[3]].curHealth + " / " + character[partyManager.list[3]].maxHealth;
            partyHealthBar[3].localScale = new Vector3(character[partyManager.list[3]].curHealth / character[partyManager.list[3]].maxHealth, 1, 1);
            for (int i = 0; i < 4; i++)
            {
                party4characterImg[i].color = new Color(1, 1, 1, character[i].value == partyManager.list[3] ? 1 : 0);
            }
        }
    }

    void GetInput()
    {
        lDown = Input.GetButtonDown("LButton");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void Choice()
    {
        choicePanel.SetActive(true);
    }

    public void ChoiceOut()
    {
        choicePanel.SetActive(false);
    }

    public void Choice1()
    {
        swapChoice[0] = true;
        ChoiceOut();
        Invoke("SwapOut", 0.05f);
    }

    public void Choice2()
    {
        swapChoice[1] = true;
        ChoiceOut();
        Invoke("SwapOut", 0.05f);
    }

    public void Choice3()
    {
        swapChoice[2] = true;
        ChoiceOut();
        Invoke("SwapOut", 0.05f);
    }

    void SwapOut()
    {
        for(int i = 0; i < 3; i++)
        {
            swapChoice[i] = false;
        }
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void UpgradeChoice()
    {
        Time.timeScale = 0.0f;
        isUpgrading = true;
        Debug.Log("업그레이드 중입니다! 멈췄습니다.");
        //upgradePanel.SetActive(true);
    }

    void UpgradeButton()
    {
        upgradePanel.SetActive(true);
        cam.Shake(0f, 0.1f);
        if (sDown1)
        {
            if (partyManager.list[0] == -1 || partyManager.characterScripts[0].curUpgradeLV >= 5)
            {
                Debug.Log("0번 업글 불가");
                return;
            }

            partyManager.characterScripts[0].curUpgradeLV++;
            Debug.Log("0번 캐릭터 업글");
            Time.timeScale = 1f;
            isUpgrading = false;
            upgradePanel.SetActive(false);
        }
        else if (sDown2)
        {
            if (partyManager.list[1] == -1 || partyManager.characterScripts[1].curUpgradeLV >= 5)
            {
                Debug.Log("1번 업글 불가");
                return;
            }

            partyManager.characterScripts[1].curUpgradeLV++;
            Debug.Log("1번 캐릭터 업글");
            Time.timeScale = 1f;
            isUpgrading = false;
            upgradePanel.SetActive(false);
        }
        else if (sDown3)
        {
            if (partyManager.list[2] == -1 || partyManager.characterScripts[2].curUpgradeLV >= 5)
            {
                Debug.Log("2번 업글 불가");
                return;
            }

            partyManager.characterScripts[2].curUpgradeLV++;
            Debug.Log("2번 캐릭터 업글");
            Time.timeScale = 1f;
            isUpgrading = false;
            upgradePanel.SetActive(false);
        }
    }

    IEnumerator CoolTime()
    {
        Character curChar = partyManager.characterScripts[partyManager.charactersIndex];

        for(int i = 0; i < classInfos.Length; i++)
        {
            if (i == partyManager.list[partyManager.charactersIndex]) classInfos[i].group.SetActive(true);
            else classInfos[i].group.SetActive(false);
        }

        if (curChar.maxChargeDelay > curChar.curChargeDelay) //Charge
        {
            int max = (int)Math.Ceiling(curChar.maxChargeDelay);
            int cur = (int)Math.Ceiling(curChar.curChargeDelay);

            classInfos[partyManager.list[partyManager.charactersIndex]].skillTransform[1].gameObject.SetActive(true);
            classInfos[partyManager.list[partyManager.charactersIndex]].skillDelay[1].gameObject.SetActive(true);

            classInfos[partyManager.list[partyManager.charactersIndex]].skillTransform[1].localScale = new Vector3(1, 1 - curChar.curChargeDelay / curChar.maxChargeDelay, 1);
            classInfos[partyManager.list[partyManager.charactersIndex]].skillDelay[1].text = (max + 1 - cur) + ""; //LeaderSkill

        }
        else
        {
            classInfos[partyManager.list[partyManager.charactersIndex]].skillTransform[1].gameObject.SetActive(false);
            classInfos[partyManager.list[partyManager.charactersIndex]].skillDelay[1].gameObject.SetActive(false);
        }
        if (curChar.maxLSkillDelay > curChar.curLSkillDelay) //LSkill
        {
            int max = (int)Math.Ceiling(curChar.maxLSkillDelay);
            int cur = (int)Math.Ceiling(curChar.curLSkillDelay);

            classInfos[partyManager.list[partyManager.charactersIndex]].skillTransform[2].gameObject.SetActive(true);
            classInfos[partyManager.list[partyManager.charactersIndex]].skillDelay[2].gameObject.SetActive(true);

            classInfos[partyManager.list[partyManager.charactersIndex]].skillTransform[2].localScale = new Vector3(1, 1 - curChar.curLSkillDelay / curChar.maxLSkillDelay, 1);
            classInfos[partyManager.list[partyManager.charactersIndex]].skillDelay[2].text = (max+1 - cur) + ""; //LeaderSkill

        }
        else
        {
            classInfos[partyManager.list[partyManager.charactersIndex]].skillTransform[2].gameObject.SetActive(false);
            classInfos[partyManager.list[partyManager.charactersIndex]].skillDelay[2].gameObject.SetActive(false);
        }
        if (curChar.maxMSkillDelay > curChar.curMSkillDelay) //MSkill
        {
            int max = (int)Math.Ceiling(curChar.maxMSkillDelay);
            int cur = (int)Math.Ceiling(curChar.curMSkillDelay);

            classInfos[partyManager.list[partyManager.charactersIndex]].skillTransform[3].gameObject.SetActive(true);
            classInfos[partyManager.list[partyManager.charactersIndex]].skillDelay[3].gameObject.SetActive(true);

            classInfos[partyManager.list[partyManager.charactersIndex]].skillTransform[3].localScale = new Vector3(1, 1 - curChar.curMSkillDelay / curChar.maxMSkillDelay, 1);
            classInfos[partyManager.list[partyManager.charactersIndex]].skillDelay[3].text = (max + 1 - cur) + ""; //LeaderSkill

        }
        else
        {
            classInfos[partyManager.list[partyManager.charactersIndex]].skillTransform[3].gameObject.SetActive(false);
            classInfos[partyManager.list[partyManager.charactersIndex]].skillDelay[3].gameObject.SetActive(false);
        }



        yield return null;
    }
}
