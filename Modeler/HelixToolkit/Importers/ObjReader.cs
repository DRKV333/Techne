﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media;

namespace HelixToolkit
{
    /// <summary>
    /// Obj (Wavefront) file reader
    /// http://en.wikipedia.org/wiki/Obj
    /// http://www.martinreddy.net/gfx/3d/OBJ.spec
    /// http://www.eg-models.de/formats/Format_Obj.html
    /// </summary>
    public class ObjReader : IModelReader
    {

        private StreamReader reader { get; set; }
        private Point3DCollection Points { get; set; }
        private PointCollection TexCoords { get; set; }
        private Vector3DCollection Normals { get; set; }
        public string TexturePath { get; set; }

        public class Group
        {
            public MeshBuilder MeshBuilder { get; set; }
            public Material Material { get; set; }
            public string Name { get; set; }
            public int Smoothing { get; set; }
            public Group(string name)
            {
                Name = name;
                Material = MaterialHelper.CreateMaterial(Brushes.Green);
                MeshBuilder = new MeshBuilder();
            }
        }

        public class MaterialDefinition
        {
            // http://en.wikipedia.org/wiki/Material_Template_Library

            public Color Ambient { get; set; }
            public Color Diffuse { get; set; }
            public Color Specular { get; set; }
            public double SpecularCoefficient { get; set; }
            public double Dissolved { get; set; }
            public int Illumination { get; set; }
            public string AmbientMap { get; set; }
            public string AlphaMap { get; set; }
            public string DiffuseMap { get; set; }
            public string SpecularMap { get; set; }
            public string BumpMap { get; set; }

            public Material GetMaterial()
            {
                var mg = new MaterialGroup();
                if (DiffuseMap == null)
                {
                    var diffuseBrush = new SolidColorBrush(Diffuse) { Opacity = Dissolved };
                    mg.Children.Add(new DiffuseMaterial(diffuseBrush));
                }
                else
                {
                    // var path = Path.Combine(TexturePath, DiffuseMap);
                    var path = DiffuseMap;
                    if (File.Exists(path))
                    {

                        var img = new BitmapImage(new Uri(path, UriKind.Relative));
                        var textureBrush = new ImageBrush(img) { Opacity = Dissolved };
                        mg.Children.Add(new DiffuseMaterial(textureBrush));
                    }
                }

                mg.Children.Add(new SpecularMaterial(new SolidColorBrush(Specular), SpecularCoefficient));

                return mg;
            }
        }

        public Dictionary<string, MaterialDefinition> Materials { get; private set; }
        public Collection<Group> Groups { get; private set; }

        public ObjReader()
        {
            Points = new Point3DCollection();
            TexCoords = new PointCollection();
            Normals = new Vector3DCollection();

            Groups = new Collection<Group>();
            Materials = new Dictionary<string, MaterialDefinition>();
        }

        public Model3DGroup Read(string path)
        {
            TexturePath = Path.GetDirectoryName(path);
            var s = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var result = Read(s);
            s.Close();
            return result;
        }

        /// <summary>
        /// Reads a GZipStream compressed OBJ file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public Model3DGroup ReadZ(string path)
        {
            TexturePath = Path.GetDirectoryName(path);
            var s = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var deflateStream = new GZipStream(s, CompressionMode.Decompress, true);
            var result = Read(deflateStream);
            deflateStream.Close();
            s.Close();
            return result;
        }

        public Model3DGroup Read(Stream s)
        {
            reader = new StreamReader(s);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine().Trim();
                if (line.StartsWith("#") || line.Length == 0)
                    continue;
                string id, values;
                SplitLine(line, out id, out values);
                switch (id)
                {
                    case "v":
                        AddVertex(values);
                        break;
                    case "vt":
                        AddTexCoord(values);
                        break;
                    case "vn":
                        AddNormal(values);
                        break;
                    case "f":
                        AddFace(values);
                        break;
                    case "g":
                        Groups.Add(new Group(values));
                        break;
                    case "mtllib":
                        LoadMaterialLib(values);
                        break;
                    case "usemtl":
                        SetMaterial(values);
                        break;
                    case "s":
                        SetSmoothing(values);
                        break;
                    case "o":
                        break;
                }
            }
            reader.Close();

            return BuildModel();
        }
        Group CurrentGroup
        {
            get
            {
                if (Groups.Count == 0)
                    Groups.Add(new Group("default"));
                return Groups[Groups.Count - 1];
            }
        }

        private void SetSmoothing(string s)
        {
            int smoothing;
            int.TryParse(s, out smoothing);
            CurrentGroup.Smoothing = smoothing;
        }

        private void LoadMaterialLib(string mtlFile)
        {
            if (!File.Exists(mtlFile))
                return;

            var mreader = new StreamReader(mtlFile);

            MaterialDefinition currentMaterial = null;

            while (!reader.EndOfStream)
            {
                var line = mreader.ReadLine().Trim();
                if (line.StartsWith("#") || line.Length == 0)
                    continue;
                string id, values;
                SplitLine(line, out id, out values);
                switch (id)
                {
                    case "newMtl":
                        currentMaterial = new MaterialDefinition();
                        Materials.Add(values, currentMaterial);
                        break;
                    case "Ka":
                        if (currentMaterial != null)
                            currentMaterial.Ambient = ColorParse(values);
                        break;
                    case "Kd":
                        if (currentMaterial != null)
                            currentMaterial.Diffuse = ColorParse(values);
                        break;
                    case "Ks":
                        if (currentMaterial != null)
                            currentMaterial.Specular = ColorParse(values);
                        break;
                    case "Ns":
                        if (currentMaterial != null)
                            currentMaterial.SpecularCoefficient = DoubleParse(values);
                        break;
                    case "d":
                    case "Tr":
                        if (currentMaterial != null)
                            currentMaterial.Dissolved = DoubleParse(values);
                        break;
                    case "illum":
                        if (currentMaterial != null)
                            currentMaterial.Illumination = int.Parse(values);
                        break;
                    case "map_Ka":
                        if (currentMaterial != null)
                            currentMaterial.AmbientMap = values;
                        break;
                    case "map_Kd":
                        if (currentMaterial != null)
                            currentMaterial.DiffuseMap = values;
                        break;
                    case "map_Ks":
                        if (currentMaterial != null)
                            currentMaterial.SpecularMap = values;
                        break;
                    case "map_d":
                        if (currentMaterial != null)
                            currentMaterial.AlphaMap = values;
                        break;
                    case "map_bump":
                    case "bump":
                        if (currentMaterial != null)
                            currentMaterial.BumpMap = values;
                        break;
                }
            }
            reader.Close();
        }

        private static Color ColorParse(string values)
        {
            double[] fields = Split(values);
            return Color.FromRgb((byte)(fields[0] * 255), (byte)(fields[1] * 255), (byte)(fields[2] * 255));
        }

        private void SetMaterial(string materialName)
        {
            CurrentGroup.Material = GetMaterial(materialName);
        }

        private Material GetMaterial(string materialName)
        {
            MaterialDefinition mat;
            if (Materials.TryGetValue(materialName, out mat))
                return mat.GetMaterial();
            return MaterialHelper.CreateMaterial(Brushes.Gold);
        }

        private void AddFace(string values)
        {
            // A polygonal face. The numbers are indexes into the arrays of vertex positions, 
            // texture coordinates, and normals respectively. A number may be omitted if, 
            // for example, texture coordinates are not being defined in the model.
            // There is no maximum number of vertices that a single polygon may contain. 
            // The .obj file specification says that each face must be flat and convex. 

            string[] fields = values.Split(' ');
            var pts = new Point3DCollection();
            var tex = new PointCollection();
            var norm = new Vector3DCollection();
            foreach (string field in fields)
            {
                var ff = field.Split('/');
                int vi = int.Parse(ff[0]);
                int vti = ff.Length > 1 && ff[1].Length > 0 ? int.Parse(ff[1]) : -1;
                int vni = ff.Length > 2 && ff[2].Length > 0 ? int.Parse(ff[2]) : -1;
                pts.Add(Points[vi - 1]);
                if (vti >= 0)
                    tex.Add(TexCoords[vti - 1]);
                if (vni >= 0)
                    norm.Add(Normals[vni - 1]);
            }
            if (tex.Count == 0) tex = null;
            if (norm.Count == 0) norm = null;

            // QUAD
            if (pts.Count == 4)
            {
                CurrentGroup.MeshBuilder.AddQuads(pts, norm, tex);
                return;
            }

            // TRIANGLE
            if (pts.Count == 3)
            {
                CurrentGroup.MeshBuilder.AddTriangles(pts, norm, tex);
                return;
            }

            // POLYGONS (flat and convex)
            var poly3d = new Polygon3D(pts);
            // Transform the polygon to 2D
            var poly2d = poly3d.Flatten();
            // Triangulate
            var tri = poly2d.Triangulate();
            if (tri != null)
            {
                // Add the triangle indices with the 3D points
                var mesh = new MeshBuilder();
                mesh.Append(pts,tri);
                CurrentGroup.MeshBuilder.Append(mesh);
            }
        }

        private void AddNormal(string values)
        {
            double[] fields = Split(values);
            Normals.Add(new Vector3D(fields[0], fields[1], fields[2]));
        }

        private void AddTexCoord(string values)
        {
            double[] fields = Split(values);
            TexCoords.Add(new Point(fields[0], fields[1]));
        }

        private void AddVertex(string values)
        {
            double[] fields = Split(values);
            Points.Add(new Point3D(fields[0], fields[1], fields[2]));
        }

        private static double[] Split(string values)
        {
            values = values.Trim();
            string[] fields = values.Split(' ');
            var result = new double[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                result[i] = DoubleParse(fields[i]);
            return result;
        }

        private static double DoubleParse(string s)
        {
            return double.Parse(s, CultureInfo.InvariantCulture);
        }

        private static void SplitLine(string line, out string id, out string values)
        {
            int idx = line.IndexOf(' ');
            id = line.Substring(0, idx).ToLower();
            values = line.Substring(idx + 1);
        }

        private Model3DGroup BuildModel()
        {
            var modelGroup = new Model3DGroup();
            foreach (var g in Groups)
            {
                var gm = new GeometryModel3D();
                gm.Geometry = g.MeshBuilder.ToMesh();
                gm.Material = g.Material;
                gm.BackMaterial = gm.Material;
                modelGroup.Children.Add(gm);
            }
            return modelGroup;
        }

    }
}
