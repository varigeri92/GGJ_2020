using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemManager : MonoBehaviour
{
    //Singleton:

    static GameSystemManager instance;
    public static GameSystemManager Instance { get => instance; set => instance = value; }



    public List<Wall> walls = new List<Wall>();

    public Vector3 worldMousePosition;

    // Test// 

    public Rigidbody WreckingBall;


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


        foreach( Wall wall in GameObject.FindObjectsOfType<Wall>())
        {
            walls.Add(wall);
        }
    }

    void OnCanonShot()
    {
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
    }


    /// TEST ///
    /// 

    private void Update()
    {
        worldMousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
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
}
