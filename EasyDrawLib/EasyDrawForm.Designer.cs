namespace EasyDrawLib
{
    /// <summary>
    /// The form to use for easily drawing stuff
    /// </summary>
    partial class EasyDrawForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.TheHelpButton = new System.Windows.Forms.Button();
            this.DrawBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).BeginInit();
            this.SuspendLayout();
            // 
            // saveDialog
            // 
            this.saveDialog.DefaultExt = "png";
            this.saveDialog.FileName = "*.png";
            // 
            // updateTimer
            // 
            this.updateTimer.Interval = 17;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // TheHelpButton
            // 
            this.TheHelpButton.Location = new System.Drawing.Point(647, 0);
            this.TheHelpButton.Name = "TheHelpButton";
            this.TheHelpButton.Size = new System.Drawing.Size(75, 39);
            this.TheHelpButton.TabIndex = 0;
            this.TheHelpButton.Text = "HELP";
            this.TheHelpButton.UseVisualStyleBackColor = true;
            this.TheHelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // DrawBox
            // 
            this.DrawBox.Location = new System.Drawing.Point(0, 0);
            this.DrawBox.Name = "DrawBox";
            this.DrawBox.Size = new System.Drawing.Size(100, 50);
            this.DrawBox.TabIndex = 1;
            this.DrawBox.TabStop = false;
            this.DrawBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseMove);
            // 
            // EasyDrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(722, 593);
            this.Controls.Add(this.TheHelpButton);
            this.Controls.Add(this.DrawBox);
            this.Name = "EasyDrawForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.EasyDrawForm_Load);
            this.Shown += new System.EventHandler(this.EasyDrawForm_Shown);
            this.Resize += new System.EventHandler(this.EasyDrawForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.Timer updateTimer;
        /// <summary>
        /// The help button
        /// </summary>
        protected System.Windows.Forms.Button TheHelpButton;
        protected System.Windows.Forms.PictureBox DrawBox;
    }
}