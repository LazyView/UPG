using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace UPG_SP_2024
{
    public partial class MainForm : Form
    {
        private double mouseOrX;
        private double mouseOrY;

        public MainForm(string parameter, int gridGapWidth, int gridGapHeight)
        {
            InitializeComponent();
            // Store the parameter in a field or use it to initialize the DrawingPanel
            this.drawingPanel.parameter = parameter;
            this.drawingPanel.gridCellWidth = gridGapWidth;
            this.drawingPanel.gridCellHeight = gridGapHeight;
            this.Size = new Size(800, 600);
            BackColor = Color.LightGoldenrodYellow;
        }

        private void drawingPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void drawingPanel_MouseClick(object sender, MouseEventArgs e)
        {
            drawingPanel.mouseClick = true;
            drawingPanel.mouseX = e.X;
            drawingPanel.mouseY = e.Y;
        }

        private void drawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            drawingPanel.mouseDown = true;
        }

        private void drawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            drawingPanel.mouseDown = false;
        }

        private void drawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
            drawingPanel.mouseX = (e.X - mouseOrX);
            drawingPanel.mouseY =(e.Y - mouseOrY);
            }
        }
    }
}
