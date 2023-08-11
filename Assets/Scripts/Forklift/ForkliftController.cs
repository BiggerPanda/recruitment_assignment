using UnityEngine;
using Zenject;

public class ForkliftController : MonoBehaviour
{
    [SerializeField] private ArticulationBody connectedForkBody;
    [SerializeField] private float maxWeight = 3000f;
    [SerializeField] private Transform middleOfFork;
    [SerializeField] private float maxYPosition = 5f;
    [SerializeField] private float minYPosition = 0f;
    [SerializeField] private float forkMoveAccuracy = 1f;
    [SerializeField] private float objectUpDetectionRayLength = 1f;
    [SerializeField] private float objectBelowDetectionRayLength = 0.3f;

    private float maxForce = 20f;
    private float applyForce = 0f;
    private ArticulationDrive yDrive;
    private Transform item;
    private Rigidbody itemRigidbody;
    private float movePosition = 0f;
    private bool isObjectOnFork;
    private ForkliftUIController forkliftUIController;

    [Inject]
    private void Init(ForkliftUIController _forkliftUIController)
    {
        forkliftUIController = _forkliftUIController;
    }

    private void Start()
    {
        yDrive = connectedForkBody.yDrive;
        maxForce = maxWeight * -Physics.gravity.y;
        yDrive.forceLimit = maxForce;
        yDrive.upperLimit = minYPosition;
        yDrive.lowerLimit = minYPosition;
        connectedForkBody.yDrive = yDrive;
        applyForce = maxForce;
    }

    private void Update()
    {
        if (InputController.Instance.MoveForkliftUp())
        {
            isObjectOnFork = Physics.Raycast(middleOfFork.position, Vector3.up, out RaycastHit hit,
                objectUpDetectionRayLength);

            if (isObjectOnFork)
            {
                if (item != null && itemRigidbody != null)
                {
                    return;
                }

                item = hit.transform;
                itemRigidbody = item.GetComponent<Rigidbody>();
                item.SetParent(middleOfFork);
            }

            if (movePosition < maxYPosition)
            {
                movePosition += forkMoveAccuracy * Time.deltaTime;
                yDrive.upperLimit = movePosition;
                connectedForkBody.yDrive = yDrive;
            }
        }

        if (InputController.Instance.MoveForkliftDown())
        {
            if (Physics.Raycast(middleOfFork.position, Vector3.down, out RaycastHit hit, objectBelowDetectionRayLength)
                || movePosition <= minYPosition)
            {
                if (item != null)
                {
                    item.SetParent(null);
                    item = null;
                    itemRigidbody = null;
                    isObjectOnFork = false;
                }
            }

            if (movePosition > minYPosition)
            {
                movePosition -= forkMoveAccuracy * Time.deltaTime;
                yDrive.upperLimit = movePosition;
                connectedForkBody.yDrive = yDrive;
            }
        }

        forkliftUIController.UpdateForkliftData(movePosition, isObjectOnFork);
    }

    private void FixedUpdate()
    {
        //Apply the force to the ArticulationBody
        connectedForkBody.AddForce(Vector3.up * applyForce, ForceMode.Force);
    }
}