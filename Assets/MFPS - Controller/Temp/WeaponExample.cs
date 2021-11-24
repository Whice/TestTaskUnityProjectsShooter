using UnityEngine;
using System.Collections;

/// <summary>
/// This is not a functional weapon script. It just shows how to implement shooting and reloading with buttons system.
/// </summary>
public class WeaponExample : MonoBehaviour 
{
    public FP_Input playerInput;

    public float shootRate = 0.15F;
    public float reloadTime = 1.0F;
    public int ammoCount = 15;

    /// <summary>
    /// Ссылка для доступа к общему контроллеру.
    /// </summary>
    private FP_Controller controller;

    private int ammo;
    private float delay;
    private bool reloading;

	void Start () 
    {
        GameObject gameObject = GameObject.Find("Player");
        this.controller = gameObject.GetComponent<FP_Controller>();
        ammo = ammoCount;
	}
	
	void Update () 
    {
        if(playerInput.Shoot())                         //IF SHOOT BUTTON IS PRESSED (Replace your mouse input)
            if(Time.time > delay)
                Shoot();

        if (playerInput.Reload())                        //IF RELOAD BUTTON WAS PRESSED (Replace your keyboard input)
            if (!reloading && ammoCount < ammo)
                StartCoroutine("Reload");
	}

    #region Стрельба

    public GameObject bulletPrefab = null;

    void Shoot()
    {
        if (ammoCount > 0)
        {
            //Создание префаба и скрипта пули и заполнение их полей.
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 1.5f, transform.rotation);
                bullet.transform.forward = this.transform.forward;
                BulletFly bulletFly = bullet.GetComponent<BulletFly>();
                bulletFly.controller = this.controller;
                bulletFly.numberInListController = this.controller.bullets.Count;
                bulletFly.thisPerfab = bullet;
                this.controller.bullets.Add(bullet);
            }

            Debug.Log("Shoot");
            ammoCount--;
        }
        else
            Debug.Log("Empty");

        delay = Time.time + shootRate;
    }

    #endregion

    IEnumerator Reload()
    {
        reloading = true;
        Debug.Log("Reloading");
        yield return new WaitForSeconds(reloadTime);
        ammoCount = ammo;
        Debug.Log("Reloading Complete");
        reloading = false;
    }

    void OnGUI()
    {
        GUILayout.Label("AMMO: " + ammoCount);
    }
}
