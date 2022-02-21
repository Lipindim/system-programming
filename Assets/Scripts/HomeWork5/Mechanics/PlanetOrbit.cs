using Network;
using UnityEngine;

namespace Mechanics
{
    public class PlanetOrbit : NetworkMovableObject
    {
        protected override float speed => smoothTime;

        public Vector3 AroundPoint;
        public float CircleInSecond = 1f;
        public float RotationSpeed;

        [SerializeField] private float smoothTime = .3f;
        [SerializeField] private float offsetSin = 1;
        [SerializeField] private float offsetCos = 1;

        private float dist;
        private float currentAng;
        private Vector3 currentPositionSmoothVelocity;
        private float currentRotationAngle;

        private const float circleRadians = Mathf.PI * 2;

        private void Start()
        {
            if (isServer)
            {
                dist = (transform.position - AroundPoint).magnitude;
            }
            Initiate(UpdatePhase.FixedUpdate);
        }

        private void Update()
        {
            HasAuthorityMovement();
        }
        protected override void HasAuthorityMovement()
        {
            if (!isServer)
            {
                return;
            }

            Vector3 p = AroundPoint;
            p.x += Mathf.Sin(currentAng) * dist * offsetSin;
            p.z += Mathf.Cos(currentAng) * dist * offsetCos;
            transform.position = p;
            currentRotationAngle += Time.deltaTime * RotationSpeed;
            currentRotationAngle = Mathf.Clamp(currentRotationAngle, 0, 361);
            if (currentRotationAngle >= 360)
            {
                currentRotationAngle = 0;
            }
            transform.rotation = Quaternion.AngleAxis(currentRotationAngle, transform.up);
            currentAng += circleRadians * CircleInSecond * Time.deltaTime;

            SendToServer();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
        }

        protected override void SendToServer()
        {
            serverPosition = transform.position;
            serverEuler = transform.eulerAngles;
        }

        protected override void FromServerUpdate()
        {
            if (!isClient)
            {
                return;
            }
            transform.position = Vector3.SmoothDamp(transform.position,
                serverPosition, ref currentPositionSmoothVelocity, speed);
            transform.rotation = Quaternion.Euler(serverEuler);
        }
    }
}
