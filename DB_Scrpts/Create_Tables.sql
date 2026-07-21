create table Roles (
	RoleId int identity(1, 1),
	RoleName varchar(20) not null unique,
	constraint pk_Roles primary key (RoleId)
);

create table Departments(
	DepartmentId int identity(1, 1),
	DepartmentName varchar(100) not null unique,
	Description varchar(max),
	constraint pk_Departments primary key (DepartmentId)
);

create table LeaveType(
	LeaveTypeId int identity(1, 1),
	LeaveTypeName varchar(100) not null unique,
	MaxDaysPerYear int not null constraint chk_max_leave check (MaxDaysPerYear >= 0),
	IsPaid bit not null,
	constraint pk_LeaveType primary key (LeaveTypeId)
);

create table Holidays(
	HolidayId int identity(1, 1),
	HolidayName varchar(100) not null,
	HolidayDate Date not null unique,
	Description varchar(max),
	IsOptional bit default 1,
	constraint pk_HolidayId primary key (HolidayId)
);

create table Employees(
	EmployeeId varchar(200) not null,
	FirstName varchar(100) not null,
	LastName varchar(100) not null,
	Email varchar(200) not null unique,
	PhoneNumber varchar(15) not null unique,
	DateOfBirth Date not null,
	HireDate Date not null,
	DepartmentId int,
	RoleId int,
	ManagerId varchar(200),
	Salary decimal(12, 2) not null constraint chk_emp_salary check (Salary > 0.0),
	PasswordHash varchar(255) not null,
	IsActive bit not null default 1,
	CreatedAt DateTime default getdate(),
	UpdatedAt DateTime default getdate(),
	constraint pk_employee primary key (EmployeeId),
	constraint fk_employee_dept foreign key (DepartmentID) references Departments(DepartmentId),
	constraint fk_employee_role foreign key (RoleId) references Roles(RoleId),
	constraint fk_employee_manager foreign key (ManagerId) references Employees(EmployeeId),
	constraint chk_emp_hiredate check (HireDate >= DATEADD(year,18,DateOfBirth))
);

create table LeaveRequest(
	LeaveId varchar(200) not null,
	EmployeeId varchar(200) not null,
	LeaveTypeId int not null,
	StartDate Date not null,
	EndDate Date not null,
	LeaveReason varchar(max) not null,
	LeaveStatus varchar(100) not null default 'pending' constraint chk_LeaveStatus check (LeaveStatus in ('pending', 'approved', 'rejected')),
	ApprovedBy varchar(200),
	ApproveDate Date null,
	RejectionReason varchar(max),
	CreatedAt DateTime default getdate(),
	constraint pk_leaveRequest primary key (LeaveId),
	constraint fk_emp_leave foreign key (EmployeeId) references Employees(EmployeeId),
	constraint fk_leave_type foreign key (LeaveTypeId) references LeaveType(LeaveTypeId),
	constraint fk_approveby foreign key (ApprovedBy) references Employees(EmployeeId),
	constraint chk_dates check (StartDate <= EndDate)
);

create table LeaveBalance(
	LeaveBalanceId int identity(1, 1),
	EmployeeId varchar(200) not null,
	LeaveTypeId int not null,
	LeaveYear int not null default year(getdate()),
	AllocatedDays int not null constraint chk_allocatedDays check (AllocatedDays >= 0),
	UsedDays int not null default 0,
	constraint pk_leaveBalance primary key (LeaveBalanceId),
	constraint fk_emp_leave_balance foreign key (EmployeeId) references Employees(EmployeeId),
	constraint fk_emp_leave_type foreign key (LeaveTypeId) references LeaveType(LeaveTypeId),
	constraint uq_leaveBalance unique(EmployeeId, LeaveTypeId, LeaveYear),
	constraint chk_usedDays check (UsedDays <= AllocatedDays),
	constraint chk_leaveYear check (LeaveYear between 2000 and 2100)
);