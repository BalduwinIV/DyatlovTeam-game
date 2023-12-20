using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    // -- Objects --
    [SerializeField] private GameObject[] stones;
    private int stoneNumber;
    private float speedStoneAnimation;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        stoneNumber = 0;
        speedStoneAnimation = 1;
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

    
    public void startStoneAnimation()
    {
        if(stoneNumber < stones.Length)
        {
            stones[stoneNumber].GetComponent<Animator>().SetFloat("Speed", speedStoneAnimation);
            stones[stoneNumber++].GetComponent<Animator>().SetTrigger("Fall");
            speedStoneAnimation-=0.3f;
        }

    }
}