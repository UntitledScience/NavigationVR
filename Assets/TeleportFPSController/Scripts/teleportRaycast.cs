using UnityEngine;
using System.Collections;

public class teleportRaycast : MonoBehaviour {

    public float charRadius = 0.75f;
    public GameObject teleportReticle;
    public GameObject teleportReticleRed;
    public GameObject rightArm;
    public float teleportMaxDist = 6f;
    public Vector3 resetPosition = new Vector3(50, 50, 50);

    private bool canTeleport = false;
    private GameObject tpRecGreen;
    private GameObject tpRecRed;
    private int leftSteamControllerIndex;
    private int rightSteamControllerIndex;

	// Use this for initialization
	void Start () {
	
        if (GameObject.Find("teleportReticle") == null)
        {
            tpRecGreen = (GameObject)Instantiate(teleportReticle, resetPosition, Quaternion.identity);
            MakeInvisible(tpRecGreen);
        }

        if (GameObject.Find("teleportReticleRed") == null)
        {
            tpRecRed = (GameObject)Instantiate(teleportReticleRed, resetPosition, Quaternion.identity);
            MakeInvisible(tpRecRed);
        }

        // initialize Steam Controllers
        rightSteamControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);

    }
	
	// polling camera's transform before culling to render
	void OnPreCull () {

        if (SteamVR_Controller.Input(rightSteamControllerIndex).GetPress(SteamVR_Controller.ButtonMask.Trigger))

            // position teleportReticle where you desire to be teleported
            //if (Input.GetButton("Fire2"))
        {
            PositionReticle();
        }
	
	}

    void Update()
    {

        if (SteamVR_Controller.Input(rightSteamControllerIndex).GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        //if (Input.GetButtonUp("Fire2"))
        {
            TeleportToPoint();
        }
    }

    void PositionReticle()
    {
		// Vector3 fwd = Camera.main.transform.TransformDirection(transform.forward);
        RaycastHit hit;

        // if raycast hit, position reticle at raycast
		if (Physics.Raycast(rightArm.transform.position, rightArm.transform.forward, out hit, teleportMaxDist))
        {
			Debug.DrawLine(rightArm.transform.position, hit.point, Color.red);
            Vector3 dir = hit.normal;
            MakeVisible(tpRecGreen);
            tpRecGreen.transform.position = hit.point + (dir * charRadius);
            MakeInvisible(tpRecRed);
            canTeleport = true;

        } else {
            // if not, position reticle at teleportMaxDist away
            // Vector3 dir = (Camera.main.transform.position - fwd).normalized;
            // print(dir);
            MakeVisible(tpRecRed);
			tpRecRed.transform.position = rightArm.transform.position + (rightArm.transform.forward.normalized * teleportMaxDist);
            MakeInvisible(tpRecGreen);
            canTeleport = false;
        }

    }

    void TeleportToPoint()
    {
        if (canTeleport)
        {
            transform.parent.parent.position = tpRecGreen.transform.position;
            MakeInvisible(tpRecGreen);

        } else
        {
            MakeInvisible(tpRecGreen);
            MakeInvisible(tpRecRed);
        }

    }

    void MakeVisible(GameObject GO)
    {
        if (!GO.activeInHierarchy)
        {
            GO.SetActive(true);
        }   
    }

    void MakeInvisible(GameObject GO)
    {
        if (GO.activeInHierarchy)
        {
            GO.SetActive(false);
        }

        GO.transform.position = resetPosition;
    } 
}
