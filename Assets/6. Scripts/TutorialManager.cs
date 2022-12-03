using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public PartyManager party;
    public DemoStatue statue;

    public GameObject[] item;

    public Text text;

    public string[] line;
    public int lineNumber = 0;

    bool fire1; //마우스1
    bool fire1Up; //마우스1Up
    bool fire2; //마우스2
    bool fire2Down; //마우스2down

    bool oneTime = false;

    // Start is called before the first frame update
    void Start()
    {
        //Show();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Tutorial();
        Show();

        if (statue.isBreaking == true)
        {
            text.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }

    void GetInput()
    {
            fire1 = Input.GetButton("Fire1");
            fire1Up = Input.GetButtonUp("Fire1");
            fire2 = Input.GetButton("Fire2");
            fire2Down = Input.GetButtonDown("Fire2");
    }

    void Tutorial()
    {
        switch (lineNumber)
        {
            case 0:
                if (party.isRunning)
                {
                    lineNumber = 1;
                }
                break;
            case 1:
                if (!oneTime)
                {
                    oneTime = true;
                    Invoke("LinePP", 3f);
                }
                break;
            case 2:
                oneTime = false;
                int count = item.Length;
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i] == null) count--;
                }
                if(count==0)
                {
                    lineNumber = 3;
                }
                break;
            case 3:
                if (!oneTime)
                {
                    oneTime = true;
                    Invoke("LinePP", 5f);
                }
                break;
            case 4:
                oneTime = false;
                if (fire1Up)
                {
                    lineNumber = 5;
                }
                break;
            case 5:
                if (!oneTime)
                {
                    oneTime = true;
                    Invoke("LinePP", 5f);
                }
                //if (party.characterScripts[party.charactersIndex].curChargeDelay <= 0.2f) lineNumber = 6;
                break;
            case 6:
                if (fire2Down)
                {
                    lineNumber = 7;
                }
                break;
            case 7:
                if (statue.isBreaking == true)
                {
                    text.gameObject.SetActive(false);
                    this.gameObject.SetActive(false);
                }
                break;
            default:
                
                break;

        }
    }

    void Show()
    {
        text.text = line[lineNumber];
    }

    void LinePP()
    {
        lineNumber++;
    }
}
