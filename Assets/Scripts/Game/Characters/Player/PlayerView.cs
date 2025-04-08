using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : GameCharacterView
{
	#region кнопка бега.

	/// <summary>
	/// Окружность-картинка кнопки бега.
	/// </summary>
	public Image runButtonCircle = null;
	/// <summary>
	/// Текст кнопки бега.
	/// </summary>
	public Text runButtonText = null;

	#endregion кнопка бега.

	#region Поля для вывода текста.

	/// <summary>
	/// Текст, который сообщает о том, что появился босс.
	/// </summary>
	public GameObject endBossText = null;
	/// <summary>
	/// Буффер для более быстрой склейки строк.
	/// </summary>
	private StringBuilder stringBuilderForText = new StringBuilder(30);
	/// <summary>
	/// Начало текста для отображения количества пуль.
	/// </summary>
	private const String titleTextAmmoCount = "Bullets: ";
	/// <summary>
	/// Текст для отображения количества пуль.
	/// </summary>
	public Text textAmmoCount = null;
	/// <summary>
	/// Начало текста для отображения очков жизни.
	/// </summary>
	private const String titleTextHealth = "HP: ";
	/// <summary>
	/// Текст для отображения очков жизни.
	/// </summary>
	public Text textHealth = null;
	/// <summary>
	/// Начало текста для отображения количества красных кубов.
	/// </summary>
	private const String titleTextRedCubesCount = "Red cubes: ";
	/// <summary>
	/// Текст для отображения количества красных кубов.
	/// </summary>
	public Text textRedCubesCount = null;
	/// <summary>
	/// Начало текста для отображения количества зеленых кубов.
	/// </summary>
	private const String titleTextGreenCubesCount = "Green cubes: ";
	/// <summary>
	/// Текст для отображения количества зеленых кубов.
	/// </summary>
	public Text textGreenCubesCount = null;
	/// <summary>
	/// Начало текста для отображения количества желтых кубов.
	/// </summary>
	private const String titleTextYellowCubesCount = "Yellow cubes: ";
	/// <summary>
	/// Текст для отображения количества желтых кубов.
	/// </summary>
	public Text textYellowCubesCount = null;

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

	#endregion

	#region Звук.

	/// <summary>
	/// Звук всасывания трофея.
	/// </summary>
	[SerializeField]
	private AudioSource suckTrophyPrivate = null;
	/// <summary>
	/// Звук всасывания трофея.
	/// </summary>
	public AudioSource suckTrophy
	{
		get
		{
			String name = "SuckTrophy";
			if (this.suckTrophyPrivate.clip.name != name)
			{
				this.suckTrophyPrivate.clip = ArenaModel.instance.arenaView.GetAudioClip(name);
			}
			return this.suckTrophyPrivate;
		}
	}

	/// <summary>
	/// Страшная музыка.
	/// </summary>
	[SerializeField]
	private AudioSource horrorBackground = null;

	#endregion

	#region Изменение страшной музыки при движении игрока.

	/// <summary>
	/// Последнее местонахождение.
	/// </summary>
	private Vector3 lastPosition;
	/// <summary>
	/// Скорость изменения страшной музыки.
	/// </summary>
	private const Single horrorBackgroundChangeVolumeSpeed = 0.004f;

	#endregion

	private void Start()
	{
		this.lastPosition = this.transform.position;
	}

	private void Update()
	{
		//Если гг не пеердвигается, то страшная музыка становиться громче, и наоборот.
		//Предполагается, что это сподвигнет гг чаще бегать и меньше стоять.
		if (this.horrorBackground.volume < 1 && this.lastPosition == this.transform.position)
		{
			this.horrorBackground.volume += horrorBackgroundChangeVolumeSpeed;
		}
		else
		{
			if (this.horrorBackground.volume > 0.1f)
				this.horrorBackground.volume -= horrorBackgroundChangeVolumeSpeed;
		}
		this.lastPosition = this.transform.position;
	}
}
