using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace Jega
{
    public class LookAtCameraManager : MonoBehaviour
    {
        [SerializeField] private Vector3 offSet;
        private RotationConstraint rotationConstraint;
        private Transform mainCamera;
        private ConstraintSource constraintSource;

        private SessionService sessionInfoService;
        // Start is called before the first frame update
        private void Awake()
        {
            if (TryGetComponent(out RotationConstraint rotationC))
                rotationConstraint = rotationC;
            else
            {
                gameObject.AddComponent(typeof(RotationConstraint));
                rotationConstraint = GetComponent<RotationConstraint>();
            }

            sessionInfoService = SessionService.Service;
        }
        private void OnEnable()
        {
            if (rotationConstraint.sourceCount == 0)
                StartCoroutine(SetupRotationConstraint());
        }
        private IEnumerator SetupRotationConstraint()
        {
            while (sessionInfoService.GameCamera == null)
                yield return null;

            mainCamera = sessionInfoService.GameCamera.transform;
            constraintSource.sourceTransform = mainCamera;
            constraintSource.weight = 1;
            rotationConstraint.AddSource(constraintSource);
            rotationConstraint.rotationOffset += offSet;
            rotationConstraint.constraintActive = true;
        }
    }
}