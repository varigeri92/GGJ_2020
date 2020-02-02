using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemManager : MonoBehaviour
{
    //Singleton:

    static GameSystemManager instance;
    public static GameSystemManager Instance { get => instance; set => instance = value; }

    public delegate void OnBuildtimeOver();
    public static event OnBuildtimeOver onBuildtimeOver;

    public delegate void OnRoundOver();
    public static event OnRoundOver onRoundOver;

    public LayerMask layerMask;

    public List<Wall> walls = new List<Wall>();

    public Vector3 worldMousePosition;
    public bool shootOver = false;

    [SerializeField] int timetoBuild = 30;
    [SerializeField] UnityEngine.UI.Image clockImageF;
    [SerializeField] UnityEngine.UI.Image clockImageB;

    UnityEngine.UI.Image currentImage;

    [SerializeField] TMPro.TMP_Text timeText;
    float imageFill  = 0f;

    bool build = false;
    bool front = false;

    [SerializeField] int expectedPigCount;
    [SerializeField] int pigsAlive;

    [SerializeField] AiCannon cannon;
    Camera mainCam;
    // Test// 

    public Rigidbody WreckingBall;



    IEnumerator timer;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        mainCam = Camera.main;
        foreach( Wall wall in GameObject.FindObjectsOfType<Wall>())
        {
            walls.Add(wall);
        }
    }

    public void OnCanonShot()
    {
        shootOver = false;
        foreach (Wall wall in walls )
        {
            wall.TurnKinematic(false);
        }
    }

    public void OnShotOver()
    {
        foreach (Wall wall in walls)
        {
            wall.TurnKinematic(true);
        }
        if (onRoundOver != null)
        {
            onRoundOver();
        }
        FindPigs();
    }

    public void OnGameStarted()
    {
        build = true;
    }

    public void FindPigs()
    {
        pigsAlive = GameObject.FindGameObjectsWithTag("OurHeroes").Length;
    }


    public void AddWall(Wall wall)
    {
        walls.Add(wall);
    }

    /// TEST ///
    /// 

    private void Update()
    {

        BuildTimer();

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 hitpoint = Vector3.zero;
        if (Physics.Raycast(ray, out hit, 40f ,layerMask))
        {
            hitpoint = hit.point;
            hitpoint.z = 0;
            worldMousePosition = hitpoint;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            WreckingBall.AddForce(new Vector3(1,0,0) * 1, ForceMode.Impulse);

            OnCanonShot();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnShotOver();
        }
    }

    public void CreateElement(GameObject element)
    {
        if (build)
        {
            Instantiate(element, worldMousePosition, Quaternion.identity);
        }
    }

    void BuildTimer()
    {
        if (build)
        {
            imageFill += Time.deltaTime;
            if (imageFill >= 1)
            {
                imageFill = 0;
                timetoBuild--;
               
               
                if (timetoBuild == 3)
                {
                    cannon.Reset();

                }
                if (timetoBuild == 0)
                {
                    build = false;
                    if (onBuildtimeOver != null)
                    {
                        onBuildtimeOver();
                    }
                    timetoBuild = 30;
                }
            }
            if (front)
            {
                clockImageF.fillAmount = imageFill;
                clockImageB.fillAmount = 1;
            }
            else
            {
                clockImageB.fillAmount = imageFill;
                clockImageF.fillAmount = 0;
            }
            if (imageFill == 0)
            {
                front = !front;
            }

        }
    }


    IEnumerator KinematicTimer()
    {
        yield return new WaitForSeconds(5f);
        OnShotOver();
        //shootOver = true;
    }

    public void StartTimer()
    {
        if (timer == null)
        {
            timer = KinematicTimer();
            StartCoroutine(timer);
        }
        else
        {
            StopCoroutine(timer);
            StartCoroutine(timer);
        }
    }
}
