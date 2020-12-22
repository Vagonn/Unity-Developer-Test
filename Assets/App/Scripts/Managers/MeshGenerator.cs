﻿using UnityEngine;

namespace DynamicBox.Managers
{
	public class MeshGenerator : MonoBehaviour
	{
		[Header ("Parameters")] 
		[SerializeField] private int width;
		[SerializeField] private int height;
		[SerializeField] private int depth;

		[Space] 
		[SerializeField] private Material materialFront;
		[SerializeField] private Material materialBack;
		[SerializeField] private Material materialTop;
		[SerializeField] private Material materialBottom;
		[SerializeField] private Material materialLeft;
		[SerializeField] private Material materialRight;

		private Mesh mesh;
		private BoxCollider boxCollider;

		void Start ()
		{
			GameObject box = CreateBoxGameObject (width, height, depth);

			SetPivot ();
		}

		private GameObject CreateBoxGameObject (float _width, float _height, float _depth)
		{
			mesh = CreateBoxMesh (_width, _height, _depth);

			GameObject boxObject = new GameObject ("Box");
			MeshFilter meshFilter = boxObject.AddComponent<MeshFilter> ();
			meshFilter.mesh = mesh;

			boxCollider = boxObject.AddComponent<BoxCollider> ();
			boxCollider.size = new Vector3 (_width, _height, _depth);
			boxObject.AddComponent<MeshRenderer> ();

			Renderer meshRenderer = boxObject.GetComponent<Renderer> ();
			Material[] materials = new Material[6];

			materials[0] = materialFront;
			materials[1] = materialBack;
			materials[2] = materialTop;
			materials[3] = materialBottom;
			materials[4] = materialLeft;
			materials[5] = materialRight;

			meshRenderer.materials = materials;

			return boxObject;
		}

		private Mesh CreateBoxMesh (float _width, float _height, float _depth)
		{
			Mesh boxMesh = new Mesh {name = "BoxMesh"};

			#region

			// Because the box is centered at the origin, need to divide by two to find the + and - offsets
			_width = _width / 2.0f;
			_height = _height / 2.0f;
			_depth = _depth / 2.0f;

			Vector3[] boxVertices = new Vector3[36];
			Vector3[] boxNormals = new Vector3[36];
			Vector2[] boxUVs = new Vector2[36];

			Vector3 topLeftFront = new Vector3 (-_width, _height, _depth);
			Vector3 bottomLeftFront = new Vector3 (-_width, -_height, _depth);
			Vector3 topRightFront = new Vector3 (_width, _height, _depth);
			Vector3 bottomRightFront = new Vector3 (_width, -_height, _depth);
			Vector3 topLeftBack = new Vector3 (-_width, _height, -_depth);
			Vector3 topRightBack = new Vector3 (_width, _height, -_depth);
			Vector3 bottomLeftBack = new Vector3 (-_width, -_height, -_depth);
			Vector3 bottomRightBack = new Vector3 (_width, -_height, -_depth);

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

			// Front face.
			boxVertices[0] = topLeftFront;
			boxNormals[0] = frontNormal;
			boxUVs[0] = textureTopLeft;
			boxVertices[1] = bottomLeftFront;
			boxNormals[1] = frontNormal;
			boxUVs[1] = textureBottomLeft;
			boxVertices[2] = topRightFront;
			boxNormals[2] = frontNormal;
			boxUVs[2] = textureTopRight;
			boxVertices[3] = bottomLeftFront;
			boxNormals[3] = frontNormal;
			boxUVs[3] = textureBottomLeft;
			boxVertices[4] = bottomRightFront;
			boxNormals[4] = frontNormal;
			boxUVs[4] = textureBottomRight;
			boxVertices[5] = topRightFront;
			boxNormals[5] = frontNormal;
			boxUVs[5] = textureTopRight;

			// Back face.
			boxVertices[6] = topLeftBack;
			boxNormals[6] = backNormal;
			boxUVs[6] = textureTopRight;
			boxVertices[7] = topRightBack;
			boxNormals[7] = backNormal;
			boxUVs[7] = textureTopLeft;
			boxVertices[8] = bottomLeftBack;
			boxNormals[8] = backNormal;
			boxUVs[8] = textureBottomRight;
			boxVertices[9] = bottomLeftBack;
			boxNormals[9] = backNormal;
			boxUVs[9] = textureBottomRight;
			boxVertices[10] = topRightBack;
			boxNormals[10] = backNormal;
			boxUVs[10] = textureTopLeft;
			boxVertices[11] = bottomRightBack;
			boxNormals[11] = backNormal;
			boxUVs[11] = textureBottomLeft;

			// Top face.
			boxVertices[12] = topLeftFront;
			boxNormals[12] = topNormal;
			boxUVs[12] = textureBottomLeft;
			boxVertices[13] = topRightBack;
			boxNormals[13] = topNormal;
			boxUVs[13] = textureTopRight;
			boxVertices[14] = topLeftBack;
			boxNormals[14] = topNormal;
			boxUVs[14] = textureTopLeft;
			boxVertices[15] = topLeftFront;
			boxNormals[15] = topNormal;
			boxUVs[15] = textureBottomLeft;
			boxVertices[16] = topRightFront;
			boxNormals[16] = topNormal;
			boxUVs[16] = textureBottomRight;
			boxVertices[17] = topRightBack;
			boxNormals[17] = topNormal;
			boxUVs[17] = textureTopRight;

			// Bottom face. 
			boxVertices[18] = bottomLeftFront;
			boxNormals[18] = bottomNormal;
			boxUVs[18] = textureTopLeft;
			boxVertices[19] = bottomLeftBack;
			boxNormals[19] = bottomNormal;
			boxUVs[19] = textureBottomLeft;
			boxVertices[20] = bottomRightBack;
			boxNormals[20] = bottomNormal;
			boxUVs[20] = textureBottomRight;
			boxVertices[21] = bottomLeftFront;
			boxNormals[21] = bottomNormal;
			boxUVs[21] = textureTopLeft;
			boxVertices[22] = bottomRightBack;
			boxNormals[22] = bottomNormal;
			boxUVs[22] = textureBottomRight;
			boxVertices[23] = bottomRightFront;
			boxNormals[23] = bottomNormal;
			boxUVs[23] = textureTopRight;

			// Left face.
			boxVertices[24] = topLeftFront;
			boxNormals[24] = leftNormal;
			boxUVs[24] = textureTopRight;
			boxVertices[25] = bottomLeftBack;
			boxNormals[25] = leftNormal;
			boxUVs[25] = textureBottomLeft;
			boxVertices[26] = bottomLeftFront;
			boxNormals[26] = leftNormal;
			boxUVs[26] = textureBottomRight;
			boxVertices[27] = topLeftBack;
			boxNormals[27] = leftNormal;
			boxUVs[27] = textureTopLeft;
			boxVertices[28] = bottomLeftBack;
			boxNormals[28] = leftNormal;
			boxUVs[28] = textureBottomLeft;
			boxVertices[29] = topLeftFront;
			boxNormals[29] = leftNormal;
			boxUVs[29] = textureTopRight;

			// Right face. 
			boxVertices[30] = topRightFront;
			boxNormals[30] = rightNormal;
			boxUVs[30] = textureTopLeft;
			boxVertices[31] = bottomRightFront;
			boxNormals[31] = rightNormal;
			boxUVs[31] = textureBottomLeft;
			boxVertices[32] = bottomRightBack;
			boxNormals[32] = rightNormal;
			boxUVs[32] = textureBottomRight;
			boxVertices[33] = topRightBack;
			boxNormals[33] = rightNormal;
			boxUVs[33] = textureTopRight;
			boxVertices[34] = topRightFront;
			boxNormals[34] = rightNormal;
			boxUVs[34] = textureTopLeft;
			boxVertices[35] = bottomRightBack;
			boxNormals[35] = rightNormal;
			boxUVs[35] = textureBottomRight;
			
			#endregion

			boxMesh.subMeshCount = 6;

			boxMesh.vertices = boxVertices;
			boxMesh.normals = boxNormals;
			boxMesh.uv = boxUVs;

			int[][] trianglesSets = new int[6][];

			trianglesSets[0] = new[] {0, 1, 2, 3, 4, 5};
			trianglesSets[1] = new[] {6, 7, 8, 9, 10, 11};
			trianglesSets[2] = new[] {12, 13, 14, 15, 16, 17};
			trianglesSets[3] = new[] {18, 19, 20, 21, 22, 23};
			trianglesSets[4] = new[] {24, 25, 26, 27, 28, 29};
			trianglesSets[5] = new[] {30, 31, 32, 33, 34, 35};

			boxMesh.SetTriangles (trianglesSets[0], 0);
			boxMesh.SetTriangles (trianglesSets[1], 1);
			boxMesh.SetTriangles (trianglesSets[2], 2);
			boxMesh.SetTriangles (trianglesSets[3], 3);
			boxMesh.SetTriangles (trianglesSets[4], 4);
			boxMesh.SetTriangles (trianglesSets[5], 5);

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
	}
}