using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using StreamDeck = SharpLib.StreamDeck;
using System.Configuration;
using System.Runtime.Serialization;

namespace StreamDeckDemo
{
    public partial class FormMain : Form
    {

        StreamDeck.FormEditor iEditor;

        public FormMain()
        {
            //
            InitializeComponent();

            // Embed our Stream Deck Editor Form
            iEditor = new StreamDeck.FormEditor();

            iEditor.ComboBoxKeyDownEvent.Items.Add("EventOne");
            iEditor.ComboBoxKeyDownEvent.Items.Add("EventTwo");
            iEditor.ComboBoxKeyDownEvent.Items.Add("EventThree");

            iEditor.ComboBoxKeyUpEvent.Items.Add("EventOne");
            iEditor.ComboBoxKeyUpEvent.Items.Add("EventTwo");
            iEditor.ComboBoxKeyUpEvent.Items.Add("EventThree");


            iEditor.Dock = DockStyle.Fill;
            iEditor.TopLevel = false;
            iEditor.Show();

            // Put it in secondary tab to test loading issue
            tabPage2.Controls.Add(iEditor);

            // Comment out the following if you want to test load issues in background tab
            tabControl.SelectedTab = tabPage2;


        }
    }
}
