namespace ElectricFieldVis
{
    internal class Probe
    {       
        //coordinates of the vector.
        private float[] vectorPosition;


        /// <summary>Initializes a new instance of the <see cref="Probe" /> class.</summary>
        public Probe() { }

        /*
         * Method to calculate the intensity of vector field using Coulumb's law.
         */
        public double Calculate_Intensity(double[] charges, double[,] positions, float elapsed)
        {
            // Rotation around a circle at the speed of PI/6.
            double angularSpeed = MathF.PI / 6f;
            double X = Math.Cos(angularSpeed * elapsed);
            double Y = -Math.Sin(angularSpeed * elapsed);

            // Defining size of vector.
            vectorPosition = new float[4] { 0,0,0,0 };
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
            double nMagnitude = Math.Sqrt(coulombXY[0] * coulombXY[0] + coulombXY[1] * coulombXY[1]);

            // Assigning final coordinates of vector.
            vectorPosition[0] = (float)X;
            vectorPosition[1] = (float)Y;
            vectorPosition[2] = (float)(X + (coulombXY[0] / nMagnitude));
            vectorPosition[3] = (float)(Y + (coulombXY[1] / nMagnitude));

            return nMagnitude;
           
        }


        /*
         * Method to draw the vector and it's descriptiom.
         */
        public void AddVector(Graphics g, float scale, double result)
        {
            // Scaling the coordinates.
            vectorPosition[0] *= (50 * scale);
            vectorPosition[1] *= (50 * scale);
            vectorPosition[2] *= (50 * scale);
            vectorPosition[3] *= (50 * scale);

            // Calculate the direction vector and its length.
            double u_x = vectorPosition[2] - vectorPosition[0];
            double u_y = vectorPosition[3] - vectorPosition[1];
            double length = (float)Math.Sqrt(u_x * u_x + u_y * u_y);

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
            double tipLength = 5f * scale; // Length of the arrowhead.
            double tipAngle = (double)(Math.PI / 6); // 30 degrees for arrowhead angle.

            // Position of the arrow shaft end.
            float c_x = vectorPosition[2];
            float c_y = vectorPosition[3];

            // Calculate the points of the arrowhead.
            double angle1 = Math.Atan2(u_y, u_x) + tipAngle; // Right side of the arrowhead.
            double angle2 = Math.Atan2(u_y, u_x) - tipAngle; // Left side of the arrowhead.

            float e_x1 = (float)(c_x - tipLength * Math.Cos(angle1));
            float e_y1 = (float)(c_y - tipLength * Math.Sin(angle1));
            float e_x2 = (float)(c_x - tipLength * Math.Cos(angle2));
            float e_y2 = (float)(c_y - tipLength * Math.Sin(angle2));

            // Font and position of vector description.
            Font f = new Font("Arial", 4f * scale);
            string resultStr = "|E| = " + Math.Round(result/1000000000, 2) + " GN/C";
            float stringX = vectorPosition[0] - g.MeasureString(resultStr, f).Width / 2;
            float stringY = vectorPosition[1] - g.MeasureString(resultStr, f).Height;
            
            // Assign size of the probe.
            float size = 2f * scale;

            // Drawing the probe, arrow and description.
            g.DrawLine(Pens.Black, vectorPosition[0], vectorPosition[1], vectorPosition[2], vectorPosition[3]); // Draw the arrow line.
            g.DrawLine(Pens.Black, c_x, c_y, e_x1, e_y1); // Right side of the arrowhead.
            g.DrawLine(Pens.Black, c_x, c_y, e_x2, e_y2); // Left side of the arrowhead.
            g.FillEllipse(Brushes.Black, (vectorPosition[0] - size / 2), (vectorPosition[1] - size / 2), size, size);
            g.DrawString(resultStr, f, Brushes.Black, stringX, stringY);
        }
    }

}
