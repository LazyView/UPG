using ElectricFieldVis;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace UPG_SP_2024
{

    /// <summary>
    /// The main panel with the custom visualization
    /// </summary>
    public class DrawingPanel : Panel
    {
        public string parameter;
        private int m_Start;
        private float elapsed;
        private double power;
        double[,] positions;
        private int gridGapX;
        private int gridGapY;
        private double gridPower;
        public double mouseX;
        public double MouseY;
        public bool mouseDown;
        public bool mouseClick;
        /// <summary>Initializes a new instance of the <see cref="DrawingPanel" /> class.</summary>
        public DrawingPanel()
        {
            this.Text = "Electrostatic Field Visualization";
            var timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 50;
            m_Start = Environment.TickCount;
            timer.Start();

            // Anti-flickerring.
            DoubleBuffered = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        /// <summary>TODO: Custom visualization code comes into this method</summary>
        /// <remarks>Raises the <see cref="E:System.Windows.Forms.Control.Paint">Paint</see> event.</remarks>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs">PaintEventArgs</see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Graphic context.
            Graphics g = e.Graphics;

            // Codes for better looking visualisation.
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            //Translation of the system.
            g.TranslateTransform(this.Width / 2, this.Height / 2);
            // Scaling.
            var scalenumber = 0.005f;
            var scalex = this.Width * scalenumber;
            var scaley = this.Height * scalenumber;
            var scale = Math.Min(scalex, scaley);
            
            parameter = "4";
            // Scenarios.
            Scenarious(g, parameter, scale);
        }

        /// <summary>
        /// Fires the event indicating that the panel has been resized. Inheriting controls should use this in favor of actually listening to the event, but should still call <span class="keyword">base.onResize</span> to ensure that the event is fired for external listeners.
        /// </summary>
        /// <param name="eventargs">An <see cref="T:System.EventArgs">EventArgs</see> that contains the event data.</param>
        protected override void OnResize(EventArgs eventargs)
        {
            this.Invalidate();  //ensure repaint

            base.OnResize(eventargs);
        }
        
        /// <summary>
        /// Chooses right scenario and draws it onto drawingPanel.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="parameter"></param>
        /// <param name="scale"></param>
        private void Scenarious(Graphics g, string parameter, float scale)
        {
            Probe p = new Probe();
            Grid grid = new Grid();
            gridGapX = 20;
            gridGapY = 20;
            grid.BackgroundGrid(g, gridGapX, gridGapY, this.Width, this.Height, scale);
           
            switch (parameter)
            {
                case "0":
                    Charge ch01 = new Charge(new double[] { 0, 0 }, 1);
                    ch01.Add(g, scale);
                    // Inicialization of arrays which will be held to compute the intensity.
                    positions = new double[1, 2] { { 0, 0 } };
                    elapsed = (Environment.TickCount - m_Start) / 1000f;
                    power = p.Calculate_Intensity(new double[] {ch01.GetCharge()}, positions, elapsed);
                    p.AddVector(g, scale, power);
                    //g.TranslateTransform(-this.Width/2, -this.Height/2);
                    /*
                    for (int y = 0; y < this.Height; y += gridGapY)
                    {
                        for (int x = 0; x < this.Width; x += gridGapX)
                        {
                            gridPower = grid.calculate_intensity_static(x - this.Width/2, y - this.Height/2, charges, positions, elapsed);
                            grid.addGridVector(g, gridGapX, gridGapY, scale, this.Width, this.Height);

                        }
                    }
                    */
                    break;
                    
                case "1":
                    Charge ch11 = new Charge( new double[]{ -1, 0}, 1);
                    Charge ch12 = new Charge(new double[] {1, 0}, 1);
                    // Inicialization of arrays which will be held to compute the intensity.
                    positions = new double[2, 2] { { -1, 0 }, { 1, 0 } };
                    ch11.Add(g, scale);
                    ch12.Add(g, scale);
                    elapsed = (Environment.TickCount - m_Start) / 1000f;
                    power = p.Calculate_Intensity(new double[] {ch11.GetCharge(), ch12.GetCharge()}, positions, elapsed);
                    p.AddVector(g, scale, power);
                    /*
                    for (int y = 0; y < this.Height; y += gridGapY)
                    {
                        for (int x = 0; x < this.Width; x += gridGapX)
                        {
                            gridPower = grid.calculate_intensity_static(x / this.Width, y / this.Width, charges, positions, elapsed);
                            grid.addGridVector(g, gridGapX, gridGapY, scale, this.Width, this.Height);

                        }
                    }
                    */
                    break;
                    
                    
                case "2":
                    Charge ch21 = new Charge(new double[] { -1, 0 }, -1);
                    Charge ch22 = new Charge(new double[] { 1, 0 }, 2);
                    // Inicialization of arrays which will be held to compute the intensity.
                    positions = new double[2, 2] { { -1, 0 }, { 1, 0 } };
                    ch21.Add(g, scale);
                    ch22.Add(g, scale);
                    elapsed = (Environment.TickCount - m_Start) / 1000f;
                    power = p.Calculate_Intensity(new double[] {ch21.GetCharge(), ch22.GetCharge()}, positions, elapsed);
                    p.AddVector(g, scale, power);
                    
                    for (int y = 0; y < this.Height; y += gridGapY)
                    {
                        for (int x = 0; x < this.Width; x += gridGapX)
                        {
                            gridPower = grid.Calculate_intensity_static(-this.Width / 2 + x, -this.Height / 2 + y, new float[] {ch21.GetCharge(), ch22.GetCharge() }, positions, elapsed, this.Width, this. Height);
                            grid.addGridVector(g, gridGapX, gridGapY, scale, this.Width, this.Height);

                        }
                    }
                    
                    break;
                    
                case "3":
                    Charge ch31 = new Charge( new double[]{-1, -1 }, 1);
                    Charge ch32 = new Charge(new double[] {1, -1 }, 2);
                    Charge ch33 = new Charge(new double[] { 1, 1 }, -3);
                    Charge ch34 = new Charge(new double[] { -1, 1 }, -4);
                    elapsed = (Environment.TickCount - m_Start) / 1000f;
                    ch31.Add(g, scale);
                    ch32.Add(g, scale);
                    ch33.Add(g, scale);
                    ch34.Add(g, scale);
                    // Inicialization of arrays which will be held to compute the intensity.
                    positions = new double[4, 2] { { -1, -1 }, { 1, -1 }, { 1, 1 }, { -1, 1 } };
                    power = p.Calculate_Intensity(new double[] {ch31.GetCharge(), ch32.GetCharge(), ch33.GetCharge(), ch34.GetCharge() }, positions, elapsed);
                    p.AddVector(g, scale, power);
                    /*
                    for (int y = 0; y < this.Height; y += gridGapY)
                    {
                        for (int x = 0; x < this.Width; x += gridGapX)
                        {
                            gridPower = grid.calculate_intensity_static(-this.Width / 2 + x, -this.Height / 2 + y, charges, positions, elapsed);
                            grid.addGridVector(g, gridGapX, gridGapY, scale, this.Width, this.Height);
                        }
                    }
                    
                    */
                    break;
                    
                case "4":
                    Charge ch41 = new Charge(new double[] { -1, 0 }, elapsed, 1, scale);
                    Charge ch42 = new Charge(new double[] { 1, 0 }, elapsed, 2, scale);
                    ch41.Add(g, scale);
                    ch42.Add(g, scale);
                    positions = new double[2, 2] { { -1, 0 }, { 1, 0 } };
                    elapsed = (Environment.TickCount - m_Start) / 1000f;
                    power = p.Calculate_Intensity(new double[] {ch41.GetCharge(), ch42.GetCharge()}, positions, elapsed);
                    p.AddVector(g, scale, power);
                    break;
            }
        }
    }
}
