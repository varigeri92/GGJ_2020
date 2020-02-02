using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum WallType
{
    Wood,
    Concrete
}
public class GameSystemManager : MonoBehaviour
{
    //Singleton:

    bool sandboxMode = false;
    static GameSystemManager instance;
    public static GameSystemManager Instance { get => instance; set => instance = value; }

    public delegate void OnBuildtimeOver();
    public static event OnBuildtimeOver onBuildtimeOver;

    public delegate void OnRoundOver();
    public static event OnRoundOver onRoundOver;

    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;

    public delegate void OnLevelComplette();
    public static event OnLevelComplette onLevelComplette;

    public LayerMask layerMask;

    [SerializeField]  int Wood;
    [SerializeField]  int Concrete;

    public List<Wall> walls = new List<Wall>();
    public List<Pig> pigs = new List<Pig>();

    public Vector3 worldMousePosition;

    public bool shootOver = false;

    bool gameOverOrWhatever;

    [SerializeField] TMP_Text shotLeftText;
    [SerializeField] TMP_Text pigsaliveText;
    [SerializeField] TMP_Text allpigsText;
    [SerializeField] int timetoBuild;
    int ttb = 0;
    [SerializeField] Image clockImageF;
    [SerializeField] Image clockImageB;

    [SerializeField] Button woodButton;
    [SerializeField] TMP_Text woodCountText;
    [SerializeField] Button concreteButton;
    [SerializeField] TMP_Text concreteCountText;

    [SerializeField] Button pigButton;

    [SerializeField] TMP_Text timeText;
    float imageFill  = 0f;

    bool build = false;
    bool front = false;

    [SerializeField] int expectedPigCount;
    [SerializeField] int pigsAlive;

    [SerializeField] int shotsLeft;

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

        allpigsText.text = expectedPigCount.ToString();
        mainCam = Camera.main;
        foreach( Wall wall in GameObject.FindObjectsOfType<Wall>())
        {
            walls.Add(wall);
        }
        timeText.text = timetoBuild.ToString();
        ttb = timetoBuild;

        woodCountText.text = Wood.ToString();
        concreteCountText.text = Concrete.ToString();
    }

    public void OnCanonShot()
    {
        shootOver = false;
        foreach (Wall wall in walls )
        {
            wall.TurnKinematic(false);
        }

        foreach (Pig pig in pigs)
        {
            pig.SetKinematic(false);
        }
        shotsLeft--;
        shotLeftText.text = shotsLeft.ToString();
    }

    public void OnShotOver()
    {
        if (shotsLeft <= 0)
        {
            gameOverOrWhatever = true;
            onLevelComplette();
            return;
        }

        foreach (Wall wall in walls)
        {
            wall.TurnKinematic(true);
        }
        Camera.main.GetComponent<CameraController>().ZoomIn();
        onRoundOver();
        build = true;
        ttb = timetoBuild;
        timeText.text = timetoBuild.ToString();
        FindPigs();
    }

    public void OnGameStarted()
    {
        build = true;
    }

    public void FindPigs()
    {
        pigsAlive = GameObject.FindGameObjectsWithTag("OurHeroes").Length;

        pigsaliveText.text = pigsAlive.ToString();

        if (pigsAlive == 0)
        {
            gameOverOrWhatever = true;
            build = false;
            onGameOver();
        }
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

    public void PigPlaced()
    {
        pigsAlive++;
        pigsaliveText.text = pigsAlive.ToString();
        if (pigsAlive == expectedPigCount)
        {
            pigButton.interactable = false;
        }
    }

    public void wallPlaced(WallType wallType)
    {
        switch (wallType)
        {
            case WallType.Wood:
                Wood --;
                break;
            case WallType.Concrete:
                Concrete --;
                break;

            default:
                break;
        }
        woodCountText.text = Wood.ToString();
        if (Wood == 0)
        {
            woodButton.interactable = false;
        }
        concreteCountText.text = Concrete.ToString();
        if (Concrete == 0)
        {
            concreteButton.interactable = false;
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
                ttb--;
                timeText.text = ttb.ToString();


                if (ttb == 3)
                {
                    cannon.Reset();
                }
                if (ttb == 0)
                {
                    build = false;
                    if (onBuildtimeOver != null)
                    {
                        onBuildtimeOver();
                    }
                    ttb = timetoBuild;
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
        Debug.Log("Timer started!");
        yield return new WaitForSeconds(5f);
        OnShotOver();
        //shootOver = true;
    }

    public void StartTimer()
    {
        Debug.Log("BOI!!");
        if (timer == null)
        {
            timer = KinematicTimer();
            StartCoroutine(timer);
        }
        else
        {
            timer = KinematicTimer();
            StartCoroutine(timer);
        }
    }
}
