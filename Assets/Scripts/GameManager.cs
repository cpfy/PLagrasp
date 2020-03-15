using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AbilityType
{
    Walk,    Train,    Plane,
    Deal,    Dinner,    Shut,    Mask,
    Bat,    Fight,    Rebel,    Tired,
    None
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text date;
    /*
    public Text infecttext;
    public Text recovertext;
    public Text dietext;

    private float I = 1f;
    private float R = 0f;
    private float D = 0f;

    public float r = 4f;  //接触人数
    public float b = 0.04f;   //感染率
    public float a = 0.1f;  //康复率
    public float d = 0.001f;    //死亡率
    int N = 1400000000;
    float S = 1400000000f;
    */
    public float daytime = 3f;
    float daytimer;
    int day = 1;
    int[] daypermonth = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
    string[] monthname = { "", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
    int month = 1;
    int year = 9102;
    int wholeday = 1;
    
    Vector2 position;
    public GameObject dotPrefab;
    public GameObject warningPrefab;
    private AbilityType abilityType = AbilityType.None;

    public GameObject humanHome;
    public GameObject humanPrefab;
    private List<GameObject> VirusList = new List<GameObject>();
    public float attackScale = 1;
    public float speedScale = 1;

    public int sample = 0;
    public int humanPrice = 10;
    public Text sampleText;
    private List<GameObject> HumanList = new List<GameObject>();
    private bool neverman=true;

    public GameObject endcanvas;
    public GameObject endtext;
    private bool ended = false;

    private float duringTime;
    public GameObject invincibleCanvas;
    public GameObject harderCanvas;
    private List<GameObject> HideVirusList = new List<GameObject>();
    private List<GameObject> tempList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        daytimer = daytime;
    }

    void Update()
    {
       daytimer -= Time.deltaTime;
       if (daytimer < 0)
       {
           DayGone();
           daytimer = daytime;
       }

       if(VirusList.Count==0 && HideVirusList.Count==0 && wholeday>20)
        {
            if(!ended)
            {
                EndGame();
                ended = true;
            }
        }
        if (neverman && wholeday == 3)
        {
            Addman(new Vector2(8, 10));
            neverman = false;
        } 

        if (abilityType != AbilityType.None && Input.GetMouseButtonDown(0))
        {
            switch (abilityType)
            {
                case AbilityType.Walk:
                    PutVirus(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    break;
                case AbilityType.Train:
                    PutTrainVirus(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    break;
                case AbilityType.Plane:
                    PutPlaneVirus(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    break;
                case AbilityType.Shut:
                    StartCoroutine(IMInvincible(duringTime));
                    break;
                case AbilityType.Mask:
                    StartCoroutine(HarderVirus(duringTime));
                    break;
                default:
                    break;
            }
            SetBasicBool();
        }
    }
   
   void DayGone()
   {
       //CalNum();
       wholeday++;
       CalDate();
   }
    /*
   void CalNum()
   {
       float temp1 = r * b * I * S / N;
       float temp2 = a * I;
       float temp3 = d * I;

       I += temp1 - temp2;
       S += -temp1;
       R += temp2;
       D += temp3;

       infecttext.text = I.ToString();
       recovertext.text = R.ToString();
       dietext.text = D.ToString();
   }
   */
   void CalDate()
   {
        day++;
        if (day> daypermonth[month])
       {
           if(month==12)
           {
               day -= daypermonth[month];
               year++;
               month = 1;
           }
           else
           {
               day -= daypermonth[month];
               month++;
           }
       }
       date.text = year.ToString()+" "+monthname[month] + "." + day.ToString();
   }

    public void SetAbility(AbilityType abilityType, float duringTime)
    {
        this.duringTime = duringTime;
        this.abilityType = abilityType;
        switch (abilityType)
        {
            case AbilityType.Deal:
                StartCoroutine(RandomAppear(duringTime));
                SetBasicBool();
                break;
            case AbilityType.Dinner:
                StartCoroutine(RandomSpread(duringTime));
                SetBasicBool();
                break;
            case AbilityType.Bat:
                SetBasicBool();
                break;
            case AbilityType.Fight:
                humanPrice += 2;
                SetBasicBool();
                break;
            case AbilityType.Rebel:
                SlowerAttackPower();
                SetBasicBool();
                break;
            case AbilityType.Tired:
                SlowerSpeed();
                SetBasicBool();
                break;
            default:
                break;
        }
    }

    public void SetBasicBool()
    {
        GameObject.Find(abilityType.ToString()).GetComponent<AbilityButton>().StartCD();
        this.abilityType = AbilityType.None;
    }

    public void PutVirus(Vector2 position)
    {
        GameObject newvirus = Instantiate(dotPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        RegisterVirus(newvirus);

        GameObject newwarning = Instantiate(warningPrefab, new Vector3(position.x, position.y+0.7f, 0), Quaternion.identity);
        Destroy(newwarning, 3);
    }

    public void PutTrainVirus(Vector2 position)
    {
        GameObject newvirus = Instantiate(dotPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        var particles = newvirus.GetComponent<ParticleSystem>().main;
        particles.startLifetime = 60f;
        RegisterVirus(newvirus);

        GameObject newwarning = Instantiate(warningPrefab, new Vector3(position.x, position.y + 0.7f, 0), Quaternion.identity);
        Destroy(newwarning, 3);
    }

    public void PutPlaneVirus(Vector2 position)
    {
        GameObject newvirus = Instantiate(dotPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        var particles = newvirus.GetComponent<ParticleSystem>().main;
        particles.startLifetime = 60f;
        particles.startSpeed = 0.2f;
        RegisterVirus(newvirus);

        GameObject newwarning = Instantiate(warningPrefab, new Vector3(position.x, position.y + 0.7f, 0), Quaternion.identity);
        Destroy(newwarning, 3);
    }

    IEnumerator RandomAppear(float itsduringTime)
    {
        while (itsduringTime>0)
        {
            yield return new WaitForSeconds(0.1f);
            itsduringTime -= 0.1f;
            if(Random.value<0.5 / 10)
            {
                PutVirus(new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f)));
                print("随机生成！");
            }
        }
    }

    IEnumerator RandomSpread(float itsduringTime)
    {
        int viruschoose;
        Vector2 startposition;
        while (itsduringTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            itsduringTime -= 0.1f;
            if (Random.value < 0.1)
            {
                viruschoose=Random.Range(0, VirusList.Count);
                startposition = VirusList[viruschoose].transform.position;
                PutVirus(new Vector2(startposition.x + Random.Range(1f, 2f), startposition.y + Random.Range(1f, 2f)));
                print("随机散布！");
            }
        }
    }

    IEnumerator IMInvincible(float itsduringTime)
    {
        print("无敌于天下！");
        float radius = 5.3f;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject yellowcircle=Instantiate(invincibleCanvas, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        for (int i = VirusList.Count-1; i >=0 ; i--)
        {
            GameObject item = VirusList[i];
            if (Vector3.Distance(item.transform.position, new Vector3(pos.x,pos.y,0)) < radius)
            {
                item.GetComponent<Virus>().isInvincible = true;
                VirusList.Remove(item);
                HideVirusList.Add(item);
                //print("有一个");
            }
        }

        yield return new WaitForSeconds(itsduringTime);

        for (int i = HideVirusList.Count-1; i >0; i--)
        {
            GameObject item = HideVirusList[i];
            item.GetComponent<Virus>().isInvincible = false;
            HideVirusList.Remove(item);
            VirusList.Add(item);
        }
        Destroy(yellowcircle,0.1f);
    }

    IEnumerator HarderVirus(float itsduringTime)
    {
        print("攻吾之盾！");
        float radius = 5.3f;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject graycircle = Instantiate(harderCanvas, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        for (int i = VirusList.Count - 1; i >= 0; i--)
        {
            GameObject item = VirusList[i];
            if (Vector3.Distance(item.transform.position, new Vector3(pos.x, pos.y, 0)) < radius)
            {
                item.GetComponent<Virus>().isHarder = true;
                tempList.Add(item);
            }
        }
        yield return new WaitForSeconds(itsduringTime);

        for(int i=tempList.Count-1;i>=0;i--)
        {
            GameObject item = tempList[i];
            item.GetComponent<Virus>().isHarder = false;
            tempList.Remove(item);
        }
        Destroy(graycircle, 0.1f);
    }

    private void SlowerAttackPower()
    {
        if (attackScale > 0.11) attackScale *= 0.9f;
        /*
        //humanPrefab.GetComponent<Human>().attackpower -= 0.1f;
        for (int i=0;i<HumanList.Count;i++)
        {
            float po=HumanList[i].GetComponent<Human>().attackpower;
            if (po > 0.1)
            {
                HumanList[i].GetComponent<Human>().attackpower -= 0.1f;
            }
        }*/
    }

    private void SlowerSpeed()
    {
        if (speedScale > 0.11) speedScale *= 0.9f;
    }

    public void RegisterVirus(GameObject virus)
    {
        VirusList.Add(virus);
    }
    public void LogOutVirus(GameObject virus)
    {
        VirusList.Remove(virus);
    }

    public Transform GetHumanTarget(Transform humanTransform)
    {
        Transform targetTrans = humanTransform;
        float direction = 100;
        for(int i=0;i<VirusList.Count;i++)
        {
            GameObject item = VirusList[i];
            if (Vector3.Distance(item.transform.position, humanTransform.position) < direction)
            {
                direction = Vector3.Distance(item.transform.position, humanTransform.position);
                targetTrans = item.transform;
            }
        }
        return targetTrans;
    }

    public void AddLab(int num)
    {
        sample += num;
        sampleText.text = sample.ToString();
        Invoke("BuyHuman", 1f);
    }

    public void BuyHuman()
    {
        if (sample / humanPrice != 0)
        {
            int num = sample / humanPrice;
            for (int i = 0; i < num; i++)
            {
                Addman(new Vector2(humanHome.transform.position.x + Random.Range(0f, 2f), humanHome.transform.position.y + Random.Range(0, 2)));
            }
            sample = sample % humanPrice;
            sampleText.text = sample.ToString();
        }
    }
    public void Addman(Vector2 position)
    {
        GameObject newman = Instantiate(humanPrefab, new Vector3(position.x,position.y,0), Quaternion.identity);
        HumanList.Add(newman);
    }

    private void EndGame()
    {
        Time.timeScale = 0.2f;
        TextOne();
        Invoke("ChangeText", 1f);
        Invoke("PreQuit", 2.6f);
    }
    private void PreQuit()
    {
        endcanvas.SetActive(false);
        Application.Quit();
    }

    private void TextOne()
    {
        endtext.GetComponent<Text>().text = "第" + wholeday.ToString() + "日，后遂无问津者";
        endcanvas.SetActive(true);
    }

    private void ChangeText()
    {
        endtext.GetComponent<Text>().text = "疾肆毒涌君莫嘲，已云消，拾樱好，共赏花娇。";
    }

}