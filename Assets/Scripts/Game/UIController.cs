using System;
using UnityEngine;

namespace QFramework.WuZiQi.Test
{
    public class UIController : MonoBehaviour
    {
        private void Start()
        {
            TypeEventSystem.Global.Register<GameOverEvent>(OnGameOver);
        }

        private void OnGameOver(GameOverEvent obj)
        {
            UIKit.OpenPanel<UIGameOver>(new UIGameOverData()
            {
                BlackWin = obj.IsBlackWin
            });
        }

        private void OnDestroy()
        {
            TypeEventSystem.Global.UnRegister<GameOverEvent>(OnGameOver);
        }
    }
}