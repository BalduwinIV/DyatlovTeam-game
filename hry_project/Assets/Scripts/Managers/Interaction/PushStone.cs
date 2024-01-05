using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushStone : MonoBehaviour
{
    [SerializeField] private float attachmentDistance = 2.1f;
    [SerializeField] private GameObject firstStone;
    [SerializeField] private GameObject secondStone;
    [SerializeField] private GameObject thirdStone;

    private GameObject objectToPush;
    private float distance;
    private bool canPush = false;
    private bool isPushed = false;
    private bool isPushing = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        objectToPush = FindNearestObjectWithTag("StoneToPush");
        if (objectToPush != null){
            distance = Vector3.Distance(this.transform.position, objectToPush.transform.position);
            if (distance <= attachmentDistance){
                    canPush = true;
            }
            else 
            {
                canPush = false;
            }
        }
        if (canPush && PlayerManager.instance.actionButtonIsPressed() && !isPushed && !isPushing)
        {
            PlayerManager.instance.setIsPushing(true);
            isPushing = true;
        }
        if(canPush && !PlayerManager.instance.actionButtonIsPressed() && !isPushed && isPushing)
        {
            PlayerManager.instance.setIsPushing(false);
            isPushing = false;
        }
    }

    void SetIsPushed()
    {
        isPushed = true;
    }

    GameObject FindNearestObjectWithTag(string tag){
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        if (objectsWithTag.Length == 0) return null;
        GameObject nearestObject = null;
        float nearestDistance = float.MaxValue;
        foreach(GameObject obj in objectsWithTag){
            if (obj != null) {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < nearestDistance){
                    nearestObject = obj;
                    nearestDistance = distance;
                }
            }
        }
        return nearestObject;
    }

    void activateStoneAnimation()
    {
        PuzzleManager.instance.startStoneAnimation();
    }

}
