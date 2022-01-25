using System.Collections;
using System.Collections.Generic;
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
    private Animation rotateAnimationField = null;
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
        if (!this.rotateAnimation.isPlaying && !this.enemyModel.IsNearWithPlayer)
        {
            this.rotateAnimation.Play();
        }
    }
}
