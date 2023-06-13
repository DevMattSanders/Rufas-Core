using Sirenix.OdinInspector;
using UnityEngine;

namespace DMS.Components
{
	public class SetWorldSpaceEventCamera : MonoBehaviour
	{
		private Canvas canvas;

		public bool setLayer = false;
		[ShowIf("setLayer")] public int setLayerTo;

		private void Start()
		{
			canvas = transform.GetComponent<Canvas>();
		}

		private void Update()
		{
			if (canvas.worldCamera == null)
			{
				canvas.worldCamera = Camera.main;

				if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
				{
					canvas.planeDistance = 0.5f;
				}
				else if (canvas.renderMode == RenderMode.WorldSpace)
				{
				}
			}
			else
			{
			}

			if (setLayer)
			{
				canvas.overrideSorting = true;
				canvas.sortingOrder = setLayerTo;
			}

			Destroy(this);
		}
	}
}
