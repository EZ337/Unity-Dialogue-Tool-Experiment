using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Dialogue dlg;
    public Port iPort;
    public Port oPort;
    public Action<NodeView> NodeSelectAction;

    public NodeView(Dialogue dlg) : base("Assets/Editor/NodeView.uxml")
    {
        this.dlg = dlg;
        this.title = dlg.name;
        this.viewDataKey = dlg.guid;
        style.left = dlg.EditorPosition.x;
        style.top = dlg.EditorPosition.y;

        CreateInputPort();
        CreateOutputPort();
    }


    private void CreateInputPort()
    {
        if (dlg.IsStartingTopic)
            return;

        iPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Multi, typeof(Dialogue));
        iPort.portName = "";
        iPort.style.flexDirection = FlexDirection.Column;
        inputContainer.Add(iPort);
    }
    private void CreateOutputPort()
    {
        oPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(Dialogue));
        oPort.portName = "";
        oPort.style.flexDirection = FlexDirection.ColumnReverse;
        outputContainer.Add(oPort);
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        dlg.EditorPosition.x = newPos.xMin;
        dlg.EditorPosition.y = newPos.yMin;
        EditorUtility.SetDirty(dlg);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        NodeSelectAction?.Invoke(this);
    }
}
