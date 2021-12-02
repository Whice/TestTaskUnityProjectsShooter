using System;
using UnityEngine;

/// <summary>
/// Информация о летящей пуле.
/// </summary>
public class BulletFly : MonoBehaviour
{
    /// <summary>
    /// Информация об игроке.
    /// </summary>
    private PlayerInfo playerInfo = null;
    /// <summary>
    /// Указывает на объект-заготовку пули, которому принадлежит этот скрипт.
    /// </summary>
    public GameObject thisPerfab;
    /// <summary>
    /// Скорость движения пули.
    /// </summary>
    public Single speed = 100f;
    /// <summary>
    /// Таймер для подсчета растояния на основе скорости.
    /// </summary>
    private Single timerForSpeed = 0f;
    /// <summary>
    /// Ссылка для доступа к общему контроллеру.
    /// </summary>
    public FP_Controller controller;
    /// <summary>
    /// Номер в списке пуль контроллера.
    /// </summary>
    public Int32 numberInListController = 0;
    /// <summary>
    /// Дальность полета. При превышении пуля уничтожается.
    /// </summary>
    private Single rangeOfFlight;
    /// <summary>
    /// Материал объекта пули.
    /// </summary>
    private Material bulletMaterial = null;
    /// <summary>
    /// Установить случайный цвет ящика.
    /// </summary>
    public void SetColor(Color color)
    {
        if (this.bulletMaterial == null)
        {
            this.bulletMaterial = this.GetComponent<Renderer>().material;
        }
        this.bulletMaterial.color = color;
    }

    private void Start()
    {
        if(this.controller.shotSound==null)
            this.controller.shotSound = GetComponent<AudioSource>();
        this.controller.shotSound.Play();
        this.rangeOfFlight = GameObject.Find("Floor").transform.localScale.x*5+10;
    }

    void Update()
    {
        this.timerForSpeed += Time.deltaTime;
        //Движение пули каждый кадр.
        this.transform.position = this.transform.position + this.transform.forward * this.speed*timerForSpeed;
        this.timerForSpeed = 0f;

        //Уничтожение снаряда, если он улетел далеко.
        if (this.transform.position.y<-1||//Снаряд под землей.
            this.transform.position.x > this.rangeOfFlight ||
            this.transform.position.y > this.rangeOfFlight ||
            this.transform.position.z > this.rangeOfFlight
            )
        {
            this.controller.bullets.Remove(this.thisPerfab);
            this.controller.bulletsForDelete.Add(this.thisPerfab);
        }
    }
}
