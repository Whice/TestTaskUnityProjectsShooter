using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerModel : GameCharacterModel
{
	/// <summary>
	/// Скорость ходьбы.
	/// </summary>
	public Single walkSpeed = 6.0f;
	/// <summary>
	/// Скрорсть бега.
	/// </summary>
	public Single runSpeed = 11.0f;
	/// <summary>
	/// Максимальный запас пуль.
	/// </summary>
	public Int32 maxAmmoCount = 47;
	/// <summary>
	/// Запас пуль.
	/// </summary>
	private Int32 ammoCountPrivate = 37;
	/// <summary>
	/// Запас пуль.
	/// </summary>
	public Int32 ammoCount
	{
		get => this.ammoCountPrivate;
		set => SetValueProperty(nameof(ammoCount), ref this.ammoCountPrivate, value);
	}
	public PlayerView playerView
	{
		get => this.view as PlayerView;
	}
	public GameUI gameUI;

	#region Реализация синглтона

	/// <summary>
	/// Объект главного класса приложения.
	/// </summary>
	public static PlayerModel instance = null;
	public bool isDestroyed = false;
	private void Awake()
	{
		if (PlayerModel.instance == null)
		{
			PlayerModel.instance = this;
			DontDestroyOnLoad(this);
			this.isDestroyed = false;
		}
		else
		{
			Destroy(this.gameObject);
			this.isDestroyed = true;
		}
	}

	void OnDestroy()
	{
		this.isDestroyed = true;
	}
	#endregion

	#region Цвета кубиков и снарядов.

	/// <summary>
	/// Количество красных кубов.
	/// </summary>
	private Int32 redCubesCountPrivate = 0;
	/// <summary>
	/// Количество красных кубов.
	/// </summary>
	public Int32 redCubesCount
	{
		get => this.redCubesCountPrivate;
	}
	/// <summary>
	/// Количество зеленых кубов.
	/// </summary>
	private Int32 greenCubesCountPrivate = 0;
	/// <summary>
	/// Количество зеленых кубов.
	/// </summary>
	public Int32 greenCubesCount
	{
		get => this.greenCubesCountPrivate;
	}
	/// <summary>
	/// Количество желтых кубов.
	/// </summary>
	private Int32 yellowCubesCountPrivate = 0;
	/// <summary>
	/// Количество желтых кубов.
	/// </summary>
	public Int32 yellowCubesCount
	{
		get => this.yellowCubesCountPrivate;
	}
	/// <summary>
	/// Цвет пуль игрока.
	/// </summary>
	private Color bulletColorPrivate = Color.white;
	/// <summary>
	/// Цвет пуль игрока.
	/// </summary>
	public Color bulletColor
	{
		get => this.bulletColorPrivate;
	}

	/// <summary>
	/// Добавить куб определенного цвета к статистике игрока.
	/// </summary>
	/// <param name="cubeColor">Цвет собранного куба.</param>
	public void AddColoredCube(Color cubeColor)
	{
		this.bulletColorPrivate = cubeColor;

		if (cubeColor == Color.red)
		{
			++this.redCubesCountPrivate;
		}
		else if (cubeColor == Color.green)
		{
			++this.greenCubesCountPrivate;
		}
		else if (cubeColor == Color.yellow)
		{
			++this.yellowCubesCountPrivate;
		}

		//Пополнение очков жизни при накоплении ящиков.
		if (cubeColor == Color.green && this.greenCubesCount % 10 == 0)
		{
			this.healthPoints += 10;
		}
		else if (cubeColor == Color.red && this.redCubesCount % 10 == 0)
		{
			this.healthPoints += 10;
		}
		else if (cubeColor == Color.yellow && this.yellowCubesCount % 10 == 0)
		{
			this.healthPoints += 10;
		}

		//Обновить текст в интерфейсе
		this.playerView.UpdateText();
	}

	#endregion

	void Start()
	{
		this.healthPoints = this.maxHealthPoint;
		this.ammoCountPrivate = 47;
		//Обновить текст в интерфейсе
		this.playerView.UpdateText();
		gameUI.mainMenuButton.onClick.AddListener(LoadMainMenu);
	}
	public void LoadMainMenu()
	{
		SceneManager.LoadSceneAsync("MainMenu");
		if (ArenaModel.instance != null)
		{
			ArenaModel.instance.SetInGame(false);
		}
	}
	protected override void OnChanged(string propertyName, object oldValue, object newValue)
	{
		base.OnChanged(propertyName, oldValue, newValue);

		switch (propertyName)
		{
			case (nameof(this.healthPoints)):
				{
					if (isDead)
					{
						LoadMainMenu();
						MainApplicationClass.instance.DeactivateAllSingletons();
					}

					//Обновить текст в интерфейсе
					this.playerView.UpdateText();
					break;
				}
			case (nameof(this.ammoCount)):
				{
					//Обновить текст в интерфейсе
					this.playerView.UpdateText();
					break;
				}
		}
	}

	#region Расстояние до стен.

	/// <summary>
	/// Стены.
	/// </summary>
	public GameObject[] walls
	{
		get => ArenaModel.instance.walls;
	}
	/// <summary>
	/// Получить расстояние от игрока до ближайшей стены.
	/// <br/>Расстояние до стены не может стать меньше, чем 0.55.
	/// </summary>
	/// <returns></returns>
	public Single GetDistanceToWall()
	{
		/*
		 * Т.к. стены расположены перпендикулярно на арене, 
		 * то расчет можно выполнять по минимальному x или y
		 * к ближайшей из стен.
		 * Минимальное значение - 0.5, когда игрок стоит вплотную.
		 */

		Single nearestDistance = Single.MaxValue;
		Single distanceToWall;
		Vector3 wallPosition;
		foreach (GameObject wall in this.walls)
		{
			wallPosition = wall.transform.position;
			if (wallPosition.x != 0)
			{
				distanceToWall = Math.Abs(wall.transform.position.x - this.transform.position.x);
				if (distanceToWall < nearestDistance)
				{
					nearestDistance = distanceToWall;

				}
			}

			if (wallPosition.z != 0)
			{
				distanceToWall = Math.Abs(wall.transform.position.z - this.transform.position.z);
				if (distanceToWall < nearestDistance)
				{
					nearestDistance = distanceToWall;
				}
			}
		}

		return nearestDistance;
	}

	#endregion
}