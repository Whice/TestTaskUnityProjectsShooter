using UnityEngine;

/// <summary>
/// Представление врага в игре.
/// </summary>
public class EnemyView : GameCharacterView
{
    private EnemyModel enemyModel
    {
        get => this.model as EnemyModel;
    }
    /// <summary>
    /// Анимация поворота колеса.
    /// </summary>
    private Animation rotateAnimationField = null;
    /// <summary>
    /// Анимация поворота колеса.
    /// </summary>
    private Animation rotateAnimation
    {
        get
        {
            if (this.rotateAnimationField == null)
            {
                GameObject whills = this.transform.Find("Whills").gameObject;
                this.rotateAnimationField = whills.GetComponent<Animation>();
            }
            return this.rotateAnimationField;
        }
    }

    private void Update()
    {
        //Если враг далеко от игрока.
        if (!this.enemyModel.IsNearWithPlayer)
        {
            //Если проигрывание анимации уже кончилось.
            if (!this.rotateAnimation.isPlaying)
            {
                //Запустить колесо вращаться снова.
                this.rotateAnimation.Play();
            }
        }
    }
}
