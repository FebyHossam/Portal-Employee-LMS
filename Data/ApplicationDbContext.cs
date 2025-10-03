using Leave_Mangement_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Leave_Mangement_System.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeavePolicy> LeavePolicies { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(e => e.Department)
                       .WithMany(d => d.Employees)
                       .HasForeignKey(e => e.DeptId)
                       .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(e => e.LeavePolicy)
                      .WithMany(p => p.Employees)
                      .HasForeignKey(e => e.PolicyId)
                      .OnDelete(DeleteBehavior.Restrict);

                 entity.HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            });


            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.HasOne(lr => lr.Employee)
                     .WithMany(e => e.LeaveRequests)
                     .HasForeignKey(lr => lr.EmpId)
                     .OnDelete(DeleteBehavior.Cascade);

            });


            SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
               new Department { DeptId = 1, Name = "Human Resources" },
               new Department { DeptId = 2, Name = "Information Technology" },
               new Department { DeptId = 3, Name = "Finance" },
               new Department { DeptId = 4, Name = "Marketing" },
               new Department { DeptId = 5, Name = "Operations" },
               new Department { DeptId = 6, Name = "Sales" }
           );

            modelBuilder.Entity<LeavePolicy>().HasData(
                new LeavePolicy { PolicyId = 1, PolicyName = "Standard Policy", RegularDays = 21, ExceptionDays = 5 },
                new LeavePolicy { PolicyId = 2, PolicyName = "Senior Policy", RegularDays = 30, ExceptionDays = 10 },
                new LeavePolicy { PolicyId = 3, PolicyName = "Management Policy", RegularDays = 35, ExceptionDays = 15 },
                new LeavePolicy { PolicyId = 4, PolicyName = "Executive Policy", RegularDays = 40, ExceptionDays = 20 }
            );

            modelBuilder.Entity<Employee>().HasData(
               new Employee
               {
                   EmpId = 1,
                   EmployeeName = "Ahmed Hassan",
                   Email = "ahmed.hassan@company.com",
                   PhoneNumber = "01234567890",
                   Address = "Cairo, Egypt",
                   JobTitle = "HR Manager",
                   DeptId = 1,
                   Manager = true,
                   Hr = true,
                   LeaveBalance = 35,
                   ManagerName = null,
                   ManagerEmail = null,
                   PolicyId = 3
               },
               new Employee
               {
                   EmpId = 2,
                   EmployeeName = "Fatma Mohamed",
                   Email = "fatma.mohamed@company.com",
                   PhoneNumber = "01234567891",
                   Address = "Giza, Egypt",
                   JobTitle = "HR Specialist",
                   DeptId = 1,
                   Manager = false,
                   Hr = true,
                   LeaveBalance = 28,
                   ManagerName = "Ahmed Hassan",
                   ManagerEmail = "ahmed.hassan@company.com",
                   PolicyId = 2
               },
               new Employee
               {
                   EmpId = 3,
                   EmployeeName = "Omar Ali",
                   Email = "omar.ali@company.com",
                   PhoneNumber = "01234567892",
                   Address = "Alexandria, Egypt",
                   JobTitle = "IT Manager",
                   DeptId = 2,
                   Manager = true,
                   Hr = false,
                   LeaveBalance = 30,
                   ManagerName = null,
                   ManagerEmail = null,
                   PolicyId = 3
               },
               new Employee
               {
                   EmpId = 4,
                   EmployeeName = "Mona Ibrahim",
                   Email = "mona.ibrahim@company.com",
                   PhoneNumber = "01234567893",
                   Address = "Cairo, Egypt",
                   JobTitle = "Software Developer",
                   DeptId = 2,
                   Manager = false,
                   Hr = false,
                   LeaveBalance = 21,
                   ManagerName = "Omar Ali",
                   ManagerEmail = "omar.ali@company.com",
                   PolicyId = 1
               },
               new Employee
               {
                   EmpId = 5,
                   EmployeeName = "Khaled Mahmoud",
                   Email = "khaled.mahmoud@company.com",
                   PhoneNumber = "01234567894",
                   Address = "Mansoura, Egypt",
                   JobTitle = "Senior Developer",
                   DeptId = 2,
                   Manager = false,
                   Hr = false,
                   LeaveBalance = 25,
                   ManagerName = "Omar Ali",
                   ManagerEmail = "omar.ali@company.com",
                   PolicyId = 2
               },
               new Employee
               {
                   EmpId = 6,
                   EmployeeName = "Sara Ahmed",
                   Email = "sara.ahmed@company.com",
                   PhoneNumber = "01234567895",
                   Address = "Cairo, Egypt",
                   JobTitle = "Finance Manager",
                   DeptId = 3,
                   Manager = true,
                   Hr = false,
                   LeaveBalance = 32,
                   ManagerName = null,
                   ManagerEmail = null,
                   PolicyId = 3
               },
               new Employee
               {
                   EmpId = 7,
                   EmployeeName = "Mohamed Youssef",
                   Email = "mohamed.youssef@company.com",
                   PhoneNumber = "01234567896",
                   Address = "Giza, Egypt",
                   JobTitle = "Accountant",
                   DeptId = 3,
                   Manager = false,
                   Hr = false,
                   LeaveBalance = 18,
                   ManagerName = "Sara Ahmed",
                   ManagerEmail = "sara.ahmed@company.com",
                   PolicyId = 1
               },
               new Employee
               {
                   EmpId = 8,
                   EmployeeName = "Nour Kamal",
                   Email = "nour.kamal@company.com",
                   PhoneNumber = "01234567897",
                   Address = "Alexandria, Egypt",
                   JobTitle = "Marketing Manager",
                   DeptId = 4,
                   Manager = true,
                   Hr = false,
                   LeaveBalance = 28,
                   ManagerName = null,
                   ManagerEmail = null,
                   PolicyId = 3
               },
               new Employee
               {
                   EmpId = 9,
                   EmployeeName = "Amr Salah",
                   Email = "amr.salah@company.com",
                   PhoneNumber = "01234567898",
                   Address = "Cairo, Egypt",
                   JobTitle = "Marketing Specialist",
                   DeptId = 4,
                   Manager = false,
                   Hr = false,
                   LeaveBalance = 22,
                   ManagerName = "Nour Kamal",
                   ManagerEmail = "nour.kamal@company.com",
                   PolicyId = 1
               },
               new Employee
               {
                   EmpId = 10,
                   EmployeeName = "Heba Mostafa",
                   Email = "heba.mostafa@company.com",
                   PhoneNumber = "01234567899",
                   Address = "Tanta, Egypt",
                   JobTitle = "Operations Manager",
                   DeptId = 5,
                   Manager = true,
                   Hr = false,
                   LeaveBalance = 35,
                   ManagerName = null,
                   ManagerEmail = null,
                   PolicyId = 3
               },
               new Employee
               {
                   EmpId = 11,
                   EmployeeName = "Tamer Rashad",
                   Email = "tamer.rashad@company.com",
                   PhoneNumber = "01234567800",
                   Address = "Cairo, Egypt",
                   JobTitle = "Operations Specialist",
                   DeptId = 5,
                   Manager = false,
                   Hr = false,
                   LeaveBalance = 19,
                   ManagerName = "Heba Mostafa",
                   ManagerEmail = "heba.mostafa@company.com",
                   PolicyId = 1
               },
               new Employee
               {
                   EmpId = 12,
                   EmployeeName = "Yasmin Fouad",
                   Email = "yasmin.fouad@company.com",
                   PhoneNumber = "01234567801",
                   Address = "Alexandria, Egypt",
                   JobTitle = "Sales Manager",
                   DeptId = 6,
                   Manager = true,
                   Hr = false,
                   LeaveBalance = 30,
                   ManagerName = null,
                   ManagerEmail = null,
                   PolicyId = 2
               },
               new Employee
               {
                   EmpId = 13,
                   EmployeeName = "Hassan Gamal",
                   Email = "hassan.gamal@company.com",
                   PhoneNumber = "01234567802",
                   Address = "Cairo, Egypt",
                   JobTitle = "Sales Representative",
                   DeptId = 6,
                   Manager = false,
                   Hr = false,
                   LeaveBalance = 15,
                   ManagerName = "Yasmin Fouad",
                   ManagerEmail = "yasmin.fouad@company.com",
                   PolicyId = 1
               },
               new Employee
               {
                   EmpId = 14,
                   EmployeeName = "Rana Tarek",
                   Email = "rana.tarek@company.com",
                   PhoneNumber = "01234567803",
                   Address = "Giza, Egypt",
                   JobTitle = "Senior Sales Representative",
                   DeptId = 6,
                   Manager = false,
                   Hr = false,
                   LeaveBalance = 26,
                   ManagerName = "Yasmin Fouad",
                   ManagerEmail = "yasmin.fouad@company.com",
                   PolicyId = 2
               },
               
               new Employee
               {
                   EmpId = 15,
                   EmployeeName = "Mohamed",
                   Email = "mohamed@gmail.com",
                   PhoneNumber = "01028742819",
                   Address = "edgetgase",
                   JobTitle = "Software Developer",
                   DeptId = 5,
                   Manager = false,
                   Hr = false,
                   LeaveBalance = 3,
                   ManagerName = "Heba Mostafa",
                   ManagerEmail = "heba.mostafa@company.com",
                   PolicyId = 1
               }
           );

           
            modelBuilder.Entity<LeaveRequest>().HasData(
                 new LeaveRequest
                 {
                     RequestId = 1,
                     EmpId = 4,
                     StartDate = new DateTime(2024, 12, 15),
                     EndDate = new DateTime(2024, 12, 19),
                     NumberOfDays = 5,
                     LeaveType = "Annual",
                     HrApproved = true,
                     ManagerApproved = true,
                     IsRejected = false
                 },
                 new LeaveRequest
                 {
                     RequestId = 2,
                     EmpId = 7,
                     StartDate = new DateTime(2025, 1, 5),
                     EndDate = new DateTime(2025, 1, 7),
                     NumberOfDays = 3,
                     LeaveType = "Sick",
                     HrApproved = false,
                     ManagerApproved = true,
                     IsRejected = false
                 },
                 new LeaveRequest
                 {
                     RequestId = 3,
                     EmpId = 9,
                     StartDate = new DateTime(2025, 2, 1),
                     EndDate = new DateTime(2025, 2, 10),
                     NumberOfDays = 10,
                     LeaveType = "Annual",
                     HrApproved = false,
                     ManagerApproved = false,
                     IsRejected = true
                 },
                 new LeaveRequest
                 {
                     RequestId = 4,
                     EmpId = 11,
                     StartDate = new DateTime(2025, 3, 15),
                     EndDate = new DateTime(2025, 3, 20),
                     NumberOfDays = 6,
                     LeaveType = "Casual",
                     HrApproved = false,
                     ManagerApproved = false,
                     IsRejected = false
                 },
                 new LeaveRequest
                 {
                     RequestId = 5,
                     EmpId = 13,
                     StartDate = new DateTime(2025, 1, 20),
                     EndDate = new DateTime(2025, 1, 22),
                     NumberOfDays = 3,
                     LeaveType = "Annual",
                     HrApproved = true,
                     ManagerApproved = true,
                     IsRejected = false
                 },
                 new LeaveRequest
                 {
                     RequestId = 6,
                     EmpId = 5,
                     StartDate = new DateTime(2025, 4, 1),
                     EndDate = new DateTime(2025, 4, 5),
                     NumberOfDays = 5,
                     LeaveType = "Emergency",
                     HrApproved = false,
                     ManagerApproved = true,
                     IsRejected = false
                 }
             );
        }


    } 
}