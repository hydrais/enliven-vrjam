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
        StartGame();
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

    public static void StartGame()
    {
        EnemyController.doUpdate = true;
        Flashlight.doUpdate = true;
        var splineController = GameObject.FindGameObjectWithTag("Cart").GetComponent<SplineController>();
        splineController.FollowSpline();
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
                Flashlight.doUpdate = false;
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
            
        //}
    }
}
