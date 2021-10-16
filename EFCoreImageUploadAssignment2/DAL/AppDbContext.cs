using EFCoreImageUploadAssignment2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreImageUploadAssignment2.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }


        public DbSet<Image> Image { get; set; }


        /// <summary>
        /// SK: Get employee records based on employee Id
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public Employee usp_GetEmployee(int EmployeeId)
        {
            Employee employee = new Employee();

            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "GetEmployeeDetails";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter("EmployeeId", EmployeeId);
                command.Parameters.Add(parameter);
                Database.GetDbConnection().Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    employee.EmpId = reader.GetInt32("EmpId");
                    employee.Name = reader.GetString("Name");
                    employee.Address = reader.GetString("Address");
                    employee.DeptId = reader.GetInt32("DeptId");
                    employee.ImageId = reader.GetInt32("ImageId");
                }
                reader.Close();
                
            }
            Database.CloseConnection();
            return employee;

        }


        public int UpdateImage(int ImageId, string ImageName, string ImagePath)
        {
            using (var command = Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "UpdateImage";
                command.CommandType = System.Data.CommandType.StoredProcedure;
       
                SqlParameter parameter1 = new SqlParameter("@ImageId", ImageId);
                SqlParameter parameter2 = new SqlParameter("@ImageName", ImageName);
                SqlParameter parameter3 = new SqlParameter("@ImagePath", ImagePath);
                
                command.Parameters.Add(parameter1);
                command.Parameters["@ImageId"].Direction = ParameterDirection.Output;
                command.Parameters.Add(parameter2);
                command.Parameters.Add(parameter3);

                Database.GetDbConnection().Open();
                var reader = command.ExecuteNonQuery();
             
                int _ImageId = Convert.ToInt32(command.Parameters["@ImageId"].Value);

                
              
                Database.CloseConnection();

                return _ImageId;
            }
        }
    }
}
