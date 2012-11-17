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
    public partial class ProfessionsModifyForm : Form
    {
        #region Variables
        private Font dataGridViewHeaderFont;
        private GnomoriaEnhanced.Main _parent;
        private GnomoriaEnhanced.UI.WorldForm _worldForm;
        private List<CharacterSkillType> _professionSkillList;
        private GnomanEmpire _gnomanEmpire;
        private int _startSkillsCol = 2;
        private bool _loaded;
        private bool _needReload;
        #endregion

        #region Form
        public ProfessionsModifyForm(GnomoriaEnhanced.UI.WorldForm parent)
        {
            this._worldForm = parent;
            this._parent = parent.parent;
            this._gnomanEmpire = parent.gnomanEmpire;

            dataGridViewHeaderFont = new Font(new FontFamily("Arial"), 11);
            InitializeComponent();
            this.Width = 1187;
            this.Height = 486;
            this.DoubleBuffered = true;

            _professionSkillList = SkillDef.AllLaborSkills();

            LoadDataTable();
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            MemoryUsage();
        }

        private void LoadDataTable()
        {
            dataGridView.CurrentCellDirtyStateChanged += dataGridView_CurrentCellDirtyStateChanged;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ColumnHeadersHeight = 18;
            dataGridView.ColumnCount = 2;
            dataGridView.Columns[0].Name = "ID";
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].Name = "Name";
            dataGridView.Columns[1].Frozen = true;
            foreach (CharacterSkillType skill in _professionSkillList)
            {
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                dataGridView.Columns.Add(chk);
                chk.HeaderText = skill.ToString();
                chk.Name = skill.ToString();
                if (chk.HeaderText == "LaborEnd")
                    chk.HeaderText = "Hauling";
                if (chk.HeaderText == "AnimalHusbandry")
                    chk.HeaderText = "Animal Husbandry";
            }



            _parent.Debug(String.Format("Data Grid View Columns: {0}", dataGridView.ColumnCount));
            int i = 0;
            foreach (Profession profession in _gnomanEmpire.Fortress.Professions)
            {
                _parent.Debug(String.Format("[{0}] {1}", i, profession.Title));
                dataGridView.Rows.Add();
                dataGridView.Rows[i].Cells[0].Value = i;                // ID
                dataGridView.Rows[i].Cells[1].Value = profession.Title; // Title
                int a = 2;
                foreach (CharacterSkillType skill in _professionSkillList)
                {
                    dataGridView.Rows[i].Cells[a].Value = profession.AllowedSkills.IsSkillAllowed(skill);
                    dataGridView.Rows[i].Cells[a].Tag = skill;
                    a++;
                }
                i++;
            }

            dataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.Sort(dataGridView.Columns[0], ListSortDirection.Ascending);

            this.dataGridView_CellClick(null, new DataGridViewCellEventArgs(-1, -1));
            this._loaded = true;
            MemoryUsage();
        }

        private void dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView.IsCurrentCellDirty)
            {
                dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
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

                for (int cl = 0; cl < dataGridView.Columns.Count; cl++)
                {
                    int width = TextRenderer.MeasureText(dataGridView.Columns[cl].HeaderCell.Value.ToString(), dataGridViewHeaderFont).Width;

                    dataGridView.Columns[cl].Width = dataGridView.Columns[cl].MinimumWidth = (int)dataGridViewHeaderFont.Size * 3;
                    if (width > dataGridView.ColumnHeadersHeight)
                        dataGridView.ColumnHeadersHeight = width + (int)(dataGridViewHeaderFont.Size * 1.85);

                }
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this._loaded && e.ColumnIndex >= _startSkillsCol && e.RowIndex >= 0)
            {
                Profession profession = _gnomanEmpire.Fortress.Professions[e.RowIndex];
                //int val = e.ColumnIndex - _startSkillsCol;
                //_parent.Debug(String.Format("Column: {0}", e.ColumnIndex));
                //_parent.Debug(String.Format("Row:    {0}", e.RowIndex));
                //_parent.Debug(String.Format("Profession: {0}", profession.Title));
                //_parent.Debug(String.Format("Skill: {0}", dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString()));
                //_parent.Debug(String.Format("Enabled: {0}", dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue));
                //_parent.Debug("Value: " + dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                if ((bool)dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)
                {
                    //_parent.Debug(String.Format("Enabled: true"));
                    profession.AllowedSkills.AddSkill((CharacterSkillType)dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag);
                    _worldForm.isDirty = true;
                }
                else
                {
                    //_parent.Debug(String.Format("Enabled: false"));
                    profession.AllowedSkills.RemoveSkill((CharacterSkillType)dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag);
                    _worldForm.isDirty = true;
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

        #region Accessors
        public bool NeedReload
        {
            get { return _needReload; }
            set { _needReload = true; }
        }
        #endregion
    }
}
