using System;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    [Range (0.01f, 10f)]
    public float pushForce = 1.0f;
    [Range (0.1f, 10f)]
    public float pushTriggerDistance = 1.5f;

    private GameObject objectToPush;

    private bool isPushing = false;
    private bool canPush = false;

    void FixedUpdate(){
        canPush = false;
        Ray pushRay = new Ray(transform.position + Vector3.up, transform.forward);
        if (Physics.Raycast(pushRay, out RaycastHit hitInfo, pushTriggerDistance)) {
            if (hitInfo.rigidbody.gameObject.tag == "ObjectToPush") {
                Debug.DrawRay(transform.position + Vector3.up, transform.forward * pushTriggerDistance, Color.green);
                objectToPush = hitInfo.rigidbody.gameObject;
                canPush = true;
            } else {
                Debug.DrawRay(transform.position + Vector3.up, transform.forward * pushTriggerDistance, Color.yellow);
            }
        } else {
            Debug.DrawRay(transform.position + Vector3.up, transform.forward * pushTriggerDistance, Color.yellow);
        }

        if (canPush && PlayerManager.instance.actionButtonIsPressed() && !isPushing){
            isPushing = true;
            PlayerManager.instance.setIsPushing(true);
        }
        if ((!canPush || !PlayerManager.instance.actionButtonIsPressed()) && isPushing) {
            isPushing = false;
            PlayerManager.instance.setIsPushing(false);
        }
        if (isPushing){
            float inputHorizontal = PlayerManager.instance.getRawMovementInputX();
            float pushSpeed = pushForce * inputHorizontal * Time.fixedDeltaTime;
       
            objectToPush.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(pushForce, 0, 0), transform.position, ForceMode.Impulse);
        }
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
