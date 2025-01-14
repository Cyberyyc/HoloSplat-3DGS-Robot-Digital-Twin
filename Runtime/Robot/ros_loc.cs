
using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;


public class ros_loc : MonoBehaviour
{
    public GameObject Robot;
    ROSConnection ros;
    
    
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<OdometryMsg>("/odom", ReceiveROSCmd);
    }

    
    void ReceiveROSCmd(OdometryMsg loc_rob)
    {
        Quaternion rosQuaternion = new Quaternion(0f, -(float)loc_rob.pose.pose.orientation.z, 0f, (float)loc_rob.pose.pose.orientation.w);

        Robot.transform.localPosition= new Vector3((float)loc_rob.pose.pose.position.y, -3.93f, (float)loc_rob.pose.pose.position.x);
        
        Robot.transform.localRotation = rosQuaternion;
    }
}
