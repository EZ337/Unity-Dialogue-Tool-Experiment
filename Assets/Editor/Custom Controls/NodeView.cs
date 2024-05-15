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
    }
}
