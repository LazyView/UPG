namespace ElectricFieldVis
{
    public class Grid
    {
        private float[] gridPosition;


        public Grid() { }
        /// <summary>
        /// Draws a grid in the background using DrawLine function.
        /// </summary>
        /// <param name="g"> Graphics context </param>
        /// <param name="x"> Width gap in between lines </param>
        /// <param name="y"> Height gap in between lines</param>
        /// <param name="width"> Width of the drawingPanel </param>
        /// <param name="height"> Height of the drawingPanel</param>
        public void BackgroundGrid(Graphics g, int x, int y, int width, int height, float scale)
        {
            var s = g.Transform;
            Pen p = new Pen(Brushes.Gray, 1f);
            g.TranslateTransform(-width/2, -height/2);
            
            for(int i = 0;i < height; i = i + x)
            {
                g.DrawLine(p, 0, i, width, i);

                for(int j = 0;j < width; j = j + y)
                {
                    g.DrawLine(p, j, 0, j, height);
                }
            }
            g.Transform = s;
        }

        public double Calculate_intensity_static(double x, double y, float[] charges, int[,] positions, float elapsed, int width, int height)
        {
            double X = (x + width/2) / width;
            double Y = (y + height/2) / height;

            // Defining size of vector.
            gridPosition = new float[4] { (float)X, (float)Y, 0, 0 };
            double epsilonZero = 8.854e-12;
            double firstPart = (1 / (4 * Math.PI * epsilonZero));
            double[] coordXY = new double[2] { 0, 0 };

            for (int i = 0; i < charges.Length; i++)
            {
                double chargeX = positions[i, 0];
                double chargeY = -positions[i, 1];
                double nX = X - chargeX;
                double nY = Y - chargeY;
                double magnitude = Math.Pow(nX * nX + nY * nY, 1.5);
                coordXY[0] += (charges[i] * (nX / magnitude));
                coordXY[1] += (charges[i] * (nY / magnitude));
            }
            // Vector of intensity.
            double[] coulombXY = new double[2] { coordXY[0] * firstPart, coordXY[1] * firstPart };

            // Calculation of magnitude.
            double nMagnitude = Math.Sqrt(coulombXY[0] * coulombXY[0] + coulombXY[1] * coulombXY[1]) * 3;

            // Assigning final coordinates of vector.
            gridPosition[0] = (float)x;
            gridPosition[1] = (float)y;
            gridPosition[2] = (float)(X + (coulombXY[0] / nMagnitude));
            gridPosition[3] = (float)(Y + (coulombXY[1] / nMagnitude));

            return nMagnitude;
        }

        public void addGridVector(Graphics g, float gapX, float gapY, float scale, double width, double height)
        {
            // Scaling the coordinates.
            gridPosition[0] -= gapX / 2;
            gridPosition[1] -= gapY / 2;
            gridPosition[2] -= gapX / 2;
            gridPosition[3] -= gapY / 2;

            // Calculate the direction vector and its length.
            double u_x = gridPosition[2] - gridPosition[0];
            double u_y = gridPosition[3] - gridPosition[1];
            double length = (float)5;

            // Normalize the direction vector if length is greater than zero.
            if (length > 0)
            {
                u_x /= length;
                u_y /= length;
            }
            else
            {
                // If length is zero, default direction (arbitrary).
                u_x = 1;
                u_y = 0;
            }

            // Calculate length and position of the arrowhead.
            double tipLength = 2f * scale; // Length of the arrowhead.
            double tipAngle = (double)(Math.PI / 6); // 30 degrees for arrowhead angle.

            // Position of the arrow shaft end.
            float c_x = gridPosition[2];
            float c_y = gridPosition[3];

            // Calculate the points of the arrowhead.
            double angle1 = Math.Atan2(u_y, u_x) + tipAngle; // Right side of the arrowhead.
            double angle2 = Math.Atan2(u_y, u_x) - tipAngle; // Left side of the arrowhead.

            float e_x1 = (float)(c_x - tipLength * Math.Cos(angle1));
            float e_y1 = (float)(c_y - tipLength * Math.Sin(angle1));
            float e_x2 = (float)(c_x - tipLength * Math.Cos(angle2));
            float e_y2 = (float)(c_y - tipLength * Math.Sin(angle2));
         

            // Drawing the probe, arrow and description.
            g.DrawLine(Pens.Black, gridPosition[0], gridPosition[1], gridPosition[2], gridPosition[3]); // Draw the arrow line.
            g.DrawLine(Pens.Black, c_x, c_y, e_x1, e_y1); // Right side of the arrowhead.
            g.DrawLine(Pens.Black, c_x, c_y, e_x2, e_y2); // Left side of the arrowhead.
        }

        public Color GetColorForIntensity(double intensity, double maxIntensity)
        {
            // Normalize the intensity to a value between 0 and 1.
            double normalized = intensity / maxIntensity;
            int r = (int)(normalized * 255);
            int g = (int)((1 - normalized) * 255);
            return Color.FromArgb(r, g, 0); // Gradient from red to green.
        }

        public void DrawHeatMap(Graphics g, int width, int height, int gridGapX, int gridGapY, float[] charges, int[,] positions, float elapsed)
        {
            double maxIntensity = 0;
            double intensity1 = 100;
            // First pass: determine the maximum intensity
            for (int i = 0; i < height; i += gridGapY)
            {
                for (int j = 0; j < width; j += gridGapX)
                {
                    intensity1 = Calculate_intensity_static(j / (double)gridGapX, i / (double)gridGapY, charges, positions, elapsed, width, height);
                    if (intensity1 > maxIntensity)
                    {
                        maxIntensity = intensity1;
                    }
                }
            }

            // Second pass: draw the heat map
            for (int i = 0; i < height; i += gridGapY)
            {
                for (int j = 0; j < width; j += gridGapX)
                {
                    intensity1 = Calculate_intensity_static(j / (double)gridGapX, i / (double)gridGapY, charges, positions, elapsed, width, height);
                    Color color = GetColorForIntensity(intensity1, maxIntensity);
                    using (Brush brush = new SolidBrush(color))
                    {
                        g.FillRectangle(brush, j, i, gridGapX, gridGapY);
                    }
                }
            }
        }
    }
}
