using System;
using UnityEngine;

/// <summary>
/// Модель врага, который ускоряется раз в некоторое время.
/// </summary>
public class DodgerEnemyModel : SimpleEnemyModel
{
    /// <summary>
    /// Интервал между уклонениями.
    /// </summary>
    [SerializeField]
    private Single intervalBetweenDodge = 6f;
    /// <summary>
    /// Таймер для интервала уклонения.
    /// </summary>
    private Single dodgeTimer = 0f;
    private void Update()
    {
        OnUpdate();
        if (this.dodgeTimer < this.intervalBetweenDodge)
        {
            this.dodgeTimer += Time.deltaTime;
        }
    }
    /// <summary>
    /// Уклониться.
    /// </summary>
    public void Dodge()
    {
        if (this.dodgeTimer >= this.intervalBetweenDodge)
        {
            this.transform.position = new Vector3
                (
                this.transform.position.x + UnityEngine.Random.Range(-5f, 5f),
                this.transform.position.y,
                this.transform.position.z
                );
            this.dodgeTimer = 0;
        }
    }

    public override void Activate()
    {
        base.Activate();
        //После активации враг сразу готов уклоняться.
        this.dodgeTimer = this.intervalBetweenDodge;
    }
}
