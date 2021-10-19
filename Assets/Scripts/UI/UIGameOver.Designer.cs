using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.WuZiQi.Test
{
	// Generate Id:dfc4f1d3-f83d-4083-9d63-96276ac087da
	public partial class UIGameOver
	{
		public const string Name = "UIGameOver";
		
		[SerializeField]
		public UnityEngine.UI.Image ResultImage;
		
		private UIGameOverData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ResultImage = null;
			
			mData = null;
		}
		
		public UIGameOverData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIGameOverData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIGameOverData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
