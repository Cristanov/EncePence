using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace MWlodarz.Controls
{
    /// <summary>
    /// Interaction logic for MPopup.xaml
    /// </summary>
    public partial class MPopup : UserControl
    {       
        #region Ctors
        public MPopup()
        {
            InitializeComponent();
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        public string HeadText
        {
            get
            {
                return tbHead.Text;
            }
            set
            {
                tbHead.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the content text.
        /// </summary>
        public string ContentText
        {
            get
            {
                return tbContent.Text;
            }
            set
            {
                tbContent.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the MPopup is open.
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return Popup.IsOpen;
            }
            set
            {
                Popup.IsOpen = value;
            }
        }


        private Position position; 
        /// <summary>
        /// Gets or sets a value that indicates the MPopup position on the screen.
        /// </summary>
        public Position PopupPosition
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                switch (position)
                {
                    case Position.TopLeft:
                        {
                            SetToTopLeft();
                            break;
                        }
                    case Position.TopRight:
                        {
                            SetToTopRight();
                            break;
                        }
                    case Position.BottomLeft:
                        {
                            SetToBottomLeft();
                            break;
                        }
                    case Position.BottomRight:
                        {
                            SetToBottomRight();
                            break;
                        }
                    
                }
            }
        }

        private void SetToBottomRight()
        {
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            Popup.HorizontalOffset = screenWidth - Popup.Width;
            Popup.VerticalOffset = screenHeight - Popup.Height;
        }

        private void SetToBottomLeft()
        {
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            Popup.HorizontalOffset = 0;
            Popup.VerticalOffset = screenHeight - Popup.Height;
        }

        private void SetToTopRight()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            Popup.VerticalOffset = 0;
            Popup.HorizontalOffset = screenWidth - Popup.Width;
        }

        private void SetToTopLeft()
        {
            Popup.HorizontalOffset = 0;
            Popup.VerticalOffset = 0;
        } 
        #endregion

        #region EventHandlers
        private void CloseButton(object sender, RoutedEventArgs e)
        {
            Popup.IsOpen = false;
        } 
        #endregion

        #region Enums
        /// <summary>
        /// Specifies the MPopup position on the screen.
        /// </summary>
        public enum Position
        {
            /// <summary>
            /// The MPopup is on the top left corner.
            /// </summary>
            [Description("Lewy górny")]
            TopLeft,
            /// <summary>
            /// The MPopup is on the top right corner.
            /// </summary>
            [Description("Prawy górny")]
            TopRight,
            /// <summary>
            /// The MPopup is on the bottom left corner.
            /// </summary>
            [Description("Lewy dolny")]
            BottomLeft,
            /// <summary>
            /// The MPopup is on the bottom right corner.
            /// </summary>
            [Description("Prawy dolny")]
            BottomRight
        } 
        #endregion

        #region Methods
        private static string GetDescription(object enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            if (null != fi)
            {
                object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
                else return "";
            }
            else
            {
                return "";
            }

        }

        public override string ToString()
        {
            return GetDescription(position);
        }
        #endregion

        #region InternalClasses
        public class EnumHelper
        {

            public EnumHelper(Position pos)
            {
                this.Position = pos;
            }

            public Position Position
            {
                get;
                set;
            }
        } 
        #endregion
    }
}
