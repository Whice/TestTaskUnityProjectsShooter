using System;
using UnityEngine;

public class Whills : MonoBehaviour
{
    private Single rotate = 0;
    // Update is called once per frame
    private void Update()
    {
        //Повернуться
        this.rotate += 0.001f;
        this.transform.RotateAroundLocal(new Vector3(1, 0, 0), this.rotate*Time.deltaTime);
    }
}
