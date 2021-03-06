﻿using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetTopologySuite.IO.ShapeFile.Test
{
    [TestFixture]
    public class Issue178Fixture
    {
        [SetUp]
        public void SetUp()
        {
            // Set current dir to shapefiles dir
            Environment.CurrentDirectory = CommonHelpers.TestShapefilesDirectory;
        }

        [Test]
        public void TestCorruptedShapeFile()
        {
            IGeometryFactory factory = GeometryFactory.Default;
            const string filename = "christchurch-canterbury-h.shp";
            Assert.Throws<ShapefileException>(() =>
            {
                var reader = new ShapefileReader(filename, factory);
                Assert.Fail("Invalid file: code should be unreachable");
            });

            // ensure file isn't locked
            string path = Path.Combine(Environment.CurrentDirectory, filename);
            bool ok;
            using (FileStream file = File.OpenRead(path))
            {
                using (BinaryReader reader = new BinaryReader(file))
                {
                    // read a value
                    int val = reader.Read();
                    Console.WriteLine("read a value: " + val);
                    ok = true;
                }
            }
            Assert.That(ok, Is.True);
        }
    }
}
