using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Game;
using GameLibrary;

namespace GnomoriaEnhanced.UI.World
{
    public partial class GnomesSkills : Form
    {
        #region Variables
        private Font dataGridViewHeaderFont;
        private GnomoriaEnhanced.Main _parent;
        private GnomoriaEnhanced.UI.WorldForm _worldForm;
        private List<CharacterSkillType> _professionSkillList;
        private GnomanEmpire _gnomanEmpire;
        private int _startSkillsCol = 4;
        private bool _loaded;

        private DataTable mCharSkills;
        #endregion

        #region Form
        public GnomesSkills(GnomoriaEnhanced.UI.WorldForm parent)
        {
            this._worldForm = parent;
            this._parent = parent.parent;
            this._gnomanEmpire = parent.gnomanEmpire;

            
            InitializeComponent();

            this.Width = 1187;
            this.Height = 486;
            this.DoubleBuffered = true;

            _professionSkillList = SkillDef.AllLaborSkills();
            dataGridViewHeaderFont = new Font(new FontFamily("Arial"), 11);

            LoadDataTable();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            _parent.MemoryUsage();
        }

        private void Form_Show(object sender, EventArgs e)
        {
            this.dataGridView_CellClick(null, new DataGridViewCellEventArgs(-1, -1));
        }

        private void LoadDataTable()
        {
            if (mCharSkills != null)
            {
                mCharSkills.Clear();
                mCharSkills.Dispose();
                mCharSkills = null;
            }

            mCharSkills = new DataTable("mCharSkills");

            DataColumn cNum = new DataColumn("Num", typeof(string));
            mCharSkills.PrimaryKey = new DataColumn[] { mCharSkills.Columns.Add(cNum.ColumnName) };

            DataColumn cName = mCharSkills.Columns.Add("Name", typeof(string));
            DataColumn cProfession = mCharSkills.Columns.Add("Profession", typeof(string));
            DataColumn cJob = mCharSkills.Columns.Add("Current Job", typeof(string));

            foreach (CharacterSkillType profession in _professionSkillList)
            {
                mCharSkills.Columns.Add(profession.ToString(), typeof(int));
            }

            foreach (var Char in _gnomanEmpire.World.AIDirector.PlayerFaction.Members)
            {
                DataRow tmpRow = mCharSkills.NewRow();
                tmpRow[0] = Char.Key;
                tmpRow[1] = Char.Value.Name();
                tmpRow[2] = Char.Value.Title();
                if (Char.Value.Job == null)
                {
                    if (Char.Value.Body.IsSleeping == true)
                    {
                        tmpRow[3] = "Sleeping";
                    }
                    else
                    {
                        tmpRow[3] = "Idle";
                    }
                }
                else
                {
                    tmpRow[3] = Char.Value.Job.JobName();
                }

                int tmpCol = _startSkillsCol;
                foreach (CharacterSkillType skill in _professionSkillList)
                {
                    tmpRow[tmpCol] = Char.Value.SkillLevel(skill);
                    tmpCol++;
                }
                mCharSkills.Rows.Add(tmpRow);
                _parent.Debug(String.Format("* Added Char: {0}", Char.Value.Name()));
            }

            mCharSkills.Columns[0].ReadOnly = true;
            mCharSkills.Columns[1].ReadOnly = true;
            mCharSkills.Columns[2].ReadOnly = true;
            mCharSkills.Columns[3].ReadOnly = true;

            dataGridView.SuspendLayout();

            dataGridView.DataSource = mCharSkills;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;

            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].Frozen = true;
            dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dataGridView.Sort(dataGridView.Columns[1], ListSortDirection.Ascending); // Sort by Name Asc 

            this.dataGridView_CellClick(null, new DataGridViewCellEventArgs(-1, -1));

            dataGridView.ResumeLayout();
        }

        private void dataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= _startSkillsCol)
            {
                e.PaintBackground(e.ClipBounds, true);
                Rectangle rect = this.dataGridView.GetColumnDisplayRectangle(e.ColumnIndex, true);
                Size titleSize = TextRenderer.MeasureText(e.Value.ToString(), dataGridViewHeaderFont);
                if (this.dataGridView.ColumnHeadersHeight < titleSize.Width)
                {
                    this.dataGridView.ColumnHeadersHeight = titleSize.Width;
                }

                e.Graphics.TranslateTransform(0, titleSize.Width);
                e.Graphics.RotateTransform(-90.0F);

                // This is the key line for bottom alignment - we adjust the PointF based on the 
                // ColumnHeadersHeight minus the current text width. ColumnHeadersHeight is the
                // maximum of all the columns since we paint cells twice - though this fact
                // may not be true in all usages!   
                e.Graphics.DrawString(e.Value.ToString(), dataGridViewHeaderFont, Brushes.Black, new PointF(rect.Y - (dataGridView.ColumnHeadersHeight - titleSize.Width), rect.X));

                // The old line for comparison
                //e.Graphics.DrawString(e.Value.ToString(), this.Font, Brushes.Black, new PointF(rect.Y, rect.X));


                e.Graphics.RotateTransform(90.0F);
                e.Graphics.TranslateTransform(0, -titleSize.Width);
                e.Handled = true;
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 && e.RowIndex == -1)
            {
                dataGridView.ColumnHeadersHeight = 5;

                for (int cl = _startSkillsCol; cl < dataGridView.Columns.Count; cl++)
                {
                    int width = TextRenderer.MeasureText(dataGridView.Columns[cl].HeaderCell.Value.ToString(), dataGridViewHeaderFont).Width;

                    dataGridView.Columns[cl].Width = dataGridView.Columns[cl].MinimumWidth = (int)dataGridViewHeaderFont.Size * 3;
                    if (width > dataGridView.ColumnHeadersHeight)
                        dataGridView.ColumnHeadersHeight = width + (int)(dataGridViewHeaderFont.Size * 1.85);

                }
            }
        }

        
        #endregion 

        #region Debug
        private void MemoryUsage()
        {
            long usage = GC.GetTotalMemory(true);
            usage = (usage / 1024L) / 1024L;
            _parent.Debug(String.Format("Total Memory: {0:0.00}mb", usage));
        }
        #endregion

        
    }
}
