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
    public int ammoCount = 37;

    /// <summary>
    /// Ссылка для доступа к общему контроллеру.
    /// </summary>
    private FP_Controller controller;

    private int ammo;
    private float delay;
    private bool reloading;

	void Start () 
    {
        PlayerModel.instance.playerView.textAmmoCount.text = "Запас пуль: " + this.ammoCount.ToString();
        this.controller = gameObject.GetComponent<FP_Controller>();
        ammo = ammoCount;
        this.reloadBulletSound = GetComponent<AudioSource>();
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

    /// <summary>
    /// Объект пули.
    /// </summary>
    public GameObject bulletPrefab = null;
    /// <summary>
    /// Объект камеры.
    /// </summary>
    public GameObject camera = null;
    void Shoot()
    {
        if (ammoCount > 0)
        {
            //Создание префаба и скрипта пули и заполнение их полей.
            {
                Vector3 cameraPosition = this.camera.transform.position;
                Vector3 cameraForward = this.camera.transform.forward;
                BulletModel bulletModel = ArenaModel.instance.GetLastNotActiveBullet();
                Vector3 bulletPosition = new Vector3
                    (
                    cameraPosition.x + cameraForward.x*3,
                    cameraPosition.y,
                    cameraPosition.z + cameraForward.z*3
                    );
                bulletModel.Activate(bulletPosition, cameraForward);

                this.ammoCount--;
                PlayerModel.instance.playerView.textAmmoCount.text = "Запас пуль: " + this.ammoCount.ToString();
            }
        }

        delay = Time.time + shootRate;
    }

    #endregion


    /// <summary>
    /// Звук перезарядки пуль.
    /// </summary>
    private AudioSource reloadBulletSound = null;
    IEnumerator Reload()
    {
        this.reloadBulletSound.Play();
        this.reloading = true;
        Debug.Log("Reloading");
        yield return new WaitForSeconds(reloadTime);
        this.ammoCount = this.ammo;
        Debug.Log("Reloading Complete");
        reloading = false;
        PlayerModel.instance.playerView.textAmmoCount.text = "Запас пуль: " + this.ammoCount.ToString();
    }

    void OnGUI()
    {
       // GUILayout.Label("AMMO: " + ammoCount);
    }
}
