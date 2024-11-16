using BudgetManagement.Controllers;
using BudgetManagement.Models;
using BudgetManagement.Models.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BudgetManagement.Services
{
    public class CommonService
    {
        public BMSContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public CommonService(BMSContext context)
        {
            _context = context;
        }
        //public AuthorisedUser ObjUser { get; set; }
        public async Task<Boolean> ErrorLog(Int64 userId, string errorMessage, string errorAction, Int32 errorCode)
        {
            return true;
        }
    }
}
