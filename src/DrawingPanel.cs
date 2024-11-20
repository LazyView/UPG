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
        //---- public variables ---- //
        public string parameter;
        public int[] coordinates;
        public int gridCellWidth;
        public int gridCellHeight;
        // Mouse handeling
        public double mouseX;
        public double mouseY;
        public bool mouseDown;
        public bool mouseClick;

        //-------------------------------//
        
        //---- private variables ----//
        private float scale;
        private int m_Start;
        private float elapsed;
        // Storing the values for positions and charges
        private double[,] positions;
        private double[] charges;
        // Size of the grid cell width and height
        
        
        
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

        /// <summary>Main function where everything draws on the drawingPanel</summary>
        /// <remarks>Raises the <see cref="E:System.Windows.Forms.Control.Paint">Paint</see> event.</remarks>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs">PaintEventArgs</see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //-------------------------------//
            Graphics g = e.Graphics;
            //-------------------------------//
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            //-------------------------------//
            g.TranslateTransform(this.Width / 2, this.Height / 2);
            //-------------------------------//
            var scalenumber = 0.005f;
            var scalex = this.Width * scalenumber;
            var scaley = this.Height * scalenumber;
            scale = Math.Min(scalex, scaley);

            //-------------------------------//
            parameter = "4";
            Scenarious(g, parameter);
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
        /// <param name="g"> Graphics content </param>
        /// <param name="parameter"> Decides which scenerio to play </param>
        /// <param name="scale"> Scaling variable </param>
        private void Scenarious(Graphics g, string parameter)
        {
            gridCellWidth = 40;
            gridCellHeight = 40;
            
            elapsed = (Environment.TickCount - m_Start) / 1000f;
            
            switch (parameter)
            {
                case "0":
                    //-------------------------------//
                    Charge ch01 = new(new double[] { 0, 0 }, 1);
                    //-------------------------------//
                    positions = new double[1, 2] { { 0, 0 } };
                    charges = new double[] { ch01.GetCharge() };
                    //-------------------------------//
                    Probe p0 = new(elapsed, this.Width, this.Height, charges, positions);
                    Grid grid0 = new(gridCellWidth, gridCellHeight, this.Width, this.Height, charges, positions);
                    //-------------------------------//
                    grid0.Add_Heat_Map_Background(g, scale);
                    grid0.BackgroundGrid(g, scale);
                    grid0.Add_Grid_Vectors(g, scale);
                    ch01.Add(g, scale);
                    p0.AddVector(g, scale);
                    
                    break;
                    
                case "1":
                    //-------------------------------//
                    Charge ch11 = new(new double[] { -1, 0 }, 1);
                    Charge ch12 = new(new double[] { 1, 0 }, 1);
                    //-------------------------------//
                    positions = new double[2, 2] { { -1, 0 }, { 1, 0 } };
                    charges = new double[] { ch11.GetCharge(), ch12.GetCharge() };
                    //-------------------------------//
                    Probe p1 = new(elapsed, this.Width, this.Height, charges, positions);
                    Grid grid1 = new(gridCellWidth, gridCellHeight, this.Width, this.Height, charges, positions);
                    //-------------------------------//
                    grid1.Add_Heat_Map_Background(g, scale);
                    grid1.BackgroundGrid(g, scale);
                    grid1.Add_Grid_Vectors(g, scale);
                    ch11.Add(g, scale);
                    ch12.Add(g, scale);
                    p1.AddVector(g, scale);
                    break;
                    
                    
                case "2":
                    //-------------------------------//
                    Charge ch21 = new(new double[] { -1, 0 }, -1);
                    Charge ch22 = new(new double[] { 1, 0 }, 2);
                    //-------------------------------//
                    positions = new double[2, 2] { { -1, 0 },{ 1, 0 } };
                    charges = new double[]{ ch21.GetCharge(), ch22.GetCharge() };
                    //-------------------------------//
                    Probe p2 = new(elapsed, this.Width, this.Height, charges, positions);
                    Grid grid2 = new(gridCellWidth, gridCellHeight, this.Width, this.Height, charges, positions);
                    //-------------------------------//
                    grid2.Add_Heat_Map_Background(g, scale);
                    grid2.BackgroundGrid(g, scale);
                    grid2.Add_Grid_Vectors(g, scale);
                    ch21.Add(g, scale);
                    ch22.Add(g, scale);
                    p2.AddVector(g, scale);
                    break;
                    
                case "3":
                    //-------------------------------//
                    Charge ch31 = new(new double[]{ -1, -1 }, 1);
                    Charge ch32 = new(new double[] { 1, -1 }, 2);
                    Charge ch33 = new(new double[] { 1, 1 }, -3);
                    Charge ch34 = new(new double[] { -1, 1 }, -4);
                    //-------------------------------//
                    positions = new double[4, 2] { { -1, -1 }, { 1, -1 }, { 1, 1 }, { -1, 1 } };
                    charges = new double[] { ch31.GetCharge(), ch32.GetCharge(), ch33.GetCharge(), ch34.GetCharge() };
                    //-------------------------------//
                    Probe p3 = new(elapsed, this.Width, this.Height, charges, positions);
                    Grid grid3 = new(gridCellWidth, gridCellHeight, this.Width, this.Height, charges, positions);
                    //-------------------------------//
                    grid3.Add_Heat_Map_Background(g, scale);
                    grid3.BackgroundGrid(g, scale);
                    grid3.Add_Grid_Vectors(g, scale);
                    ch31.Add(g, scale);
                    ch32.Add(g, scale);
                    ch33.Add(g, scale);
                    ch34.Add(g, scale);
                    p3.AddVector(g, scale);
                    break;
                    
                case "4":
                    //-------------------------------//
                    Charge ch41 = new(new double[] { -1, 0 }, elapsed, 1);
                    Charge ch42 = new(new double[] { 1, 0 }, elapsed, 2);
                    //-------------------------------//
                    positions = new double[2, 2] { { -1, 0 }, { 1, 0 } };
                    charges = new double[] { ch41.GetCharge(), ch42.GetCharge() };
                    //-------------------------------//
                    Probe p4 = new(elapsed, this.Width, this.Height, charges, positions);
                    Grid grid4 = new(gridCellWidth, gridCellHeight, this.Width, this.Height, charges, positions);
                    //-------------------------------//
                    grid4.Add_Heat_Map_Background(g, scale);
                    grid4.BackgroundGrid(g, scale);
                    grid4.Add_Grid_Vectors(g, scale);
                    ch41.Add(g, scale);
                    ch42.Add(g, scale);
                    p4.AddVector(g, scale);
                    break;
                    
            }
            
        }

        /// <summary>
        /// Creates a static probe when mouse is clicked.
        /// </summary>
        /// <param name="g"> Graphics context </param>
        private void Probe_Static(Graphics g)
        {
            
            if (mouseClick)
            {
                g.TranslateTransform(-Width / 2, -Height / 2);
                double[] mouse_Position = [mouseX, mouseY];
                Probe probe = new(mouse_Position, this.Width, this.Height, charges, positions);
                var old = g.Transform;
                probe.AddProbeStatic(g, scale);
                g.Transform = old;
            };
        }
    }
}
