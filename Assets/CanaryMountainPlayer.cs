using UnityEngine;
using System.Collections;

public class CanaryMountainPlayer : MonoBehaviour {

    public float fadeSpeed;

    private HomeMenu homeMenu;
    private EnemyController enemyController;
    private SplineInterpolator splineInterpolator;
    private bool sceneEnding = false;
    private bool sceneEnded = false;

	void Start () {
        homeMenu = GameObject.FindGameObjectWithTag("DeathMenu").GetComponent<HomeMenu>();
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        splineInterpolator = GetComponent<SplineInterpolator>();
	}

    void Update()
    {
        if (sceneEnding)
        {
            //FadeToGrey();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!sceneEnded && !sceneEnding && collider.gameObject.tag == "Enemy")
        {
            var enemy = collider.gameObject.GetComponent<Enemy>();
            if (!enemy.dead)
            {
                FadeToGrey();
                splineInterpolator.Stop();
                sceneEnding = true;
                enemyController.GameOver();
                enemy.EatPlayer();
            }
            
        }
    }

    void FadeToGrey()
    {
        //RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 2F, fadeSpeed * Time.deltaTime);
        //if (RenderSettings.fogDensity >= 1F)
        //{
            sceneEnding = false;
            sceneEnded = true;
            homeMenu.ShowMenu(true);
        //}
    }
}
