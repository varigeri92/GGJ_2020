using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemManager : MonoBehaviour
{
    //Singleton:

    static GameSystemManager instance;
    public static GameSystemManager Instance { get => instance; set => instance = value; }

    public LayerMask layerMask;

    public List<Wall> walls = new List<Wall>();

    public Vector3 worldMousePosition;
    public bool shootOver = false;

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
            if (wall.GetMagnitude() < 0.5)
            {
                wall.TurnKinematic(true);
            }
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
        Instantiate(element, worldMousePosition, Quaternion.identity);
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
