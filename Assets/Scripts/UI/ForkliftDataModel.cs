using System;

[Serializable]
public class ForkliftDataModel
{
    public float forkPosition;
    public float horizontalInput;
    public float verticalInput;
    public float speed;
    public bool isObjectOnFork;
}