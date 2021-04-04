using System;
using AirCoder.TJ.Core.Extensions;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
        private CanvasGroup _cGroup;
        protected bool isOpen;
       
        protected CanvasGroup canvasGroup
        {
            get
            {
                if (_cGroup == null) _cGroup = GetComponent<CanvasGroup>();
                return _cGroup;
            }
        }

          //- Canvas visibility methods
        public virtual void OpenPanelImmediately()
        {
            isOpen = true;
            gameObject.SetActive(true);
            canvasGroup.alpha = 1f;
        }
        
        public virtual void OpenPanel(Action callBack = null)
        {
            isOpen = true;
            gameObject.SetActive(true);

            canvasGroup.alpha = 0f;
            canvasGroup.TweenOpacity(1f, 0.25f).OnComplete(() =>
            {
                callBack?.Invoke();
            }).Play();
        }
        
        public virtual void ClosePanelImmediately()
        {
            isOpen = false;
            canvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }
        public virtual void ClosePanel(Action callBack = null)
        {
            isOpen = false;

            canvasGroup.alpha = 1f;
            canvasGroup.TweenOpacity(0f, 0.25f).OnComplete(() =>
            {
                callBack?.Invoke();
                gameObject.SetActive(false);
            }).Play();
        }
       
}