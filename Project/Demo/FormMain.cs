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

            iEditor.ComboBoxEvents.Items.Add("EventOne");
            iEditor.ComboBoxEvents.Items.Add("EventTwo");
            iEditor.ComboBoxEvents.Items.Add("EventThree");

            iEditor.Dock = DockStyle.Fill;
            iEditor.TopLevel = false;
            Controls.Add(iEditor);
            iEditor.Show();



        }
    }
}
