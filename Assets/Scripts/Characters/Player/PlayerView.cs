using System;
using System.Text;
using UnityEngine.UI;

public class PlayerView : GameCharacterView
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

	public void UpdateText()
	{
		//Вывести данные о состоянии игрока и его достижениях на экран.
		{
			PlayerModel playerModel = model as PlayerModel;
			StringBuilder text = this.stringBuilderForText;
			text.Clear();
			text.Append("Очки жизни: ");
			text.Append(playerModel.healthPoints.ToString());

			this.textHealthAndCountCubes.text = text.ToString();
			text.Clear();
			text.Append("Красных кубов: ");
			text.Append(playerModel.redCubesCount.ToString());

			this.textRedCubesCount.text = text.ToString();
			text.Clear();
			text.Append("Зеленых кубов: ");
			text.Append(playerModel.greenCubesCount.ToString());

			this.textGreenCubesCount.text = text.ToString();
			text.Clear();
			text.Append("Желтых кубов: ");
			text.Append(playerModel.yellowCubesCount.ToString());

			this.textYellowCubesCount.text = text.ToString();
		}
	}
}
