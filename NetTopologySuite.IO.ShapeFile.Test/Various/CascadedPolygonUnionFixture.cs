﻿using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Overlay;
using NetTopologySuite.Operation.Overlay.Snap;
using NetTopologySuite.Operation.Union;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetTopologySuite.IO.ShapeFile.Test.Various
{
    [TestFixture]
    public class CascadedPolygonUnionFixture
    {
        protected IGeometryFactory Factory { get; private set; }

        protected WKTReader Reader { get; private set; }

        public CascadedPolygonUnionFixture()
        {
            // Set current dir to shapefiles dir
            string format = String.Format("..{0}..{0}..{0}NetTopologySuite.Samples.Shapefiles", Path.DirectorySeparatorChar);
            Environment.CurrentDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, format);

            this.Factory = new GeometryFactory();
            this.Reader = new WKTReader();
        }

        [Test]
        public void PerformCascadedPolygonUnion()
        {
            var reader = new ShapefileReader("tnp_pol.shp");
            var collection = reader.ReadAll().Where(e => e is IPolygon).ToList();
            var u1 = collection[0];
            for (var i = 1; i < collection.Count; i++)
                u1 = SnapIfNeededOverlayOp.Overlay(u1, collection[i], SpatialFunction.Union);
            var u2 = CascadedPolygonUnion.Union(collection);
            if (!u1.Equals(u2))
                Assert.Fail("failure");
        }
    }
}