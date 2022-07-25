using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //scripts
    public PartyManager partyManager;
    public Character[] character;
    

    //canvas
    public GameObject gamePanel;
    public GameObject gameOverSet;
    public GameObject choicePanel;
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

    //bool
    public bool[] swapChoice;

    void LateUpdate()
    {
        health.text = character[partyManager.list[partyManager.charactersIndex]].curHealth + " / " + character[partyManager.list[partyManager.charactersIndex]].maxHealth;
        playerHealthBar.localScale = new Vector3(character[partyManager.list[partyManager.charactersIndex]].curHealth / character[partyManager.list[partyManager.charactersIndex]].maxHealth, 1, 1);
        ammo.text = character[partyManager.list[partyManager.charactersIndex]].curAmmo + " / " + character[partyManager.list[partyManager.charactersIndex]].maxAmmo;

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
}
