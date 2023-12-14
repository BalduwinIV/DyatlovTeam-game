using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    // -- Objects --
    [SerializeField] private GameObject wallExample;
    [SerializeField] private bool showWallExample;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        foreach(GameObject invisibleWall in GameObject.FindGameObjectsWithTag("InvisibleWall"))
        {
            if (invisibleWall.GetComponent<MeshRenderer>())
            {
                invisibleWall.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    void Update() 
    {
        setWallExampleActiveState(showWallExample);
    }

    void setWallExampleActiveState(bool state) 
    {
        wallExample.SetActive(state);
    }
}