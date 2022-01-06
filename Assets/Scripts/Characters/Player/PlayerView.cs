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
	/// Начало текста для отображения количества пуль.
	/// </summary>
	private const String titleTextAmmoCount = "Запас пуль: ";
	/// <summary>
	/// Текст для отображения количества пуль.
	/// </summary>
	public Text textAmmoCount = null;
	/// <summary>
	/// Начало текста для отображения очков жизни.
	/// </summary>
	private const String titleTextHealth = "Очки жизни: ";
	/// <summary>
	/// Текст для отображения очков жизни.
	/// </summary>
	public Text textHealth = null;
	/// <summary>
	/// Начало текста для отображения количества красных кубов.
	/// </summary>
	private const String titleTextRedCubesCount = "Красных кубов: ";
	/// <summary>
	/// Текст для отображения количества красных кубов.
	/// </summary>
	public Text textRedCubesCount = null;
	/// <summary>
	/// Начало текста для отображения количества зеленых кубов.
	/// </summary>
	private const String titleTextGreenCubesCount = "Зеленых кубов: ";
	/// <summary>
	/// Текст для отображения количества зеленых кубов.
	/// </summary>
	public Text textGreenCubesCount = null;
	/// <summary>
	/// Начало текста для отображения количества желтых кубов.
	/// </summary>
	private const String titleTextYellowCubesCount = "Желтых кубов: ";
	/// <summary>
	/// Текст для отображения количества желтых кубов.
	/// </summary>
	public Text textYellowCubesCount = null;

	#endregion

	/// <summary>
	/// Создать строку текста на экране.
	/// </summary>
	/// <param name="textOnCanvas"></param>
	/// <param name="title"></param>
	/// <param name="value"></param>
	private void CreateTextLine(Text textOnCanvas, String title, Int32 value)
	{
		if (textOnCanvas != null)
		{
			StringBuilder text = this.stringBuilderForText;
			text.Clear();
			text.Append(title);
			text.Append(value);
			textOnCanvas.text = text.ToString();
		}
	}
	/// <summary>
	/// Обновить все строки текста в интерфейсе.
	/// </summary>
	public void UpdateText()
	{
		//Вывести данные о состоянии игрока и его достижениях на экран.
		{
			PlayerModel playerModel = model as PlayerModel;

			CreateTextLine(this.textAmmoCount, titleTextAmmoCount, playerModel.ammoCount);
			CreateTextLine(this.textHealth, titleTextHealth, playerModel.healthPoints);
			CreateTextLine(this.textRedCubesCount, titleTextRedCubesCount, playerModel.redCubesCount);
			CreateTextLine(this.textGreenCubesCount, titleTextGreenCubesCount, playerModel.greenCubesCount);
			CreateTextLine(this.textYellowCubesCount, titleTextYellowCubesCount, playerModel.yellowCubesCount);
		}
	}
}
