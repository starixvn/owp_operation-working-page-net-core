using Epplus.Mapper.Annotations;
using System;
using System.Collections.Generic;

namespace OWP.AutoAssignment.Models.InternalApi
{
    public record ExportModel
    {
        [Cell("B1")]
        public DateTime Date { get; init; }

        public IList<ExportMatchModel> exportMatchModels
        {
            get;
            set;
        }
    }
}