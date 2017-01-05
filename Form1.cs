using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Text;
using WindowsFormsApplication1.ViewModels;
using WindowsFormsApplication1.Models;
using Microsoft.Win32;
// http://social.msdn.microsoft.com/forums/en-US/wpf/thread/61096416-457b-4f94-b79e-22b2239dbb54

// minimize: 
// http://channel9.msdn.com/Forums/TechOff/49798-C-Minimize-on-Close
namespace WindowsFormsApplication1
{
    partial class Form1 : System.Windows.Forms.Form
    {
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private SearchedTerms searchedTerms1;
        private System.ComponentModel.IContainer components;

        bool _isRealClose = false;


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        protected override void Dispose(bool disposing)
        {
            // Clean up any components being used.
            if (disposing)
                if (components != null)
                    components.Dispose();

            base.Dispose(disposing);
        }

        enum NotifyIcon { Read, UnRead }
        void changeIcon(NotifyIcon type)
        {
            switch (type)
            {
                case NotifyIcon.UnRead:
                    notifyIcon1.Icon = Properties.Resources.Unread;
                    break;

                case NotifyIcon.Read:
                    notifyIcon1.Icon = Properties.Resources.Read;
                    break;

            }
        }

        /// <summary>
        /// Don't add your custom codes in the method.
        /// The method is created/modified automatically by visual studio when you alter the UI layout in design view.
        /// </summary>
        private void InitializeComponent()
        {
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.searchedTerms1 = new WindowsFormsApplication1.SearchedTerms();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(785, 596);
            this.elementHost1.TabIndex = 0;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.searchedTerms1;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(785, 596);
            this.Controls.Add(this.elementHost1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        private void initializeOtherComponents()
        {
            this.components = new System.ComponentModel.Container();

            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem1 });

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = this.contextMenu1;


            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "Web Scrapping";
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += this.notifyIcon1_DoubleClick;

            // Set up how the form should be displayed.
            this.Text = "Web Scraping";

            this.FormClosing += this.Form1_FormClosing;
        }

        public Form1()
        {
            InitializeComponent();

            initializeOtherComponents();

#if !DEBUG
            StartupRegisterUtil.RegisterToSystem();
#endif

            initializeData();

            changeIcon(NotifyIcon.Read);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Close();
        }

        private void initializeData()
        {
            MainViewModel viewModel = new MainViewModel();
            this.searchedTerms1.DataContext = viewModel;

            viewModel.WebPageDetected += delegate(object sender, WebPageDetectedEventArgs e)
            {
                notifyIcon1.ShowBalloonTip(20, e.WebPage.Title, e.WebPage.NameOfSearchAlgorithm, ToolTipIcon.Info);
                changeIcon(NotifyIcon.UnRead);
            };

            viewModel.OnSearch();
        }

        // Show the form when the user double clicks on the notify icon.
        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            if (this.Visible == false)
                changeIcon(NotifyIcon.Read);

            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }

        protected override void OnResize(EventArgs e)
        {
            // if you don't call the base.OnResize(e), the docking doesn't work when resizing the form.
            base.OnResize(e);
        }

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            _isRealClose = true;

            // Close the form, which closes the application.
            this.Close();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && _isRealClose == false)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;

                // If ShowInTaskbar is set to false, minimize it still saves its window in the left-bottom corner.
                // So we need to hide it.
                this.ShowInTaskbar = false;
                this.Hide();
            }
        }


    }
}