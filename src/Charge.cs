using System.Drawing.Drawing2D;

namespace UPG_SP_2024
{
    public class Charge
    {
        private double[] position;
        private double charge;
        private double[] coord;
        /// <summary>
        /// Initialize a new instance of the charge with constant size.
        /// </summary>
        /// <param name="position"> Position of the charge (not scaled) </param>
        /// <param name="charge"> The value of the charge </param>
        public Charge(double[] position, int charge) {
            this.position = position;
            this.charge = charge;
            coord = new double[2];
        }

        /// <summary>
        /// Initialize a new instance of the charge with changable size.
        /// </summary>
        /// <param name="position"> Position in the field </param>
        /// <param name="elapsed"> Time scalling variable </param>
        /// <param name="type"> Decise if the calculation for positive or for negative charge will be used </param>
        public Charge(double[] position, float elapsed, int type)
        {
            this.position = position;
            double var = Math.PI / 2;
            coord = new double[2];
            if (type == 1)
            {
                this.charge = (float)(1 + 0.5 * Math.Sin(var * elapsed));
            }
            else if(type == 2)
            {
                this.charge = (float)(1 - 0.5 * Math.Sin(var * elapsed));
            }
        }

        /// <summary>
        /// Returns position of the charge.
        /// </summary>
        /// <returns> position as int[] array </returns>
        public double[] GetCoord()
        {
            return coord;
        }

        /// <summary>
        /// return charge of the charge.
        /// </summary>
        /// <returns> charge as a float number </returns>
        public double GetCharge()
        {
            return charge;
        }

        /// <summary>
        /// Sets the position of charge to a new position.
        /// </summary>
        /// <param name="position"> Position to which is the charge suppose to be set. </param>
        public void SetCoord(double[] position) {
            this.coord = position;
        }

        /// <summary>
        /// Draws the charge into the drawingPanel
        /// </summary>
        /// <param name="g"> Graphics context </param>
        /// <param name="scale"> Scalling variable </param>
        public void Add(Graphics g, float scale)
        {
            // Calculating size and position of the charge.
            double size = Math.Abs(GetCharge() * 20 * scale);
            coord = [position[0] * 50 * scale, -position[1] * 50 * scale];

            // Creating path for the charge.
            GraphicsPath chargePath = new GraphicsPath();
            chargePath.AddEllipse((float)(coord[0] - size / 2), (float)(coord[1] - size / 2), (float)size, (float)size);

            PathGradientBrush brush = new PathGradientBrush(chargePath);
            brush.CenterPoint = new PointF((float)coord[0], (float)coord[1]);
            
            // Determines of the charge is suppose to use blue or red color.
            if (charge > 0)
            {
                brush.CenterColor = Color.Red;
                brush.InterpolationColors = new ColorBlend()
                {
                    Colors = [Color.FromArgb(244, 255, 0), 
                        Color.FromArgb(244, 200, 55), 
                        Color.FromArgb(255, 150, 55), 
                        Color.FromArgb(255, 100, 55), 
                        Color.FromArgb(255, Color.Red), 
                        Color.FromArgb(255, Color.Red)],
                    Positions = [0f, 0.04f, 0.08f, 0.1f, 0.12f, 1f]
                };
            }
            else
            {
                brush.CenterColor = Color.Blue;
                brush.InterpolationColors = new ColorBlend()
                {
                    Colors = [Color.FromArgb(244, 255, 0),
                        Color.FromArgb(144, 200, 255),
                        Color.FromArgb(55, 150, 255),
                        Color.FromArgb(55, 100, 255), 
                        Color.FromArgb(255, Color.Blue), 
                        Color.FromArgb(255, Color.Blue)],
                    Positions = [0f, 0.04f, 0.08f, 0.1f, 0.12f, 1f]
                };
            }

            // Creating region and inserting the chargepath inside.
            var ChargeRegion = new Region(chargePath);

            string ch = Convert.ToString(GetCharge().ToString("0.##")) + " charge";
            Font f = new("Arial", 0.04f * (float)size * scale);

            g.FillRegion(brush, ChargeRegion);
            float coord_X = (float)(coord[0] - g.MeasureString(ch, f).Width / 2);
            float coord_Y = (float)(coord[1] - g.MeasureString(ch, f).Height / 2);
            g.DrawString(ch, f, Brushes.Black, coord_X , coord_Y);
        }
    }
}
