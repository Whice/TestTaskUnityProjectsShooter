using System;
using UnityEngine;


public class ItemModelViewContoller : MonoBehaviour
{
    /// <summary>
    /// Вызывается, когда проиходит изменение в свойстве с вызовом метода SetValueProperty.
    /// </summary>
    /// <param name="propertyName">Имя свойства.</param>
    /// <param name="oldValue">Старое значение.</param>
    /// <param name="newValue">Новое значение.</param>
    protected virtual void OnChanged(String propertyName, object oldValue, object newValue)
    {

    }
    /// <summary>
    /// Установить новое значение для свойства.
    /// После ввода данных задействует метод OnChanged.
    /// </summary>
    /// <typeparam name="T">Тип изменяемых данных.</typeparam>
    /// <param name="propertyName">Имя свойства.</param>
    /// <param name="field">Поле, куда помещается значение.</param>
    /// <param name="newValue">Новое значение.</param>
    protected void SetValueProperty<T>(String propertyName, ref T field, T newValue)
    {
        //Если оба неназначены, то ничего не поменялось
        if(field is null && newValue is null)
        {
            return;
        }

        //Если поля не равны
        if (!(field is null) && !field.Equals(newValue))
        {
            T oldValue = field;
            field = newValue;
            OnChanged(propertyName, oldValue, newValue);
        }
    }
    /// <summary>
    /// Объект модели.
    /// </summary>
    protected ItemModelViewContoller itemModelProtected = null;
    /// <summary>
    /// Объект модели.
    /// </summary>
    public ItemModelViewContoller model { get => this.itemModelProtected; }
    /// <summary>
    /// Объект представления.
    /// </summary>
    protected ItemModelViewContoller itemViewProtected = null;
    /// <summary>
    /// Объект представления.
    /// </summary>
    public ItemModelViewContoller view { get => this.itemViewProtected; }
    /// <summary>
    /// Объект контроллера.
    /// </summary>
    protected ItemModelViewContoller itemControllerProtected = null;
    /// <summary>
    /// Объект контроллера.
    /// </summary>
    public ItemModelViewContoller controller { get => this.itemControllerProtected; }
    private void Awake()
    {
    }

}
