using System;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    [SerializeField] private float pushForce = 1.0f;
    [SerializeField] private float attachmentDistance = 2.1f;
    [SerializeField] private float speedReduction = 0.5f;
    private float distance;
    private float originalSpeed;

    private GameObject objectToPush;
    private Vector3 attachmentPoint;

    private bool isAttached = false;
    private bool isPushing = false;
    private bool canPush = false;


    void Start(){
        objectToPush = FindNearestObjectWithTag("ObjectToPush");
        if (objectToPush != null){
            distance = Vector3.Distance(this.transform.position, objectToPush.transform.position);
            if (distance <= attachmentDistance){
                    canPush = true;
            }
        }
    }

    void FixedUpdate(){
        //originalSpeed = PlayerManager.instance.getMovementVectorXComponent();
        Debug.Log($"Speed: {originalSpeed}");
        if (canPush && PlayerManager.instance.actionButtonIsPressed()){
            TogglePush();
        }
        if (isAttached){
            float inputHorizontal = PlayerManager.instance.getRawMovementInputX();
            
            if (isPushing){
                float pushSpeed = pushForce * inputHorizontal;
                objectToPush.transform.Translate(Vector3.right * pushSpeed * Time.deltaTime);
                // PlayerManager.instance.setMovementVectorXComponent(originalSpeed * speedReduction);
                // Debug.Log($"Reduced speed:: {originalSpeed * speedReduction}");
            }
        }
        // else {
        //     PlayerManager.instance.setMovementVectorXComponent(originalSpeed);
        // }
    }

    public float getSpeedReduction(){
        return speedReduction;
    }

    void TogglePush(){
        isPushing = !isPushing;
        PlayerManager.instance.setPushingState(isPushing);

        if (isPushing){
            AttachToObject();
        }
        else{
            DetachFromObject();
        }
    }

    void AttachToObject(){
        float playerWidth = GetComponent<Collider>().bounds.size.x;
        float objectWidth = objectToPush.GetComponent<Collider>().bounds.size.x;

        float offset = (playerWidth + objectWidth) / 4;
        float attachmnetX = objectToPush.transform.position.x;

        if (transform.position.x < objectToPush.transform.position.x) {
            attachmnetX -= offset;
        }
        else {
            attachmnetX += offset + objectWidth;
        }
        attachmentPoint = new Vector3(attachmnetX, transform.position.y, transform.position.z);

        transform.position = attachmentPoint;
        isAttached = true;
    }

    void DetachFromObject(){
        isAttached = false;
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
}
