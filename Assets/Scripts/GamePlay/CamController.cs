using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static Utility.Utility;

namespace GamePlay
{
    public class CamController : MonoBehaviour
    {
        public Camera MainCam;
        public Transform CamPivot;
        public Transform TargetObj;
        public Button LeftTurn;
        public Button RightTurn;
        public Button ZoomIn;
        public Button ZoomOut;

        [Header("Camera Setting")] 
        [SerializeField] private float CamSmooth;
        [SerializeField] private Vector3 CamPos;
        private float startCamSize;
        [SerializeField] private float TargetCamSize;

        [SerializeField] private Vector3 TargetCamPos;
        [SerializeField] private Quaternion TargetRot;
        public Quaternion CurrentRot;

        private void Awake()
        {
            CurrentRot = CamPivot.transform.rotation;
            TargetRot = CurrentRot;
            LeftTurn.onClick.AddListener(OnClickLeft);
            RightTurn.onClick.AddListener(OnClickRight);
            ZoomIn.onClick.AddListener(OnZoomIn);
            ZoomOut.onClick.AddListener(OnZoomOut);
        }

        private void OnDestroy()
        {
            LeftTurn.onClick.RemoveAllListeners();
            RightTurn.onClick.RemoveAllListeners();
            ZoomIn.onClick.RemoveAllListeners();
            ZoomOut.onClick.RemoveAllListeners();
        }

        void Start()
        {
            var position = CamPivot.transform.position;
            CamPos = position;
            TargetCamSize = MainCam.transform.localPosition.z;
            startCamSize = TargetCamSize;
            TargetCamPos = TargetObj.transform.position;
            TargetRot = CurrentRot;
        }

        void Update()
        {
            var camPivotTransform = CamPivot.transform;
            var position = camPivotTransform.position;
            var rotation = camPivotTransform.rotation;
            CamPos = position;
            CurrentRot = rotation;
            
            TargetCamPos = TargetObj.transform.position;
        
            if (true)
            {
                if (Input.GetKeyDown(KeyCode.KeypadPlus))
                {
                    TargetCamSize -= 5;
                
                }
                if (Input.GetKeyDown(KeyCode.KeypadMinus))
                {
                    TargetCamSize += 0.8f * Mathf.Abs(TargetCamSize);
                }
                if (TargetCamSize >= -5)
                {
                    TargetCamSize = startCamSize;
                }
            }

            CamPos = LerpVector3(CamPos, TargetCamPos, CamSmooth);
            position = CamPos;
            CamPivot.transform.position = position;
            var transformPosition = MainCam.transform.localPosition;
            transformPosition.z = Mathf.Lerp(transformPosition.z,TargetCamSize,CamSmooth);
            MainCam.transform.localPosition = transformPosition;
            
            var camRo = Quaternion.Lerp(CurrentRot,TargetRot,CamSmooth);
            CamPivot.transform.rotation = camRo;
        }

        private float targetY = 45;

        private void OnClickLeft()
        {
            var temp = TargetRot.eulerAngles;
            targetY -= 90;
            temp.y = targetY;
            TargetRot = Quaternion.Euler(temp);
        }
        
        private void OnClickRight()
        {
            var temp = TargetRot.eulerAngles;
            targetY += 90;
            temp.y = targetY;
            TargetRot = Quaternion.Euler(temp);
        }

        private void OnZoomIn()
        {
            TargetCamSize -= 5;
        }

        private void OnZoomOut()
        {
            TargetCamSize += 0.8f * Mathf.Abs(TargetCamSize);
        }
    }
}
