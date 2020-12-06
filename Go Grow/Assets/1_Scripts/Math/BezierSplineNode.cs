using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyHippo
{
    public class BezierSplineNode
	{
		public Vector3 position;
		public Vector3 rightTangent;
		public Vector3 leftTangent;

		public BezierSplineNode(Vector3 position, Vector3 rightTangent, Vector3 leftTangent)
		{
			this.position = position;
			this.rightTangent = rightTangent;
			this.leftTangent = leftTangent;
		}
	}
}
