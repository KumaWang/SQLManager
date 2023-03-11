using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Manager
{
    public partial class Form1 : Form
    {
		const string LEVEL = @"
系统管理
	用户注册:LOG-用户注册
	用户信息修改:LOG-用户信息修改
	用户登录:LOG-用户登录
	系统退出:LOG-系统退出
基础数据管理
	单位信息管理:SQL-DEPT
	职工信息管理:SQL-EMP
	班级信息管理:SQL-ClassInf
	学生信息管理:SQL-Student
	教学业绩项目设置:SQL-AchInfo
教学业绩管理
	指导创新创业竞赛:SQL-Competition
	指导创新创业成果展:SQL-Exhibition
	指导创新训练项目:SQL-Training
	指导优秀毕业设计:SQL-GraduProject
	指导本科生发表论文:SQL-PubPaper
	指导本科生申请专利:SQL-Patent
	教学成果获奖:SQL-TeachAward
	参加教研教改项目:SQL-TeachReform
	发表教研教改论文:SQL-TeachPaper
	教学竞赛获奖:SQL-TeachComp
	课程与教材建设:SQL-CourseConstruct
	教学平台与团队:SQL-TeachPlatTeam
	专业建设:SQL-ProfConstruct
教学业绩查询与统计
	个人教学业绩查询:LOG-个人教学业绩查询
	部门职工情况查询:LOG-部门职工情况查询
	部门教学业绩统计:LOG-部门教学业绩统计";

		private Control mRight;

		public Form1()
        {
            InitializeComponent();

            SQLDatabase database = new SQLDatabase("teachgrade", "root", "x5");
     
			var tree = new TreeView();
            tree.AfterSelect += Tree_AfterSelect;
			tree.Dock = DockStyle.Left;
			tree.Width = 360;

			var level = 0;
			TreeNodeCollection parent = tree.Nodes;
			TreeNodeCollection last = tree.Nodes;

			var sr = new StringReader(LEVEL);
			var line = sr.ReadLine();   // 去掉头行
			while ((line = sr.ReadLine()) != null)
			{
				// 读取\t判断分级
				var ctx = line.TrimStart('\t');
				var lev = line.Length - ctx.Length;

				// 读取:后命令
				Control page = null;
				var cmd = ctx.Split(':');
				if (cmd.Length > 1)
				{
					var ccc = cmd[1];
					var cc2 = ccc.Split('-');
					var type = cc2[0];
					var arg = cc2[1];

					switch (type)
					{
						case "SQL":
							page = SQLFactory.CreateEditor(database.GetTable(arg));
							break;
						case "LOG":
							break;
					}
				}

				// 生成node
				var node = new TreeNode(cmd[0]);
				node.Tag = page;
				
				if (lev > level)
					parent = last;

				if (lev < level)
					parent = parent[0].Parent.Parent?.Nodes ?? tree.Nodes;

				parent.Add(node);

				level = lev;
				last = node.Nodes;
			}

			mRight = new Control();
			mRight.Dock = DockStyle.Fill;

			this.Controls.Add(mRight);
			this.Controls.Add(tree);

			this.Size = new System.Drawing.Size(1366, 758);
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
			mRight.Controls.Clear();
			var node = e.Node;
			if (node != null)
			{
				var page = node.Tag as Control;
				if (page != null)
				{
					page.Dock = DockStyle.Fill;
					mRight.Controls.Add(page);
					mRight.Invalidate();
				}
			}
		}

    }
}
