using System;
using System.Collections.Generic;
using System.Linq;
using QAssetBundle;
using UnityEngine;

namespace QFramework.WuZiQi.Test
{
	public partial class Gameplay : ViewController
	{
		public enum ChessPosStatus
		{
			None = 0,
			Black = 1,
			White = -1
		}
		
		private ResLoader mResLoader = null;

		private GameObject mBlackChessPrefab;

		private GameObject mWhiteChessPrefab;
		
		private Vector2[,] mChessPos;

		private ChessPosStatus mTurn = ChessPosStatus.Black;

		private List<List<ChessPosStatus>> mSnapshotMap;

		private bool isGameOver = false;

		public float Threshold = 0.2f;
		
		private void Awake()
		{
			ResMgr.Init();
		}

		private void Start()
		{
			// ResLoader 的申请要确保在 ResMgr.Init 之后
			mResLoader = ResLoader.Allocate();

			mBlackChessPrefab = mResLoader.LoadSync<GameObject>(Blackchess_prefab.BLACKCHESS);
			mWhiteChessPrefab = mResLoader.LoadSync<GameObject>(Whitechess_prefab.WHITECHESS);

			InitBoard();

			var mousePos = Vector2.zero;
			
			//鼠标的点击输入事件处理
			this.Repeat()
				.Until(() => Input.GetMouseButton(0) && !isGameOver)
				.Event(() =>
				{
					mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

					for (int row = 0; row < 15; row++)
					{
						for (int col = 0; col < 15; col++)
						{
							if (GameplayUtil.IsNearBy(mousePos, mChessPos[row, col], Threshold))
							{
								CreateChess(row, col,mChessPos[row, col]);
							}
						}
					}
				}).Begin();

			TypeEventSystem.Global.Register<GameResetEvent>(ResetGame);
		}

		private void ResetGame(GameResetEvent obj)
		{
			Chesses.DestroyChildren();

			foreach (var line in mSnapshotMap)
			{
				for (int i = 0; i < line.Count; i++)
				{
					line[i] = ChessPosStatus.None;
				}
			}

			isGameOver = false;
		}

		private void InitBoard()
		{
			mChessPos = new Vector2[15, 15];

			mSnapshotMap = Enumerable.Range(0, 15)
				.Select(_ => Enumerable.Range(0, 15)
					.Select(i => new ChessPosStatus())
					.ToList())
				.ToList();

			for (int row = 0; row < 15; row++)
			{
				for (int col = 0; col < 15; col++)
				{
					mChessPos[row, col] = new Vector2(Borders.GridWidth * (col - 7), Borders.GridHeight * (row - 7));
				}
			}
		}

		private void CreateChess(int row, int col, Vector2 putPosition)
		{
			if (mSnapshotMap[row][col] == ChessPosStatus.None)
			{
				mSnapshotMap[row][col] = mTurn;

				if (mTurn == ChessPosStatus.Black)
				{
					mBlackChessPrefab.Instantiate()
						.transform
						.Parent(Chesses)
						.LocalIdentity()
						.Position(putPosition.x, putPosition.y, 0)
						.Name(mTurn.ToString() + row + "_" + col);
				}
				else
				{
					mWhiteChessPrefab.Instantiate()
						.transform
						.Parent(Chesses)
						.LocalIdentity()
						.Position(putPosition.x, putPosition.y, 0)
						.Name(mTurn.ToString() + row + "_" + col);
				}

				CheckWinner(row, col);

				mTurn = mTurn == ChessPosStatus.Black ? ChessPosStatus.White : ChessPosStatus.Black;
			}
		}

		private void CheckWinner(int row, int col)
		{
			if (GameplayUtil.IsWin(row, col, mTurn, mSnapshotMap))
			{
				isGameOver = true;

				TypeEventSystem.Global.Send(new GameOverEvent()
				{
					IsBlackWin = mTurn == ChessPosStatus.Black
				});
			}
		}

		private void OnDestroy()
		{
			TypeEventSystem.Global.UnRegister<GameResetEvent>(ResetGame);
			
			mResLoader.Recycle2Cache();
			mResLoader = null;
		}
	}
}
