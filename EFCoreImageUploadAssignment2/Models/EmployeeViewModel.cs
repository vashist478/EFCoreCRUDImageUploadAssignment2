using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreImageUploadAssignment2.Models
{
    public class EmployeeViewModel
    {
        public int EmpId { get; set; }
        public int DeptId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string DepartmentName { get; set; }
        public string ImagePath { get; set; }
        public int ImageId { get; set; }

    }
}
