using NaughtyAttributes;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [BoxGroup("Suspension")] [SerializeField]
    private float restDistance = 0.5f;

    [BoxGroup("Suspension")] [SerializeField]
    private float springTravel = 0.2f;

    [BoxGroup("Suspension")] [SerializeField]
    private float springStiffness = 120000f;

    [BoxGroup("Suspension")] [SerializeField]
    private float springDamping = 10000f;

    [BoxGroup("Wheel")] [SerializeField] private float wheelRadius = 1f;
    [BoxGroup("Wheel")] [SerializeField] private bool isLeftWheel = true;
    [BoxGroup("Wheel")] [SerializeField] private float steerTime = 10f;

    private float steerAngle = 0f;
    private float wheelAngle = 0f;

    //Cached values
    private float minLength = 0f;
    private float maxLength = 0f;
    private float lastLength = 0f;
    private float sprigVelocity = 0f;
    private float springLength = 0f;
    private float springForce = 0f;
    private float damperForce = 0f;
    private float forwardForce = 0f;
    private float sidewaysForce = 0f;
    private float wheelCircumference = 0f;
    private float wheelRpmAngle = 0f;

    private ArticulationBody mainBody;
    private GameObject wheelMesh;

    private Vector3 springForceVector = Vector3.zero;
    private Vector3 wheelVelocity = Vector3.zero;
    private float acceleration = 2f;

    private void Start()
    {
        mainBody = transform.root.root.GetComponent<ArticulationBody>();
        wheelMesh = transform.GetChild(0).gameObject;

        minLength = restDistance - springTravel;
        maxLength = restDistance + springTravel;
        wheelCircumference = 2 * Mathf.PI * wheelRadius;
    }

    private void Update()
    {
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, Time.deltaTime * steerTime);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x,
            transform.localRotation.y + wheelAngle,
            transform.localRotation.z);
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, -transform.up * (maxLength + wheelRadius), Color.red);
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            sprigVelocity = (lastLength - springLength) / Time.fixedDeltaTime;

            springForce = (restDistance - springLength) * springStiffness;
            damperForce = sprigVelocity * springDamping;

            springForceVector = transform.up * (springForce + damperForce);

            wheelVelocity = transform.InverseTransformDirection(mainBody.GetPointVelocity(hit.point));


            mainBody.AddForceAtPosition(
                springForceVector + (forwardForce * transform.forward) + (sidewaysForce * -transform.right), hit.point);
        }

        applySpeedRotation();
    }

    public void ApplyAckermannAngle(float _ackermannAngleLeft, float _ackermannAngleRight)
    {
        if (isLeftWheel)
        {
            steerAngle = _ackermannAngleLeft;
        }
        else
        {
            steerAngle = _ackermannAngleRight;
        }
    }

    public void ApplyForce(float _force)
    {
        forwardForce = InputController.Instance.GetMoveVector().y * _force;
        sidewaysForce = wheelVelocity.x * _force;
    }

    private void applySpeedRotation()
    {
        if (InputController.Instance.GetMoveVector().y >= 0)
        {
            wheelRpmAngle = Mathf.Lerp(wheelRpmAngle, mainBody.velocity.magnitude / wheelRadius,
                Time.deltaTime * acceleration);
            wheelMesh.transform.Rotate(wheelRpmAngle, 0f, 0f);
        }
        else
        {
            wheelRpmAngle = Mathf.Lerp(wheelRpmAngle, -mainBody.velocity.magnitude / wheelRadius,
                Time.deltaTime * acceleration);
            wheelMesh.transform.Rotate(wheelRpmAngle, 0f, 0f);
        }
    }
}