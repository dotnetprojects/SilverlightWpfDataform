using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPF.DataForm.ColorPicker
{
    public class ColorPicker : Control
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.RegisterAttached("Color", typeof(Color), typeof(ColorPicker), new PropertyMetadata(new PropertyChangedCallback(ColorPicker.OnColorPropertyChanged)));
        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker item = d as ColorPicker;
            if (item != null)
            {
                item.UpdateControls();
                item.OnColorChanged();
            }
        }

        public ColorPicker()
        {
            DefaultStyleKey = typeof(ColorPicker);
            InitializeColorBoard();
        }

        public Color Color
        {
            get
            {
                return (Color)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        public event EventHandler<PropertyChangedEventArgs> ColorChanged;
        private void OnColorChanged()
        {
            if (ColorChanged != null)
            {
                ColorChanged(this, new PropertyChangedEventArgs("Color"));
            }
        }

        private FrameworkElement rootElement;
 
        private Button buttonDropDown;
        private Rectangle rectangleColor;
        private TextBlock textBlockColor;

        private Popup popupDropDown;
        private Canvas canvasOutside;
        private Canvas canvasOutsidePopup;
        private ColorBoard colorBoard;

        private void InitializeColorBoard()
        {
            colorBoard = new ColorBoard();
            colorBoard.IsTabStop = true;
            colorBoard.MouseLeftButtonDown += new MouseButtonEventHandler(ColorBoard_MouseLeftButtonDown);
            colorBoard.KeyDown += new KeyEventHandler(ColorBoard_KeyDown);
            colorBoard.SizeChanged += new SizeChangedEventHandler(ColorBoard_SizeChanged);
            colorBoard.DoneClicked += new RoutedEventHandler(ColorBoard_DoneClicked);
        }

        private void ColorBoard_DoneClicked(object sender, RoutedEventArgs e)
        {
            Color = colorBoard.Color;
            popupDropDown.IsOpen = false;
        }
        private void ColorBoard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
        private void ColorBoard_KeyDown(object sender, KeyEventArgs e)
        {
        }
        private void ColorBoard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetPopupPosition();
        }
        private void SetPopupPosition()
        {
            if (colorBoard != null && Application.Current != null && Application.Current.Host != null && Application.Current.Host.Content != null)
            {
                double contentheight = Application.Current.Host.Content.ActualHeight;
                double contentwidth = Application.Current.Host.Content.ActualWidth;
                double colorboardheight = colorBoard.ActualHeight;
                double colorboardwidth = colorBoard.ActualWidth;
                double pickerheight = ActualHeight;
                double pickerwidth = ActualWidth;

                if (rootElement != null)
                {
                    GeneralTransform transform = rootElement.TransformToVisual(null);
                    if (transform != null)
                    {
                        Point point00 = transform.Transform( new Point(0.0, 0.0));
                        Point point10 = transform.Transform( new Point(1.0, 0.0));
                        Point point01 = transform.Transform( new Point(0.0, 1.0));

                        double x00 = point00.X;
                        double y00 = point00.Y;
                        double x = x00;
                        double y = y00 + pickerheight;

                        if (contentheight < (y + colorboardheight))
                        {
                            y = y00 - colorboardheight;
                        }
                        if (contentwidth < (x + colorboardwidth))
                        {
                            x = x00 + pickerwidth - colorboardwidth;
                        }

                        popupDropDown.HorizontalOffset = 0.0;
                        popupDropDown.VerticalOffset = 0.0;
                        canvasOutsidePopup.Width = contentwidth;
                        canvasOutsidePopup.Height = contentheight;
                        colorBoard.HorizontalAlignment = HorizontalAlignment.Left;
                        colorBoard.VerticalAlignment = VerticalAlignment.Top;
                        Canvas.SetLeft(colorBoard, x - x00);
                        Canvas.SetTop(colorBoard, y - y00);

                        Matrix identity = Matrix.Identity;
                        identity.M11 = point10.X - point00.X;
                        identity.M12 = point10.Y - point00.Y;
                        identity.M21 = point01.X - point00.X;
                        identity.M22 = point01.Y - point00.Y;
                        identity.OffsetX = point00.X;
                        identity.OffsetY = point00.Y;

                        MatrixTransform matrixtransform = new MatrixTransform();
                        InvertMatrix(ref identity);

                        matrixtransform.Matrix = identity;
                        canvasOutsidePopup.RenderTransform = matrixtransform;
                    }
                }
            }
        }
        private bool InvertMatrix(ref Matrix matrix)
        {
            double d = (matrix.M11 * matrix.M22) - (matrix.M12 * matrix.M21);
            if (d == 0.0)
            {
                return false;
            }

            Matrix orgmatrix = matrix;
            matrix.M11 = orgmatrix.M22 / d;
            matrix.M12 = (-1.0 * orgmatrix.M12) / d;
            matrix.M21 = (-1.0 * orgmatrix.M21) / d;
            matrix.M22 = orgmatrix.M11 / d;
            matrix.OffsetX = (orgmatrix.OffsetY * orgmatrix.M21 - orgmatrix.OffsetX * orgmatrix.M22) / d;
            matrix.OffsetY = (orgmatrix.OffsetX * orgmatrix.M12 - orgmatrix.OffsetY * orgmatrix.M11) / d;
            return true;
        }

        private void UpdateControls()
        {
            if (rectangleColor != null)
            {
                if (rectangleColor.Fill is SolidColorBrush)
                {
                    (rectangleColor.Fill as SolidColorBrush).Color = Color;
                }
                else
                {
                    rectangleColor.Fill = new SolidColorBrush(Color);
                }
            }

            if (textBlockColor != null)
            {
                textBlockColor.Text = PredefinedColor.GetColorName(Color);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            rootElement = GetTemplateChild("Root") as FrameworkElement;

            buttonDropDown = GetTemplateChild("DropDownButton") as Button;
            popupDropDown = GetTemplateChild("Popup") as Popup;

            rectangleColor = (Rectangle)GetTemplateChild("RectangleColor");
            textBlockColor = (TextBlock)GetTemplateChild("TextBlockColor");

            if (buttonDropDown != null)
            {
                buttonDropDown.Click += new RoutedEventHandler(buttonDropDown_Click);
            }
            if (popupDropDown != null)
            {
                if (canvasOutside == null)
                {
                    canvasOutsidePopup = new Canvas();
                    canvasOutsidePopup.Background = new SolidColorBrush(Colors.Transparent);
                    canvasOutsidePopup.MouseLeftButtonDown += new MouseButtonEventHandler(canvasOutsidePopup_MouseLeftButtonDown);

                    canvasOutside = new Canvas();
                    canvasOutside.Children.Add(canvasOutsidePopup);
                    canvasOutside.Children.Add(colorBoard);
                }
                popupDropDown.Child = canvasOutside;
            }

            UpdateControls();
        }

        private void buttonDropDown_Click(object sender, RoutedEventArgs e)
        {
            colorBoard.Color = Color;
            popupDropDown.IsOpen = true;
        }
        private void canvasOutsidePopup_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popupDropDown.IsOpen = false;
        }
    }
}
