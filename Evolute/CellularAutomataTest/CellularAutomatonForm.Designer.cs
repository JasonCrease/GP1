namespace CellularAutomataTest
{
    partial class CellularAutomatonForm
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
            this.pnlMatrixDisplay = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlDrawProgram = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlMatrixDisplay
            // 
            this.pnlMatrixDisplay.Location = new System.Drawing.Point(12, 12);
            this.pnlMatrixDisplay.Name = "pnlMatrixDisplay";
            this.pnlMatrixDisplay.Size = new System.Drawing.Size(294, 280);
            this.pnlMatrixDisplay.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 326);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // pnlDrawProgram
            // 
            this.pnlDrawProgram.Location = new System.Drawing.Point(379, 13);
            this.pnlDrawProgram.Name = "pnlDrawProgram";
            this.pnlDrawProgram.Size = new System.Drawing.Size(218, 279);
            this.pnlDrawProgram.TabIndex = 1;
            // 
            // CellularAutomatonForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 555);
            this.Controls.Add(this.pnlDrawProgram);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlMatrixDisplay);
            this.Name = "CellularAutomatonForm";
            this.Text = "Form1";
            this.Resize += new System.EventHandler(this.CellularAutomatonForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlMatrixDisplay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlDrawProgram;

    }
}

