using System.Drawing.Drawing2D;

namespace UPG_SP_2024
{
    public class Charge
    {
        double[] position;
        double charge;
        double[] coord;
        /// <summary>
        /// Initialize a new instance of the charge, which has constant size
        /// </summary>
        /// <param name="position"></param>
        /// <param name="charge"></param>
        public Charge(double[] position, int charge) {
            this.position = position;
            this.charge = charge;
        }
        /// <summary>
        /// Initialize a new instance of the charge with changable size
        /// </summary>
        /// <param name="position"> Position in the field </param>
        /// <param name="elapsed"> Time scalling variable </param>
        /// <param name="type"> Decise if the calculation for positive or for negative charge will be used </param>
        /// <param name="scale"> Size scalling variable</param>
        public Charge(double[] position, float elapsed, int type, float scale)
        {
            this.position = position;
            double var = Math.PI / 2;
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
        public double[] GetPosition()
        {
            return position;
        }

        /// <summary>
        /// return charge of the charge.
        /// </summary>
        /// <returns> charge as a float number </returns>
        public double GetCharge()
        {
            return charge;
        }

        public void SetPosition(double[] position) { 
            this.coord= position;
        }

        /// <summary>
        /// Draws the charge into the drawingPanel
        /// </summary>
        /// <param name="g"> Graphics context </param>
        /// <param name="scale"> Scalling variable </param>
        public void Add(Graphics g, float scale)
        {
            // Calculating size and position of the charge.
            double size = Math.Abs(GetCharge() * 15 * scale);
            coord = [position[0] * 50 * scale, -position[1] * 50 * scale];

            // Creating path for the charge.
            GraphicsPath chargePath = new GraphicsPath();
            chargePath.AddEllipse((float)(coord[0] - size / 2), (float)(coord[1] - size / 2), (float)size, (float)size);

            // Defining brush.
            PathGradientBrush brush = new PathGradientBrush(chargePath);
            brush.CenterPoint = new PointF((float)coord[0], (float)coord[1]);
            if (charge > 0)
            {
                brush.CenterColor = Color.Red;
                brush.InterpolationColors = new ColorBlend()
                {
                    Colors = new Color[] { Color.FromArgb(255, Color.Red), Color.FromArgb(180, Color.Red), Color.FromArgb(130, Color.Red) },
                    Positions = new float[] { 0f, 0.5f, 1f }
                };
            }
            else
            {
                brush.CenterColor = Color.Blue;
                brush.InterpolationColors = new ColorBlend()
                {
                    Colors = new Color[] { Color.FromArgb(255, Color.Blue), Color.FromArgb(180, Color.Blue), Color.FromArgb(130, Color.Blue) },
                    Positions = new float[] { 0f, 0.5f, 1f }
                };
            }

            // Creating region and inserting the chargepath inside.
            var ChargeRegion = new Region(chargePath);


            // Converting charge to a string and creating font.
            string ch = Convert.ToString(GetCharge()) + " charge";
            Font f = new Font("Arial", 0.04f * (float)size * scale);

            // Drawing the charge and it's description.
            g.FillRegion(brush, ChargeRegion);
            g.DrawString(ch, f, Brushes.Black, (float)(coord[0] - g.MeasureString(ch, f).Width / 2), (float)(coord[1] - g.MeasureString(ch, f).Height / 2));
        }
    }
}
