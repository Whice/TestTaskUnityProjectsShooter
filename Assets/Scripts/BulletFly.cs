using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : MonoBehaviour
{
    /// <summary>
    /// Указывает на объект-заготовку пули, которому принадлежит этот скрипт
    /// </summary>
    public GameObject thisPerfab;
    /// <summary>
    /// Скорость движения пули.
    /// </summary>
    public Single speed = 0.1f;
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
    private Single rangeOfFlight = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Движение пули каждый кадр.
        this.transform.position = this.transform.position + this.transform.forward * this.speed;

        //Уничтожение снаряда, если он улетел далеко.
        if(this.transform.position.x>this.rangeOfFlight || 
            this.transform.position.y> this.rangeOfFlight || 
            this.transform.position.z> this.rangeOfFlight
            )
        {
            this.controller.bullets.Remove(this.thisPerfab);
            this.controller.bulletsForDelete.Add(this.thisPerfab);
        }
    }
}
