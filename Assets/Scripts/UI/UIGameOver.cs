using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.WuZiQi.Test
{
	public class UIGameOverData : UIPanelData
	{
		public bool BlackWin = true;
	}
	public partial class UIGameOver : UIPanel
	{
		private ResLoader mResLoader = ResLoader.Allocate();
		
		protected override void ProcessMsg(int eventId, QMsg msg)
		{
			throw new System.NotImplementedException();
		}
		
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIGameOverData ?? new UIGameOverData();
			// please add init code here

			if (mData.BlackWin)
			{
				ResultImage.sprite = mResLoader.LoadSprite(QAssetBundle.Blackwin_jpg.BLACKWIN);
			}
			else
			{
				ResultImage.sprite = mResLoader.LoadSprite(QAssetBundle.Whitewin_jpg.WHITEWIN);
			}

			this.Sequence()
				.Delay(1.0f)
				.Until(() => Input.GetMouseButtonDown(0))
				.Event(() =>
				{
					CloseSelf();
					TypeEventSystem.Global.Send<GameResetEvent>();
				})
				.Begin();
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
			mResLoader.Recycle2Cache();
			mResLoader = null;
		}
	}
}
