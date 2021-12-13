using System;
using System.Collections;
using UnityEngine;

namespace RPG.Core
{
    public class ActionManager : MonoBehaviour
    {
        private IAction currentAction;
        public event Action OnStateChange;

        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            currentAction = action;
            OnStateChange?.Invoke();
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }

        public bool HasActiveAction()
		{
            return currentAction != null;
		}
	}
}