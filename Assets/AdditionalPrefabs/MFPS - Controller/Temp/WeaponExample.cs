using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This is not a functional weapon script. It just shows how to implement shooting and reloading with buttons system.
/// </summary>
public class WeaponExample : MonoBehaviour
{
    public FP_Input playerInput;

    public float shootRate = 0.15F;
    public float reloadTime = 1.0F;
    /// <summary>
    /// Запас пуль.
    /// </summary>
    public Int32 ammoCount
    {
        get
        {
            return PlayerModel.instance.ammoCount;
        }
        set
        {
            PlayerModel.instance.ammoCount = value;
        }
    }
    /// <summary>
    /// Ссылка для доступа к общему контроллеру.
    /// </summary>
    private FP_Controller controller;

    /// <summary>
	/// Максимальный запас пуль.
	/// </summary>
    private int maxAmmoCount
    {
        get => PlayerModel.instance.maxAmmoCount;
    }
    private float delay;
    private bool reloading;

    void Start()
    {
        PlayerModel playerModel = PlayerModel.instance;
        PlayerView playerView = playerModel.playerView;
        playerView.textAmmoCount.text = "Запас пуль: " + this.ammoCount.ToString();
        PlayerModel.instance.playerView.textAmmoCount.text = "Запас пуль: " + this.ammoCount.ToString();
        this.controller = gameObject.GetComponent<FP_Controller>();
    }

    void Update()
    {
        if (playerInput.Shoot())                         //IF SHOOT BUTTON IS PRESSED (Replace your mouse input)
            if (Time.time > delay)
                Shoot();

        if (playerInput.Reload())                        //IF RELOAD BUTTON WAS PRESSED (Replace your keyboard input)
            if (!reloading && ammoCount < maxAmmoCount)
            {
                StartCoroutine(Reload());
            }
    }

    #region Стрельба

    /// <summary>
    /// Объект камеры.
    /// </summary>
    private GameObject cameraPrivate = null;
    /// <summary>
    /// Объект камеры.
    /// </summary>
    public GameObject camera
    {
        get
        {
            if (this.cameraPrivate == null)
            {
                this.cameraPrivate = CameraModel.instance.gameObject;
            }
            return this.cameraPrivate;
        }
    }
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
                    cameraPosition.x + cameraForward.x * 3,
                    cameraPosition.y,
                    cameraPosition.z + cameraForward.z * 3
                    );
                bulletModel.Activate(bulletPosition, cameraForward);

                //Установить цвет пули
                BulletView bulletView = bulletModel.view as BulletView;
                bulletView.SetColor(PlayerModel.instance.bulletColor);

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
    [SerializeField]
    private AudioSource reloadBulletSoundField = null;
    /// <summary>
    /// Звук перезарядки пуль.
    /// </summary>
    private AudioSource reloadBulletSound
    {
        get
        {
            String name = "ReloadBullets";
            if (this.reloadBulletSoundField.name!=name)
            {
                this.reloadBulletSoundField.clip = ArenaModel.instance.arenaView.GetAudioClip(name);
            }
            return this.reloadBulletSoundField;
        }
    }
    IEnumerator Reload()
    {
		Debug.LogError("Reload!");
        this.reloadBulletSound.Play();
        this.reloading = true;
        this.ammoCount = this.maxAmmoCount;
        reloading = false;
        PlayerModel.instance.playerView.textAmmoCount.text = "Запас пуль: " + this.ammoCount.ToString();
        yield return new WaitForSeconds(reloadTime);
    }

    void OnGUI()
    {
       // GUILayout.Label("AMMO: " + ammoCount);
    }
}
