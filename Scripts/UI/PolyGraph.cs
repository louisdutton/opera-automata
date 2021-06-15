using System;
using UnityEngine;
using Attribute = AI.Attribute;

namespace UI
{
    public class PolyGraph : MonoBehaviour
    {
        public new CanvasRenderer renderer;
        public CanvasRenderer background;
        public Material material;
        public Material backgroundMaterial;
        public Vector2 origin;
        public Vector2 radius = new Vector2(25f, 200f);
        public Attribute[] attributes;

        private void Awake()
        {
            renderer.SetMaterial(material, null);
            background.SetMaterial(backgroundMaterial, null);
        }
        
        private void Update()
        {
            var mesh = GeneratePolyMesh();
            renderer.SetMesh(mesh);
        }

        public void SetAttributes(Attribute[] attributes)
        {
            var regenerate = attributes.Length != this.attributes.Length;

            this.attributes = attributes;

            if (regenerate)
            {
                var bg = GenerateBackgroundMesh();
                background.SetMesh(bg);
            }

            var polyMesh = GeneratePolyMesh();
            renderer.SetMesh(polyMesh);
        }

        private int[] CreateTriangles()
        {
            var tris = new int[attributes.Length * 3];
            for (int i = 0; i < attributes.Length; i++)
            {
                var t = i * 3;
            
                tris[t] = 0;
                tris[t+1] = i + 1;
                tris[t+2] = i + 2;
            }
            tris[tris.Length - 1] = 1;
            return tris;
        }
        
        private Vector3[] CreateScaledVertices()
        {
            var verts = new Vector3[attributes.Length + 1];
            verts[0] = origin;
            var angle = 2f * Mathf.PI / attributes.Length;
        
            for (int i = 1; i < verts.Length; i++)
            {
                // radius & angle
                var r = Mathf.Lerp(radius.x, radius.y, attributes[i-1].value);
                var a = angle * (i - 1);
            
                // coordinates
                var x = origin.x + r * Mathf.Sin(a);
                var y = origin.y + r * Mathf.Cos(a);
            
                verts[i] = new Vector3(x, y);
            }

            return verts;
        }
        
        private Vector3[] CreateVertices()
        {
            var verts = new Vector3[attributes.Length + 1];
            verts[0] = origin;
            var angle = 2f * Mathf.PI / attributes.Length;
        
            for (int i = 1; i < verts.Length; i++)
            {
                // radius & angle
                var r = radius.y;
                var a = angle * (i - 1);
            
                // coordinates
                var x = origin.x + r * Mathf.Sin(a);
                var y = origin.y + r * Mathf.Cos(a);
            
                verts[i] = new Vector3(x, y);
            }

            return verts;
        }

        private Mesh GeneratePolyMesh()
        {
            var mesh = new Mesh();
            mesh.vertices = CreateScaledVertices();
            mesh.triangles = CreateTriangles();
            return mesh;
        }
        
        private Mesh GenerateBackgroundMesh()
        {
            var mesh = new Mesh();
            mesh.vertices = CreateVertices();
            mesh.triangles = CreateTriangles();
            return mesh;
        }
    }
}
