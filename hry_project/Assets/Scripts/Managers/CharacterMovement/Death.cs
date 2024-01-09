using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private Vector3 startPosition;
    public string columnTag = "Wall";
    // public string fallingStoneTag = "FallingStone";

    void Start()
    {
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if(this.transform.position.y <= -6.5f)
        {
            if(this.transform.position.x > -60f && this.transform.position.x < -20f)
            {
                this.transform.position = new Vector3(-60f, 5f, this.transform.position.z);
            }
            else if(this.transform.position.x >= -20f && this.transform.position.x < 40f)
            {
                this.transform.position = new Vector3(-15f, 5.5f, this.transform.position.z);
                ResetCollumnPuzzle();
            }
        }
    }

    private void ResetCollumnPuzzle()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag(columnTag);

        foreach (GameObject obstacle in obstacles)
        {
            
            Animator obstacleAnimator = obstacle.GetComponent<Animator>();
            if (obstacleAnimator != null)
            {
                obstacleAnimator.Play("ColumnIdle", 0, 0); 
            }
            
        }
    }
    


}
