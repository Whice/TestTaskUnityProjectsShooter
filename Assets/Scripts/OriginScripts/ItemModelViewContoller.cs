using UnityEngine;


public class ItemModelViewContoller : MonoBehaviour
{
    protected ItemModelViewContoller itemModelProtected;
    public ItemModelViewContoller model { get => this.itemModelProtected; }
    protected ItemModelViewContoller itemViewProtected;
    public ItemModelViewContoller view { get => this.itemViewProtected; }
    protected ItemModelViewContoller itemControllerProtected;
    public ItemModelViewContoller controller { get => this.itemControllerProtected; }

    private ArenaApplication arenaApplicationPrivate = null;
    public ArenaApplication arenaApplication { get => this.arenaApplicationPrivate; }
    private void Awake()
    {
        this.arenaApplicationPrivate = new ArenaApplication();//getApp
    }

}
