using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChoicePanel : MonoBehaviour
{
    public bool t;
    public bool tUp;

    public GameManager gameManager;
    public GameObject wheelParent;
    public Camera playerCamera;

    public Vector2 dot;

    [SerializeField] Canvas canvas;
    GraphicRaycaster gr;
    PointerEventData ped;

    int num = -1;
    public int cNum;

    Vector2 center;
    Vector2 mousePos;

    void Start()
    {
        gr = canvas.GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
        DisableWheel();
        center = dot;
    }

    void Update()
    {
        GetInput();
        if (t)
        {
            EnableWheel();
            RaycastShoot();
        }
        else if (tUp)
        {
            DoSwitch();
            DisableWheel();
        }


    }

    void GetInput()
    {
        t = Input.GetButton("tab");
        tUp = Input.GetButtonUp("tab");
    }

    void EnableWheel()
    {
        if (wheelParent != null)
            wheelParent.SetActive(true);
    }

    void RaycastShoot()
    {
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        if(results.Count != 0)
        {
            GameObject obj = results[0].gameObject;
            if(obj.CompareTag("ChoiceButton")) //히트 된 오브젝트의 태그와 맞으면 실행
            {
                switch (obj.name)
                {
                    case "Choice1":
                        num = 1;
                        break;
                    case "Choice2":
                        num = 2;
                        break;
                    case "Choice3":
                        num = 3;
                        break;
                    default:
                        num = -1;
                        break;
                        
                }
            }
        }
    }

    void DoSwitch()
    {
        cNum = num;
    }

    void DisableWheel()
    {
        if (wheelParent != null)
            wheelParent.SetActive(false);
    }
    /*
    float Area(Vector2 v1, Vector2 v2, Vector2 v3)
    {
        return Mathf.Abs((v1.x * (v2.y - v3.y) + v2.x * (v3.y - v1.y) + v3.x * (v1.y - v2.y)) / 2f);
    }

    bool IsInside(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v)
    {
        float A = Area(v1, v2, v3);
        float A1 = Area(v1, v2, v);
        float A2 = Area(v1, v, v3);
        float A3 = Area(v, v2, v3);

        return (Mathf.Abs(A1 + A2 + A3 - A) < 1f);
    }

    void CheckForCurrentWeapon()
    {
        if (playerCamera == null)
            return;

        for(int i = 0; i < pos.Length; i++)
        {
            //Changing World coordinates to screen coordinates
            pos[i] = playerCamera.WorldToScreenPoint(dots[i].position);
        }

        mousePos = Input.mousePosition;

        if(IsInside(pos[0], pos[1], pos[2], mousePos))
        {
            //Selected Weapon is 0
            EnableHighLight(0);
            
        }
        if (IsInside(pos[0], pos[2], pos[3], mousePos))
        {
            //Selected Weapon is 1
            EnableHighLight();

        }
        if (IsInside(pos[0], pos[3], pos[4], mousePos))
        {
            //Selected Weapon is 2
            EnableHighLight(0);

        }
    }

    void EnableHighLight(int index)
    {
        for(int i = 0; i < wheels.Length; i++)
        {
            if(wheels[i].wheel != null && wheels[i].highlightSprite != null)
            {
                if (i == index)
                    wheels[i].wheel.sprite = wheels[i].highlightSprite;
                else
                    wheels[i].wheel.sprite = wheels[i].NormalSprite;
            }
        }
    }

    void DisableHighLight()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i].wheel != null)
            {
                    wheels[i].wheel.sprite = wheels[i].NormalSprite;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        for(int i = 0; i < dots.Length; i++)
        {
            pos[i].x = dots[i].position.x;
            pos[i].y = dots[i].position.y;
        }

        start.x = pos[0].x;
        start.y = pos[0].y;

        for(int i = 0; i < pos.Length; ++i)
        {
            end.x = pos[i].x;
            end.y = pos[i].y;
            Debug.DrawLine(start, end, Color.red);
        }
        for(int i = 0; i < pos.Length - 1; ++i)
        {
            start.x = pos[i].x;
            start.y = pos[i].y;
            end.x = pos[i + 1].x;
            end.y = pos[i + 1].y;
            Debug.DrawLine(start, end, Color.red);
        }

        //For the last Triangle
        start.x = pos[3].x;
        start.y = pos[3].y;
        end.x = pos[1].x;
        end.y = pos[1].y;
        Debug.DrawLine(start, end, Color.red);
    }*/
}
