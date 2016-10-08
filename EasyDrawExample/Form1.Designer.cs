namespace EasyDrawExample
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.scoreLabel = new System.Windows.Forms.Label();
            this.somebodyWonLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).BeginInit();
            this.SuspendLayout();
            // 
            // TheHelpButton
            // 
            this.TheHelpButton.Visible = false;
            // 
            // DrawBox
            // 
            this.DrawBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("DrawBox.BackgroundImage")));
            this.DrawBox.Size = new System.Drawing.Size(720, 390);
            this.DrawBox.MouseHover += new System.EventHandler(this.DrawBox_MouseHover);
            this.DrawBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawBox_MouseMove);
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Location = new System.Drawing.Point(328, 9);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(39, 13);
            this.scoreLabel.TabIndex = 1;
            this.scoreLabel.Text = "0   |   0";
            // 
            // somebodyWonLabel
            // 
            this.somebodyWonLabel.AutoSize = true;
            this.somebodyWonLabel.Location = new System.Drawing.Point(300, 160);
            this.somebodyWonLabel.Name = "somebodyWonLabel";
            this.somebodyWonLabel.Size = new System.Drawing.Size(86, 13);
            this.somebodyWonLabel.TabIndex = 2;
            this.somebodyWonLabel.Text = "Somebody Won!";
            this.somebodyWonLabel.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(720, 390);
            this.Controls.Add(this.somebodyWonLabel);
            this.Controls.Add(this.scoreLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.ShowGrid = true;
            this.ReadyToDraw += new System.EventHandler(this.Form1_ReadyToDraw);
            this.OnUpdate += new System.EventHandler(this.Form1_OnUpdate);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.Controls.SetChildIndex(this.DrawBox, 0);
            this.Controls.SetChildIndex(this.TheHelpButton, 0);
            this.Controls.SetChildIndex(this.scoreLabel, 0);
            this.Controls.SetChildIndex(this.somebodyWonLabel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.DrawBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Label somebodyWonLabel;

    }
}

