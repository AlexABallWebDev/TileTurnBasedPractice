using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private Grid grid;

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
        grid.Set((int)this.transform.position.x, (int)this.transform.position.y, this);
    }

    public void Move(int x, int y)
    {
        grid.Unset((int)this.transform.position.x, (int)this.transform.position.y);
        this.transform.position = (new Vector3(x, y, 0));
        grid.Set((int)this.transform.position.x, (int)this.transform.position.y, this);
    }
}