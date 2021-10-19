using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.WuZiQi.Test
{
	// Generate Id:afaa55a6-c2bb-4c2b-b27e-2b347217d22a
	public partial class UIHomePanel
	{
		public const string Name = "UIHomePanel";
		
		[SerializeField]
		public UnityEngine.UI.Button Button;
		
		private UIHomePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			Button = null;
			
			mData = null;
		}
		
		public UIHomePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIHomePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIHomePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
