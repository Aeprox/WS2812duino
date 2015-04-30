using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedControl
{
    

    public partial class main : Form
    {
        private LedEffects leds;
        private byte red, green, blue;

        public main()
        {
            InitializeComponent();
            leds = new LedEffects();
            fillComboboxes();
            setColor(60, 30, 5);                  
        }


        //general functions
        private void fillComboboxes(){
            
            string[] ports = SerialPort.GetPortNames();
            foreach(string s in ports) {
                this.comboBoxCom.Items.Add(s);
            }
            if (ports.Count() != 0)
            {
                this.comboBoxCom.SelectedIndex = 0;
                startSerialCon();
            }


            foreach (string mode in Enum.GetNames(typeof(LedModes))) {
                this.comboBoxEffects.Items.Add(mode);
            }
            this.comboBoxEffects.SelectedIndex = 0;
        }

        private void setColor(byte red, byte green, byte blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;

            textBoxR.Text = red.ToString();
            textBoxG.Text = green.ToString();
            textBoxB.Text = blue.ToString();

            pictureBox.BackColor = Color.FromArgb(red, green, blue);

        }

        private void startSerialCon()
        {
            string comPort = this.comboBoxCom.SelectedItem.ToString();
            this.leds.startSerialCom(comPort, 115200);
        }


        //handlers
        private void buttonCOM_Click(object sender, EventArgs e)
        {
            startSerialCon();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            byte[] options = new byte[3]{red,green,blue};
            this.leds.startEffect((LedModes)comboBoxEffects.SelectedIndex, options);
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            DialogResult result = colorPicker.ShowDialog();
            if (result == DialogResult.OK)
            {
                setColor(colorPicker.Color.R, colorPicker.Color.G, colorPicker.Color.B);
            }
        }

        private void buttonTurnOff_Click(object sender, EventArgs e)
        {
            this.leds.turnOff();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            setColor(byte.Parse(textBoxR.Text), byte.Parse(textBoxG.Text), byte.Parse(textBoxB.Text));
        }

        private void comboBoxEffects_SelectedIndexChanged(object sender, EventArgs e)
        {
            //change usercontrol
        }

    }
}
