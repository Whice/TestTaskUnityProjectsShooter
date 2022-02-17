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
                    if (this.healthPoints<1)
                    {
                        this.Deactivate();
                    }
                }
                break;
        }
    }

    #region Действия при создании объекта.

    /// <summary>
    /// Установить урон для этого врага.
    /// </summary>
    protected virtual void SetDamage()
    {
        this.damage = 5;
    }
    /// <summary>
    /// Установить ХП для этого врага.
    /// </summary>
    protected virtual void SetHealth()
    {
        this.healthPoints = 10;
    }

    /// <summary>
    /// Задание значений полям при создании объекта. 
    /// </summary>
    protected virtual void InitializeEnemy()
    {
        this.floorHeight = ArenaModel.instance.arenaFloor.transform.position.y;
        SetDamage();
    }

    private void Start()
    {
        InitializeEnemy();
    }

    #endregion

    #region Действия при каждом кадре.

    /// <summary>
    /// Убить снеговика, который упал слишком низко.
    /// </summary>
    protected void KillEnemyWithoutArena()
    {
        if (this.transform.position.y < this.floorHeight - 5)
        {
            this.Deactivate();
        }
    }
    /// <summary>
    /// Поворот к игроку включен.
    /// </summary>
    protected Boolean isEnableTurnToPlayer = true;
    /// <summary>
    /// Повернуться к игроку.
    /// </summary>
    protected void TurnToPlayer()
    {
        if(this.isEnableTurnToPlayer)
        {
            this.transform.LookAt(this.targetPosition);
        }
    }

    private void Update()
    {
        //Убить снеговика, который упал слишком низко.
        KillEnemyWithoutArena();

        //Повернуться к игроку
        TurnToPlayer();
    }

    #endregion

    #region Активация/деактивация

    /// <summary>
    /// Физика этого врага.
    /// </summary>
    public Rigidbody rigidbody = null;
    protected ArenaModel arenaModel
    {
        get => ArenaModel.instance;
    }
    /// <summary>
    /// Получить трофей за этого врага.
    /// </summary>
    protected virtual void GetTrophy() { }
    /// <summary>
    /// Сделать неактивным.
    /// </summary>
    public virtual void Deactivate()
    {
        this.rigidbody.useGravity = false;
        this.gameObject.SetActive(false);
        GetTrophy();
    }
    /// <summary>
    /// Задать начальную позицию.
    /// </summary>
    protected virtual void SetBeginPosition()
    {
    }
    /// <summary>
    /// Добавить врага к живым.
    /// </summary>
    public virtual void Activate()
    {
        this.gameObject.SetActive(true);

        //Задать начальную позицию.
        SetBeginPosition();

        this.rigidbody.useGravity = true;
        this.rigidbody.AddForce(new Vector3(0, 10, 0));
    }

    #endregion

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

    #endregion

    #region Данные о местоположении врага или игрока.

    /// <summary>
    /// Ссылка на камеру игрока.
    /// </summary>
    protected Camera mainCamera
    {
        get=> CameraModel.instance.cameraView.mainCamera;
    }
    /// <summary>
    /// Значение высоты врага.
    /// </summary>
    protected Single yHeight
    {
        get
        {
            if (this.transform.position.y > 0.2f)
            {
                if(!this.rigidbody.useGravity)
                    this.rigidbody.useGravity = true;
            }
            else
            {
                if (this.rigidbody.useGravity)
                    this.rigidbody.useGravity = false;


                if (this.transform.position.y < this.floorHeight)
                {
                    this.transform.position = new Vector3
                       (
                       this.transform.transform.position.x,
                       this.floorHeight + this.floorHeight/10,
                       this.transform.transform.position.z
                       );
                }
                else if (this.transform.position.y > this.floorHeight)
                {
                    this.transform.position = new Vector3
                       (
                       this.transform.transform.position.x,
                       this.floorHeight - this.floorHeight/10,
                       this.transform.transform.position.z
                       );
                }

            }

            return this.transform.position.y;
        }
        set
        {
            this.transform.position = new Vector3
                (
                this.transform.transform.position.x,
                value,
                this.transform.transform.position.z
                );
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
