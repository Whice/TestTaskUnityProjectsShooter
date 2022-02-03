using System;
using UnityEngine;

/// <summary>
/// Модель врага в игре.
/// </summary>
public class EnemyModel : GameCharacterModel
{
    protected override void OnChanged(string propertyName, object oldValue, object newValue)
    {
        base.OnChanged(propertyName, oldValue, newValue);
        switch(propertyName)
        {
            case nameof(this.healthPoints):
                {
                    if(this.healthPoints<1)
                    {
                        this.Deactivate();
                    }
                }
                break;
        }
    }

    protected virtual void Start()
    {
        this.floorHeight = ArenaModel.instance.arenaFloor.transform.position.y;
    }
    protected virtual void Update()
    {
        //Убить снеговика, который упал слишком низко.
        if(this.transform.position.y<-5)
        {
            this.Deactivate();
        }

        //Повернуться к игроку
        this.transform.LookAt(this.targetPosition);
    }

    /// <summary>
    /// Физика этого врага.
    /// </summary>
    public Rigidbody rigidbody = null;
    protected ArenaModel arenaModel
    {
        get => ArenaModel.instance;
    }
    /// <summary>
    /// Сделать неактивным.
    /// </summary>
    public virtual void Deactivate()
    {
        this.rigidbody.useGravity = false;
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// Добавить врага к живым.
    /// </summary>
    public virtual void Activate()
    {
        this.gameObject.SetActive(true);

        //Задать ему начальную позицию.
        this.SetRandomPositionWithoutCameraVision();

        this.rigidbody.useGravity = true;
        this.rigidbody.AddForce(new Vector3(0, 10, 0));
    }


    #region Установка случайного местоположения.

    /// <summary>
    /// Установить случайное местоположение вне зоны видимости камеры.
    /// </summary>
    /// <returns></returns>
    public virtual void SetRandomPositionWithoutCameraVision()
    {
        
    }

    #endregion

    #region Урон.

    /// <summary>
    /// Таймер для сдерживания атаки.
    /// </summary>
    protected Single timerAtack = 0;

    /// <summary>
    /// Нанести урон этому врагу.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void ApplyDamage(Int32 damage)
    {
        this.healthPoints -= damage;
    }

    #endregion

    #region Данные о местоположении врага или игрока.


    /// <summary>
    /// Ссылка на камеру игрока.
    /// </summary>
    private Camera mainCameraPrivate = null;
    /// <summary>
    /// Ссылка на камеру игрока.
    /// </summary>
    protected Camera mainCamera
    {
        get
        {
            if (this.mainCameraPrivate == null)
            {
                this.mainCameraPrivate = CameraModel.instance.gameObject.GetComponent<Camera>();
            }
            return this.mainCameraPrivate;
        }
    }
    /// <summary>
    /// Значение высоты врага.
    /// </summary>
    protected Single yHeightField = 0;
    /// <summary>
    /// Значение высоты врага.
    /// </summary>
    protected Single yHeight
    {
        get
        {
            return this.transform.position.y;
        }
        set
        {
            this.yHeightField = value;
            /*
             * Если враг над полом, то пусть падает.
             * Если нет, то убрать физику
             */
            if (this.yHeightField > 0)
            {
                this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                this.yHeightField = 0;

                this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            }
        }
    }
    /// <summary>
    /// Цель, к которой стремится снеговик.
    /// </summary>
    protected Vector3 targetPosition
    {
        get => new Vector3
            (
            this.mainCamera.transform.position.x,
            this.yHeight,
            this.mainCamera.transform.position.z
            );
    }
    /// <summary>
    /// Высота на которой находится пол.
    /// Y координата.
    /// </summary>
    protected Single floorHeight;

    #endregion
}
