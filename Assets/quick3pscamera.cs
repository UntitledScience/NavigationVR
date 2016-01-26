using UnityEngine;
using System.Collections;

public class quick3pscamera : MonoBehaviour {

    public bool thirdPersonTracking = false;
    public GameObject target;
    public float lerpRate = 0.3f;
    public float offsetThreshold = 0.01f;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - target.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (thirdPersonTracking && (offset.magnitude > offsetThreshold))
        {
            // if magnitude difference isn't bigger than offsetThreshold, don't translate the camera and reduce judder
            Vector3 lerp = Vector3.Lerp(transform.position, (target.transform.position + offset), lerpRate);
            Vector3 diff = lerp - transform.position;
            if (diff.magnitude > offsetThreshold)
            {
                transform.position = lerp;
            }
        }
	}
}
