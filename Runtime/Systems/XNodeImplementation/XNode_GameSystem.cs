using Rufas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

[NodeWidth(350)]
public class XNode_GameSystem : Node {

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
	
	private string GetName()
	{
		if (gameSystem) return gameSystem.DesiredName();

		return "";
	}

	private bool ShowToggleButton() { return gameSystem != null; }
	
    [ShowIf("ShowToggleButton")]
    [PropertyOrder(0)]
	[Button("$GetName")]
	private void ToggleVisuals()
	{
		showSystem = !showSystem;
	}

	[HideInInspector] public bool showSystem = false;

	private bool ShouldShowSystem()
	{
		if (gameSystem == null) return true;

		return showSystem;
    }

	[PropertyOrder(1)]
	[ShowIf("ShouldShowSystem")]
	[HideLabel]
    [InlineEditor(InlineEditorModes.GUIOnly, InlineEditorObjectFieldModes.Hidden), HideDuplicateReferenceBox]
    public GameSystemParentClass gameSystem;

}