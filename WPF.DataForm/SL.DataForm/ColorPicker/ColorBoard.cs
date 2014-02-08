using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPF.DataForm.ColorPicker
{
    public class ColorBoard : Control
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.RegisterAttached("Color", typeof(Color), typeof(ColorBoard), new PropertyMetadata(new PropertyChangedCallback(ColorBoard.OnColorPropertyChanged)));
        private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorBoard control = d as ColorBoard;
            if (control != null && control.rootElement != null)
            {
                if (control.Updating)
                {
                    return;
                }

                Color color = (Color)e.NewValue;
                control.UpdateControls(color, true, true, true);
            }
        }

        public ColorBoard()
        {
            DefaultStyleKey = typeof(ColorBoard);
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

        public event RoutedEventHandler DoneClicked;
        private void OnDoneClicked()
        {
            if (DoneClicked != null)
            {
                DoneClicked(this, new RoutedEventArgs());
            }
        }

        private FrameworkElement rootElement = null;

        private Canvas canvasHSV;
        private Rectangle rectangleRootHSV;
        private GradientStop gradientStopHSVColor;
        private Rectangle rectangleHSV;
        private Ellipse ellipseHSV;

        private Slider sliderHSV;

        private Slider sliderA;
        private GradientStop gradientStopA0;
        private GradientStop gradientStopA1;

        private Slider sliderR;
        private GradientStop gradientStopR0;
        private GradientStop gradientStopR1;

        private Slider sliderG;
        private GradientStop gradientStopG0;
        private GradientStop gradientStopG1;

        private Slider sliderB;
        private GradientStop gradientStopB0;
        private GradientStop gradientStopB1;

        private TextBox textBoxA;
        private TextBox textBoxR;
        private TextBox textBoxG;
        private TextBox textBoxB;

        private ComboBox comboBoxColor;
        private Rectangle rectangleColor;
        private SolidColorBrush brushColor;
        private TextBox textBoxColor;

        private Button buttonDone;

        public override void OnApplyTemplate()
        {            
            base.OnApplyTemplate();

            rootElement = (FrameworkElement)GetTemplateChild("RootElement");

            canvasHSV = (Canvas)GetTemplateChild("CanvasHSV");
            rectangleRootHSV = (Rectangle)GetTemplateChild("RectangleRootHSV");
            gradientStopHSVColor = (GradientStop)GetTemplateChild("GradientStopHSVColor");
            rectangleHSV = (Rectangle)GetTemplateChild("RectangleHSV");
            ellipseHSV = (Ellipse)GetTemplateChild("EllipseHSV");
            sliderHSV = (Slider)GetTemplateChild("SliderHSV");

            sliderA = (Slider)GetTemplateChild("SliderA");
            gradientStopA0 = (GradientStop)GetTemplateChild("GradientStopA0");
            gradientStopA1 = (GradientStop)GetTemplateChild("GradientStopA1");
            sliderR = (Slider)GetTemplateChild("SliderR");
            gradientStopR0 = (GradientStop)GetTemplateChild("GradientStopR0");
            gradientStopR1 = (GradientStop)GetTemplateChild("GradientStopR1");
            sliderG = (Slider)GetTemplateChild("SliderG");
            gradientStopG0 = (GradientStop)GetTemplateChild("GradientStopG0");
            gradientStopG1 = (GradientStop)GetTemplateChild("GradientStopG1");
            sliderB = (Slider)GetTemplateChild("SliderB");
            gradientStopB0 = (GradientStop)GetTemplateChild("GradientStopB0");
            gradientStopB1 = (GradientStop)GetTemplateChild("GradientStopB1");

            textBoxA = (TextBox)GetTemplateChild("TextBoxA");
            textBoxR = (TextBox)GetTemplateChild("TextBoxR");
            textBoxG = (TextBox)GetTemplateChild("TextBoxG");
            textBoxB = (TextBox)GetTemplateChild("TextBoxB");

            comboBoxColor = (ComboBox)GetTemplateChild("ComboBoxColor");
            rectangleColor = (Rectangle)GetTemplateChild("RectangleColor");
            brushColor = (SolidColorBrush)GetTemplateChild("BrushColor");
            textBoxColor = (TextBox)GetTemplateChild("TextBoxColor");
            buttonDone = (Button)GetTemplateChild("ButtonDone");

            rectangleHSV.MouseLeftButtonDown += new MouseButtonEventHandler(HSV_MouseLeftButtonDown);
            rectangleHSV.MouseMove += new MouseEventHandler(HSV_MouseMove);
            rectangleHSV.MouseLeftButtonUp += new MouseButtonEventHandler(HSV_MouseLeftButtonUp);
            rectangleHSV.MouseLeave += new MouseEventHandler(HSV_MouseLeave);

            sliderHSV.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderHSV_ValueChanged);

            sliderA.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderA_ValueChanged);
            sliderR.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderR_ValueChanged);
            sliderG.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderG_ValueChanged);
            sliderB.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderB_ValueChanged);

            textBoxA.LostFocus += new RoutedEventHandler(textBoxA_LostFocus);
            textBoxR.LostFocus += new RoutedEventHandler(textBoxR_LostFocus);
            textBoxG.LostFocus += new RoutedEventHandler(textBoxG_LostFocus);
            textBoxB.LostFocus += new RoutedEventHandler(textBoxB_LostFocus);

            comboBoxColor.SelectionChanged += new SelectionChangedEventHandler(comboBoxColor_SelectionChanged);
            textBoxColor.GotFocus += new RoutedEventHandler(textBoxColor_GotFocus);
            textBoxColor.LostFocus += new RoutedEventHandler(textBoxColor_LostFocus);
            buttonDone.Click += new RoutedEventHandler(buttonDone_Click);

            InitializePredefined();
            UpdateControls(Color, true, true, true);
        }

        private Dictionary<Color, PredefinedColorItem> dictionaryColor;
        private void InitializePredefined()
        {
            if (dictionaryColor != null)
            {
                return;
            }

            List<PredefinedColor> list = PredefinedColor.All;
            dictionaryColor = new Dictionary<Color, PredefinedColorItem>();
            foreach (PredefinedColor color in list)
            {
                PredefinedColorItem item = new PredefinedColorItem(color.Value, color.Name);
                comboBoxColor.Items.Add(item);

                if (!dictionaryColor.ContainsKey(color.Value))
                {
                    dictionaryColor.Add(color.Value, item);
                }
            }
        }

        private bool trackingHSV;
        private void HSV_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            trackingHSV = rectangleHSV.CaptureMouse();

            Point point = e.GetPosition(rectangleHSV);

            Size size = ellipseHSV.RenderSize;

            ellipseHSV.SetValue(Canvas.LeftProperty, point.X - ellipseHSV.ActualWidth / 2);
            ellipseHSV.SetValue(Canvas.TopProperty, point.Y - ellipseHSV.ActualHeight / 2);

            if (Updating)
            {
                return;
            }

            Color color = GetHSVColor();
            UpdateControls(color, false, true, true);
        }
        private void HSV_MouseMove(object sender, MouseEventArgs e)
        {
            if (trackingHSV)
            {
                Point point = e.GetPosition(rectangleHSV);
                Size size = ellipseHSV.RenderSize;

                ellipseHSV.SetValue(Canvas.LeftProperty, point.X - ellipseHSV.ActualWidth / 2);
                ellipseHSV.SetValue(Canvas.TopProperty, point.Y - ellipseHSV.ActualHeight / 2);

                if (Updating)
                {
                    return;
                }

                Color color = GetHSVColor();
                UpdateControls(color, false, true, true);
            }
        }
        private void HSV_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            trackingHSV = false;
            rectangleHSV.ReleaseMouseCapture();
        }
        private void HSV_MouseLeave(object sender, MouseEventArgs e)
        {
            trackingHSV = false;
            rectangleHSV.ReleaseMouseCapture();
        }

        private void sliderHSV_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Updating)
            {
                return;
            }

            gradientStopHSVColor.Color = ColorHelper.HSV2RGB(e.NewValue, 1d, 1d);

            Color color = GetHSVColor();
            UpdateControls(color, false, true, true);
        }

        private Color GetHSVColor()
        {
            double h = sliderHSV.Value;

            double x = (double)ellipseHSV.GetValue(Canvas.LeftProperty) + ellipseHSV.ActualWidth / 2;
            double y = (double)ellipseHSV.GetValue(Canvas.TopProperty) + ellipseHSV.ActualHeight / 2;

            double s = x / (rectangleHSV.ActualWidth - 1);
            if (s < 0d)
                s = 0d;
            else if (s > 1d)
                s = 1d;

            double v = 1 - y / (rectangleHSV.ActualHeight - 1);
            if (v < 0d)
                v = 0d;
            else if (v > 1d)
                v = 1d;

            return ColorHelper.HSV2RGB(h, s, v);
        }

        private void sliderA_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }
        private void sliderR_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }
        private void sliderG_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }
        private void sliderB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Updating)
            {
                return;
            }

            Color color = GetRGBColor();
            UpdateControls(color, true, true, true);
        }

        private Color GetRGBColor()
        {
            byte a = (byte)sliderA.Value;
            byte r = (byte)sliderR.Value;
            byte g = (byte)sliderG.Value;
            byte b = (byte)sliderB.Value;

            return Color.FromArgb(a, r, g, b);
        }

        private void textBoxA_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(textBoxA.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                sliderA.Value = value;
            }
        }
        private void textBoxR_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(textBoxR.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                sliderR.Value = value;
            }
        }
        private void textBoxG_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(textBoxG.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                sliderG.Value = value;
            }
        }
        private void textBoxB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            int value = 0;
            if (int.TryParse(textBoxB.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                sliderB.Value = value;
            }
        }

        private void UpdateControls(Color color, bool hsv, bool rgb, bool predifined)
        {
            if (Updating)
            {
                return;
            }

            try
            {
                BeginUpdate();

                // HSV
                if (hsv)
                {
                    double h = ColorHelper.GetHSV_H(color);
                    double s = ColorHelper.GetHSV_S(color);
                    double v = ColorHelper.GetHSV_V(color);

                    sliderHSV.Value = h;
                    gradientStopHSVColor.Color = ColorHelper.HSV2RGB(h, 1d, 1d);

                    double x = s * (rectangleHSV.ActualWidth - 1);
                    double y = (1 - v) * (rectangleHSV.ActualHeight - 1);

                    ellipseHSV.SetValue(Canvas.LeftProperty, x - ellipseHSV.ActualWidth / 2);
                    ellipseHSV.SetValue(Canvas.TopProperty, y - ellipseHSV.ActualHeight / 2);
                }

                if (rgb)
                {
                    byte a = color.A;
                    byte r = color.R;
                    byte g = color.G;
                    byte b = color.B;

                    sliderA.Value = a;
                    gradientStopA0.Color = Color.FromArgb(0, r, g, b);
                    gradientStopA1.Color = Color.FromArgb(255, r, g, b);
                    textBoxA.Text = a.ToString("X2");

                    sliderR.Value = r;
                    gradientStopR0.Color = Color.FromArgb(255, 0, g, b);
                    gradientStopR1.Color = Color.FromArgb(255, 255, g, b);
                    textBoxR.Text = r.ToString("X2");

                    sliderG.Value = g;
                    gradientStopG0.Color = Color.FromArgb(255, r, 0, b);
                    gradientStopG1.Color = Color.FromArgb(255, r, 255, b);
                    textBoxG.Text = g.ToString("X2");

                    sliderB.Value = b;
                    gradientStopB0.Color = Color.FromArgb(255, r, g, 0);
                    gradientStopB1.Color = Color.FromArgb(255, r, g, 255);
                    textBoxB.Text = b.ToString("X2");
                }

                if (predifined)
                {
                    brushColor.Color = color;
                    if (dictionaryColor.ContainsKey(color))
                    {
                        comboBoxColor.SelectedItem = dictionaryColor[color];
                        textBoxColor.Text = "";
                    }
                    else
                    {
                        comboBoxColor.SelectedItem = null;
                        textBoxColor.Text = color.ToString();
                    }
                }

                Color = color;
            }
            finally
            {
                EndUpdate();
            }
        }

        private void comboBoxColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            PredefinedColorItem coloritem = comboBoxColor.SelectedItem as PredefinedColorItem;
            if (coloritem != null)
            {
                Color = coloritem.Color;
            }
        }
        private void textBoxColor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            try
            {
                BeginUpdate();

                comboBoxColor.SelectedItem = null;
                textBoxColor.Text = Color.ToString();
            }
            finally
            {
                EndUpdate();
            }
        }
        private void textBoxColor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Updating)
            {
                return;
            }

            string text = textBoxColor.Text.TrimStart('#');
            uint value = 0;
            if (uint.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
            {
                byte b = (byte)(value & 0xFF);
                value >>= 8;
                byte g = (byte)(value & 0xFF);
                value >>= 8;
                byte r = (byte)(value & 0xFF);
                value >>= 8;
                byte a = (byte)(value & 0xFF);

                if (text.Length <= 6)
                {
                    a = 0xFF;
                }

                Color color = Color.FromArgb(a, r, g, b);
                Color = color;
            }
            else
            {
                Color = Colors.White;
            }
        }
        private void buttonDone_Click(object sender, RoutedEventArgs e)
        {
            OnDoneClicked();
        }

        private int isUpdating;
        private bool Updating
        {
            get
            {
                return isUpdating != 0;
            }
        }
        private void BeginUpdate()
        {
            isUpdating++;
        }
        private void EndUpdate()
        {
            isUpdating--;
        }
    }
}
