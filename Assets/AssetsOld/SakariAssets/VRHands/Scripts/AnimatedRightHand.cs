using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NewtonVR
{

    public class AnimatedRightHand : MonoBehaviour
    {

//        private NVRHand hand;
        private GameObject VRhand;
        public Animator anim;
        public float h;
        public float g;
		public float horizontal;
        public float vertical;
        bool Pointing = false;

        // Use this for initialization
        void Start()
        {
            
            VRhand = GameObject.FindGameObjectWithTag("RHand");
//            hand = GetComponent<NVRHand>();
            anim = VRhand.GetComponent<Animator>();

        }

        void Update()
        {
            if (Pointing == false)
                StopPoint();
            else
                Point();

        }


        // Update is called once per frame
        void Normal()
        {
//            float g = hand.Inputs[NVRButtons.Trigger].Axis.x;
            anim.SetFloat("Trigger", g);
//			float horizontal = hand.Inputs [NVRButtons.Touchpad].Axis.x;
			anim.SetLayerWeight (anim.GetLayerIndex ("Finger"), horizontal);
			anim.SetLayerWeight (anim.GetLayerIndex ("Devilshorn"), -horizontal);
//            float vertical = hand.Inputs[NVRButtons.Touchpad].Axis.y;
            anim.SetLayerWeight(anim.GetLayerIndex("Shocker"), vertical);
            anim.SetLayerWeight(anim.GetLayerIndex("Surf"), -vertical);
            Pointing = false;

        }

        public void Point()
        {
            Pointing = true;
            if (h < 1)
            {
                h += Time.deltaTime * 10;
                anim.SetFloat("Point", h);
                print(h);
            }
            
        }

        public void StopPoint()
        {
            Pointing = false;
            if (h > 0)
            {
                h -= Time.deltaTime * 10;
                anim.SetFloat("Point", h);
                print(h);
            }
            if (h <= 0)
            {
                Normal();
            }
 

        }




    }
}
