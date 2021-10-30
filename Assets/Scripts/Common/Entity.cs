using UnityEngine;

/// <summary>
/// Базовый класс всех интерактивных игровых объектов на сцене.
/// </summary>
public abstract  class Entity : MonoBehaviour
{
    /// <summary>
    /// Название объекта для пользователя
    /// </summary>

    [SerializeField]
    private string m_NickName;
    public string NIckname => m_NickName;
}
