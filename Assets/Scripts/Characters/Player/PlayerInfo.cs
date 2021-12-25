using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
///  Класс информации об игроке.
/// </summary>
public class PlayerInfo : CharacterInfo
{
    #region Поля для вывода текста.

    /// <summary>
    /// Буффер для более быстрой склейки строк.
    /// </summary>
    private StringBuilder stringBuilderForText = new StringBuilder(30);
	/// <summary>
	/// Текст для отображения очков жизни.
	/// </summary>
	public Text textAmmoCount = null;
	/// <summary>
	/// Текст для отображения очков жизни.
	/// </summary>
	public Text textHealthAndCountCubes = null;
	/// <summary>
	/// Текст для отображения количества красных кубов.
	/// </summary>
	public Text textRedCubesCount = null;
	/// <summary>
	/// Текст для отображения количества зеленых кубов.
	/// </summary>
	public Text textGreenCubesCount = null;
	/// <summary>
	/// Текст для отображения количества желтых кубов.
	/// </summary>
	public Text textYellowCubesCount = null;

    #endregion

    #region Цвета кубиков и снарядов.

    /// <summary>
    /// Количество красных кубов.
    /// </summary>
    private Int32 redCubesCount = 0;
	/// <summary>
	/// Количество зеленых кубов.
	/// </summary>
	private Int32 greenCubesCount = 0;
	/// <summary>
	/// Количество желтых кубов.
	/// </summary>
	private Int32 yellowCubesCount = 0;
	/// <summary>
	/// Цвет пуль игрока.
	/// </summary>
	public Color bulletColor = Color.white;

	/// <summary>
	/// Добавить куб определенного цвета к статистике игрока.
	/// </summary>
	/// <param name="cubeColor">Цвет собранного куба.</param>
	public void AddColoredCube(Color cubeColor)
	{
		this.bulletColor = cubeColor;

		if (cubeColor == Color.red)
		{
			++this.redCubesCount;
		}
		else if(cubeColor == Color.green)
        {
			++this.greenCubesCount;
        }
		else if(cubeColor == Color.yellow)
        {
			++this.yellowCubesCount;
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

	}

	#endregion    

	// Start is called before the first frame update  
	void Start()
	{
		this.healthPoints = 100;
	}

	// Update is called once per frame
	void Update()
	{
		if(isDead)
        {
			SceneManager.LoadSceneAsync(0);
        }
		//Вывести данные о состоянии игрока и его достижениях на экран.
		{
			StringBuilder text = this.stringBuilderForText;
			text.Clear();
			text.Append("Очки жизни: ");
			text.Append(this.healthPoints.ToString());

			this.textHealthAndCountCubes.text = text.ToString();
			text.Clear();
			text.Append("Красных кубов: ");
			text.Append(this.redCubesCount.ToString());

			this.textRedCubesCount.text = text.ToString();
			text.Clear();
			text.Append("Зеленых кубов: ");
			text.Append(this.greenCubesCount.ToString());

			this.textGreenCubesCount.text = text.ToString();
			text.Clear();
			text.Append("Желтых кубов: ");
			text.Append(this.yellowCubesCount.ToString());

			this.textYellowCubesCount.text = text.ToString();
		}
	}
}