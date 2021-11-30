using UnityEngine;

/// <summary>
/// Информация об игровом ящике.
/// </summary>
public class GameBoxInfo : MonoBehaviour
{
    /// <summary>
    /// Список ящиков вне арены.
    /// </summary>
    public System.Collections.Generic.List<GameObject> withoutArenaBoxes = new System.Collections.Generic.List<GameObject>(100);
    /// <summary>
    /// Список ящиков на арене.
    /// </summary>
    public System.Collections.Generic.List<GameObject> onArenaBoxes = new System.Collections.Generic.List<GameObject>(100);
    /// <summary>
    /// Цвет ящика.
    /// </summary>
    private Color boxColor;
    /// <summary>
    /// Ссылка на материал ящика.
    /// </summary>
    private Material boxMaterial = null;
    /// <summary>
    /// Установить случайный цвет ящика.
    /// </summary>
    public void SetRandomColorForThisBox()
    {
        int rValue = Random.Range(1, 5);
        switch (rValue)
        {
            case 1:
                {
                    boxColor = Color.red;
                }
                break;
            case 2:
                {
                    boxColor = Color.yellow;
                }
                break;
            case 3:
                {
                    boxColor = Color.green;
                }
                break;
            default:
                {
                    boxColor = Color.green;
                }
                break;
        }
        if (this.boxMaterial == null)
        {
            this.boxMaterial = this.GetComponent<Renderer>().material;
        }
        this.boxMaterial.color = boxColor;
    }
    /// <summary>
    /// Ссылка на инфо игрока.
    /// </summary>
    private PlayerInfo playerInfo = null;
    // Start is called before the first frame update
    void Start()
    {
        this.playerInfo = GameObject.Find("Player").GetComponent<PlayerInfo>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            this.playerInfo.AddColoredCube(this.boxColor);
            this.onArenaBoxes.Remove(this.gameObject);
            this.withoutArenaBoxes.Add(this.gameObject);

            this.gameObject.transform.position = new Vector3
                (
                this.gameObject.transform.position.x,
                -99,
                this.gameObject.transform.position.z
                );
        }
    }
}
