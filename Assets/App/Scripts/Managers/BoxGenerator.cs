using System.Linq;
using DynamicBox.Helpers;
using UnityEngine;
using Random = System.Random;

namespace DynamicBox.Managers
{
	public class BoxGenerator : MonoBehaviour
	{
		// [Header ("Parameters")] 
		// [SerializeField] private float width;
		// [SerializeField] private float height;
		// [SerializeField] private float depth;

		[Space] [SerializeField] private Material materialFront;
		[SerializeField] private Material materialBack;
		[SerializeField] private Material materialTop;
		[SerializeField] private Material materialBottom;
		[SerializeField] private Material materialLeft;
		[SerializeField] private Material materialRight;
		[SerializeField] private Material materialBlack;

		private Mesh mesh;
		private BoxCollider boxCollider;
		private float depth;

		private static BoxGenerator _instance;
		public static BoxGenerator Instance => _instance;

		#region Unity Methods

		void Awake ()
		{
			if (_instance != null)
			{
				Destroy (gameObject);
			}

			_instance = this;
		}

		void Start ()
		{
			// CreateBox (width, height, depth);
		}

		#endregion

		public void CreateBox (float width, float height, Transform[] clickPoints)
		{
			GameObject box = CreateBoxGameObject (width, height, clickPoints);

			SetPivot ();

			GenerateInstanceID (box);
		}

		private GameObject CreateBoxGameObject (float width, float height, Transform[] clickPoints)
		{
			mesh = CreateBoxMesh (width, height, clickPoints);

			GameObject boxObject = new GameObject ("Box");
			boxObject.tag = "Box";

			MeshFilter meshFilter = boxObject.AddComponent<MeshFilter> ();
			meshFilter.mesh.Clear ();
			meshFilter.mesh = mesh;

			boxCollider = boxObject.AddComponent<BoxCollider> ();
			boxCollider.size = new Vector3 (width, height, depth);

			boxObject.AddComponent<MeshRenderer> ();
			Renderer meshRenderer = boxObject.GetComponent<Renderer> ();

			Material[] materials = new Material[6];

			materials[0] = materialBlack;
			materials[1] = materialBlack;
			materials[2] = materialFront;
			materials[3] = materialLeft;
			materials[4] = materialBack;
			materials[5] = materialRight;

			meshRenderer.materials = materials;

			return boxObject;
		}

		private Mesh CreateBoxMesh (float width, float height, Transform[] clickPoints)
		{
			Vector3 direction = clickPoints[1].position - clickPoints[0].position;
			float angle = Mathf.Atan (direction.z / direction.x) * 180 / Mathf.PI;
			clickPoints[0].rotation = Quaternion.Euler (0, 90 - angle, 0);
			clickPoints[1].rotation = Quaternion.Euler (0, 90 - angle, 0);

			return CreateBoxMeshFixedTransforms (width, height, clickPoints);
		}

		private Mesh CreateBoxMeshFixedTransforms (float width, float height, Transform[] clickPoints)
		{
			Mesh boxMesh = new Mesh {name = "BoxMesh"};

			#region Calculations

			// Because the box is centered at the origin, need to divide by two to find the + and - offsets
			// width = width / 2.0f;
			// height = height / 2.0f;
			// depth = depth / 2.0f;

			Vector3[] boxVertices = new Vector3[24];
			Vector3[] boxNormals = new Vector3[24];
			Vector2[] boxUVs = new Vector2[24];

			depth = Vector3.Distance (clickPoints[0].position, clickPoints[1].position);

			Vector3 topLeftFront = new Vector3 (-width, height, depth);
			Vector3 bottomLeftFront = new Vector3 (-width, -height, depth);
			Vector3 topRightFront = new Vector3 (width, height, depth);
			Vector3 bottomRightFront = new Vector3 (width, -height, depth);
			Vector3 topLeftBack = new Vector3 (-width, height, -depth);
			Vector3 topRightBack = new Vector3 (width, height, -depth);
			Vector3 bottomLeftBack = new Vector3 (-width, -height, -depth);
			Vector3 bottomRightBack = new Vector3 (width, -height, -depth);

			Vector2 textureTopLeft = new Vector2 (0.0f, 0.0f);
			Vector2 textureTopRight = new Vector2 (1.0f, 0.0f);
			Vector2 textureBottomLeft = new Vector2 (0.0f, 1.0f);
			Vector2 textureBottomRight = new Vector2 (1.0f, 1.0f);

			Vector3 frontNormal = new Vector3 (0.0f, 0.0f, 1.0f);
			Vector3 backNormal = new Vector3 (0.0f, 0.0f, -1.0f);
			Vector3 topNormal = new Vector3 (0.0f, 1.0f, 0.0f);
			Vector3 bottomNormal = new Vector3 (0.0f, -1.0f, 0.0f);
			Vector3 leftNormal = new Vector3 (-1.0f, 0.0f, 0.0f);
			Vector3 rightNormal = new Vector3 (1.0f, 0.0f, 0.0f);

			#region Vertices

			boxVertices = new[]
			{
				// down
				clickPoints[0].position + clickPoints[0].right * width / 2f,
				clickPoints[0].position - clickPoints[0].right * width / 2f,
				clickPoints[1].position - clickPoints[1].right * width / 2f,
				clickPoints[1].position + clickPoints[1].right * width / 2f,

				// up
				clickPoints[0].position + clickPoints[0].right * width / 2f + clickPoints[0].up * height,
				clickPoints[0].position - clickPoints[0].right * width / 2f + clickPoints[0].up * height,
				clickPoints[1].position - clickPoints[1].right * width / 2f + clickPoints[0].up * height,
				clickPoints[1].position + clickPoints[1].right * width / 2f + clickPoints[0].up * height,

				// front
				clickPoints[0].position + clickPoints[0].right * width / 2f,
				clickPoints[0].position - clickPoints[0].right * width / 2f,
				clickPoints[0].position - clickPoints[0].right * width / 2f + clickPoints[0].up * height,
				clickPoints[0].position + clickPoints[0].right * width / 2f + clickPoints[0].up * height,

				//left
				clickPoints[1].position - clickPoints[1].right * width / 2f,
				clickPoints[1].position - clickPoints[1].right * width / 2f + clickPoints[0].up * height,
				clickPoints[0].position - clickPoints[0].right * width / 2f + clickPoints[0].up * height,
				clickPoints[0].position - clickPoints[0].right * width / 2f,

				clickPoints[1].position + clickPoints[1].right * width / 2f,
				clickPoints[1].position + clickPoints[1].right * width / 2f + clickPoints[0].up * height,
				clickPoints[1].position - clickPoints[1].right * width / 2f + clickPoints[0].up * height,
				clickPoints[1].position - clickPoints[1].right * width / 2f,

				clickPoints[0].position + clickPoints[0].right * width / 2f,
				clickPoints[0].position + clickPoints[0].right * width / 2f + clickPoints[0].up * height,
				clickPoints[1].position + clickPoints[1].right * width / 2f + clickPoints[0].up * height,
				clickPoints[1].position + clickPoints[1].right * width / 2f,
			};

			#region old

			// // Front face.
			// boxVertices[0] = topLeftFront;
			// boxVertices[1] = bottomLeftFront;
			// boxVertices[2] = topRightFront;
			// boxVertices[3] = bottomLeftFront;
			// boxVertices[4] = bottomRightFront;
			// boxVertices[5] = topRightFront;
			//
			// // Back face.
			// boxVertices[6] = topLeftBack;
			// boxVertices[7] = topRightBack;
			// boxVertices[8] = bottomLeftBack;
			// boxVertices[9] = bottomLeftBack;
			// boxVertices[10] = topRightBack;
			// boxVertices[11] = bottomRightBack;
			//
			// // Top face.
			// boxVertices[12] = topLeftFront;
			// boxVertices[13] = topRightBack;
			// boxVertices[14] = topLeftBack;
			// boxVertices[15] = topLeftFront;
			// boxVertices[16] = topRightFront;
			// boxVertices[17] = topRightBack;
			//
			// // Bottom face. 
			// boxVertices[18] = bottomLeftFront;
			// boxVertices[19] = bottomLeftBack;
			// boxVertices[20] = bottomRightBack;
			// boxVertices[21] = bottomLeftFront;
			// boxVertices[22] = bottomRightBack;
			// boxVertices[23] = bottomRightFront;
			//
			// // Left face.
			// boxVertices[24] = topLeftFront;
			// boxVertices[25] = bottomLeftBack;
			// boxVertices[26] = bottomLeftFront;
			// boxVertices[27] = topLeftBack;
			// boxVertices[28] = bottomLeftBack;
			// boxVertices[29] = topLeftFront;
			//
			// // Right face. 
			// boxVertices[30] = topRightFront;
			// boxVertices[31] = bottomRightFront;
			// boxVertices[32] = bottomRightBack;
			// boxVertices[33] = topRightBack;
			// boxVertices[34] = topRightFront;
			// boxVertices[35] = bottomRightBack;

			#endregion

			#endregion

			#region Normals

			boxNormals[0] = frontNormal;
			boxNormals[1] = frontNormal;
			boxNormals[2] = frontNormal;
			boxNormals[3] = frontNormal;
			boxNormals[4] = frontNormal;
			boxNormals[5] = frontNormal;

			boxNormals[6] = backNormal;
			boxNormals[7] = backNormal;
			boxNormals[8] = backNormal;
			boxNormals[9] = backNormal;
			boxNormals[10] = backNormal;
			boxNormals[11] = backNormal;

			boxNormals[12] = topNormal;
			boxNormals[13] = topNormal;
			boxNormals[14] = topNormal;
			boxNormals[15] = topNormal;
			boxNormals[16] = topNormal;
			boxNormals[17] = topNormal;

			boxNormals[18] = bottomNormal;
			boxNormals[19] = bottomNormal;
			boxNormals[20] = bottomNormal;
			boxNormals[21] = bottomNormal;
			boxNormals[22] = bottomNormal;
			boxNormals[23] = bottomNormal;

			// boxNormals[24] = leftNormal;
			// boxNormals[25] = leftNormal;
			// boxNormals[26] = leftNormal;
			// boxNormals[27] = leftNormal;
			// boxNormals[28] = leftNormal;
			// boxNormals[29] = leftNormal;
			//
			// boxNormals[30] = rightNormal;
			// boxNormals[31] = rightNormal;
			// boxNormals[32] = rightNormal;
			// boxNormals[33] = rightNormal;
			// boxNormals[34] = rightNormal;
			// boxNormals[35] = rightNormal;

			#endregion

			#region UVs

			boxUVs[0] = textureTopLeft;
			boxUVs[1] = textureBottomLeft;
			boxUVs[2] = textureTopRight;
			boxUVs[3] = textureBottomLeft;

			boxUVs[4] = textureBottomRight;
			boxUVs[5] = textureTopRight;
			boxUVs[6] = textureTopRight;
			boxUVs[7] = textureTopLeft;

			boxUVs[8] = textureBottomRight;
			boxUVs[9] = textureBottomRight;
			boxUVs[10] = textureTopLeft;
			boxUVs[11] = textureBottomLeft;

			boxUVs[12] = textureBottomLeft;
			boxUVs[13] = textureTopRight;
			boxUVs[14] = textureTopLeft;
			boxUVs[15] = textureBottomLeft;

			boxUVs[16] = textureBottomRight;
			boxUVs[17] = textureTopRight;
			boxUVs[18] = textureTopLeft;
			boxUVs[19] = textureBottomLeft;

			boxUVs[20] = textureBottomRight;
			boxUVs[21] = textureTopLeft;
			boxUVs[22] = textureBottomRight;
			boxUVs[23] = textureTopRight;

			// //Left face.
			// boxUVs[24] = textureTopRight;
			// boxUVs[25] = textureBottomLeft;
			// boxUVs[26] = textureBottomRight;
			// boxUVs[27] = textureTopLeft;
			// boxUVs[28] = textureBottomLeft;
			// boxUVs[29] = textureTopRight;
			//
			// //Right face. 
			// boxUVs[30] = textureTopLeft;
			// boxUVs[31] = textureBottomLeft;
			// boxUVs[32] = textureBottomRight;
			// boxUVs[33] = textureTopRight;
			// boxUVs[34] = textureTopLeft;
			// boxUVs[35] = textureBottomRight;

			#endregion

			#endregion

			boxMesh.vertices = boxVertices;
			boxMesh.normals = boxNormals;
			boxMesh.uv = boxUVs;

			#region Triangles

			boxMesh.subMeshCount = 6;
			int[][] trianglesSets = new int[6][];

			#region old

			// trianglesSets[0] = new[] {0, 1, 2, 3, 4, 5};
			// trianglesSets[1] = new[] {6, 7, 8, 9, 10, 11};
			// trianglesSets[2] = new[] {12, 13, 14, 15, 16, 17};
			// trianglesSets[3] = new[] {18, 19, 20, 21, 22, 23};
			// trianglesSets[4] = new[] {24, 25, 26, 27, 28, 29};
			// trianglesSets[5] = new[] {30, 31, 32, 33, 34, 35};
			//
			// boxMesh.SetTriangles (trianglesSets[0], 0);
			// boxMesh.SetTriangles (trianglesSets[1], 1);
			// boxMesh.SetTriangles (trianglesSets[2], 2);
			// boxMesh.SetTriangles (trianglesSets[3], 3);
			// boxMesh.SetTriangles (trianglesSets[4], 4);
			// boxMesh.SetTriangles (trianglesSets[5], 5);

			#endregion

			trianglesSets[0] = new[] {0, 1, 2, 2, 3, 0};
			trianglesSets[1] = new[] {6, 5, 4, 4, 7, 6};
			trianglesSets[2] = new[] {10, 9, 8, 8, 11, 10};
			trianglesSets[3] = new[] {14, 13, 12, 12, 15, 14};
			trianglesSets[4] = new[] {18, 17, 16, 16, 19, 18};
			trianglesSets[5] = new[] {22, 21, 20, 20, 23, 22};

			boxMesh.SetTriangles (trianglesSets[0].Reverse ().ToArray (), 0);
			boxMesh.SetTriangles (trianglesSets[1].Reverse ().ToArray (), 1);
			boxMesh.SetTriangles (trianglesSets[2].Reverse ().ToArray (), 2);
			boxMesh.SetTriangles (trianglesSets[3].Reverse ().ToArray (), 3);
			boxMesh.SetTriangles (trianglesSets[4].Reverse ().ToArray (), 4);
			boxMesh.SetTriangles (trianglesSets[5].Reverse ().ToArray (), 5);

			#endregion

			boxMesh.Optimize ();
			boxMesh.RecalculateBounds ();
			boxMesh.RecalculateNormals ();

			return boxMesh;
		}

		private void SetPivot ()
		{
			// Calculate difference in 3d position
			Vector3 diff = Vector3.Scale (mesh.bounds.extents, new Vector3 (0, 1, 0));

			// Move object position
			boxCollider.transform.position -= Vector3.Scale (diff, boxCollider.transform.localScale);

			// Iterate over all vertices and move them in the opposite direction of the object position movement
			Vector3[] verts = mesh.vertices;
			for (int i = 0; i < verts.Length; i++)
			{
				verts[i] += diff;
			}

			mesh.vertices = verts;
			mesh.RecalculateBounds ();

			boxCollider.center += diff;
		}

		private void GenerateInstanceID (GameObject box)
		{
			Random random = new Random ();
			string randomString = string.Empty;
			for (int i = 0; i < 13; i++)
			{
				int randomNumber = random.Next (9) + 1;
				randomString = string.Concat (randomString, randomNumber.ToString ());
			}

			double generatedID = double.Parse (randomString);

			// Debug.Log ("generatedID: " + generatedID);

			ObjectData objectData = box.AddComponent<ObjectData> ();
			objectData.SetInstanceID (generatedID);
		}

		public void ClearScene ()
		{
			foreach (GameObject boxItem in GameObject.FindGameObjectsWithTag ("Box"))
			{
				Destroy (boxItem);
			}
		}
	}
}