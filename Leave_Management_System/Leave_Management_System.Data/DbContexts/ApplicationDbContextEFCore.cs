using Leave_Management_System.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Leave_Management_System.Data.DbContexts
{
    public class ApplicationDbContextEFCore : DbContext
    {
        public ApplicationDbContextEFCore(DbContextOptions<ApplicationDbContextEFCore> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Holidays> Holidays { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LeaveRequest>()
               .HasOne(l => l.Employee)
               .WithMany(e => e.LeaveRequests)
               .HasForeignKey(l => l.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<LeaveRequest>()
                .HasOne(l => l.Approval)
                .WithMany()
                .HasForeignKey(l => l.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            List<Department> departments = new List<Department>
            {
                new Department
                {
                    DepartmentId = 1,
                    DepartmentName = ".NET",
                    Description = "Development team working on .NET applications and services."
                },
                new Department
                {
                    DepartmentId = 2,
                    DepartmentName = "Python",
                    Description = "Development team responsible for Python-based applications and APIs."
                },
                new Department
                {
                    DepartmentId = 3,
                    DepartmentName = "React",
                    Description = "Frontend development team using React.js."
                },
                new Department
                {
                    DepartmentId = 4,
                    DepartmentName = "Angular",
                    Description = "Frontend development team using Angular framework."
                },
                new Department
                {
                    DepartmentId = 5,
                    DepartmentName = "React Native",
                    Description = "Mobile application development team using React Native."
                },
                new Department
                {
                    DepartmentId = 6,
                    DepartmentName = "QA",
                    Description = "Quality Assurance team responsible for testing and quality control."
                },
                new Department
                {
                    DepartmentId = 7,
                    DepartmentName = "DevOps",
                    Description = "Infrastructure, CI/CD, cloud deployment, and automation team."
                },
                new Department
                {
                    DepartmentId = 8,
                    DepartmentName = "PHP",
                    Description = "Development team responsible for PHP-based web applications."
                },
            };

            List<Holidays> holidays = new List<Holidays>
            {
                new Holidays
                {
                    HolidayId = 1,
                    HolidayName = "Makar Sankranti",
                    HolidayDate = new DateTime(2026, 1, 14),
                    Description = "Harvest festival celebrated across India.",
                    IsOptional = false
                },
                new Holidays
                {
                    HolidayId = 2,
                    HolidayName = "Maha Shivratri",
                    HolidayDate = new DateTime(2026, 2, 15),
                    Description = "Festival dedicated to Lord Shiva.",
                    IsOptional = false
                },
                new Holidays
                {
                    HolidayId = 3,
                    HolidayName = "Holika Dahan",
                    HolidayDate = new DateTime(2026, 3, 3),
                    Description = "Bonfire marking the victory of good over evil.",
                    IsOptional = true
                },
                new Holidays
                {
                    HolidayId = 4,
                    HolidayName = "Holi (Dhulandi)",
                    HolidayDate = new DateTime(2026, 3, 4),
                    Description = "Festival of Colors.",
                    IsOptional = false
                },
                new Holidays
                {
                    HolidayId = 5,
                    HolidayName = "Ram Navami",
                    HolidayDate = new DateTime(2026, 3, 27),
                    Description = "Birth anniversary of Lord Rama.",
                    IsOptional = false
                },
                new Holidays
                {
                    HolidayId = 6,
                    HolidayName = "Hanuman Jayanti",
                    HolidayDate = new DateTime(2026, 4, 2),
                    Description = "Birth anniversary of Lord Hanuman.",
                    IsOptional = true
                },
                new Holidays
                {
                    HolidayId = 7,
                    HolidayName = "Akshaya Tritiya",
                    HolidayDate = new DateTime(2026, 4, 20),
                    Description = "Auspicious day for prosperity and new beginnings.",
                    IsOptional = true
                },
                new Holidays
                {
                    HolidayId = 8,
                    HolidayName = "Guru Purnima",
                    HolidayDate = new DateTime(2026, 7, 29),
                    Description = "Festival honoring spiritual and academic teachers.",
                    IsOptional = true
                },
                new Holidays
                {
                    HolidayId = 9,
                    HolidayName = "Raksha Bandhan",
                    HolidayDate = new DateTime(2026, 8, 28),
                    Description = "Celebration of the bond between brothers and sisters.",
                    IsOptional = true
                },
                new Holidays
                {
                    HolidayId = 10,
                    HolidayName = "Janmashtami",
                    HolidayDate = new DateTime(2026, 9, 3),
                    Description = "Birth anniversary of Lord Krishna.",
                    IsOptional = false
                },
                new Holidays
                {
                    HolidayId = 11,
                    HolidayName = "Ganesh Chaturthi",
                    HolidayDate = new DateTime(2026, 9, 14),
                    Description = "Birth of Lord Ganesha.",
                    IsOptional = false
                },
                new Holidays
                {
                    HolidayId = 12,
                    HolidayName = "Sharad Navratri Begins",
                    HolidayDate = new DateTime(2026, 10, 11),
                    Description = "Beginning of the nine-day festival dedicated to Goddess Durga.",
                    IsOptional = true
                },
                new Holidays
                {
                    HolidayId = 13,
                    HolidayName = "Durga Ashtami",
                    HolidayDate = new DateTime(2026, 10, 18),
                    Description = "Important day during Navratri.",
                    IsOptional = true
                },
                new Holidays
                {
                    HolidayId = 14,
                    HolidayName = "Dussehra (Vijayadashami)",
                    HolidayDate = new DateTime(2026, 10, 20),
                    Description = "Victory of Lord Rama over Ravana.",
                    IsOptional = false
                },
                new Holidays
                {
                    HolidayId = 15,
                    HolidayName = "Karva Chauth",
                    HolidayDate = new DateTime(2026, 10, 29),
                    Description = "Festival observed by married Hindu women.",
                    IsOptional = true
                },
                new Holidays
                {
                    HolidayId = 16,
                    HolidayName = "Dhanteras",
                    HolidayDate = new DateTime(2026, 11, 8),
                    Description = "Beginning of the Diwali festivities.",
                    IsOptional = true
                },
                new Holidays
                {
                    HolidayId = 17,
                    HolidayName = "Naraka Chaturdashi",
                    HolidayDate = new DateTime(2026, 11, 9),
                    Description = "Also known as Choti Diwali.",
                    IsOptional = true
                },
                new Holidays
                {
                    HolidayId = 18,
                    HolidayName = "Diwali (Lakshmi Puja)",
                    HolidayDate = new DateTime(2026, 11, 10),
                    Description = "Festival of Lights.",
                    IsOptional = false
                },
                new Holidays
                {
                    HolidayId = 19,
                    HolidayName = "Govardhan Puja",
                    HolidayDate = new DateTime(2026, 11, 11),
                    Description = "Celebration of Lord Krishna lifting Govardhan Hill.",
                    IsOptional = true
                },
                new Holidays
                {
                    HolidayId = 20,
                    HolidayName = "Bhai Dooj",
                    HolidayDate = new DateTime(2026, 11, 12),
                    Description = "Celebration of the bond between brothers and sisters.",
                    IsOptional = true
                }
            };

            var leaveTypes = new List<LeaveType>
            {
                new LeaveType
                {
                    LeaveTypeId = 1,
                    LeaveTypeName = "Sick Leave",
                    MaxDaysPerYear = 6,
                    IsPaid = true
                },
                new LeaveType
                {
                    LeaveTypeId = 2,
                    LeaveTypeName = "Employee Leave (EL)",
                    MaxDaysPerYear = 12,
                    IsPaid = true
                },
                new LeaveType
                {
                    LeaveTypeId = 3,
                    LeaveTypeName = "Flexible Festival Leave (FFL)",
                    MaxDaysPerYear = 1,
                    IsPaid = true
                },
                new LeaveType
                {
                    LeaveTypeId = 4,
                    LeaveTypeName = "Management Leave (ML)",
                    MaxDaysPerYear = 3,
                    IsPaid = true
                }
            };

            var roles = new List<Role>
            {
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "HR" },
                new Role { RoleId = 3, RoleName = "Manager" },
                new Role { RoleId = 4, RoleName = "Employee" }
            };

            modelBuilder.Entity<Department>().HasData(departments);
            modelBuilder.Entity<Holidays>().HasData(holidays);
            modelBuilder.Entity<LeaveType>().HasData(leaveTypes);
            modelBuilder.Entity<Role>().HasData(roles);

        }

    }
}
