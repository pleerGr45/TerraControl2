using TC_basic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildButtonScript : MonoBehaviour {

    [SerializeField] private int slot;
    public Cell cell;
    public int i, j;
    public BuildingType type;

    public void Build() {
        cell.CellZone.Build(slot, type, new Vector3(GameExecutor.cells_model[i][j].transform.position.x, 0, GameExecutor.cells_model[i][j].transform.position.z));
    }

    public void Destroy()
    {
        cell.CellZone.DestroyBuild(slot);
    }

}