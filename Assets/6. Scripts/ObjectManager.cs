//////////////////////////////오브젝트 풀링 사용방법/////////////////////////////////////////////////////////////////////////////////
///0. 넣어서 사용하고 싶은 프리펩이 있다!
///1. ManagerGroup에 있는 ObjectManager를 클릭
///2. ObjectInfos의 사이즈를 원하는 만큼 늘린다
///3. 만들어진 빈 클래스에 사용하고 싶은 프리펩을 넣는다!(이름, 프리펩, 개수) : 이름은 반드시 프리팹 이름과 동일해야 함!
///4. 생성: 사전에 ObjectManager objectManager 깔아준 후 objectManager.MakeObj(이름, 위치, 각도)를 적어주면 된다. 
///       ex)GameObject bullet = objectManager.MakeObj(6, this.transform.position, Quaternion.Euler(0, 0, 0));
///5. 삭제: objectManager.ObjReturn(this.gameObject)를 사용한다. 자세한 것은 bullet.cs에 있는 Update()와 Dequeue()함수 참조할 것!
///       (bullet의 경우 미리 만들어 둔 Dequeue() 함수를 사용하면 된다.
///6. Profit!
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo //프리펩 정보 클래스
{
    public string objectName; //오브젝트 이름
    public GameObject perfab; //프리펩
    public int count; //개수
}

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    [SerializeField]
    ObjectInfo[] objectInfos = null;

    [Header("오브젝트 풀의 위치")]
    [SerializeField]
    Transform tfPoolParent;

    public List<Queue<GameObject>> objectPoolList;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        objectPoolList = new List<Queue<GameObject>>();
        ObjectPoolState();
    }

    void ObjectPoolState()
    {
        if (objectInfos != null)
        {
            for (int i = 0; i < objectInfos.Length; i++)
            {
                objectPoolList.Add(InsertQueue(objectInfos[i]));
            }
        }
    }

    Queue<GameObject> InsertQueue(ObjectInfo perfab_objectInfo)
    {
        Queue<GameObject> test_queue = new Queue<GameObject>();

        for (int i = 0; i < perfab_objectInfo.count; i++)
        {
            GameObject objectClone = Instantiate(perfab_objectInfo.perfab) as GameObject;
            objectClone.SetActive(false);
            objectClone.transform.SetParent(tfPoolParent);
            test_queue.Enqueue(objectClone);
        }

        return test_queue;
    }

    public GameObject MakeObj(string name, Vector3 pos, Quaternion rot) //프리펩 활성화(이름, 위치, 각도)
    {
        GameObject objTest = null;

        for (int i = 0; i < objectInfos.Length; i++) //이름 찾기
        {
            if (name == objectInfos[i].objectName)
            {
                if (objectPoolList[i].Count > 0) //큐가 비어있지 않을 경우(가져다 씀)
                {
                    objTest = ObjectManager.instance.objectPoolList[i].Dequeue();
                }
                else //큐가 비어있을 경우(새로 생성)
                {
                    objTest = Instantiate(objectInfos[i].perfab) as GameObject;
                    objTest.SetActive(false);
                    objTest.transform.SetParent(tfPoolParent);
                    //ObjectManager.instance.objectPoolList[num].Enqueue(objTest);
                    //objTest = ObjectManager.instance.objectPoolList[num].Dequeue();
                }
                break;
            }
        }

        objTest.transform.position = pos;
        objTest.transform.rotation = rot;
        objTest.SetActive(true);
        return objTest;
    }

    public IEnumerator ObjReturn(GameObject _obj) //프리펩 비활성화
    {
        string realName = _obj.name.Substring(0, _obj.name.Length - 7); //진짜 이름. 씬 내에서 이름(clone)이 붙기 때문에 (clone)을 제거해주는 용도
        

        for(int i = 0; i < objectInfos.Length; i++) //이름 찾기
        {
            if (realName == objectInfos[i].objectName)
            {
                ObjectManager.instance.objectPoolList[i].Enqueue(_obj);
                _obj.SetActive(false);
                yield return null;
            }
        }

        yield return null;
    }
}
