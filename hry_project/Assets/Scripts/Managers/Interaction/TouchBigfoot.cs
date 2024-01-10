using UnityEngine;

public class TouchBigfoot : MonoBehaviour
{
    [SerializeField] private float attachmentDistance = 2.1f;
    private GameObject objectToTouch;
    private float distance;
    private bool canTouch = false;
    private bool isTouched = false;

    void FixedUpdate()
    {
        objectToTouch = FindNearestObjectWithTag("Bigfoot");
        if (objectToTouch != null){
            distance = Vector3.Distance(this.transform.position, objectToTouch.transform.position);
            if (distance <= attachmentDistance){
                    canTouch = true;
            }
            else
            {
                canTouch = false;
            }
        }
        if (canTouch && PlayerManager.instance.actionButtonIsPressed() && !isTouched)
        {
            PlayerManager.instance.lockMovement();
            PlayerManager.instance.setTouchingTrigger();
            ActivateBigfootAttack();
            isTouched = true;
        }
        
    }

    void SetIsTouched()
    {
        isTouched = true;
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

    void ActivateBigfootAttack()
    {
        PuzzleManager.instance.startBigfootAttackAnimation();
    }
}
