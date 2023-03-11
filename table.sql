
create database teachgrade default character set utf8;
use teachgrade;

drop table   if exists   DEPT;
drop table   if exists   EMP;
drop table   if exists   ClassInf;
drop table   if exists   Student;
drop table   if exists   Competition;
drop table   if exists   Exhibition;
drop table   if exists   Training;
drop table   if exists   GraduProject;
drop table   if exists   PubPaper;
drop table   if exists   Patent;
drop table   if exists   TeachAward;
drop table   if exists   TeachReform;
drop table   if exists   TeachPaper;
drop table   if exists   TeachComp;
drop table   if exists   CourseConstruct;
drop table   if exists   TeachPlatTeam;
drop table   if exists   ProfConstruct;
drop table   if exists   PersonalAch;
drop table  if exists   AchInfo; 

/*单位表*/
create table DEPT(
    ID             int	  not null  PRIMARY KEY AUTO_INCREMENT,
    DEPTID         varchar(6)            not null unique comment '部门编号',
    DNAME          varchar(30)           not null comment '部门名称',
    LEADER         varchar(20)           null     comment '负责人',
    CONTRACT       varchar(30)           null     comment '联系人' ,
    TELEPHONE	   varchar(30)           null     comment '联系电话' ,
    ADDRESS        varchar(60)           null     comment '详细地址',
    index ind_DNAME(DNAME)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*教职工表*/
create table EMP(
    ID             int	  not null AUTO_INCREMENT PRIMARY KEY,
    EmpID          varchar(9)            not null comment  '教职工编号',
    EName          varchar(20)           null     comment '教职工姓名',
    PYSX	   varchar(12) 		 null 	  comment '姓名拼音缩写',
    DeptID         varchar(6)            not null comment '部门编号',
    LogID          varchar(20)           not null comment '登录ID',
    LogPWD         varchar(20)           not null comment '登录密码', 
    Birth          date                  null     comment '出生日期',
    Tele           varchar(30)           null     comment '手机',
    Sex            char(2)      default '男'         null     comment '性别',
    Schor          varchar(20)           null     comment '最高学历',
    Degree         varchar(20)           null     comment '最高学位',
    Prof	   varchar(30)	 	 null     comment '专业',
    Nation         varchar(20)           null     comment '籍贯',
    Vect          varchar(30)           null     comment '职称', 
    Qautor	   varchar(20)         	 null     comment '从事岗位',
    Jobdate        date                  null     comment '聘用日期', 
    IsAdmin        tinyint               not null comment '是否管理员，1或0',
    IsValid        tinyint               not null comment '是否有效,1或0',
    INDEX IND_DEPTID(DeptID),
    UNIQUE uni_empid(EmpID)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
 
/*班级表*/ 
create table ClassInf(
    ID             int	  not null PRIMARY KEY AUTO_INCREMENT,
    ClassID         varchar(6)            not null unique comment '班级编号',
    ClassName          varchar(30)           not null comment '班级名称',
    LEADER         varchar(20)           null     comment '班主任',
    Monitor        varchar(20)           null     comment '班长' ,
    Telephone	   varchar(30)           null     comment '联系电话' ,
    Prof	   varchar(30)	 	 null     comment '专业',
    index ind_ClassName(ClassName)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


/*学生表*/ 
create table Student(
    ID             int	  not null PRIMARY KEY  AUTO_INCREMENT,
    StudID          varchar(9)            not null  comment '学号',
    SName          varchar(20)           null     comment '学生姓名',
    PYSX	   varchar(12) 		 null 	  comment '姓名拼音缩写',
    Sex	   char(2)   default '男'	 null 	  comment '性别',
    ClassID         varchar(6)            not null comment '班级编号', 
    Birth          date                  null     comment '出生日期',
    Tele           varchar(30)           null     comment '手机', 
    Nation         varchar(20)           null     comment '籍贯',  
    INDEX IND_classID(classID),
    UNIQUE  uni_studid(studID)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*指导创新创业竞赛*/
CREATE table Competition(
    ID             int   not null PRIMARY KEY AUTO_INCREMENT,
   CompName  varchar(50) not null comment '大赛名称',
   Complvl  varchar(20)  comment '竞赛等级，包括I级甲等、I级乙等、I级丙等、II级甲等、II级乙等及以下5类',
   Title  varchar(50) not null  comment '作品名称',
   Awardlvl  varchar(20)  comment '获奖等级，包括特等奖、一等奖、二等奖、三等奖、优秀奖、有效参赛6类',
   Members   varchar(50) comment '获奖成员',
   FirTeachID  varchar(9)             not null comment '第一指导老师',
   SecTeachID varchar(9)             not null comment '第二指导老师',
   GetTime    Date  comment  '取得时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_CompName(CompName),
     index ind_Title(Title)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*指导创新创业成果展*/ 
 CREATE table Exhibition(
    ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   ExhibName  varchar(50) not null comment '展会名称',
   Exhiblvl  varchar(20)  comment '展会等级，包括国家级、省部级、市厅级、校院级4类',
   Title  varchar(50) not null  comment '作品名称',
   Awardlvl  varchar(20)  comment '获奖等级，包括特等奖、一等奖、二等奖、三等奖、优秀奖、有效参赛6类',
   Members   varchar(50) comment '获奖成员',
   FirTeachID   varchar(9)            not null comment '第一指导老师',
   SecTeachID  varchar(9)            not null comment '第二指导老师',
   GetTime    Date  comment  '取得时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_ExhibName(ExhibName),
  index ind_Title(Title)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
	 
/*指导创新训练项目*/ 
 CREATE table Training(
    ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   TrainName  varchar(50) not null comment '项目名称',
   Trainlvl  varchar(20)  comment '项目等级，包括国家级、省部级、市厅级、校院级4类',
   Title  varchar(50) not null  comment '申报项目名称',
   Approval  varchar(20)  comment '获批情况，包括获批、结题、有效申报3类',
   Members   varchar(50) comment '获奖成员',
   FirTeachID   varchar(9)            not null comment '第一指导老师',
   SecTeachID  varchar(9)            not null comment '第二指导老师',
   GetTime    Date  comment  '取得时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_TrainName(TrainName),
   index ind_Title(Title)
)ENGINE=InnoDB DEFAULT CHARSET=utf8; 	 		

/*优秀毕业设计*/ 
 CREATE table GraduProject(
    ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   GraduName  varchar(100) not null comment '毕业设计名称',
   Gradulvl  varchar(20)  comment '奖项等级，包括国家级、省部级、市厅级、校院级4类',
   Depart  varchar(50) not null  comment '授奖部门',
   Awardlvl  varchar(20)  comment '获奖等级，包括特等奖、一等奖、二等奖、三等奖、优秀奖、有效参赛6类',
   Members   varchar(50) comment '获奖成员',
   FirTeachID   varchar(9)            not null comment '第一指导老师',
   SecTeachID  varchar(9)            not null comment '第二指导老师',
   GetTime    Date  comment  '取得时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_GraduName(GraduName)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
	 
/*指导本科生发表论文*/ 
 CREATE table PubPaper(
    ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   Title  varchar(120) not null comment '论文题目',
   Employ  varchar(20) not null  comment '收录情况，包括SCI期刊、EI期刊、EI会议、中文核心、一般刊物',
   Journal   varchar(100) not null comment '期刊名称',
   VolIssue varchar(20)  comment '卷期',
    Pages   varchar(20)  comment '页码范围',
   Members   varchar(50) comment '作者成员',
   FirTeachID   varchar(9)            not null comment '第一指导老师',
   SecTeachID  varchar(9)            not null comment '第二指导老师',
   PubTime    Date  comment  '发表时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_PubTitle(Title)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;							
 							
/*指导本科生申请专利*/ 
 CREATE table Patent(
    ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   PatentTitle  varchar(120) not null comment '专利名称',
   PStatus  varchar(20) not null  comment '专业状态，包括发明专利申请、发明专利授权、实用新型申请、实用新型授权4种',
   PatentMan  varchar(100) not null comment '专利权人',
   ApplyID varchar(20)  comment '申请号', 
   Members   varchar(50) comment '发明人',
   FirTeachID   varchar(9)            not null comment '第一指导老师',
   SecTeachID  varchar(9)            not null comment '第二指导老师',
   AppAndAuthTime    Date  comment  '申报或授权时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_PatentTitle(PatentTitle)
)ENGINE=InnoDB DEFAULT CHARSET=utf8; 			


/*教学成果奖*/ 
 CREATE table TeachAward(
    ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   AwardName  varchar(100) not null comment '获奖项目名称',
   Teachlvl  varchar(20)  comment '奖项等级，包括国家级、省部级、市厅级、校院级、培育项目5类',
   Depart  varchar(50) not null  comment '授奖部门',
   Awardlvl  varchar(20)  comment '获奖等级，包括特等奖、一等奖、二等奖、三等奖、优秀奖、有效参评6类',
   Members   varchar(60) comment '获奖成员',
   DeptSorts  varchar(40) not null comment '单位排序，包括南工程第一单位，南工程非第一单位2类',
   ApplyTime    Date  comment  '申报时间',
   AwardTime    Date  comment  '授奖时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_AwardName(AwardName)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;										
 

/*教研教改项目*/ 
 CREATE table TeachReform(
    ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   ReformName  varchar(100) not null comment '项目名称',
   Reformlvl  varchar(20)  comment '项目等级，包括国家级、省部级、国家一级学会、市厅级、校院级5类',
   Depart  varchar(50) not null  comment ' 立项部门',
   Awardlvl  varchar(20)  comment '获批情况，包括获批、结题、有效申报3类',
   Members   varchar(60) comment '获奖成员',
   DeptSorts  varchar(40)  not null comment '单位排序，包括南工程第一单位，南工程非第一单位2类',
   DeclareTime    Date  comment  '申报时间',
   AwardTime      Date  comment  '授奖时间',
   ApproveTime   Date  comment  '批准立项时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_ReformName(ReformName)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;										
						 
/*发表教研教改论文*/ 
 CREATE table TeachPaper(
    ID      int   not null AUTO_INCREMENT PRIMARY KEY,
   Title  varchar(120) not null comment '论文题目',
   Employ  varchar(20) not null  comment '收录情况，包括核心期刊、省级期刊、一般刊物',
   Journal   varchar(100) not null comment '期刊名称',
   VolIssue varchar(20)  comment '卷期',
    Pages   varchar(20)  comment '页码范围',
   Members   varchar(50) comment '作者成员',
   FirstAuthor   varchar(9)            not null comment '第一作者', 
   PubTime    Date  comment  '发表时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_TeachTitle(Title)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;	
 	 
	
 
/*教学竞赛获奖*/
CREATE table TeachComp(
    ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   CompName  varchar(50) not null comment '获奖项目(课程)名称',
   Complvl  varchar(20)  comment '奖项等级，省部级或同层次学会（协会）、校院级 2类',
   Depart  varchar(50) not null  comment '授奖部门',
    Awardlvl varchar(20)  comment '获奖等级，包括特等奖、一等奖、二等奖、三等奖、优秀奖、有效参赛6类',
   Members   varchar(50) comment '获奖成员', 
   AwardTime    Date  comment  '获奖时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_CompName(CompName)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;
					
 /*课程与教材建设、虚拟仿真、在线课程等*/
CREATE table CourseConstruct(
    ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   CourseName  varchar(50) not null comment '课程名称',
   Itemlvl  varchar(20)  comment '项目等级，包括国家级、省部级、校院级 3类',
   Depart  varchar(50) not null  comment '立项部门',
   Awardlvl varchar(20)  comment '获批状态，包括获批、结题、有效申报3类',
   Members   varchar(50) comment '课程组成员名单', 
   AwardTime    Date  comment  '申报时间',
   ApproveTime   Date  comment  '批准时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_CourseName(CourseName)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;  

 /*教学平台与教学团队*/
CREATE table TeachPlatTeam(
    ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   PlatName  varchar(50) not null comment '平台或团队名称',
   Platlvl  varchar(20)  comment '平台或团队等级，包括国家级、省部级、校院级 3类',
   Depart  varchar(50) not null  comment '主管（批准建设）部门',
   Awardlvl varchar(20)  comment '获批状态，包括获批、结题、有效申报3类',
   Members   varchar(50) comment '课程组成员名单', 
   PtType   varchar(10) comment '类型，包括：平台、团队2个类型',
   AwardTime    Date  comment  '申报时间',
   ApproveTime   Date  comment  '批准时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_PlatName(PlatName)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;  
 								
/*专业建设*/
CREATE table ProfConstruct(
    ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   ProfName  varchar(50) not null comment '专业名称',
   Proflvl  varchar(20)  comment '专业等级，包括国家级、省部级、校院级 3类',
   Depart  varchar(50) not null  comment '主管（批准建设）部门',
   AppStatus  varchar(20)  comment '获批状态，包括获批、结题、有效申报3类',
   Members   varchar(50) comment '课程组成员名单', 
   AwardTime    Date  comment  '申报时间',
   ApproveTime   Date  comment  '批准时间',
   Score  numeric(5,2) comment '总分',
   Mark varchar(50) comment '备注',
   index ind_ProfName(ProfName)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;  	


/*个人业绩登记*/
CREATE table PersonalAch(
   ID             int   not null AUTO_INCREMENT PRIMARY KEY,
   EmpID          varchar(9)      not null comment '教职工编号',
   AchTypeID  int(2) not null comment '业绩类型编号',
   AchieveID  int  comment '具体业绩编号，如发表论文ID、竞赛ID、专利ID等',
   sort	 int  comment '排名',
   CalcYear   int(4)  comment  '业绩取得年份',
   Score  numeric(5,2) comment '个人得分',
   Mark varchar(50) comment '备注',
   index ind_EID(EmpID)
)ENGINE=InnoDB DEFAULT CHARSET=utf8; 									

/*教学业绩基础表*/
CREATE table AchInfo(
   ID      int   not null AUTO_INCREMENT PRIMARY KEY, 
   AchTypeID  int(2) not null comment '业绩类型编号，见文后17类，可用10、11、12等分类',
   AchContent varchar(60)  comment '业绩名称',  
   CalcRule varchar(100) comment '计算规则说明',
   unique ind_AchTypeID(AchTypeID)
)ENGINE=InnoDB DEFAULT CHARSET=utf8; 	
 
				
