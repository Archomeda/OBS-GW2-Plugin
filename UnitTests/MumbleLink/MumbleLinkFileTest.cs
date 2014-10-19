using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NUnit.Framework;
using ObsGw2Plugin.MumbleLink;
using ObsGw2Plugin.UnitTests.Utils;

namespace ObsGw2Plugin.UnitTests.MumbleLink
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MumbleLinkFileTest
    {
        [Test]
        unsafe public void SetDataFromLinkedMem()
        {
            uint uiVersion = 1;
            uint uiTick = 1234;
            Vector3 avatarPosition = new Vector3(34.5f, 65.25f, -12.75f);
            float[] fAvatarPosition = { (float)avatarPosition.X, (float)avatarPosition.Y, (float)avatarPosition.Z };
            Vector3 avatarFront = new Vector3(89.125f, 14.5f, 90.25f);
            float[] fAvatarFront = { (float)avatarFront.X, (float)avatarFront.Y, (float)avatarFront.Z };
            Vector3 avatarTop = new Vector3(84.5f, -37.25f, 65f);
            float[] fAvatarTop = { (float)avatarTop.X, (float)avatarTop.Y, (float)avatarTop.Z };
            string name = "Test Wars 42";
            Vector3 cameraPosition = new Vector3(95.25f, 10f, 61f);
            float[] fCameraPosition = { (float)cameraPosition.X, (float)cameraPosition.Y, (float)cameraPosition.Z };
            Vector3 cameraFront = new Vector3(-74.625f, 98.875f, 0f);
            float[] fCameraFront = { (float)cameraFront.X, (float)cameraFront.Y, (float)cameraFront.Z };
            Vector3 cameraTop = new Vector3(41.5f, 28.25f, 36.75f);
            float[] fCameraTop = { (float)cameraTop.X, (float)cameraTop.Y, (float)cameraTop.Z };
            string identity = "No identity identified";
            byte[] context = { 123, 65, 255, 0, 123, 26, 73, 186 };
            uint context_len = (uint)context.Length;
            string description = "Some awesome description is supposed to be here...";

            LinkedMem linkedMem = new LinkedMem();
            linkedMem.uiVersion = uiVersion;
            linkedMem.uiTick = uiTick;
            PointerUtils.CopyArrayToPointer(fAvatarPosition, linkedMem.fAvatarPosition);
            PointerUtils.CopyArrayToPointer(fAvatarFront, linkedMem.fAvatarFront);
            PointerUtils.CopyArrayToPointer(fAvatarTop, linkedMem.fAvatarTop);
            PointerUtils.CopyArrayToPointer(name.ToCharArray(), linkedMem.name);
            PointerUtils.CopyArrayToPointer(fCameraPosition, linkedMem.fCameraPosition);
            PointerUtils.CopyArrayToPointer(fCameraFront, linkedMem.fCameraFront);
            PointerUtils.CopyArrayToPointer(fCameraTop, linkedMem.fCameraTop);
            PointerUtils.CopyArrayToPointer(identity.ToCharArray(), linkedMem.identity);
            linkedMem.context_len = context_len;
            PointerUtils.CopyArrayToPointer(context, linkedMem.context);
            PointerUtils.CopyArrayToPointer(description.ToCharArray(), linkedMem.description);

            MumbleLinkFile mumbleLinkFile = new MumbleLinkFile();
            mumbleLinkFile.SetDataFromLinkedMem(linkedMem);

            Assert.IsTrue(mumbleLinkFile.IsValid, "IsValid");
            Assert.AreEqual(uiVersion, mumbleLinkFile.UIVersion, "UIVersion");
            Assert.AreEqual(uiTick, mumbleLinkFile.UITick, "UITick");
            Assert.AreEqual(avatarPosition, mumbleLinkFile.AvatarPosition, "AvatarPosition");
            Assert.AreEqual(avatarFront, mumbleLinkFile.AvatarFront, "AvatarFront");
            Assert.AreEqual(avatarTop, mumbleLinkFile.AvatarTop, "AvatarTop");
            Assert.AreEqual(name, mumbleLinkFile.Name, "Name");
            Assert.AreEqual(cameraPosition, mumbleLinkFile.CameraPosition, "CameraPosition");
            Assert.AreEqual(cameraFront, mumbleLinkFile.CameraFront, "CameraFront");
            Assert.AreEqual(cameraTop, mumbleLinkFile.CameraTop, "CameraTop");
            Assert.AreEqual(identity, mumbleLinkFile.Identity, "Identity");
            CollectionAssert.AreEqual(context, mumbleLinkFile.Context, "Context");
            Assert.AreEqual(context_len, mumbleLinkFile.Context.Length, "ContextLength");
            Assert.AreEqual(description, mumbleLinkFile.Description, "Description");
        }


        #region NotifyPropertyChanged tests

        public static IEnumerable<object[]> EnumerateProperties()
        {
            return new List<object[]>() {
                new object[] { "UIVersion", 1u },
                new object[] { "UITick", 1234u },
                new object[] { "AvatarPosition", new Vector3(34.5f, 65.25f, -12.75f) },
                new object[] { "AvatarFront", new Vector3(89.125f, 14.5f, 90.25f) },
                new object[] { "AvatarTop", new Vector3(84.5f, -37.25f, 65f) },
                new object[] { "Name", "Test Wars 42" },
                new object[] { "CameraPosition", new Vector3(95.25f, 10f, 61f) },
                new object[] { "CameraFront", new Vector3(-74.625f, 98.875f, 0f) },
                new object[] { "CameraTop", new Vector3(41.5f, 28.25f, 36.75f) },
                new object[] { "Identity", "No identity identified" },
                new object[] { "Description", "Some awesome description is supposed to be here..." }
            };
        }

        [Test, TestCaseSource(typeof(MumbleLinkFileTest), "EnumerateProperties")]
        public void NotifyPropertyChangedMumbleLinkFile(string propertyName, object newValue)
        {
            PropertyUtils.TestNotifyPropertyChanged(new MumbleLinkFile(), propertyName, newValue);
        }

        #endregion

    }
}
