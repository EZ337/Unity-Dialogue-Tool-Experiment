using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Dialogue dlg;

    public NodeView(Dialogue dlg)
    {
        this.dlg = dlg;
        this.title = dlg.name;
        this.viewDataKey = dlg.guid;
        style.left = dlg.EditorPosition.x;
        style.top = dlg.EditorPosition.y;
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        dlg.EditorPosition.x = newPos.xMin;
        dlg.EditorPosition.y = newPos.yMin;  
    }
}
