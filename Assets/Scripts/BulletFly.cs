using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : MonoBehaviour
{
    /// <summary>
    /// ��������� �� ������-��������� ����, �������� ����������� ���� ������
    /// </summary>
    public GameObject thisPerfab;
    /// <summary>
    /// �������� �������� ����.
    /// </summary>
    public Single speed = 0.1f;
    /// <summary>
    /// ������ ��� ������� � ������ �����������.
    /// </summary>
    public FP_Controller controller;
    /// <summary>
    /// ����� � ������ ���� �����������.
    /// </summary>
    public Int32 numberInListController = 0;
    /// <summary>
    /// ��������� ������. ��� ���������� ���� ������������.
    /// </summary>
    private Single rangeOfFlight = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�������� ���� ������ ����.
        this.transform.position = this.transform.position + this.transform.forward * this.speed;

        //����������� �������, ���� �� ������ ������.
        if(this.transform.position.x>this.rangeOfFlight || 
            this.transform.position.y> this.rangeOfFlight || 
            this.transform.position.z> this.rangeOfFlight
            )
        {
            this.controller.bullets.Remove(this.thisPerfab);
            this.controller.bulletsForDelete.Add(this.thisPerfab);
        }
    }
}
