using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyItemView : ItemView
{
    /// <summary>
    /// Звук высасывания.
    /// </summary>
    private AudioSource suckThisBoxPrivate;
    /// <summary>
    /// Звук высасывания.
    /// </summary>
    public AudioSource suckThisBox
    {
        get
        {
            if(this.suckThisBoxPrivate==null)
            {
                this.suckThisBoxPrivate = GetComponent<AudioSource>();
            }
            return this.suckThisBoxPrivate;
        }
    }

    void OnTrigger()
    {

    }
}
