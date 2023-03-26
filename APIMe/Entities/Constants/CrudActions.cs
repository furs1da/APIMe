﻿using APIMe.Entities.DataTransferObjects.Admin.Route;

namespace APIMe.Entities.Constants
{
    public class CrudAction
    {
        public int Id { get; set; }
        public string Action { get; set; }
    }

    public static class CrudActions
    {
        public static IReadOnlyList<CrudAction> Actions { get; } = new List<CrudAction>
        {
            new CrudAction { Id = 1, Action = "POST" },
            new CrudAction { Id = 2, Action = "POST" },
            new CrudAction { Id = 3, Action = "PUT" },
            new CrudAction { Id = 4, Action = "PATCH" },
            new CrudAction { Id = 5, Action = "DELETE" },
            new CrudAction { Id = 6, Action = "ERROR" },
        };
    }

    public static class DataSourceTables
    {
        public static IReadOnlyList<DataSourceDTO> DataSources { get; } = new List<DataSourceDTO>
        {
            new DataSourceDTO { Id = 1, Name = "Product" },
            new DataSourceDTO { Id = 2, Name = "Customer" }
        };
    }
}