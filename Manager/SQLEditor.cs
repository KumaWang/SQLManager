using Manager.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;

namespace Manager
{
    class SQLEditor : Control
    {
        private int id = -1;
        private Dictionary<SQLField, IPageEditor> mItemFields;
        private IPageEditor mEditor;
        private DataGridView mDataGrid;

        public SQLTable Table { get; }

        public IPageContainer Grid { get; }

        public SQLEditor(SQLTable table, IPageContainer grid, Dictionary<SQLField, IPageEditor> itemFields)
        {
            mItemFields = itemFields;

            Table = table;
            Grid = grid;

            mEditor = grid as IPageEditor;
            mEditor.Control.Dock = DockStyle.Fill;

            mDataGrid = new DataGridView();
            mDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            mDataGrid.MultiSelect = false;
            mDataGrid.Dock = DockStyle.Bottom;
            mDataGrid.Size = new System.Drawing.Size(60, 420);
            mDataGrid.SelectionChanged += MDataGrid_SelectionChanged;

            var mButtonPanel = new Panel();
            mButtonPanel.Dock = DockStyle.Bottom;
            var mBtnAdd = new Button();
            var mBtnDelete = new Button();
            var mBtnEdit = new Button();
            var mBtnQuery = new Button();

            mBtnAdd.Text = "添加";
            mBtnDelete.Text = "删除";
            mBtnEdit.Text = "修改";
            mBtnQuery.Text = "查询";

            mBtnAdd.Size = new System.Drawing.Size(60, 34);
            mBtnDelete.Size = new System.Drawing.Size(60, 34);
            mBtnEdit.Size = new System.Drawing.Size(60, 34);
            mBtnQuery.Size = new System.Drawing.Size(60, 34);

            mBtnAdd.Location = new System.Drawing.Point(8 + 70 * 1, 2);
            mBtnDelete.Location = new System.Drawing.Point(8 + 70 * 2, 2);
            mBtnEdit.Location = new System.Drawing.Point(8 + 70 * 3, 2);
            mBtnQuery.Location = new System.Drawing.Point(8 + 70 * 4, 2);

            mBtnAdd.Click += MBtnAdd_Click;
            mBtnDelete.Click += MBtnDelete_Click;
            mBtnEdit.Click += MBtnEdit_Click;
            mBtnQuery.Click += MBtnQuery_Click;

            mButtonPanel.Controls.Add(mBtnAdd);
            mButtonPanel.Controls.Add(mBtnDelete);
            mButtonPanel.Controls.Add(mBtnEdit);
            mButtonPanel.Controls.Add(mBtnQuery);

            mButtonPanel.Size = new System.Drawing.Size(80, 38);

            this.Controls.Add(mEditor.Control);
            this.Controls.Add(mButtonPanel);
            this.Controls.Add(mDataGrid);

            UpdateDatabase();
        }

        private void MBtnQuery_Click(object sender, EventArgs e)
        {
            InputForm form = new InputForm();
            if (form.ShowDialog() == DialogResult.OK) 
            {
                var selected = mDataGrid.Rows.Cast<DataGridViewRow>().FirstOrDefault(x => Object.Equals(x.Cells[0].Value, form.QueryId));
                if (selected == null)
                {
                    MessageBox.Show(this, "未找到指定id数据。");
                }
                else
                {
                    id = form.QueryId;
                    // 加载数据到编辑器
                    for (int i = 1; i < selected.Cells.Count; i++)
                    {
                        var cell = selected.Cells[i];
                        var column = mDataGrid.Columns[i];
                        var editor = mItemFields.First(x => x.Key.Command == column.DataPropertyName);
                        editor.Value.Value = cell.Value;
                    }

                    MessageBox.Show(this, "查询成功。");
                }
            }
        }

        private void MBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == -1)
                    throw new InvalidExpressionException("未选中有效数据。");

                var values = new Dictionary<string, string>();
                // 查找所有数据并组织成sql cmd
                foreach (var item in mItemFields)
                {
                    var value = item.Value.GetType().GetProperty("Value").GetValue(item.Value);
                    if (value != null)
                    {
                        var str = value.ToString();
                        if (string.IsNullOrWhiteSpace(str))
                        {
                            str = "DEFAULT";
                        }

                        values[item.Key.Name] = str;
                    }
                }

                Table.Database.ExecuteCommand($"UPDATE {Table.Name} SET {string.Join(", ", values.Select(x => $"{x.Key}='{x.Value}'"))} WHERE ID={id}");

                UpdateDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void MBtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (id == -1)
                    throw new InvalidExpressionException("未选中有效数据。");

                Table.Database.ExecuteCommand($"DELETE FROM {Table.Name} WHERE ID={id}");
                var selected = mDataGrid.Rows.Cast<DataGridViewRow>().FirstOrDefault(x => Object.Equals(x.Cells[0].Value, id));
                mDataGrid.Rows.Remove(selected);
                id = -1;

                foreach (var item in mItemFields)
                    item.Value.Value = default;

                UpdateDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void MBtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var values = new Dictionary<string, string>();
                // 查找所有数据并组织成sql cmd
                foreach (var item in mItemFields)
                {
                    var value = item.Value.GetType().GetProperty("Value").GetValue(item.Value);
                    if (value != null)
                    {
                        var str = value.ToString();
                        if (string.IsNullOrWhiteSpace(str))
                        {
                            str = "DEFAULT";
                        }

                        values[item.Key.Name] = $"'{str}'";
                    }
                }

                Table.Database.ExecuteCommand($"INSERT INTO {Table.Name}({string.Join(", ", values.Keys)}) VALUES({string.Join(", ", values.Values)})");

                UpdateDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void MDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (mDataGrid.SelectedRows.Count > 0) 
            {
                var selected = mDataGrid.SelectedRows[0];
                id = (int)selected.Cells[0].Value;
                // 加载数据到编辑器
                for (int i = 1; i < selected.Cells.Count; i++) 
                {
                    var cell = selected.Cells[i];
                    var column = mDataGrid.Columns[i];    
                    var editor = mItemFields.First(x => x.Key.Command == column.DataPropertyName);
                    editor.Value.Value = cell.Value;
                }
            }
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);

            mEditor.ComputeSize(ClientSize);
        }

        private void UpdateDatabase() 
        {
            var dataTable = new DataTable();
            var reader = Table.Database.GetTableReader(Table.Name);
            dataTable.Load(reader);
            mDataGrid.DataSource = dataTable;
            reader.Close();
        }
    }
}
