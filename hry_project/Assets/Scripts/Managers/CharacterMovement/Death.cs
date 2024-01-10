using UnityEngine;

/* Defines character behaviour on death. */
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
        /* Check if character under death trigger height. */
        if(this.transform.position.y <= -6.5f)
        {
            /* Check in which part of the level character is. */
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
