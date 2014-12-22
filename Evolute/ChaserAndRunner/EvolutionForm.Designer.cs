namespace ChaserAndRunner
{
    partial class EvolutionForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.pnlWorldView = new System.Windows.Forms.Panel();
            this.pnlDrawProgram = new System.Windows.Forms.Panel();
            this.pnlViewIslands = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 628);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // pnlWorldView
            // 
            this.pnlWorldView.Location = new System.Drawing.Point(13, 13);
            this.pnlWorldView.Name = "pnlWorldView";
            this.pnlWorldView.Size = new System.Drawing.Size(800, 600);
            this.pnlWorldView.TabIndex = 1;
            // 
            // pnlDrawProgram
            // 
            this.pnlDrawProgram.Location = new System.Drawing.Point(820, 13);
            this.pnlDrawProgram.Name = "pnlDrawProgram";
            this.pnlDrawProgram.Size = new System.Drawing.Size(26, 600);
            this.pnlDrawProgram.TabIndex = 2;
            // 
            // pnlViewIslands
            // 
            this.pnlViewIslands.Location = new System.Drawing.Point(13, 645);
            this.pnlViewIslands.Name = "pnlViewIslands";
            this.pnlViewIslands.Size = new System.Drawing.Size(800, 146);
            this.pnlViewIslands.TabIndex = 3;
            // 
            // EvolutionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 803);
            this.Controls.Add(this.pnlViewIslands);
            this.Controls.Add(this.pnlDrawProgram);
            this.Controls.Add(this.pnlWorldView);
            this.Controls.Add(this.label1);
            this.Name = "EvolutionForm";
            this.Text = "Evolution";
            this.Resize += new System.EventHandler(this.EvolutionForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlWorldView;
        private System.Windows.Forms.Panel pnlDrawProgram;
        private System.Windows.Forms.Panel pnlViewIslands;
    }
}