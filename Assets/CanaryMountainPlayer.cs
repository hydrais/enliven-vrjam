using UnityEngine;
using System.Collections;

public class CanaryMountainPlayer : MonoBehaviour {

    public float fadeSpeed;

    private HomeMenu homeMenu;
    private bool sceneEnding = false;
    private bool sceneEnded = false;

	void Start () {
        homeMenu = GameObject.FindGameObjectWithTag("DeathMenu").GetComponent<HomeMenu>();
	}

    void Update()
    {
        if (sceneEnding)
        {
            FadeToGrey();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!sceneEnded && collider.gameObject.tag == "Enemy")
        {
            FadeToGrey();
            sceneEnding = true;
        }
    }

    void FadeToGrey()
    {
        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 2F, fadeSpeed * Time.deltaTime);
        if (RenderSettings.fogDensity >= 1F)
        {
            sceneEnding = false;
            sceneEnded = true;
            homeMenu.ShowMenu(true);
        }
    }
}
