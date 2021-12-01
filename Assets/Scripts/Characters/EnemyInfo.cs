using System;
using UnityEngine;

/// <summary>
/// Информация о враге.
/// </summary>
public class EnemyInfo : CharacterInfo
{
    /// <summary>
    /// Игровой ящик.
    /// </summary>
    public GameObject gameBox = null;
    /// <summary>
    /// Список мертвых врагов.
    /// В списке мертвых врагов хораняться еще или уже не задействованые враги.
    /// Таким образом на их создание и уничтожение не тратиться время.
    /// </summary>
    public System.Collections.Generic.List<GameObject> deadEnemy =null;
    /// <summary>
    /// Список живых врагов.
    /// </summary>
    public System.Collections.Generic.List<GameObject> aliveEnemy = null;
    /// <summary>
    /// Объект игрока.
    /// </summary>
    public GameObject player = null;
    /// <summary>
    /// Объект с доп. инормацией об игроке.
    /// </summary>
    private PlayerInfo playerInfo = null;
    /// <summary>
    /// Ссылка на объект камеры.
    /// </summary>
    private GameObject camera = null;
    /// <summary>
    /// Таймер для сдерживания атаки.
    /// </summary>
    private Single timerAtack = 0;
    /// <summary>
    /// Скорость движения врагов.
    /// </summary>
    public Single speed = 0.016f;
    /// <summary>
    /// Высота, на которой ходят живые враги.
    /// </summary>
    public const Single startPositionY = 0.6f;
    

    void Start()
    {
        this.playerInfo = player.GetComponent<PlayerInfo>();
        this.camera = GameObject.Find("Main Camera");
        this.gameBox = GameObject.Find("GameBox");
    }

    void Update()
    {
        //Если этот враг жив, то он может стремиться к убийству игрока.
        if (!this.isDead)
        {
            
            Vector3 thisPosition = this.transform.position;
            Vector3 targetPosition = new Vector3
                (
                this.player.transform.position.x,
                startPositionY,
                this.player.transform.position.z
                );

            //Двигаться к игроку
            if (Math.Abs(thisPosition.x - targetPosition.x) + Math.Abs(thisPosition.z - targetPosition.z) > 1.5f)
            {
                this.transform.position = Vector3.MoveTowards(thisPosition, targetPosition, this.speed);
            }
            //Либо атаковать раз в секунду
            else
            {
                this.timerAtack += Time.deltaTime;
                if (this.timerAtack > 1)
                {
                    this.playerInfo.healthPoints -= this.damage;
                    this.timerAtack = 0;
                }
            }

            //Повернуться к игроку
            this.transform.LookAt(new Vector3
                (
                this.camera.transform.position.x, 
                startPositionY, 
                this.camera.transform.position.z 
                ));
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        //При столкновении с пулей умертвить врага.
        if (other.gameObject.name == "Bullet(Clone)")
        {
            Destroy(other.gameObject);
            this.healthPoints -= this.playerInfo.damage;

            if (this.healthPoints == 0)
            {
                GameObject box = Instantiate(this.gameBox);
                box.transform.position = this.transform.position;
                box.transform.position = new Vector3(box.transform.position.x, 0.5f, box.transform.position.z);
                GameBoxInfo boxInfo = box.GetComponent<GameBoxInfo>();
                boxInfo.SetRandomColorForThisBox();
                this.isDead = true;
                this.aliveEnemy.Remove(this.gameObject);
                this.deadEnemy.Add(this.gameObject);
                this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, -99, this.gameObject.transform.position.y);


            }
        }
        else if (other.gameObject.name == "PlayerFront")
        {

            Debug.Log("Yes");
        }
    }
}
