using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagement.Commands.Handlers
{
    public class CommandResponse
    {
        public bool Status { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 000-Success
        /// 100-Failed/Error
        /// 101-Exists
        /// </summary>
        public int Code { get; set; }
    }
}
