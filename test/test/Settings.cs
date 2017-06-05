using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace EncePence
{
    class Settings
    {
        #region Data
        private string fileName;
        #endregion

        #region Ctors

        public Settings(string xmlPath)
        {
            XmlSettingsDocument = new XmlDocument();

            try
            {
                try
                {
                    XmlSettingsDocument.Load(xmlPath);
                }
                catch (Exception)
                {
                    throw;
                }
                fileName = xmlPath;
                InitializeProporties();
            }
            catch (ArgumentException)
            {
                throw new ArgumentException();
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException();
            }
        }
        #endregion

        #region Properties

        public XmlDocument XmlSettingsDocument
        {
            get;
            set;
        }

        private double displayValue;
        public double DisplayValue
        {
            get
            {
                return displayValue;
            }
            set
            {
                this.displayValue = value;
                this.XmlSettingsDocument.GetElementsByTagName("display")[0].InnerText = value.ToString();
                this.XmlSettingsDocument.Save(fileName);
            }
        }

        private System.Windows.Media.Color headerBackground;
        public System.Windows.Media.Color HeaderBackground
        {
            get
            {
                return headerBackground;
            }
            set
            {
                this.headerBackground = value;
                this.XmlSettingsDocument.GetElementsByTagName("headerBackground")[0]
                    .InnerText = this.HeaderBackground.ToString();
                this.XmlSettingsDocument.Save(fileName);
            }
        }

        private System.Windows.Media.Color headerForeground;
        public Color HeaderForeground
        {
            get
            {
                return headerForeground;
            }
            set
            {
                headerForeground = value;
                XmlSettingsDocument.GetElementsByTagName("headerForeground")[0]
                    .InnerText = headerForeground.ToString();
                XmlSettingsDocument.Save(fileName);
            }
        }

        private System.Windows.Media.Color contentBackground;
        public Color ContentBackground
        {
            get
            {
                return contentBackground;
            }
            set
            {
                contentBackground = value;
                this.XmlSettingsDocument.GetElementsByTagName("contentBackground")[0]
                    .InnerText = this.ContentBackground.ToString();
                this.XmlSettingsDocument.Save(fileName);
            }
        }

        private System.Windows.Media.Color contentForeground;
        public Color ContentForeground
        {
            get
            {
                return contentForeground;
            }
            set
            {
                this.contentForeground = value;
                this.XmlSettingsDocument.GetElementsByTagName("contentForeground")[0]
                    .InnerText = this.ContentForeground.ToString();
                this.XmlSettingsDocument.Save(fileName);
            }
        }

        private MWlodarz.Controls.MPopup.Position position;
        public MWlodarz.Controls.MPopup.Position Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                XmlSettingsDocument.GetElementsByTagName("position")[0]
                    .InnerText = Position.ToString();
                XmlSettingsDocument.Save(fileName);
            }
        }

        private PopupAnimation windowAnimation;
        public PopupAnimation WindowAnimation
        {
            get
            {
                return windowAnimation;
            }
            set
            {
                windowAnimation = value;
                XmlSettingsDocument.GetElementsByTagName("effect")[0]
                    .InnerText = WindowAnimation.ToString();
                XmlSettingsDocument.Save(fileName);
            }
        }

        private bool isStartWithSystem;
        public bool IsStartWithSystem
        {
            get
            {
                return isStartWithSystem;
            }
            set
            {
                isStartWithSystem = value;
                XmlSettingsDocument.GetElementsByTagName("start")[0].InnerText = isStartWithSystem.ToString();
                XmlSettingsDocument.Save(fileName);
            }
        }

        private bool isCloseToTray;
        public bool IsCloseToTray
        {
            get
            {
                return isCloseToTray;
            }
            set
            {
                isCloseToTray = value;
                XmlSettingsDocument.GetElementsByTagName("isCloseToTray")[0].InnerText = isCloseToTray.ToString();
                XmlSettingsDocument.Save(fileName);
            }
        }
        #endregion

        #region Methods
        public static void CreateSettingsXmlFile(string path)
        {
            XDocument xDocument = new XDocument();
            XElement root = new XElement("settings",
                new XElement("colors",
                    new XElement("headerBackground", "#FF1919FA"),
                    new XElement("headerForeground", "#FF000000"),
                    new XElement("contentBackground", "#FF6E67FF"),
                    new XElement("contentForeground", "#FF000000")),
                new XElement("popupWindow",
                    new XElement("position", "TopLeft"),
                    new XElement("effect", "Slide")),
                new XElement("display", "2"),
                new XElement("start", "True"),
                new XElement("isCloseToTray","True"));
            xDocument.Add(root);
            xDocument.Save(path);
        }

        private void InitializeProporties()
        {
            string displayValue = XmlSettingsDocument.GetElementsByTagName("display")[0].InnerText;
            try
            {
                this.DisplayValue = Double.Parse(displayValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            string color = this.XmlSettingsDocument.GetElementsByTagName("headerBackground")[0].InnerText;
            this.HeaderBackground = (Color)ColorConverter.ConvertFromString(color);
            string headerForeground = this.XmlSettingsDocument
                .GetElementsByTagName("headerForeground")[0].InnerText;
            this.HeaderForeground = (Color)ColorConverter.ConvertFromString("#000000");
            string contentBackground = this.XmlSettingsDocument
                .GetElementsByTagName("contentBackground")[0].InnerText;
            this.ContentBackground = (Color)ColorConverter.ConvertFromString(contentBackground);
            string contentForeground = this.XmlSettingsDocument
                .GetElementsByTagName("contentForeground")[0].InnerText;
            this.ContentForeground = (Color)ColorConverter.ConvertFromString(contentForeground);
            string position = this.XmlSettingsDocument.GetElementsByTagName("position")[0].InnerText;
            this.Position = GetPosition(position);
            string windowAnimation = this.XmlSettingsDocument.GetElementsByTagName("effect")[0].InnerText;
            this.WindowAnimation = GetAnimation(windowAnimation);
            string isStart = this.XmlSettingsDocument.GetElementsByTagName("start")[0].InnerText;
            this.IsStartWithSystem = isStart == "True" ? true : false;
            string isClose = this.XmlSettingsDocument.GetElementsByTagName("isCloseToTray")[0].InnerText;
            this.IsCloseToTray = isClose == "True" ? true : false;
        }

        private PopupAnimation GetAnimation(string windowAnimation)
        {
            switch (windowAnimation)
            {
                case "None":
                    return PopupAnimation.None;
                case "Fade":
                    return PopupAnimation.Fade;
                case "Slide":
                    return PopupAnimation.Slide;
                case "Scroll":
                    return PopupAnimation.Scroll;
                default:
                    return PopupAnimation.None;
            }
        }

        private MWlodarz.Controls.MPopup.Position GetPosition(string position)
        {
            switch (position)
            {
                case "TopLeft":
                    return MWlodarz.Controls.MPopup.Position.TopLeft;
                case "TopRight":
                    return MWlodarz.Controls.MPopup.Position.TopRight;
                case "BottomLeft":
                    return MWlodarz.Controls.MPopup.Position.BottomLeft;
                case "BottomRight":
                    return MWlodarz.Controls.MPopup.Position.BottomRight;
                default:
                    return MWlodarz.Controls.MPopup.Position.TopLeft;
            }
        }
        #endregion


    }
}
