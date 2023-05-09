﻿using System.Data;

namespace WebApp.SharedKernel.Dtos.Response
{
    public class StoredProcedureResponseDto
    {
        public StoredProcedureResponseDto()
        {
            dataTable = new DataTable();
            lParameters = new List<IDataParameter>();
        }
        public DataTable dataTable { get; set; }
        public List<IDataParameter> lParameters { get; set; }
    }
}
