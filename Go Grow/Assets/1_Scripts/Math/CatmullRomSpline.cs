using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

//Interpolation between points with a Catmull-Rom spline
namespace FluffyHippo
{
	public class CatmullRomSpline : MonoBehaviour
	{
		public Vector3 position;

		//Has to be at least 4 points
		public Transform[] controlPointsList;
		//Are we making a line or a loop?
		public bool isLooping = true;
		public float resolution = 1;

		//Display without having to press play
		void OnDrawGizmos()
		{
			Gizmos.color = Color.white;

			//Draw the Catmull-Rom spline between the points
			for (int i = 0; i < controlPointsList.Length; i++)
			{
				//Cant draw between the endpoints
				//Neither do we need to draw from the second to the last endpoint
				//...if we are not making a looping line
				if ((i == 0 || i == controlPointsList.Length - 2 || i == controlPointsList.Length - 1) && !isLooping)
				{
					continue;
				}

				DisplayCatmullRomSpline(i);
			}
		}

		//Display a spline between 2 points derived with the Catmull-Rom spline algorithm
		void DisplayCatmullRomSpline(int pos)
		{
			//The 4 points we need to form a spline between p1 and p2
			Vector3 p0 = controlPointsList[ClampListPos(pos - 1)].position;
			Vector3 p1 = controlPointsList[pos].position;
			Vector3 p2 = controlPointsList[ClampListPos(pos + 1)].position;
			Vector3 p3 = controlPointsList[ClampListPos(pos + 2)].position;

			//The start position of the line
			Vector3 lastPos = p1;

			//The spline's resolution
			//Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
			float _resolution = 1f / resolution;  // 0.2f;

			//How many times should we loop?
			int loops = Mathf.FloorToInt(1f / _resolution);

			for (int i = 1; i <= loops; i++)
			{
				//Which t position are we at?
				float t = i * _resolution;

				//Find the coordinate between the end points with a Catmull-Rom spline
				Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);

				//Draw this line segment
				Gizmos.DrawLine(lastPos, newPos);

				//Save this pos so we can draw the next line segment
				lastPos = newPos;
			}
		}

		//Clamp the list positions to allow looping
		int ClampListPos(int pos)
		{
			if (pos < 0)
			{
				pos = controlPointsList.Length - 1;
			}

			if (pos > controlPointsList.Length)
			{
				pos = 1;
			}
			else if (pos > controlPointsList.Length - 1)
			{
				pos = 0;
			}

			return pos;
		}

		//Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
		//http://www.iquilezles.org/www/articles/minispline/minispline.htm
		Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
		{
			//The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
			Vector3 a = 2f * p1;
			Vector3 b = p2 - p0;
			Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
			Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

			//The cubic polynomial: a + b * t + c * t^2 + d * t^3
			Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

			return pos;
		}

		public BezierSplineNode[] ToBezierSpline()
		{
			Vector3[] points = new Vector3[ controlPointsList.Length ];
			Debug.Assert(points.Length >= 4, "CatmullRomSpline needs at least 4 points.", this);

			float scale = transform.localScale.x;
			for (int i = 0; i < points.Length; i++)
			{
				points[i] = controlPointsList[i].position * scale;
			}
			
			List<BezierSplineNode> bezierNodes = new List<BezierSplineNode>();
			Vector3 position;
			Vector3 rightTangent;
			Vector3 leftTangent;
			for (int i = 1; i < points.Length - 1; i++)
			{
				position = points[i];
				rightTangent = (points[i + 1] - points[i - 1]).normalized;
				leftTangent = Quaternion.Euler(0, 0, 180) * rightTangent;

				bezierNodes.Add(new BezierSplineNode(position, rightTangent, leftTangent));	
			}

			return bezierNodes.ToArray();
		}

		public Vector3[][] CatmullRomToBezier(Vector3[] points) 
		{
			points = new Vector3[ controlPointsList.Length ];
			for (int i = 0; i < points.Length; i++)
			{
				points[i] = controlPointsList[i].position;
			}

			List<Vector3[]> d = new List<Vector3[]>();
			int iLen = points.Length;
			for (int i = 0; iLen - 2 > i; i++)
			{
				Vector3[] p = new Vector3[4];
				if (i == 0)
				{
					p[0] = points[i];
					p[1] = points[i];
					p[2] = points[i + 1];
					p[3] = points[i + 2];
				}
				else if (i == iLen - 4)
				{
					p[0] = points[i - 1];
					p[1] = points[i];
					p[2] = points[i + 1];
					p[3] = points[i + 1];
				}
				else
				{
					p[0] = points[i - 1];
					p[1] = points[i];
					p[2] = points[i + 1];
					p[3] = points[i + 2];
				}
				
				// Catmull-Rom to Cubic Bezier conversion matrix 
				//    0       1       0       0
				//  -1/6      1      1/6      0
				//    0      1/6      1     -1/6
				//    0       0       1       0
				Vector3[] bp = new Vector3[4];
				bp[0] = p[1];
				bp[1] = new Vector3(
					(-1f * p[0].x + 6 * p[1].x + p[2].x) / 6f,
					(-1f * p[0].y + 6 * p[1].y + p[2].y) / 6f,
					p[0].z
				);
				bp[2] = new Vector3(
					(p[1].x + 6 * p[2].x - p[3].x) / 6f,
					(p[1].y + 6 * p[2].y + p[3].y) / 6f,
					p[1].z
				);
				bp[3] = p[2];

				float scale = transform.localScale.x;
				bp[0] *= scale;
				bp[1] *= scale;
				bp[2] *= scale;
				bp[3] *= scale;
				d.Add(bp);
			}
			return d.ToArray();
		}
	}
}