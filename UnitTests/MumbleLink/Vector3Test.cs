using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NUnit.Framework;
using ObsGw2Plugin.MumbleLink;

// Disable warning about comparison to same variable (CS1718)
#pragma warning disable 1718

namespace ObsGw2Plugin.UnitTests.MumbleLink
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Vector3Test
    {
        [Test]
        public void Constructor()
        {
            double x = 1;
            double y = 2;
            double z = 3;
            Vector3 vector = new Vector3(x, y, z);

            Assert.AreEqual(x, vector.X, "x");
            Assert.AreEqual(y, vector.Y, "y");
            Assert.AreEqual(z, vector.Z, "z");
        }

        [Test]
        public void EqualitySelf()
        {
            Vector3 vector = new Vector3(1, 2, 3);

            Assert.IsTrue(vector.Equals(vector), "Equals");
            Assert.IsTrue(vector == vector, "==");
            Assert.IsFalse(vector != vector, "!=");
            Assert.AreEqual(vector.GetHashCode(), vector.GetHashCode());
        }

        [Test]
        public void EqualityVector3()
        {
            Vector3 vectorA = new Vector3(1, 2, 3);
            Vector3 vectorB = new Vector3(1, 2, 3);

            Assert.IsTrue(vectorA.Equals(vectorB), "Equals");
            Assert.IsTrue(vectorA == vectorB, "==");
            Assert.IsFalse(vectorA != vectorB, "!=");
            Assert.AreEqual(vectorA.GetHashCode(), vectorB.GetHashCode());
        }

        [Test]
        public void EqualityOther()
        {
            object vectorA = new Vector3(1, 2, 3);
            object vectorB = new Vector3(1, 2, 3);

            Assert.IsTrue(vectorA.Equals(vectorB), "Equals");
            Assert.AreEqual(vectorA.GetHashCode(), vectorB.GetHashCode());
        }

        [Test]
        public void InequalityVector3()
        {
            Vector3 vectorA = new Vector3(1, 2, 3);
            Vector3 vectorB = new Vector3(2, 3, 4);

            Assert.IsFalse(vectorA.Equals(vectorB), "Equals");
            Assert.IsFalse(vectorA == vectorB, "==");
            Assert.IsTrue(vectorA != vectorB, "!=");
        }

        [Test]
        public void InequalityOther()
        {
            object vector = new Vector3(1, 2, 3);
            object obj = new object();

            Assert.IsFalse(vector.Equals(obj), "Equals");
        }

        [Test]
        public void InequalityNull()
        {
            Vector3 vector = new Vector3(1, 2, 3);

            Assert.IsFalse(vector.Equals(null), "Equals");
            Assert.IsFalse(vector == null, "==");
            Assert.IsTrue(vector != null, "!=");
        }
    }
}
