using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Shape selectedShape;
        Polygon selectedPolygon;
        PointCollection points;
        Point NewPoint;
        double[,] TranslationMatrix;
        double[,] RotateMatrix;
        double[,] ScaleMatrix;

        #region zad1

        private void MoveButtonClicked7(object sender, RoutedEventArgs e)
        {
            PolygonButton7.IsEnabled = true;
            MoveButton7.IsEnabled = false;
            SelectButton7.IsEnabled = true;
            RotateButton7.IsEnabled = true;
            ScaleButton7.IsEnabled = true;
        }

        private void SaveButtonClicked7(object sender, RoutedEventArgs e)
        {
            PolygonButton7.IsEnabled = true;

            MoveButton7.IsEnabled = true;
            RotateButton7.IsEnabled = true;
            ScaleButton7.IsEnabled = true;

            SelectButton7.IsEnabled = true;

            SaveFileDialog fileDialog = new()
            {
                Filter = "XML File | *.xml"
            };

            bool? result = fileDialog.ShowDialog();

            if (result is not true)
                return;

            string fileName = fileDialog.FileName;
            if (selectedShape!=null)
            {
                string xml = XamlWriter.Save(selectedShape);

                using StreamWriter writer = new(fileName, false);
                writer.Write(xml);
            }
        }

        private void LoadButtonClicked7(object sender, RoutedEventArgs e)
        {
            PolygonButton7.IsEnabled = true;

            MoveButton7.IsEnabled = true;
            RotateButton7.IsEnabled = true;
            ScaleButton7.IsEnabled = true;

            OpenFileDialog dialog = new()
            {
                Filter = "XML File | *.xml"
            };
            ;
            bool? result = dialog.ShowDialog();

            if (result is not true) return;

            string fileName = dialog.FileName;

            using FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);
            Object reader = XamlReader.Load(fs);

            MyCanvas7.Children.Add((UIElement)reader);
        }

        private void SelectButtonClicked7(object sender, RoutedEventArgs e)
        {
            PolygonButton7.IsEnabled = true;

            MoveButton7.IsEnabled = true;
            SelectButton7.IsEnabled = false;
            RotateButton7.IsEnabled = true;
            ScaleButton7.IsEnabled = true;
        }

        private void Canva7MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!PolygonButton7.IsEnabled)
            {
                if(points.Count > 2)
                {
                    selectedPolygon.Points = points;
                    MyCanvas7.Children.Remove(selectedPolygon);
                    MyCanvas7.Children.Add(selectedPolygon);
                }
            }
        }

        private void Canva7MouseDown(object sender, MouseButtonEventArgs e)
        {

            NewPoint = e.GetPosition((UIElement)sender);

            if(!PolygonButton7.IsEnabled)
            {
                points.Add(NewPoint);
            }

            if (!SelectButton7.IsEnabled)
            {
                selectedShape = (Shape)e.OriginalSource;
            }

            if (!MoveButton7.IsEnabled)
            {
                if (e.OriginalSource is Polygon)
                {
                    GetSelectedFigure((Polygon)e.OriginalSource);
                }
            }
        }

        private void GetSelectedFigure(Shape figure)
        {
            if (!SelectButton7.IsEnabled)
            {
                selectedShape = figure;
            }
        }

        private void TextChanged7(object sender, TextChangedEventArgs e)
        {
            if (!MoveButton7.IsEnabled)
            {
                if (selectedPolygon!=null && LineX.Text != "" && LineY.Text!="")
                {
                    PointCollection NewPoints = new();

                    for (int i = 0; i < points.Count; i++)
                    {
                        NewPoints.Add(new Point(points[i].X +  Convert.ToDouble(LineX.Text), points[i].Y +  Convert.ToDouble(LineY.Text)));
                    }
                    points = NewPoints;
                    selectedPolygon.Points = NewPoints;
                }
            }

            if (!RotateButton7.IsEnabled)
            {
                if (selectedPolygon!=null && RAngle.Text != "" && RPointX.Text!="" && RPointY.Text!="")
                {
                    PointCollection NewPoints = new();

                    for (int i = 0; i < points.Count; i++)
                    {
                        double Xr = Convert.ToDouble(RPointX.Text);
                        double Yr = Convert.ToDouble(RPointY.Text);
                        double nAngle = (Convert.ToDouble(RAngle.Text) * Math.PI) / 180;

                        double Xnew = Xr + (points[i].X - Xr) * Math.Cos(nAngle) - (points[i].Y - Yr) * Math.Sin(nAngle);
                        double Ynew = Yr + (points[i].X - Xr) * Math.Sin(nAngle) + (points[i].Y - Yr) * Math.Cos(nAngle);

                        NewPoints.Add(new Point(Xnew, Ynew));
                    }
                    points = NewPoints;
                    selectedPolygon.Points = NewPoints;
                }
            }
            if (!ScaleButton7.IsEnabled)
            {
                if (selectedPolygon!=null && ScaleX.Text != "" && ScaleY.Text!="" && SPointX.Text!="" && SPointY.Text!="")
                {
                    PointCollection NewPoints = new();

                    for (int i = 0; i < points.Count; i++)
                    {
                        double Xf = Convert.ToDouble(SPointX.Text);
                        double Yf = Convert.ToDouble(SPointY.Text);
                        double Sx = Convert.ToDouble(ScaleX.Text);
                        double Sy = Convert.ToDouble(ScaleY.Text);

                        double Xnew = Xf + (points[i].X - Xf) * Sx;
                        double Ynew = Yf + (points[i].Y - Yf) * Sy;

                        NewPoints.Add(new Point(Xnew, Ynew));
                    }
                    points = NewPoints;
                    selectedPolygon.Points = NewPoints;
                }
            }
        }

        private void RotateButtonClicked7(object sender, RoutedEventArgs e)
        {
            PolygonButton7.IsEnabled = true;


            MoveButton7.IsEnabled = true;
            SelectButton7.IsEnabled = true;
            RotateButton7.IsEnabled = false;
            ScaleButton7.IsEnabled = true;
        }

        private void ScaleButtonClicked7(object sender, RoutedEventArgs e)
        {
            PolygonButton7.IsEnabled = true;

            MoveButton7.IsEnabled = true;
            SelectButton7.IsEnabled = true;
            RotateButton7.IsEnabled = true;
            ScaleButton7.IsEnabled = false;

        }

        private void PolygonButtonClicked7(object sender, RoutedEventArgs e)
        {
            PolygonButton7.IsEnabled = false;
            points = new();
            selectedPolygon = new();
            selectedPolygon.Stroke = System.Windows.Media.Brushes.Black;
            selectedPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
            selectedPolygon.StrokeThickness = 2;
            selectedPolygon.HorizontalAlignment = HorizontalAlignment.Left;
            selectedPolygon.VerticalAlignment = VerticalAlignment.Center;


            MoveButton7.IsEnabled = true;
            SelectButton7.IsEnabled = true;
            RotateButton7.IsEnabled = true;
            ScaleButton7.IsEnabled = true;
        }

        #endregion

        private void MoveButtonClicked2(object sender, RoutedEventArgs e)
        {
            PolygonButton2.IsEnabled = true;
            MoveButton2.IsEnabled = false;
            SelectButton2.IsEnabled = true;
            RotateButton2.IsEnabled = true;
            ScaleButton2.IsEnabled = true;
        }

        private void SaveButtonClicked2(object sender, RoutedEventArgs e)
        {
            PolygonButton2.IsEnabled = true;

            MoveButton2.IsEnabled = true;
            RotateButton2.IsEnabled = true;
            ScaleButton2.IsEnabled = true;

            SelectButton2.IsEnabled = true;

            SaveFileDialog fileDialog = new()
            {
                Filter = "XML File | *.xml"
            };

            bool? result = fileDialog.ShowDialog();

            if (result is not true)
                return;

            string fileName = fileDialog.FileName;
            if (selectedShape!=null)
            {
                string xml = XamlWriter.Save(selectedShape);

                using StreamWriter writer = new(fileName, false);
                writer.Write(xml);
            }
        }

        private void LoadButtonClicked2(object sender, RoutedEventArgs e)
        {
            PolygonButton2.IsEnabled = true;

            MoveButton2.IsEnabled = true;
            RotateButton2.IsEnabled = true;
            ScaleButton2.IsEnabled = true;

            OpenFileDialog dialog = new()
            {
                Filter = "XML File | *.xml"
            };
            ;
            bool? result = dialog.ShowDialog();

            if (result is not true) return;

            string fileName = dialog.FileName;

            using FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);
            Object reader = XamlReader.Load(fs);

            MyCanvas2.Children.Add((UIElement)reader);
        }

        private void SelectButtonClicked2(object sender, RoutedEventArgs e)
        {
            PolygonButton2.IsEnabled = true;

            MoveButton2.IsEnabled = true;
            SelectButton2.IsEnabled = false;
            RotateButton2.IsEnabled = true;
            ScaleButton2.IsEnabled = true;
        }

        private void Canva2MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!PolygonButton2.IsEnabled)
            {
                if (points.Count > 2)
                {
                    selectedPolygon.Points = points;
                    MyCanvas2.Children.Remove(selectedPolygon);
                    MyCanvas2.Children.Add(selectedPolygon);
                }
            }
        }

        private void Canva2MouseDown(object sender, MouseButtonEventArgs e)
        {

            NewPoint = e.GetPosition((UIElement)sender);

            if (!PolygonButton2.IsEnabled)
            {
                points.Add(NewPoint);
            }

            if (!SelectButton2.IsEnabled)
            {
                selectedShape = (Shape)e.OriginalSource;
            }

            if (!MoveButton2.IsEnabled)
            {
                LineX2.Text = Math.Round(NewPoint.X).ToString();
                LineY2.Text = Math.Round(NewPoint.Y).ToString();
            }

            if (!RotateButton2.IsEnabled)
            {
                RPointX2.Text = Math.Round(NewPoint.X).ToString();
                RPointY2.Text = Math.Round(NewPoint.Y).ToString();
            }
        }

        public double[] MultiplyMatrix(double[,] ChangeMatrix, double[] PointMatrix)
        {
            double[] ResultMatrix = new double[3];

            for (int k = 0; k<3; k++)
            {
                double res = 0;
                for (int l = 0; l< 3; l++)
                {
                    res += ChangeMatrix[k, l] * PointMatrix[l];
                }
                ResultMatrix[k] = res;
            }

            return ResultMatrix;
        }

        public double[,] MultiplyMatrix3x3(double[,] ChangeMatrix, double[,] PointMatrix)
        {
            double[,] ResultMatrix = new double[3, 3];
            int rA = ChangeMatrix.GetLength(0);
            int cA = ChangeMatrix.GetLength(1);
            int rB = PointMatrix.GetLength(0);
            int cB = PointMatrix.GetLength(1);


            double temp = 0;

            for (int i = 0; i < rA; i++)
            {
                for (int j = 0; j < cB; j++)
                {
                    temp = 0;
                    for (int k = 0; k < cA; k++)
                    {
                        temp += ChangeMatrix[i, k] * PointMatrix[k, j];
                    }
                    ResultMatrix[i, j] = temp;
                }
            }

            return ResultMatrix;
        }
        

        private void TextChanged2(object sender, TextChangedEventArgs e)
        {
            if (!MoveButton2.IsEnabled)
            {
                if (selectedPolygon!=null && LineX2.Text != "" && LineY2.Text!="")
                {
                    PointCollection NewPoints = new();

                    TranslationMatrix = new double[,] {
                        {1, 0, Convert.ToDouble(LineX2.Text)},
                        {0,1, Convert.ToDouble(LineY2.Text) },
                        {0,0,1}
                    };

                    double[] ResultMatrix = new double[3];

                    for (int i = 0; i < points.Count; i++)
                    {
                        Point ToAdd = new();
                        double[] PointMatrix = new double[] {  points[i].X, points[i].Y, 1  };

                        ResultMatrix = MultiplyMatrix(TranslationMatrix, PointMatrix);

                        ToAdd.X = ResultMatrix[0] / ResultMatrix[2];
                        ToAdd.Y = ResultMatrix[1] / ResultMatrix[2];

                        NewPoints.Add(ToAdd);
                    }
                    points = NewPoints;
                    selectedPolygon.Points = NewPoints;
                }
            }

            if (!RotateButton2.IsEnabled)
            {
                if (selectedPolygon!=null && RAngle2.Text != "" && RPointX2.Text!="" && RPointY2.Text!="")
                {
                    PointCollection NewPoints = new();

                    double nAngle = (Convert.ToDouble(RAngle2.Text) * Math.PI) / 180;
                  
                    RotateMatrix = new double[,] {
                        {Math.Cos(nAngle), -Math.Sin(nAngle), 0},
                        {Math.Sin(nAngle), Math.Cos(nAngle), 0},
                        {0,0,1}
                    };

                    double[] ResultMatrix = new double[20];
                    double[,] ResultMatrix2 = new double[3,3];

                    double Xr = Convert.ToDouble(RPointX2.Text);
                    double Yr = Convert.ToDouble(RPointY2.Text);

                    double[,] FirstMatrix = new double[,] {
                        {1, 0, -Xr},
                        {0, 1, -Yr},
                        {0,0,1}
                    };

                    double[,] ThirdMatrix = new double[,] {
                        {1, 0, Xr},
                        {0, 1, Yr},
                        {0,0,1}
                    };

                    for (int i = 0; i < points.Count; i++)
                    {
                        Point ToAdd = new();
                        double[] PointMatrix = new double[] { points[i].X, points[i].Y, 1 };

                        ResultMatrix2 =MultiplyMatrix3x3(MultiplyMatrix3x3(FirstMatrix, RotateMatrix), ThirdMatrix);

                        ResultMatrix = MultiplyMatrix(RotateMatrix, PointMatrix);

                        ToAdd.X = ResultMatrix[0] / ResultMatrix[2];
                        ToAdd.Y = ResultMatrix[1] / ResultMatrix[2];

                        NewPoints.Add(ToAdd);
                    }

                    points = NewPoints;
                    selectedPolygon.Points = NewPoints;
                }
            }
            if (!ScaleButton2.IsEnabled)
            {
                if (selectedPolygon!=null && ScaleX2.Text != "" && ScaleY2.Text!="" && SPointX2.Text!="" && SPointY2.Text!="")
                {
                    
                        PointCollection NewPoints = new();

                        ScaleMatrix = new double[,] {
                        {Convert.ToDouble(ScaleX2.Text), 0, 0},
                        {0,Convert.ToDouble(ScaleY2.Text), 0 },
                        {0,0,1}
                    };

                        double[] ResultMatrix = new double[3];

                        for (int i = 0; i < points.Count; i++)
                        {
                            Point ToAdd = new();
                            double[] PointMatrix = new double[] { points[i].X, points[i].Y, 1 };

                            ResultMatrix = MultiplyMatrix(ScaleMatrix, PointMatrix);

                            ToAdd.X = ResultMatrix[0] / ResultMatrix[2];
                            ToAdd.Y = ResultMatrix[1] / ResultMatrix[2];

                            NewPoints.Add(ToAdd);
                        }
                        points = NewPoints;
                        selectedPolygon.Points = NewPoints;
                }
            }
        }

        private void RotateButtonClicked2(object sender, RoutedEventArgs e)
        {
            PolygonButton2.IsEnabled = true;

            MoveButton2.IsEnabled = true;
            SelectButton2.IsEnabled = true;
            RotateButton2.IsEnabled = false;
            ScaleButton2.IsEnabled = true;
        }

        private void ScaleButtonClicked2(object sender, RoutedEventArgs e)
        {
            PolygonButton2.IsEnabled = true;

            MoveButton2.IsEnabled = true;
            SelectButton2.IsEnabled = true;
            RotateButton2.IsEnabled = true;
            ScaleButton2.IsEnabled = false;

        }

        private void PolygonButtonClicked2(object sender, RoutedEventArgs e)
        {
            PolygonButton2.IsEnabled = false;
            points = new();
            selectedPolygon = new();
            selectedPolygon.Stroke = System.Windows.Media.Brushes.Black;
            selectedPolygon.Fill = System.Windows.Media.Brushes.LightSeaGreen;
            selectedPolygon.StrokeThickness = 2;
            selectedPolygon.HorizontalAlignment = HorizontalAlignment.Left;
            selectedPolygon.VerticalAlignment = VerticalAlignment.Center;

            MoveButton2.IsEnabled = true;
            SelectButton2.IsEnabled = true;
            RotateButton2.IsEnabled = true;
            ScaleButton2.IsEnabled = true;
        }
    }
}
