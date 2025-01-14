using RosMessageTypes.Geometry;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;

public class ros_arm : MonoBehaviour
{    
    public Transform link_1; 
    public Transform link_2;    
    public Transform link_3;    
    public Transform link_4;     
    public Transform link_5;    
    public Transform link_6;
    
    
    
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<TwistMsg>("/joint_state", arm_pos);
    }
    

    
    void arm_pos(TwistMsg joint_pos)
    {
        
        link_1.localRotation = Quaternion.Euler((float)0f, -(float)joint_pos.linear.x, (float)0f);
         link_2.localRotation = Quaternion.Euler((float)joint_pos.linear.y, (float)180f, (float)90f);
       link_3.localRotation = Quaternion.Euler((float)0f, -(float)joint_pos.linear.z+90f, (float)0f);
        link_4.localRotation = Quaternion.Euler((float)0f, (float)-joint_pos.angular.x-180f, (float)0f);
        link_5.localRotation = Quaternion.Euler((float)joint_pos.angular.y-90f, (float)0f, (float)90f);
       link_6.localRotation = Quaternion.Euler(-(float)joint_pos.angular.z, (float)0f, (float)-90f);
    }
}
