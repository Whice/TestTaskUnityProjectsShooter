using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerModel : GameCharacterModel
{
	public PlayerView playerView
    {
		get => this.view as PlayerView;
    }
	#region Реализация синглтона

	private PlayerModel() { }

	/// <summary>
	/// Объект главного класса приложения.
	/// </summary>
	private static PlayerModel instancePrivate = null;
	/// <summary>
	/// Объект главного класса приложения.
	/// </summary>
	public static PlayerModel instance
	{
		get => instancePrivate == null ? new PlayerModel() : instancePrivate;
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
		if (this.healthPoints < 100)
		{
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
		}

		//Обновить текст в интерфейсе
		this.playerView.UpdateText();
	}

	#endregion

	void Start()
	{
		this.healthPoints = 100;
	}

    protected override void OnChanged(string propertyName, object oldValue, object newValue)
    {
        base.OnChanged(propertyName, oldValue, newValue);

		switch(propertyName)
        {
			case (nameof(this.healthPoints)):
                {
					if (isDead)
					{
						SceneManager.LoadSceneAsync(0);
					}
				}
				break;
        }
    }
}