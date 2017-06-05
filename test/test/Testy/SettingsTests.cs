using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace EncePence.Testy
{
    [TestFixture]
    class SettingsTests
    {
        Settings settings, settingsFake;
        [SetUp]
        public void SetUpMethod()
        {
            settings = new Settings(@"SettingsTest.xml");
        }

        [Test]
        public void CtorTest()
        {
            Assert.IsNotNull(settings);
            Console.WriteLine(settings.ToString());
            Assert.IsNotNull(settings.XmlSettingsDocument);
        }

       
        [Test]
        public void displayValueTest()
        {
            double displayValue = settings.DisplayValue;
            Assert.AreEqual(2, displayValue);
        }

        [Test]
        public void headerBackgroundColorTest()
        {
            Color color = (Color)ColorConverter.ConvertFromString("#c0392b");
            Assert.AreEqual(color, settings.HeaderBackground);
        }

        [Test]
        public void headerForegroundColorTest()
        {
            Color color = (Color)ColorConverter.ConvertFromString("#000000");
            Assert.AreEqual(color, settings.HeaderForeground);
        }

        [Test]
        public void contentBackgroundColorTest()
        {
            Color color = (Color)ColorConverter.ConvertFromString("#e74c3c");
            Assert.AreEqual(color, settings.ContentBackground);
        }

        [Test]
        public void contentForegroundColorTest()
        {
            Color color = (Color)ColorConverter.ConvertFromString("#000000");
            Assert.AreEqual(color, settings.ContentForeground);
        }

        [Test]
        public void windowPositionTest()
        {
            Assert.AreEqual(MWlodarz.Controls.MPopup.Position.TopRight, settings.Position);
            settings.Position = MWlodarz.Controls.MPopup.Position.TopLeft;
            Assert.AreEqual(MWlodarz.Controls.MPopup.Position.TopLeft, settings.Position);
            settings.Position = MWlodarz.Controls.MPopup.Position.BottomLeft;
            Assert.AreEqual(MWlodarz.Controls.MPopup.Position.BottomLeft, settings.Position);
            settings.Position = MWlodarz.Controls.MPopup.Position.BottomRight;
            Assert.AreEqual(MWlodarz.Controls.MPopup.Position.BottomRight, settings.Position);
            settings.Position = MWlodarz.Controls.MPopup.Position.TopRight;
        }

        [Test]
        public void windowEffectTest()
        {
            Assert.AreEqual(PopupAnimation.Slide, settings.WindowAnimation);
            settings.WindowAnimation = PopupAnimation.Fade;
            Assert.AreEqual(PopupAnimation.Fade, settings.WindowAnimation);
            settings.WindowAnimation = PopupAnimation.None;
            Assert.AreEqual(PopupAnimation.None, settings.WindowAnimation);
            settings.WindowAnimation = PopupAnimation.Scroll;
            Assert.AreEqual(PopupAnimation.Scroll, settings.WindowAnimation);
            settings.WindowAnimation = PopupAnimation.Slide;
        }

        [Test]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void FileNotFoundExceptionTest()
        {
            settingsFake = new Settings("cos");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentExceptionTest()
        {
            settingsFake = new Settings("");
        }

        [Test]
        public void CloseToTrayPropertyTest()
        {
            settings.IsCloseToTray = true;
            Assert.IsTrue(settings.IsCloseToTray);
        }

    }
}
