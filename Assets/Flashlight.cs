using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour {
	private RaycastHit hit;
    private HomeMenu homeMenu;

	public string EnemyTag = "Enemy";

	public int Distance = 50;
    public float FadeSpeed = .01f;
    public float RechargeSpeed = .01f;

    private Light Light;
	private OSPAudioSource LightSwitch;
    public static float batteryLevel = 1.0f;
    public static bool FlashlightOn = true;
    public static bool doUpdate = false;

    void Start()
    {
        Light = GetComponent<Light>();
		LightSwitch = GetComponent<OSPAudioSource> ();
		Light.enabled = false;
        //homeMenu = GameObject.FindGameObjectWithTag("DeathMenu").GetComponent<HomeMenu>();
    }
	
	void Update () {
        if (doUpdate)
        {
            if (!FlashlightOn)
            {
                rechargeBattery();
                return;
            }
            if (Input.GetButtonDown("Mouse 0"))
            {
                Light.enabled = true;
                LightSwitch.Play();
            }
            if (Input.GetButtonUp("Mouse 0"))
            {
                Light.enabled = false;
            }
            if (Input.GetButton("Mouse 1"))
            {
                RestartLevel();
            }
            if (Light.enabled)
            {
                fadeBattery();
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                if (Physics.Raycast(this.transform.position, forward, out hit, Distance))
                {
                    if (hit.transform && hit.transform.gameObject)
                    {
                        this.handleHit(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                rechargeBattery();
            }
        }
	}

    public static void RestartLevel()
    {
        Application.LoadLevel(0);
    }

    private void fadeBattery()
    {
        batteryLevel = Mathf.MoveTowards(batteryLevel, 0f, FadeSpeed);
        if (batteryLevel <= 0f && FlashlightOn)
        {
            turnLightOff();
        }
    }

    private void rechargeBattery()
    {
        batteryLevel = Mathf.MoveTowards(batteryLevel, 1f, RechargeSpeed);
        if (batteryLevel >= 1f && !FlashlightOn)
        {
            turnLightOn();
        }
    }

    private void turnLightOff()
    {
        FlashlightOn = false;
        Light.enabled = false;
    }

    private void turnLightOn()
    {
        FlashlightOn = true;
    }
	
	private void handleHit (GameObject gameObject) {
		if (gameObject.tag == EnemyTag) {
			gameObject.GetComponent<Enemy>().Vanquish();
		}
	}
}
