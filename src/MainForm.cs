using System.Drawing;
using System.Windows.Forms;

namespace UPG_SP_2024
{
    public partial class MainForm : Form
    {
        private double mouseOrX;
        private double mouseOrY;

        public MainForm(string? parameter = null)
        {
            InitializeComponent();
            // Store the parameter in a field or use it to initialize the DrawingPanel
            this.drawingPanel.parameter = parameter;
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
            while (true) {
                drawingPanel.mouseClick = true;
            }
        }

        private void drawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            drawingPanel.mouseDown = true;
            mouseOrX = e.X;
            mouseOrY = e.Y;
        }

        private void drawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            drawingPanel.mouseX = e.X - mouseOrX;
            drawingPanel.MouseY = e.Y - mouseOrY;
            drawingPanel.mouseDown = false;
        }
    }
}
